using System.Text;

namespace MySarAssistModels.People
{
    public static class PersonnelTools
    {
        public static List<string> GetValidationIssues(this Personnel person)
        {
            var issues = new List<string>();
            if (string.IsNullOrEmpty(person.Name)) { issues.Add("Name is required"); }
            else if (!person.Name.Contains(" ")) { issues.Add("A full name is required (first and last)"); }

            if (string.IsNullOrEmpty(person.Email) || !person.Email.isValidEmail()) { issues.Add("A valid email is required"); }
            return issues;
        }

        public static void RemoveBadChrs(this Personnel? member)
        {
            if (member != null)
            {
                member.Name = member.Name.removeBadChrsForQR();
                member.Callsign = member.Callsign.removeBadChrsForQR();
                member.Phone = member.Phone.removeBadChrsForQR();
                member.SpecialSkills = member.SpecialSkills.removeBadChrsForQR();
                member.Reference = member.Reference.removeBadChrsForQR();
                member.Barcode = member.Barcode.removeBadChrsForQR();
                member.Email = member.Email.removeBadChrsForQR();
                member.Pronouns = member.Pronouns.removeBadChrsForQR();
                member.Address = member.Address.removeBadChrsForQR();
                member.NOKName = member.NOKName.removeBadChrsForQR();
                member.NOKRelation = member.NOKRelation.removeBadChrsForQR();
                member.NOKPhone = member.NOKPhone.removeBadChrsForQR();
                member.D4HStatus = member.D4HStatus.removeBadChrsForQR();
            }
        }



        public static string StringForQRV6(this Personnel person)
        {

            StringBuilder qr = new StringBuilder();
            qr.Append(person.PersonID.ToString()); qr.Append(';');

            qr.Append(person.Name); qr.Append(';');
            qr.Append(person.OrganizationID); qr.Append(';');
            qr.Append(person.Address); qr.Append(';');
            qr.Append(person.Phone); qr.Append(';');
            qr.Append(person.Email); qr.Append(';');
            qr.Append(person.Callsign); qr.Append(';');
            qr.Append(person.Reference); qr.Append(';');
            //qualifications
            if (person.GSAR) { qr.Append('1'); } else { qr.Append('0'); }
            if (person.GSTL) { qr.Append('1'); } else { qr.Append('0'); }
            if (person.SARM) { qr.Append('1'); } else { qr.Append('0'); }
            if (person.FirstAid) { qr.Append('1'); } else { qr.Append('0'); }
            if (person.RopeRescue) { qr.Append('1'); } else { qr.Append('0'); }
            if (person.Tracker) { qr.Append('1'); } else { qr.Append('0'); }
            if (person.Swiftwater) { qr.Append('1'); } else { qr.Append('0'); }
            if (person.MountainRescue) { qr.Append('1'); } else { qr.Append('0'); }
            qr.Append(';');

            //nok
            qr.Append(person.NOKName); qr.Append(';');
            qr.Append(person.NOKRelation); qr.Append(';');
            qr.Append(person.NOKPhone); qr.Append(';');


            return qr.ToString();

        }

        public static List<Qualification> GetPersonnelQualifications(this Personnel person)
        {
            List<Qualification> qualifications = GetQualifications();
            foreach (Qualification q in qualifications)
            {
                if (person.QualificationList.Length > q.QualificationListIndex)
                    q.PersonHas = person.QualificationList[q.QualificationListIndex];
                    
            }

            return qualifications;
        }




        public static void removeTildeFromRecord(this Personnel p)
        {
            if ((p.Name ?? string.Empty).Contains("~")) { p.Name = (p.Name ?? string.Empty).Replace("~", ""); }
            if (!string.IsNullOrEmpty(p.Address) && p.Address.Contains("~")) { p.Address = p.Address.Replace("~", ""); }
            if (!string.IsNullOrEmpty(p.Pronouns) && p.Pronouns.Contains("~")) { p.Pronouns = p.Pronouns.Replace("~", ""); }
            if (!string.IsNullOrEmpty(p.Phone) && p.Phone.Contains("~")) { p.Phone = p.Phone.Replace("~", ""); }
            if (!string.IsNullOrEmpty(p.Email) && p.Email.Contains("~")) { p.Email = p.Email.Replace("~", ""); }
            if (!string.IsNullOrEmpty(p.Callsign) && p.Callsign.Contains("~")) { p.Callsign = p.Callsign.Replace("~", ""); }
            if (!string.IsNullOrEmpty(p.Reference) && p.Reference.Contains("~")) { p.Reference = p.Reference.Replace("~", ""); }
            if (!string.IsNullOrEmpty(p.NOKName) && p.NOKName.Contains("~")) { p.NOKName = p.NOKName.Replace("~", ""); }
            if (!string.IsNullOrEmpty(p.NOKRelation) && p.NOKRelation.Contains("~")) { p.NOKRelation = p.NOKRelation.Replace("~", ""); }
            if (!string.IsNullOrEmpty(p.NOKPhone) && p.NOKPhone.Contains("~")) { p.NOKPhone = p.NOKPhone.Replace("~", ""); }
            if (!string.IsNullOrEmpty(p.Dietary) && p.Dietary.Contains("~")) { p.Dietary = p.Dietary.Replace("~", ""); }

        }


