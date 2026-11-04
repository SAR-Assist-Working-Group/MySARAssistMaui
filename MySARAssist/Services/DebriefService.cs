using MySarAssistModels.Assignments;
using MySarAssistModels.Clues;
using MySarAssistModels.RADeMS;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySARAssist.Services
{
    public class DebriefService
    {
        private readonly SQLiteAsyncConnection _conn;
        private bool _isInitialized;

        public DebriefService()
        {
            _conn = new SQLiteAsyncConnection(Constants.dbPath);
        }

        private async Task EnsureInitializedAsync()
        {
            if (_isInitialized) return;
            await _conn.CreateTableAsync<AssignmentDebrief>();
            await _conn.CreateTableAsync<Clue>();
            await _conn.CreateTableAsync<RADeMSScore>();
            _isInitialized = true;
        }

        /// <summary>
        /// Retrieves the current active (in-progress) debrief, if one exists.
        /// An active debrief is one where IsComplete is false, ordered by most recently updated.
        /// </summary>
        public async Task<AssignmentDebrief?> GetActiveDebriefAsync()
        {
            await EnsureInitializedAsync();
            var activeDebriefs = await _conn.Table<AssignmentDebrief>()
                .Where(d => !d.IsComplete)
                .OrderByDescending(d => d.LastUpdatedUTC)
                .ToListAsync();

            return activeDebriefs.FirstOrDefault();
        }

        /// <summary>
        /// Retrieves a specific debrief by its primary key ID.
        /// </summary>
        public async Task<AssignmentDebrief?> GetDebriefAsync(Guid id)
        {
            await EnsureInitializedAsync();
            return await _conn.Table<AssignmentDebrief>().FirstOrDefaultAsync(d => d.ID == id);
        }

        /// <summary>
        /// Retrieves all debriefs ordered reverse-chronologically by debrief time.
        /// </summary>
        public async Task<List<AssignmentDebrief>> GetAllDebriefsAsync()
        {
            await EnsureInitializedAsync();
            return await _conn.Table<AssignmentDebrief>()
                .OrderByDescending(d => d.DebriefTime)
                .ToListAsync();
        }

        /// <summary>
        /// Inserts or updates a debrief draft in the local SQLite database.
        /// </summary>
        public async Task<bool> SaveDebriefAsync(AssignmentDebrief debrief)
        {
            if (debrief == null) throw new ArgumentNullException(nameof(debrief));
            await EnsureInitializedAsync();

            debrief.LastUpdatedUTC = DateTime.UtcNow;

            var existing = await _conn.Table<AssignmentDebrief>().FirstOrDefaultAsync(d => d.ID == debrief.ID);
            if (existing == null)
            {
                int inserted = await _conn.InsertAsync(debrief);
                return inserted > 0;
            }
            else
            {
                int updated = await _conn.UpdateAsync(debrief);
                return updated > 0;
            }
        }

        /// <summary>
        /// Deletes a debrief and optionally its associated clues.
        /// </summary>
        public async Task<bool> DeleteDebriefAsync(Guid debriefId, bool deleteAssociatedClues = true)
        {
            await EnsureInitializedAsync();
            var debrief = await GetDebriefAsync(debriefId);
            if (debrief == null) return false;

            if (deleteAssociatedClues && debrief.AssignmentID != Guid.Empty)
            {
                var clues = await _conn.Table<Clue>()
                    .Where(c => c.AssignmentID == debrief.AssignmentID)
                    .ToListAsync();

                foreach (var clue in clues)
                {
                    await _conn.DeleteAsync(clue);
                }
            }

            int deleted = await _conn.DeleteAsync(debrief);
            return deleted > 0;
        }

        /// <summary>
        /// Marks a debrief as completed by the GSTL.
        /// </summary>
        public async Task<bool> MarkCompleteAsync(Guid debriefId, bool isComplete = true)
        {
            await EnsureInitializedAsync();
            var debrief = await GetDebriefAsync(debriefId);
            if (debrief == null) return false;

            debrief.IsComplete = isComplete;
            return await SaveDebriefAsync(debrief);
        }

        /// <summary>
        /// Marks a debrief as exported.
        /// </summary>
        public async Task<bool> MarkExportedAsync(Guid debriefId)
        {
            await EnsureInitializedAsync();
            var debrief = await GetDebriefAsync(debriefId);
            if (debrief == null) return false;

            debrief.IsExported = true;
            debrief.LastExportedUTC = DateTime.UtcNow;
            return await SaveDebriefAsync(debrief);
        }

        #region Clues Management for Debrief

        /// <summary>
        /// Retrieves clues associated with an assignment.
        /// </summary>
        public async Task<List<Clue>> GetCluesForAssignmentAsync(Guid assignmentId)
        {
            await EnsureInitializedAsync();
            if (assignmentId == Guid.Empty) return new List<Clue>();

            return await _conn.Table<Clue>()
                .Where(c => c.AssignmentID == assignmentId)
                .OrderBy(c => c.ClueNumber)
                .ToListAsync();
        }

        /// <summary>
        /// Saves or updates a clue associated with the current assignment debrief.
        /// </summary>
        public async Task<bool> SaveClueAsync(Clue clue)
        {
            if (clue == null) throw new ArgumentNullException(nameof(clue));
            await EnsureInitializedAsync();

            clue.LastUpdatedUTC = DateTime.UtcNow;

            var existing = await _conn.Table<Clue>().FirstOrDefaultAsync(c => c.ID == clue.ID || c.ClueID == clue.ClueID);
            if (existing == null)
            {
                int inserted = await _conn.InsertAsync(clue);
                return inserted > 0;
            }
            else
            {
                int updated = await _conn.UpdateAsync(clue);
                return updated > 0;
            }
        }

        /// <summary>
        /// Deletes a specific clue by its ID.
        /// </summary>
        public async Task<bool> DeleteClueAsync(Guid clueId)
        {
            await EnsureInitializedAsync();
            var clue = await _conn.Table<Clue>().FirstOrDefaultAsync(c => c.ID == clueId || c.ClueID == clueId);
            if (clue == null) return false;

            int deleted = await _conn.DeleteAsync(clue);
            return deleted > 0;
        }

        #endregion

        /// <summary>
        /// Builds a self-contained AssignmentDebriefPackage bundling the debrief, its clues, and any RADeMS scores.
        /// </summary>
        public async Task<AssignmentDebriefPackage?> CreateDebriefPackageAsync(Guid debriefId, string appVersion = "1.0")
        {
            await EnsureInitializedAsync();
            var debrief = await GetDebriefAsync(debriefId);
            if (debrief == null) return null;

            var clues = await GetCluesForAssignmentAsync(debrief.AssignmentID);

            // Fetch any radems scores (or latest score if exists)
            List<RADeMSScore> rademsScores = new();
            try
            {
                var scores = await _conn.Table<RADeMSScore>().OrderByDescending(s => s.LastUpdatedUTC).Take(2).ToListAsync();
                if (scores != null) rademsScores.AddRange(scores);
            }
            catch
            {
                // RADeMS scores table may be empty
            }

            return new AssignmentDebriefPackage(debrief, clues, rademsScores, appVersion);
        }
    }
}

