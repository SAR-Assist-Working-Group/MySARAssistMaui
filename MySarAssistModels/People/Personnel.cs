using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySarAssistModels.Interfaces;
using SQLite;

namespace MySarAssistModels.People
{
    public class Personnel : IncidentResource, IEquatable<Personnel>, ICloneable, IPersonnel
    {
        public Personnel()
        {
            MemberActive = true;
            QualificationList = new bool[30];
        }

        [Required]
        private string _Name = string.Empty;
        private string? _Group;
        private string? _Callsign;
        private string? _Phone;
        private string? _SpecialSkills;
        private bool _isAssignmentTeamLeader;
        private string? _Reference;
        private string? _barcode;
        private bool _signedIn;
        private Guid _organizationID;
        private Guid _userID;
        [Required]
        private string _Email = string.Empty;
        private Guid _CreatedByOrgID;
        private string? _Address;
        private string? _NOKName;
        private string? _NOKRelation;
        private string? _NOKPhone;
        private string? _D4HStatus;
        private int _D4HID;
        private string? _Dietary;
        private bool _Vegetarian;
        private bool _NoGluten;
        private string? _Pronouns;
        private bool[] _QualificationList = new bool[28];
        private double _PacesPer100;
        private bool _CurrentlySelected;

        public Guid PersonID { get => ID; set => ID = value; }
        public int D4HID { get => _D4HID; set => _D4HID = value; }

        public string Name { get => _Name; set => _Name = value; }
        public string? Pronouns { get => _Pronouns; set => _Pronouns = value; }

        public string? Group { get => _Group; set => _Group = value; } //Use OrganizationName for this value
        public string? NameWithGroup
        {
            get
            {
                if (!string.IsNullOrEmpty(Group)) { return Name + " (" + Group + ")"; }
                else { return Name; }
            }
        }