        public static string StringForQR(this Personnel p)
        {

            StringBuilder qr = new StringBuilder();
            qr.Append(p.PersonID.ToString()); qr.Append(";"); //remove?

            qr.Append(p.Name); qr.Append(";");
            qr.Append(p.OrganizationID); qr.Append(";");
            if (!string.IsNullOrEmpty(p.Address)) { qr.Append(p.Address.Replace(Environment.NewLine, " ")); } else { qr.Append(""); }
            qr.Append(";");
            qr.Append(p.Phone); qr.Append(";");
            qr.Append(p.Email); qr.Append(";");
            qr.Append(p.Callsign); qr.Append(";"); //remove
            qr.Append(p.Reference); qr.Append(";"); //remove
                                                    //qualifications
                                                    //pretend these are characters in a binary string and convert to int?
            List<Qualification> qualifications = GetQualifications();
            qualifications = qualifications.OrderBy(o => o.QRIndex).ToList();
            for (int x = 0; x < qualifications.Count && x < p.QualificationList.Length; x++)
            {
                Qualification q = qualifications[x];
                if (p.QualificationList[q.QualificationListIndex]) { qr.Append("1"); } else { qr.Append("0"); }
            }
            qr.Append(";");

            //nok
            qr.Append(p.NOKName); qr.Append(";");
            qr.Append(p.NOKRelation); qr.Append(";"); //remove
            qr.Append(p.NOKPhone); qr.Append(";");


            return qr.ToString();

        }

       


        public static bool IsIdentical(this Personnel orig, Personnel compareTo)
        {
            if (orig.PersonID != compareTo.PersonID) { return false; }
            if (orig.Name != compareTo.Name) { return false; }
            if (orig.Group != compareTo.Group) { return false; }
            if (orig.Callsign != compareTo.Callsign) { return false; }
            if (orig.Phone != compareTo.Phone) { return false; }
            if (orig.SpecialSkills != compareTo.SpecialSkills) { return false; }
            if (orig.isAssignmentTeamLeader != compareTo.isAssignmentTeamLeader) { return false; }
            if (orig.Reference != compareTo.Reference) { return false; }
            if (orig.Barcode != compareTo.Barcode) { return false; }
            if (orig.OrganizationID != compareTo.OrganizationID) { return false; }
            if (orig.UserID != compareTo.UserID) { return false; }
            if (orig.MemberActive != compareTo.MemberActive) { return false; }
            if (orig.LastUpdatedUTC != compareTo.LastUpdatedUTC) { return false; }
            if (orig.Email != compareTo.Email) { return false; }
            if (orig.CreatedByOrgID != compareTo.CreatedByOrgID) { return false; }
            if (orig.Address != compareTo.Address) { return false; }
            if (orig.NOKName != compareTo.NOKName) { return false; }
            if (orig.NOKRelation != compareTo.NOKRelation) { return false; }
            if (orig.NOKPhone != compareTo.NOKPhone) { return false; }
            if (orig.D4HStatus != compareTo.D4HStatus) { return false; }
            if (orig.QualificationList != compareTo.QualificationList) { return false; }
            return true;
        }







