using MySarAssistModels.Clues;
using MySarAssistModels.RADeMS;
using System;
using System.Collections.Generic;

namespace MySarAssistModels.Assignments
{
    [Serializable]
    public class AssignmentDebriefPackage
    {
        private string _FormatVersion = "1.0";
        private string _Generator = "MySAR Assist";
        private string _AppVersion = "1.0";
        private DateTime _ExportedAt = DateTime.UtcNow;

        private Guid _TaskID;
        private Guid _AssignmentID;
        private int _AssignmentNumber;
        private string _TeamName = string.Empty;

        private AssignmentDebrief? _Debrief;
        private List<RADeMSScore> _RademsScores = new();
        private List<Clue> _Clues = new();

        public string FormatVersion { get => _FormatVersion; set => _FormatVersion = value; }
        public string Generator { get => _Generator; set => _Generator = value; }
        public string AppVersion { get => _AppVersion; set => _AppVersion = value; }
        public DateTime ExportedAt { get => _ExportedAt; set => _ExportedAt = value; }

        public Guid TaskID { get => _TaskID; set => _TaskID = value; }
        public Guid AssignmentID { get => _AssignmentID; set => _AssignmentID = value; }
        public int AssignmentNumber { get => _AssignmentNumber; set => _AssignmentNumber = value; }
        public string TeamName { get => _TeamName; set => _TeamName = value; }

        public AssignmentDebrief? Debrief { get => _Debrief; set => _Debrief = value; }
        public List<RADeMSScore> RademsScores { get => _RademsScores; set => _RademsScores = value ?? new(); }
        public List<Clue> Clues { get => _Clues; set => _Clues = value ?? new(); }

        public AssignmentDebriefPackage()
        {
        }

        public AssignmentDebriefPackage(AssignmentDebrief debrief, IEnumerable<Clue>? clues = null, IEnumerable<RADeMSScore>? rademsScores = null, string appVersion = "1.0")
        {
            Debrief = debrief;
            AppVersion = appVersion;
            ExportedAt = DateTime.UtcNow;

            if (debrief != null)
            {
                TaskID = debrief.TaskID;
                AssignmentID = debrief.AssignmentID;
                AssignmentNumber = debrief.AssignmentNumber;
                TeamName = debrief.TeamName;
            }

            if (clues != null)
            {
                Clues = new List<Clue>(clues);
            }

            if (rademsScores != null)
            {
                RademsScores = new List<RADeMSScore>(rademsScores);
            }
        }
    }
}
