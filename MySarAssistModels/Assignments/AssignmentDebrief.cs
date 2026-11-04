using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySarAssistModels.Assignments
{
    [Serializable]
    public class AssignmentDebrief : SyncableItem, ICloneable
    {
        private Guid _AssignmentID;
        private Guid _TaskID;
        private int _AssignmentNumber;
        private DateTime _AssignmentStart;
        private DateTime _AssignmentEnd;
        private DateTime _DebriefTime;
        private string _TeamName = string.Empty;
         private string? _WhatYourTeamDid;
         private bool _TracksProvided;
         private bool _Deviation;
         private string? _EnvironmentFactors;
         private string? _SearcherFactors;
         private string? _Gaps;
         private string? _Hazards;
         private string? _Weather;
         private string? _Comments;
         private Guid _DebriefedBy;
         private double _DebriefPOD;
         private int _DebriefRADeMSOperationalRisk;
         private int _DebriefRADeMSResponseCapability;
         private double _DebriefSpacing;
         private double _DebriefTravelSpeedPerHour;
         private string? _TrackImageBytes;
         private double _DebriefRangeOfDetection;
         private string? _Injuries;
         private string? _Losses;

        private bool _IsComplete;
        private bool _IsExported;
        private DateTime? _LastExportedUTC;

        public AssignmentDebrief()
        {
            ID = Guid.NewGuid();
            AssignmentID = Guid.NewGuid();
            DebriefTime = DateTime.UtcNow;
            AssignmentStart = DateTime.UtcNow;
            AssignmentEnd = DateTime.UtcNow;
            LastUpdatedUTC = DateTime.UtcNow;
            Active = true;
        }

        public Guid AssignmentID { get => _AssignmentID; set => _AssignmentID = value; }
        public Guid TaskID { get => _TaskID; set => _TaskID = value; }
        public int AssignmentNumber { get => _AssignmentNumber; set => _AssignmentNumber = value; }
        public DateTime AssignmentStart { get => _AssignmentStart; set => _AssignmentStart = value; }
        public DateTime AssignmentEnd { get => _AssignmentEnd; set => _AssignmentEnd = value; }
        public DateTime DebriefTime { get => _DebriefTime; set => _DebriefTime = value; }
        public string TeamName { get => _TeamName; set => _TeamName = value; }
        public string? WhatYourTeamDid { get => _WhatYourTeamDid; set => _WhatYourTeamDid = value; }
        public bool TracksProvided { get => _TracksProvided; set => _TracksProvided = value; }
        public bool Deviation { get => _Deviation; set => _Deviation = value; }
        public string? EnvironmentFactors { get => _EnvironmentFactors; set => _EnvironmentFactors = value; }
        public string? SearcherFactors { get => _SearcherFactors; set => _SearcherFactors = value; }
        public string? Gaps { get => _Gaps; set => _Gaps = value; }
        public string? Hazards { get => _Hazards; set => _Hazards = value; }
        public string? Weather { get => _Weather; set => _Weather = value; }
        public string? Comments { get => _Comments; set => _Comments = value; }
        public Guid DebriefedBy { get => _DebriefedBy; set => _DebriefedBy = value; }

        public double DebriefPOD { get => _DebriefPOD; set => _DebriefPOD = value; }
        public int DebriefRADeMSOperationalRisk { get => _DebriefRADeMSOperationalRisk; set => _DebriefRADeMSOperationalRisk = value; }
        public int DebriefRADeMSResponseCapability { get => _DebriefRADeMSResponseCapability; set => _DebriefRADeMSResponseCapability = value; }
        public string? TrackImageBytes { get => _TrackImageBytes; set => _TrackImageBytes = value; }
        public double DebriefRangeOfDetection { get => _DebriefRangeOfDetection; set => _DebriefRangeOfDetection = value; }
        public double DebriefTravelSpeedPerHour { get => _DebriefTravelSpeedPerHour; set => _DebriefTravelSpeedPerHour = value; }
        public double DebriefSpacing { get => _DebriefSpacing; set => _DebriefSpacing = value; }
        public string? Injuries { get => _Injuries; set => _Injuries = value; }
        public string? Losses { get => _Losses; set => _Losses = value; }
        public bool HasInjuriesOrLosses { get => !string.IsNullOrEmpty(Injuries) || !string.IsNullOrEmpty(Losses); }

        public bool IsComplete { get => _IsComplete; set => _IsComplete = value; }
        public bool IsExported { get => _IsExported; set => _IsExported = value; }
        public DateTime? LastExportedUTC { get => _LastExportedUTC; set => _LastExportedUTC = value; }

        public AssignmentDebrief? Clone()
        {
            return this.MemberwiseClone() as AssignmentDebrief;
        }
        object ICloneable.Clone()
        {
            return this.Clone();
        }
    }
}