        public static string getBCSARAQualificationName(this Qualification q)
        {
            switch (q.Code)
            {
                case "GSAR": return "Ground Search and Rescue (GSAR) [Search]";
                case "GSARTL": return "Ground Search Team Leader (GSTL) [Search]";
                case "SARM1": return "Search and Rescue Manager 1 (post Nov 2014) [Search]";
                case "SARM2": return "Search and Rescue Manager 2 [Search]";
                case "FA7h": return "07+ Hour First Aid Training or Equivalent [Medical]";
                case "FA40h": return "32-50+ Hour First Aid Training or Equivalent [Medical]";
                case "FA90h": return "70+ Hour First Aid Training or Equivalent [Medical]";
                case "PARAM": return "Licensed Paramedic (annual licensing body continuing competency requirements) [Medical]";
                case "ALS": return string.Empty;
                case "FR": return string.Empty;
                case "SWO": return "Swiftwater Operations (6h annual practice + 3y re-certification) [Swiftwater]";
                case "SWTL": return "Swiftwater Rescue Technician (20h annual practice + 3y re-certification) [Swiftwater]";
                case "SWA": return "Swiftwater Rescue Technician - Advanced (20h annual practice + 3y re-certification) [Swiftwater]";
                case "RR1": return "Rope Rescue Technician 1 (20h annual practice) [Rope Rescue]";
                case "RR2": return "Rope Rescue Technician 2 (20h annual practice) [Rope Rescue]";
                case "RRTL": return "Rope Rescue Team Leader (20h annual practice) [Rope Rescue]";
                case "OARTM": return "Organized Avalanche Response Team Member [Avalanche and Ice]";
                case "OARTL": return "Organized Avalanche Response Team Leader [Avalanche and Ice]";
                case "TA": return "BCTA - 1 - Track Aware [Tracking]";
                case "TK": return "BCTA - 2 - Tracker [Tracking]";
                case "AT": return "BCTA - 3 - Advanced Tracker [Tracking]";
                case "MR1": return "Mountain Rescue 1 [Mountain Rescue]";
                case "MR2": return "Mountain Rescue 2 [Mountain Rescue]";
                case "MR3": return "Mountain Rescue 3 [Mountain Rescue]";
                case "ORV": return "Off Road Vehicle (ORV) Operations [Vehicles]";
                case "MARINE": return "Small Vessel Operator Proficiency [Boat]";
                case "CDFL": return "HOTP Level 3 - CDFL (annual re-certification) [Aircraft]";
                case "K9": return "K-9 Wilderness Certification [Dog Handler]";
                case "UAV": return "UAV/Drone Operator";
                case "COM": return "Radio Communications Operator";



            }
            return string.Empty;
        }

        public static List<Qualification> GetQualifications()
        {
            List<Qualification> qualifications = new List<Qualification>();
            qualifications.Add(new Qualification("GSAR", "Ground Search and Rescue", 0, 0, 0, ""));
            qualifications.Add(new Qualification("GSARTL", "GSAR Team Leader", 1, 0, 1, "GSTL"));
            qualifications.Add(new Qualification("SARM1", "Search Manager 1", 2, 0, 2, "SARM"));
            qualifications.Add(new Qualification("SARM2", "Search Manager 2", 3, 0, 8, "SARM"));
            qualifications.Add(new Qualification("FA7h", "First Aid (7+ hrs)", 4, 0, 3, ""));
            qualifications.Add(new Qualification("FA40h", "First Aid (40+ hrs)", 5, 0, 9, ""));
            qualifications.Add(new Qualification("FA90h", "First Aid (90+ hrs)", 6, 0, 10, "OFA3"));
            qualifications.Add(new Qualification("PARAM", "Paramedic", 7, 0, 11, "OFA3"));
            qualifications.Add(new Qualification("ALS", "ALS", 8, 0, 12, "OFA3"));
            qualifications.Add(new Qualification("FR", "First Responder", 9, 0, 13, "OFA3"));
            qualifications.Add(new Qualification("SWO", "Swift Water Operations", 10, 0, 14, "SSO"));
            qualifications.Add(new Qualification("SWTL", "Swift Water Tech", 11, 0, 6, "SRT"));
            qualifications.Add(new Qualification("SWA", "Swift Water Advanced", 12, 0, 15, "SRT"));
            qualifications.Add(new Qualification("RR1", "Rope Tech 1", 13, 0, 4, "RRTM"));
            qualifications.Add(new Qualification("RR2", "Rope Tech 2", 14, 0, 16, "RRTM"));
            qualifications.Add(new Qualification("RRTL", "Rope Team Leader", 15, 0, 17, "RRTL"));
            qualifications.Add(new Qualification("OARTM", "Avalanche Team Member", 16, 0, 18, "OAR"));
            qualifications.Add(new Qualification("OARTL", "Avalanche Team Leader", 17, 0, 19, "OARTL"));
            qualifications.Add(new Qualification("TA", "Track Aware", 18, 0, 20, "TA"));
            qualifications.Add(new Qualification("TK", "Tracker", 19, 0, 5, "TK"));
            qualifications.Add(new Qualification("AT", "Advanced Tracker", 20, 0, 21, "AT"));
            qualifications.Add(new Qualification("MR1", "Mountain Rescue 1", 21, 0, 7, "MR"));
            qualifications.Add(new Qualification("MR2", "Mountain Rescue 2", 22, 0, 22, "MR2"));
            qualifications.Add(new Qualification("MR3", "Mountain Rescue 3", 23, 0, 23, "MR3"));
            qualifications.Add(new Qualification("ORV", "Off Road Vehicle", 24, 0, 24, ""));
            qualifications.Add(new Qualification("MARINE", "Marine / Boat Operator", 25, 0, 25, ""));
            qualifications.Add(new Qualification("CDFL", "Hoist / CDFL", 26, 0, 26, ""));
            qualifications.Add(new Qualification("K9", "K-9 Search", 27, 0, 27, ""));
            qualifications.Add(new Qualification("UAV", "UAV/Drone Operator", 28, 0, 28, ""));
            qualifications.Add(new Qualification("COM", "Radio Communications", 29, 0, 29, ""));


            return qualifications;
        }