        public string? NameWithGroupAndCurrent
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                if (CurrentlySelected) { sb.Append("*"); }
                sb.Append(Name);
                if (!string.IsNullOrEmpty(Group))
                {
                    sb.Append(" ("); sb.Append(Group); sb.Append(")");
                }
                return sb.ToString();
            }
        }
        public string? NameWithTL
        {
            get
            {
                if (GSTL) { return Name + " (GSTL)"; }
                else { return Name; }
            }
        }
        public string? NameWithSARM
        {
            get
            {
                if (SARM) { return Name + " (SAR Mgr)"; }
                else { return Name; }
            }
        }
        
        [Ignore]
        public bool[] QualificationList
        {
            get
            {
                if (_QualificationList == null) { _QualificationList = new bool[30]; }
                if(_QualificationList.Length < 30) { Array.Resize(ref _QualificationList, 30); }
                return _QualificationList;
            }
            set => _QualificationList = value;
        }

        public string? Callsign { get => _Callsign; set => _Callsign = value; }
        public string? Phone { get => _Phone; set => _Phone = value; }
        public bool RopeRescue { get { if (QualificationList != null && (QualificationList[13] || QualificationList[14] || QualificationList[15])) { return true; } return false; } }
        public bool Tracker { get { if (QualificationList != null && (QualificationList[18] || QualificationList[19] || QualificationList[20])) { return true; } return false; } }
        public bool FirstAid { get { if (QualificationList != null && (QualificationList[4] || QualificationList[5] || QualificationList[6] || QualificationList[7] || QualificationList[8] || QualificationList[9])) { return true; } return false; } }
        public bool FirstAid40Plus { get { if (QualificationList != null && (QualificationList[5] || QualificationList[6] || QualificationList[7] || QualificationList[8] || QualificationList[9])) { return true; } return false; } }
        public bool GSAR { get => QualificationList != null && QualificationList[0]; }
        public string? SpecialSkills { get => _SpecialSkills; set => _SpecialSkills = value; }
        public bool isAssignmentTeamLeader { get => _isAssignmentTeamLeader; set => _isAssignmentTeamLeader = value; }
        public string? Reference { get { return _Reference; } set { _Reference = value; } }
        public bool GSTL { get { return QualificationList != null && QualificationList[1]; } }
        public bool SARM { get { return QualificationList != null && (QualificationList[2] || QualificationList[3]); } }
        public bool Swiftwater { get { if (QualificationList != null && (QualificationList[10] || QualificationList[11] || QualificationList[12])) { return true; } return false; } }
        public bool MountainRescue { get { if (QualificationList != null && (QualificationList[21] || QualificationList[22] || QualificationList[23])) { return true; } return false; } }
        public bool BoatOperator { get { if (QualificationList != null && QualificationList[24]) { return true; } return false; } }
        public bool CDFL { get { if (QualificationList != null && QualificationList[26]) { return true; } return false; } }
        public bool K9 { get { if (QualificationList != null && QualificationList[27]) { return true; } return false; } }
        public bool OffRoadVehicleOperator { get { if (QualificationList != null && QualificationList[24]) { return true; } return false; } }


        public string? Barcode { get { return _barcode; } set { _barcode = value; } }
        public bool SignedInToTask { get { return _signedIn; } set { _signedIn = value; } }
        public Guid OrganizationID { get => _organizationID; set => _organizationID = value; }
        public Guid UserID { get => _userID; set => _userID = value; }
        public bool MemberActive { get => Active; set => Active = value; }
        public string Email { get => _Email; set => _Email = value; }
        public Guid CreatedByOrgID { get => _CreatedByOrgID; set => _CreatedByOrgID = value; } //used when a team creates members from outside their group for a task.
        public string? Address { get => _Address; set => _Address = value; }
        public string? AddressWithPhone
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrEmpty(Address))
                {
                    sb.Append(Address.Replace("\r\n", " "));
                    if (!string.IsNullOrEmpty(Phone)) { sb.Append("\n"); }
                }
                if (!string.IsNullOrEmpty(Phone)) { sb.Append(Phone); }

                return sb.ToString();
            }
        }
        public string? NOKName { get => _NOKName; set => _NOKName = value; }
        public string? NOKRelation { get => _NOKRelation; set => _NOKRelation = value; }
        public string? NOKPhone { get => _NOKPhone; set => _NOKPhone = value; }
        public string? NOKOneLine
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrEmpty(NOKName)) { sb.Append(NOKName); sb.Append(" "); }
                if (!string.IsNullOrEmpty(NOKRelation)) { sb.Append("("); sb.Append(NOKRelation); sb.Append(") "); }
                if (!string.IsNullOrEmpty(NOKPhone)) { sb.Append(NOKPhone); }
                return sb.ToString();
            }
        }
        public string? D4HStatus { get => _D4HStatus; set => _D4HStatus = value; }
        public string? Dietary { get => _Dietary; set => _Dietary = value; }
        public bool Vegetarian { get => _Vegetarian; set => _Vegetarian = value; }
        public bool NoGluten { get => _NoGluten; set => _NoGluten = value; }
        public double PacesPer100 { get => _PacesPer100; set => _PacesPer100 = value; }
        public bool CurrentlySelected { get => _CurrentlySelected; set => _CurrentlySelected = value; }
        [Ignore]
        public Organization? MemberOrganization { get => _MemberOrganization; set => _MemberOrganization = value; }
        private Organization? _MemberOrganization;


        /* this is so qualifications get saved in sqllite */
        public bool Qualification0 { get => _QualificationList[0]; set => _QualificationList[0] = value; }
        public bool Qualification1 { get => _QualificationList[1]; set => _QualificationList[1] = value; }
        public bool Qualification2 { get => _QualificationList[2]; set => _QualificationList[2] = value; }
        public bool Qualification3 { get => _QualificationList[3]; set => _QualificationList[3] = value; }
        public bool Qualification4 { get => _QualificationList[4]; set => _QualificationList[4] = value; }
        public bool Qualification5 { get => _QualificationList[5]; set => _QualificationList[5] = value; }
        public bool Qualification6 { get => _QualificationList[6]; set => _QualificationList[6] = value; }
        public bool Qualification7 { get => _QualificationList[7]; set => _QualificationList[7] = value; }
        public bool Qualification8 { get => _QualificationList[8]; set => _QualificationList[8] = value; }
        public bool Qualification9 { get => _QualificationList[9]; set => _QualificationList[9] = value; }
        public bool Qualification10 { get => _QualificationList[10]; set => _QualificationList[10] = value; }
        public bool Qualification11 { get => _QualificationList[11]; set => _QualificationList[11] = value; }
        public bool Qualification12 { get => _QualificationList[12]; set => _QualificationList[12] = value; }
        public bool Qualification13 { get => _QualificationList[13]; set => _QualificationList[13] = value; }
        public bool Qualification14 { get => _QualificationList[14]; set => _QualificationList[14] = value; }
        public bool Qualification15 { get => _QualificationList[15]; set => _QualificationList[15] = value; }
        public bool Qualification16 { get => _QualificationList[16]; set => _QualificationList[16] = value; }
        public bool Qualification17 { get => _QualificationList[17]; set => _QualificationList[17] = value; }
        public bool Qualification18 { get => _QualificationList[18]; set => _QualificationList[18] = value; }
        public bool Qualification19 { get => _QualificationList[19]; set => _QualificationList[19] = value; }
        public bool Qualification20 { get => _QualificationList[20]; set => _QualificationList[20] = value; }
        public bool Qualification21 { get => _QualificationList[21]; set => _QualificationList[21] = value; }
        public bool Qualification22 { get => _QualificationList[22]; set => _QualificationList[22] = value; }
        public bool Qualification23 { get => _QualificationList[23]; set => _QualificationList[23] = value; }
        public bool Qualification24 { get => _QualificationList[24]; set => _QualificationList[24] = value; }
        public bool Qualification25 { get => _QualificationList[25]; set => _QualificationList[25] = value; }
        public bool Qualification26 { get => _QualificationList[26]; set => _QualificationList[26] = value; }
        public bool Qualification27 { get => _QualificationList[27]; set => _QualificationList[27] = value; }
        public bool Qualification28 { get => _QualificationList[28]; set => _QualificationList[28] = value; }
        public bool Qualification29 { get => _QualificationList[29]; set => _QualificationList[29] = value; }



        public bool Equals(Personnel other)
        {
            // Would still want to check for null etc. first.
            return PersonID == other.PersonID;
        }




        public new Personnel? Clone()
        {
            if (this != null)
            {
                Personnel cloneTo = MemberwiseClone() as Personnel;
                return cloneTo;
            }
            return null;
        }
        object ICloneable.Clone()
        {
            return Clone();
        }

    }
}

