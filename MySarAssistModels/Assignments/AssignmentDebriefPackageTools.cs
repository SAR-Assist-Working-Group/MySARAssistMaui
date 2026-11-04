using MySarAssistModels.Clues;
using MySarAssistModels.RADeMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MySarAssistModels.Assignments
{
    public static class AssignmentDebriefPackageTools
    {
        private static readonly JsonSerializerOptions CompactSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = false
        };

        private static readonly JsonSerializerOptions IndentedSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true
        };

        /// <summary>
        /// Serializes an AssignmentDebriefPackage to JSON.
        /// </summary>
        public static string ToJson(AssignmentDebriefPackage package, bool indented = false)
        {
            if (package == null) throw new ArgumentNullException(nameof(package));
            return JsonSerializer.Serialize(package, indented ? IndentedSerializerOptions : CompactSerializerOptions);
        }

        /// <summary>
        /// Deserializes JSON string to an AssignmentDebriefPackage.
        /// </summary>
        public static AssignmentDebriefPackage? FromJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return null;
            return JsonSerializer.Deserialize<AssignmentDebriefPackage>(json, CompactSerializerOptions);
        }

        /// <summary>
        /// Creates a lightweight copy of the package with all embedded photos removed (for QR code export).
        /// </summary>
        public static AssignmentDebriefPackage StripPhotos(AssignmentDebriefPackage package)
        {
            if (package == null) throw new ArgumentNullException(nameof(package));

            AssignmentDebrief? strippedDebrief = null;
            if (package.Debrief != null)
            {
                strippedDebrief = package.Debrief.Clone();
                if (strippedDebrief != null)
                {
                    strippedDebrief.TrackImageBytes = null;
                }
            }

            List<Clue> strippedClues = new();
            if (package.Clues != null)
            {
                foreach (var clue in package.Clues)
                {
                    var clonedClue = clue.Clone() as Clue ?? new Clue
                    {
                        ClueID = clue.ClueID,
                        TaskID = clue.TaskID,
                        AssignmentID = clue.AssignmentID,
                        ClueNumber = clue.ClueNumber,
                        Description = clue.Description,
                        DateFound = clue.DateFound,
                        FoundBy = clue.FoundBy,
                        LocationFound = clue.LocationFound,
                        DescriptionOfLocation = clue.DescriptionOfLocation,
                        ClueValueId = clue.ClueValueId,
                        CurrentStatus = clue.CurrentStatus,
                        Evaluation = clue.Evaluation,
                        FollowUpActions = clue.FollowUpActions,
                        Latitude = clue.Latitude,
                        Longitude = clue.Longitude,
                        RecordedBy = clue.RecordedBy,
                        LocationRouteFlagged = clue.LocationRouteFlagged
                    };
                    clonedClue.ImageBytes = null;
                    strippedClues.Add(clonedClue);
                }
            }

            List<RADeMSScore> clonedScores = new();
            if (package.RademsScores != null)
            {
                foreach (var score in package.RademsScores)
                {
                    if (score.Clone() is RADeMSScore clonedScore)
                    {
                        clonedScores.Add(clonedScore);
                    }
                }
            }

            return new AssignmentDebriefPackage
            {
                FormatVersion = package.FormatVersion,
                Generator = package.Generator,
                AppVersion = package.AppVersion,
                ExportedAt = package.ExportedAt,
                TaskID = package.TaskID,
                AssignmentID = package.AssignmentID,
                AssignmentNumber = package.AssignmentNumber,
                TeamName = package.TeamName,
                Debrief = strippedDebrief,
                Clues = strippedClues,
                RademsScores = clonedScores
            };
        }

        /// <summary>
        /// Calculates the approximate UTF-8 byte count of the serialized JSON payload.
        /// </summary>
        public static int EstimateByteSize(AssignmentDebriefPackage package, bool indented = false)
        {
            string json = ToJson(package, indented);
            return Encoding.UTF8.GetByteCount(json);
        }

        /// <summary>
        /// Checks whether the package can fit within a single 2D QR code payload threshold (default 2048 bytes).
        /// </summary>
        public static bool CanFitInQrCode(AssignmentDebriefPackage package, int maxBytes = 2048)
        {
            return EstimateByteSize(package, indented: false) <= maxBytes;
        }
    }
}