        public static void SetQualificationByCode(this Personnel p, string code, bool value)
        {
            int index = GetQualificationIndexByCode(code);
            if (index >= 0) { p.QualificationList[index] = value; }
        }
        public static int GetQualificationIndexByCode(string code)
        {
            List<Qualification> qualifications = GetQualifications();
            if (qualifications.Any(o => o.Code.Equals(code)))
            {
                return qualifications.First(o => o.Code.Equals(code)).QualificationListIndex;

            }
            return -1;
        }
        public static Qualification? GetQualificationByCode(string code)
        {
            List<Qualification> qualifications = GetQualifications();
            if (qualifications.Any(o => o.Code.Equals(code)))
            {
                return qualifications.First(o => o.Code.Equals(code));

            }
            return null;
        }



        public static string ExportPersonnelToCSV(List<Personnel> personnel, string delimiter = ",")
        {
            List<Qualification> qualifications = GetQualifications();

            StringBuilder csv = new StringBuilder();
            csv.Append("Name"); csv.Append(delimiter);
            csv.Append("Pronouns"); csv.Append(delimiter);
            csv.Append("Group"); csv.Append(delimiter);
            csv.Append("Callsign"); csv.Append(delimiter);
            csv.Append("Phone"); csv.Append(delimiter);
            csv.Append("Email"); csv.Append(delimiter);

            csv.Append("Address"); csv.Append(delimiter);

            csv.Append("SpecialSkills"); csv.Append(delimiter);

            csv.Append("Reference"); csv.Append(delimiter);



            csv.Append("NOKName"); csv.Append(delimiter);
            csv.Append("NOKRelation"); csv.Append(delimiter);
            csv.Append("NOKPhone"); csv.Append(delimiter);
            csv.Append("Other Dietary"); csv.Append(delimiter);
            csv.Append("Vegetarian"); csv.Append(delimiter);
            csv.Append("NoGluten"); csv.Append(delimiter);
            // csv.Append("Phone"); csv.Append(delimiter);
            csv.Append("Pronouns"); csv.Append(delimiter);


            foreach (Qualification q in qualifications)
            {
                csv.Append(q.FullName); csv.Append(delimiter);
            }

            csv.Append(Environment.NewLine);

            foreach (Personnel item in personnel)
            {

                csv.Append("\""); csv.Append(item.Name.EscapeQuotes()); csv.Append("\""); csv.Append(delimiter);
                csv.Append("\""); csv.Append(item.Pronouns.EscapeQuotes()); csv.Append("\""); csv.Append(delimiter);
                csv.Append("\""); csv.Append(item.Group.EscapeQuotes()); csv.Append("\""); csv.Append(delimiter);
                csv.Append("\""); csv.Append(item.Callsign.EscapeQuotes()); csv.Append("\""); csv.Append(delimiter);
                csv.Append("\""); csv.Append(item.Phone.EscapeQuotes()); csv.Append("\""); csv.Append(delimiter);
                csv.Append("\""); csv.Append(item.Email.EscapeQuotes()); csv.Append("\""); csv.Append(delimiter);
                csv.Append("\""); csv.Append(item.Address.EscapeQuotes()); csv.Append("\""); csv.Append(delimiter);
                csv.Append("\""); csv.Append(item.SpecialSkills.EscapeQuotes()); csv.Append("\""); csv.Append(delimiter);
                csv.Append("\""); csv.Append(item.Reference.EscapeQuotes()); csv.Append("\""); csv.Append(delimiter);
                csv.Append("\""); csv.Append(item.NOKName.EscapeQuotes()); csv.Append("\""); csv.Append(delimiter);
                csv.Append("\""); csv.Append(item.NOKRelation.EscapeQuotes()); csv.Append("\""); csv.Append(delimiter);
                csv.Append("\""); csv.Append(item.NOKPhone.EscapeQuotes()); csv.Append("\""); csv.Append(delimiter);

                csv.Append("\""); csv.Append(item.Dietary.EscapeQuotes()); csv.Append("\""); csv.Append(delimiter);
                csv.Append("\""); if (item.Vegetarian) { csv.Append("YES"); } else { csv.Append("NO"); }
                csv.Append("\""); csv.Append(delimiter);
                csv.Append("\""); if (item.NoGluten) { csv.Append("YES"); } else { csv.Append("NO"); }
                csv.Append("\""); csv.Append(delimiter);

                foreach (Qualification q in qualifications)
                {
                    csv.Append("\"");
                    if (item.QualificationList != null && item.QualificationList.Length >= q.QualificationListIndex)
                    {
                        if (item.QualificationList[q.QualificationListIndex]) { csv.Append("YES"); } else { csv.Append("NO"); }

                    }
                    csv.Append("\""); csv.Append(delimiter);
                }

                csv.Append("\""); csv.Append(item.Pronouns.EscapeQuotes()); csv.Append("\""); csv.Append(delimiter);

                csv.Append(Environment.NewLine);
            }

            return csv.ToString();
        }





