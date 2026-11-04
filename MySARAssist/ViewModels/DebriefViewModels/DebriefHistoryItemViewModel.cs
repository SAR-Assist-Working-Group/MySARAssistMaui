using CommunityToolkit.Mvvm.ComponentModel;
using MySarAssistModels.Assignments;
using System;

namespace MySARAssist.ViewModels.DebriefViewModels
{
    public partial class DebriefHistoryItemViewModel : ObservableObject
    {
        public AssignmentDebrief Model { get; }

        public Guid ID => Model.ID;
        public Guid AssignmentID => Model.AssignmentID;
        public int AssignmentNumber => Model.AssignmentNumber;
        public string TeamName => string.IsNullOrWhiteSpace(Model.TeamName) ? "Unassigned Team" : Model.TeamName;

        public string Title => Model.AssignmentNumber > 0 
            ? $"Assignment #{Model.AssignmentNumber} — {TeamName}" 
            : $"Debrief — {TeamName}";

        public string DebriefDateFormatted => Model.DebriefTime.ToLocalTime().ToString("yyyy-MMM-dd HH:mm");

        public bool IsComplete => Model.IsComplete;
        public bool IsExported => Model.IsExported;

        public string StatusBadgeText
        {
            get
            {
                if (!Model.IsComplete) return "In Progress";
                if (!Model.IsExported) return "Needs Export";
                return "Exported";
            }
        }

        public string StatusBadgeColor
        {
            get
            {
                if (!Model.IsComplete) return "#D97706"; // Amber / Warning
                if (!Model.IsExported) return "#2563EB"; // Blue / Info
                return "#16A34A"; // Green / Success
            }
        }

        public DebriefHistoryItemViewModel(AssignmentDebrief debrief)
        {
            Model = debrief ?? throw new ArgumentNullException(nameof(debrief));
        }
    }
}