        public static Personnel? getMemberFromSimplifiedQR(string qr)
        {
            if (!string.IsNullOrEmpty(qr))
            {
                Personnel member = new Personnel();
                string[] bits = qr.Split(';');
                if (bits.Length > 0)
                {
                    // member.PersonID = new Guid(bits[0]);
                    member.Name = bits[0];
                    try
                    {
                        string shortOrgID = bits[1];
                        List<Organization> allOrgs = OrganizationTools.GetStaticOrganizations(Guid.Empty);// new Organization().getStaticOrganizationList();
                        if (allOrgs.Any(o => o.ShortOrganizationID == shortOrgID))
                        {
                            Organization org = allOrgs.First(o => o.ShortOrganizationID == shortOrgID);
                            member.OrganizationID = org.OrganizationID;
                            member.Group = org.OrganizationName;
                        }
                    }
                    catch (Exception) { }

                    member.Address = bits[2];
                    member.Phone = bits[3];
                    member.Email = bits[4];

                    //quals are 7
                    if (bits[5].Length >= 1)
                    {
                        int qualifications = 0;
                        if (int.TryParse(bits[5], out qualifications))
                        {
                            string binary = Convert.ToString(qualifications, 2);
                            while (binary.Length < 8)
                            {
                                binary = "0" + binary;
                            }

                            member.SetQualificationByCode("GSAR", binary.Substring(0, 1).Equals("1"));
                            member.SetQualificationByCode("GSARTL", binary.Substring(1, 1).Equals("1"));
                            member.SetQualificationByCode("SARM1", binary.Substring(2, 1).Equals("1"));
                            member.SetQualificationByCode("FA7h", binary.Substring(3, 1).Equals("1"));
                            member.SetQualificationByCode("RR1", binary.Substring(4, 1).Equals("1"));
                            member.SetQualificationByCode("TK", binary.Substring(5, 1).Equals("1"));
                            if (binary.Length > 6)
                            {
                                member.SetQualificationByCode("SWT", binary.Substring(6, 1).Equals("1"));
                            }
                            if (binary.Length > 7)
                            {
                                member.SetQualificationByCode("MR1", binary.Substring(7, 1).Equals("1"));
                            }

                        }


                    }
                    member.NOKName = bits[6];
                    member.NOKPhone = bits[7];
                }

                return member;
            }
            else { return null; }
        }




        private static bool YesOrFuture(string fieldText)
        {

            if (string.IsNullOrEmpty(fieldText)) { return false; }
            else if (fieldText.Equals("Yes")) { return true; }
            else
            {
                DateTime test = DateTime.MinValue;
                if (DateTime.TryParse(fieldText, out test))
                {
                    if (test > DateTime.Now) { return true; }
                    else { return false; }
                }
                else { return false; }
            }
        }



    }
}

