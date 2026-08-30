

using System.Drawing;
using System.Text;

namespace MySarAssistModels.RADeMS
{
    public static class RADeMSTools
    {
        public static Color GetGradientColor(int[] ScoreValue)
        {

            if (ScoreValue[0] == 0 && ScoreValue[1] == 0) { return Color.FromArgb(14, 173, 105); }

            if (Math.Max(ScoreValue[0], ScoreValue[1]) == 1 && Math.Min(ScoreValue[0], ScoreValue[1]) == 0) { return Color.FromArgb(0, 166, 81); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 2 && Math.Min(ScoreValue[0], ScoreValue[1]) == 0) { return Color.FromArgb(0, 171, 80); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 3 && Math.Min(ScoreValue[0], ScoreValue[1]) == 0) { return Color.FromArgb(75, 184, 77); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 4 && Math.Min(ScoreValue[0], ScoreValue[1]) == 0) { return Color.FromArgb(139, 198, 72); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 5 && Math.Min(ScoreValue[0], ScoreValue[1]) == 0) { return Color.FromArgb(187, 214, 66); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 6 && Math.Min(ScoreValue[0], ScoreValue[1]) == 0) { return Color.FromArgb(225, 228, 57); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 7 && Math.Min(ScoreValue[0], ScoreValue[1]) == 0) { return Color.FromArgb(245, 232, 50); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 8 && Math.Min(ScoreValue[0], ScoreValue[1]) == 0) { return Color.FromArgb(246, 199, 48); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 9 && Math.Min(ScoreValue[0], ScoreValue[1]) == 0) { return Color.FromArgb(244, 167, 45); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 10 && Math.Min(ScoreValue[0], ScoreValue[1]) == 0) { return Color.FromArgb(241, 139, 42); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 1 && Math.Min(ScoreValue[0], ScoreValue[1]) == 1) { return Color.FromArgb(0, 170, 80); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 2 && Math.Min(ScoreValue[0], ScoreValue[1]) == 1) { return Color.FromArgb(0, 170, 80); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 3 && Math.Min(ScoreValue[0], ScoreValue[1]) == 1) { return Color.FromArgb(127, 195, 74); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 4 && Math.Min(ScoreValue[0], ScoreValue[1]) == 1) { return Color.FromArgb(173, 209, 69); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 5 && Math.Min(ScoreValue[0], ScoreValue[1]) == 1) { return Color.FromArgb(210, 223, 61); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 6 && Math.Min(ScoreValue[0], ScoreValue[1]) == 1) { return Color.FromArgb(240, 235, 51); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 7 && Math.Min(ScoreValue[0], ScoreValue[1]) == 1) { return Color.FromArgb(245, 220, 49); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 8 && Math.Min(ScoreValue[0], ScoreValue[1]) == 1) { return Color.FromArgb(246, 186, 45); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 9 && Math.Min(ScoreValue[0], ScoreValue[1]) == 1) { return Color.FromArgb(244, 161, 45); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 10 && Math.Min(ScoreValue[0], ScoreValue[1]) == 1) { return Color.FromArgb(241, 136, 42); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 2 && Math.Min(ScoreValue[0], ScoreValue[1]) == 2) { return Color.FromArgb(133, 197, 73); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 3 && Math.Min(ScoreValue[0], ScoreValue[1]) == 2) { return Color.FromArgb(177, 210, 68); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 4 && Math.Min(ScoreValue[0], ScoreValue[1]) == 2) { return Color.FromArgb(214, 224, 59); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 5 && Math.Min(ScoreValue[0], ScoreValue[1]) == 2) { return Color.FromArgb(240, 235, 51); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 6 && Math.Min(ScoreValue[0], ScoreValue[1]) == 2) { return Color.FromArgb(246, 213, 48); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 7 && Math.Min(ScoreValue[0], ScoreValue[1]) == 2) { return Color.FromArgb(246, 185, 46); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 8 && Math.Min(ScoreValue[0], ScoreValue[1]) == 2) { return Color.FromArgb(243, 157, 44); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 9 && Math.Min(ScoreValue[0], ScoreValue[1]) == 2) { return Color.FromArgb(242, 135, 41); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 10 && Math.Min(ScoreValue[0], ScoreValue[1]) == 2) { return Color.FromArgb(240, 108, 39); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 3 && Math.Min(ScoreValue[0], ScoreValue[1]) == 3) { return Color.FromArgb(209, 222, 61); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 4 && Math.Min(ScoreValue[0], ScoreValue[1]) == 3) { return Color.FromArgb(242, 236, 49); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 5 && Math.Min(ScoreValue[0], ScoreValue[1]) == 3) { return Color.FromArgb(246, 216, 49); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 6 && Math.Min(ScoreValue[0], ScoreValue[1]) == 3) { return Color.FromArgb(245, 184, 46); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 7 && Math.Min(ScoreValue[0], ScoreValue[1]) == 3) { return Color.FromArgb(243, 159, 44); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 8 && Math.Min(ScoreValue[0], ScoreValue[1]) == 3) { return Color.FromArgb(241, 131, 41); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 9 && Math.Min(ScoreValue[0], ScoreValue[1]) == 3) { return Color.FromArgb(240, 109, 39); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 10 && Math.Min(ScoreValue[0], ScoreValue[1]) == 3) { return Color.FromArgb(239, 83, 37); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 4 && Math.Min(ScoreValue[0], ScoreValue[1]) == 4) { return Color.FromArgb(246, 211, 49); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 5 && Math.Min(ScoreValue[0], ScoreValue[1]) == 4) { return Color.FromArgb(245, 182, 47); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 6 && Math.Min(ScoreValue[0], ScoreValue[1]) == 4) { return Color.FromArgb(242, 151, 43); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 7 && Math.Min(ScoreValue[0], ScoreValue[1]) == 4) { return Color.FromArgb(241, 129, 41); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 8 && Math.Min(ScoreValue[0], ScoreValue[1]) == 4) { return Color.FromArgb(240, 106, 39); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 9 && Math.Min(ScoreValue[0], ScoreValue[1]) == 4) { return Color.FromArgb(238, 78, 38); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 10 && Math.Min(ScoreValue[0], ScoreValue[1]) == 4) { return Color.FromArgb(237, 49, 36); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 5 && Math.Min(ScoreValue[0], ScoreValue[1]) == 5) { return Color.FromArgb(242, 152, 43); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 6 && Math.Min(ScoreValue[0], ScoreValue[1]) == 5) { return Color.FromArgb(241, 125, 40); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 7 && Math.Min(ScoreValue[0], ScoreValue[1]) == 5) { return Color.FromArgb(239, 97, 38); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 8 && Math.Min(ScoreValue[0], ScoreValue[1]) == 5) { return Color.FromArgb(238, 67, 37); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 9 && Math.Min(ScoreValue[0], ScoreValue[1]) == 5) { return Color.FromArgb(237, 36, 36); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 10 && Math.Min(ScoreValue[0], ScoreValue[1]) == 5) { return Color.FromArgb(237, 29, 36); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 6 && Math.Min(ScoreValue[0], ScoreValue[1]) == 6) { return Color.FromArgb(239, 96, 38); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 7 && Math.Min(ScoreValue[0], ScoreValue[1]) == 6) { return Color.FromArgb(237, 70, 37); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 8 && Math.Min(ScoreValue[0], ScoreValue[1]) == 6) { return Color.FromArgb(237, 35, 36); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 9 && Math.Min(ScoreValue[0], ScoreValue[1]) == 6) { return Color.FromArgb(237, 29, 36); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 10 && Math.Min(ScoreValue[0], ScoreValue[1]) == 6) { return Color.FromArgb(237, 29, 36); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 7 && Math.Min(ScoreValue[0], ScoreValue[1]) == 7) { return Color.FromArgb(237, 39, 36); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 8 && Math.Min(ScoreValue[0], ScoreValue[1]) == 7) { return Color.FromArgb(237, 29, 36); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 9 && Math.Min(ScoreValue[0], ScoreValue[1]) == 7) { return Color.FromArgb(237, 29, 36); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 10 && Math.Min(ScoreValue[0], ScoreValue[1]) == 7) { return Color.FromArgb(237, 29, 36); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 8 && Math.Min(ScoreValue[0], ScoreValue[1]) == 8) { return Color.FromArgb(237, 29, 36); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 9 && Math.Min(ScoreValue[0], ScoreValue[1]) == 8) { return Color.FromArgb(237, 29, 36); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 10 && Math.Min(ScoreValue[0], ScoreValue[1]) == 8) { return Color.FromArgb(237, 29, 36); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 9 && Math.Min(ScoreValue[0], ScoreValue[1]) == 9) { return Color.FromArgb(237, 29, 36); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 10 && Math.Min(ScoreValue[0], ScoreValue[1]) == 9) { return Color.FromArgb(237, 29, 36); }
            else if (Math.Max(ScoreValue[0], ScoreValue[1]) == 10 && Math.Min(ScoreValue[0], ScoreValue[1]) == 10) { return Color.FromArgb(237, 29, 36); }
            return Color.FromArgb(14, 173, 105);
        }


        public static RADeMSScore? GetScoreFromShortCode(string shortCode)
        {
            if (shortCode == null) { return null; }
            string[] parts = shortCode.Split('-');

            RADeMSScore score = new RADeMSScore();
            if (parts.Length == 3)
            {
                int[] OpRisk = getScoreFromSequenceNumber(int.Parse(parts[0])) ?? new int[5];
                int[] RespCap = getScoreFromSequenceNumber(int.Parse(parts[2])) ?? new int[5];

                score.CategoryID = GetRademsCategoryIDFromShortCode(parts[1]);
                for (int x = 0; x < 5; x++) { score.Scores[x] = OpRisk[x]; }
                for (int x = 5; x < 10; x++) { score.Scores[x] = RespCap[x - 5]; }
            }

            return score;
        }

        public static string GetScoreShortCode(RADeMSScore score)
        {
            StringBuilder sb = new StringBuilder();


            sb.Append(getScoreSequenceNumber(score.OpRiskScores)); sb.Append("-");
            sb.Append(GetRademsCategoryShortCode(score.CategoryID)); sb.Append("-");
            sb.Append(getScoreSequenceNumber(score.RespCapScores));
            return sb.ToString();
        }

        private static string GetRademsCategoryShortCode(int categoryID)
        {
            switch (categoryID)
            {
                case 1: return "Alpha";
                case 2: return "Bravo";
                case 3: return "Charlie";
                case 4: return "Delta";
                case 5: return "Echo";
                case 6: return "Foxtrot";
                default: return string.Empty;
            }
        }
        private static int GetRademsCategoryIDFromShortCode(string shortCode)
        {
            switch (shortCode)
            {
                case "Alpha": return 1;
                case "Bravo": return 2;
                case "Charlie": return 3;
                case "Delta": return 4;
                case "Echo": return 5;
                case "Foxtrot": return 6;
                default: return 0;
            }
        }


        private static int[]? getScoreFromSequenceNumber(int sequenceNumber)
        {
            int[,] sequence = GetAllScoresInSequence();

            if (sequenceNumber < sequence.Length)
            {
                int[] score = new int[5];
                score[0] = sequence[sequenceNumber, 0];
                score[1] = sequence[sequenceNumber, 1];
                score[2] = sequence[sequenceNumber, 2];
                score[3] = sequence[sequenceNumber, 3];
                score[4] = sequence[sequenceNumber, 4];
                return score;
            }
            return null;
        }

        private static string getScoreSequenceNumber(int[] scores)
        {
            int[,] sequence = GetAllScoresInSequence();

            for (int i = 0; i < sequence.Length; i++)
            {
                if (sequence[i, 0] == scores[0] && sequence[i, 1] == scores[1] && sequence[i, 2] == scores[2] && sequence[i, 3] == scores[3] && sequence[i, 4] == scores[4])
                {
                    return i.ToString();
                }
            }
            return string.Empty;
        }

        private static int[,] GetAllScoresInSequence()
        {
            int[,] sequence = new int[243,5];

            int index = 0;

            for(int a = 0; a < 3; a++)
            {
                for (int b = 0; b < 3; b++)
                {
                    for (int c = 0; c < 3; c++)
                    {
                        for (int d = 0; d < 3; d++)
                        {
                            for (int e = 0; e < 3; e++)
                            {
                                sequence[index, 0] = a;
                                sequence[index, 1] = b;
                                sequence[index, 2] = c;
                                sequence[index, 3] = d;
                                sequence[index, 4] = e;
                                index++;
                            }
                        }
                    }
                }
            }



            return sequence;
        }



        public static List<RADeMSCategory> GetCategories()
        {
            List<RADeMSCategory> categories = new List<RADeMSCategory>();

            categories.Add(new RADeMSCategory(1, "Ground Search", GetQuestions(1)));
            categories.Add(new RADeMSCategory(2, "Human Disease Outbreak", GetQuestions(2)));
            categories.Add(new RADeMSCategory(3, "Motor Vehicle Operations", GetQuestions(3)));
            categories.Add(new RADeMSCategory(4, "Mountain Rescue", GetQuestions(4)));
            categories.Add(new RADeMSCategory(5, "Rope Rescue", GetQuestions(5)));
            categories.Add(new RADeMSCategory(6, "Swiftwater Rescue", GetQuestions(6)));


            return categories;
        }

        public static RADeMSCategory GetCategory(int id)
        {
            List<RADeMSCategory> categories = GetCategories();
            if (categories.Any(o => o.ID == id)) { return categories.First(o => o.ID == id); }
            else { return null; }
        }

        public static List<RADeMSQuestion>? GetQuestions(int CategoryID)
        {
            List<RADeMSQuestion> questions = new List<RADeMSQuestion>();
            switch (CategoryID)
            {
                case 1: //gsar
                    questions.Add(new RADeMSQuestion(1, 1, "Operational complexity", "How complex & complicated is the task", true, "Simple operation, requiring one or two single activities (e.g. Locate and escort to safety)~1-8 hour duration~Minor subject injuries", "More complex operation, requiring multiple activities (e.g. locate and evacuate using a litter) with some challenges both in access and egress~8-24 hour duration~Significant non-life-threatening injuries", "Very complex operation involving multiple teams, convergent volunteers~Difficult access and egress~Helicopter transport~Multi-day or multi-organization response~Life threatening injuries"));
                    questions.Add(new RADeMSQuestion(2, 1, "Activity hazards", "How high are the hazards in the activities", true, "Simple basic GSAR activities; orienteering, tracking and/or sweep searching~Simple terrain~Hazards are low probability and low severity", "Potential for several disciplines required to access and evacuate the subject~Challenging terrain~Hazard probability high but severity manageable", "Repsonse may require use of several technical disciplines; rope rescue, swiftwater and/or avalanche assessment~Difficult terrain~Hazards are high probability and high severity"));
                    questions.Add(new RADeMSQuestion(3, 1, "Environmental conditions", "How high are the environmental hazards", true, "Minimal hazards~Good weather, stable forecast, daylight, warm temperatures and/or good visibility~Low elevation", "Moderate hazards~Uncertain forecast, low light, freezing temperature, moderate precipitation and/or broken visibility", "Extreme hazards~Impending forecast, drastic weather system, darkness, cold temperatures, heavy precipitation, high wind and/or obscured visibility~High elevation"));
                    questions.Add(new RADeMSQuestion(4, 1, "Vulnerability", "How exposed and vulnerable are the team members", true, "Minimal exposure to personnel. Good searching terrain. <1 hour exposure. <3 searchers exposed. Full 2-way communications.", "Moderate exposure to personnel due to activity/environmental hazards and/or weather. Moderate terrain hazards. 1-4 hours exposure. 3-12 searchers exposed. Intermittent communications and/or relay available if required.", "High exposure due to extreme activity/environmental hazards and/or weather. Broken terrain, talus slopes and/or cliff bands. >6 hours exposure. >12 searchers exposed. Poor or no communications with field personnel."));
                    questions.Add(new RADeMSQuestion(5, 1, "External influence", "What is the level of pressure due to survivability, media, family, and/or other", true, "Little or no degree of urgency due to either confirmed survivability or confirmed deceased (recovery).~Little or no pressure from family, media or agency.", "Some degree of urgency due to subject survivability factors.~Local publicity.~Family on scene.~Agency and/or press asking questions.", "High degree of urgency due to critical subject survivability factors.~High profile subject.~Family on scene.~Agency, national media influence and/or political pressure to resolve."));
                    questions.Add(new RADeMSQuestion(6, 1, "Personnel Training", "What level of training do personnel have?", false, "Documented high training proficiency and practice in the use of PPE and other protective measures required under current directions", "Some training proficiency and practice in the use of PPE required under current directions", "Minimum instructions given on, and proficiency in, the use of PPE required under current directions "));
                    questions.Add(new RADeMSQuestion(7, 1, "Personnel Experience", "What level of experience do personnel have", false, "Extensive experience appropriate for the activities required.~Routinely respond to incident site/area.~>20 similar responses~Many years of recreational experience.~Professional experience.", "Some experience related to the activities required. ~Some previous responses to site/area. ~5-20 similar responses.~Significant recreational experience.", "Little or no experience related to the activities required. ~Completely unfamiliar with the location. ~<5 similar responses. ~Little or no recreational experience."));
                    questions.Add(new RADeMSQuestion(8, 1, "Personnel Mental & Physical Preparedness", "How mentally and physically prepared are personnel", false, "Personnel are in good spirits, well-rested and exhibit characteristics of a cohesive team.", "Personnel are generally positive although some are tiring.~Adequate physical condition.", "Personnel are negative and question decisions (see note on page 13).~Resources have been on task through several assignments and are showing signs of exhaustion.~Searchers have been involved for multi-operational periods.~Unfit/does not exercise on a regular basis."));
                    questions.Add(new RADeMSQuestion(9, 1, "Planning", "How much planning has there been", false, "Plans (and/or Pre-plans) are in place, including contingencies, all documented.", "Overall plan is in place, with some consultation.~Basic notes taken about the site.", "Basic discussions on overall strategies and tactics.~Nothing in writing for this site."));
                    questions.Add(new RADeMSQuestion(10, 1, "Resources", "What is the level of resources available", false, "Appropriate resources are in place to cover all anticipated eventualities, contingencies and multi-operational periods.", "Basic resources are in place to cover response.~Back-up is available, but not on site.", "Resources are barely adequate.~No back-up or contingency available."));
                    break;
                case 2: //covid
                    questions.Add(new RADeMSQuestion(1, 2, "Operational complexity", "How complex & complicated is the task", true, "Simple operation, few members required to safely respond while maintaining reasonable physical distancing at the Incident Command Post and in the field", "More complex operation, additional resources required to safely respond but physical distancing is possible for the majority of activities except for transportation", "Very complex operation requiring large numbers of responders with technical rescue techniques requiring close contact between members and/or the subject"));
                    questions.Add(new RADeMSQuestion(2, 2, "Activity hazards", "How high are the hazards in the activities", true, "Travel with as few in vehicles where possible, no aircraft transportation required or few in aircraft if needed, wide area search with physical distancing being maintained", "Travel in vehicle and/or aircraft at capacity due to limited resources, technical rescue techniques requiring additional personnel, subject may have been exposed to pathogen", "Subject(s) require assessment, treatment, and/or transportation; Subject is known to be infected with the pathogen "));
                    questions.Add(new RADeMSQuestion(3, 2, "Environmental conditions", "How high are the environmental hazards", true, "Terrain allows for physical distancing weather is conducive to wearing PPE (dry, not excessively humid).", "Terrain requires responders to operate in close quarters with some use of hand holds or rope assist, weather is conducive for wearing gloves but challenging for wearing masks or other PPE for extended time", "Terrain requires using common hand holds, rope assist or ladders, and/or operating in close quarters.~Weather is wet or humid potentially rendering PPE ineffective (eg masks becoming soaked) or high temperatures increase risk of hyperthermia if wearing PPE"));
                    questions.Add(new RADeMSQuestion(4, 2, "Vulnerability", "How exposed and vulnerable are the team members", true, "Subject is able to walk out on their own with guidance from members keeping physical distancing", "Subject requires assistance to walk out, no first aid required", "Subject is incapacitated, requires assessment, first aid, evacuation and transportation"));
                    questions.Add(new RADeMSQuestion(5, 2, "External influence", "What is the level of pressure due to survivability, media, family, and/or other", true, "Little or no degree of urgency due to either confirmed survivability or confirmed deceased (recovery).~Little pressure from family, media or agency.~Protective measures (PPE and physical distancing) are clear and achievable.", "Some degree of urgency due to subject survivability factors.~Local publicity.~Family on scene.~Agency and/or press asking questions.~Recommended protective measures somewhat clear, but achievable", "High degree of urgency due to critical subject survivability factors.~High profile subject.~Family on scene.~Agency, provincial/national media influence and/or political pressure to resolve.~Recommended protective measures not clear."));
                    questions.Add(new RADeMSQuestion(6, 2, "Personnel Training", "What level of training do personnel have?", false, "Documented high training proficiency and practice in the use of PPE and other protective measures required under current directions", "Some training proficiency and practice in the use of PPE required under current directions", "Minimum instructions given on, and proficiency in, the use of PPE required under current directions"));
                    questions.Add(new RADeMSQuestion(7, 2, "Personnel Experience", "What level of experience do personnel have", false, "Extensive experience in responding in a health threat environment", "Some experience in responding with others in a health threat environment and use of PPE", "No experience in responding with any aspect of a health threat environment or use of PPE"));
                    questions.Add(new RADeMSQuestion(8, 2, "Personnel Mental & Physical Preparedness", "How mentally and physically prepared are personnel", false, "Fit for service, not highly stressed over additional risk of contracting the pathogen above other hazards", "Fit for service, cautious over additional risk of contracting the pathogen, not sure if should be attending", "Not fit for service, overwhelmed by additional risk of contracting the pathogen or other factors"));
                    questions.Add(new RADeMSQuestion(9, 2, "Planning", "How much planning has there been", false, "Plans (and/or Pre-plans) are in place, including contingencies, all consistent with current protection requirements.", "Overall plan is in place, with consultation taking place over additional protective requirements", "Basic discussions on only overall strategies and tactics"));
                    questions.Add(new RADeMSQuestion(10, 2, "Resources", "What is the level of resources available", false, "Appropriate resources including required PPE are in place for known activities and any potential requirements as response develops", "Basic resources including required PPE are in place to cover known activities, with resources being available through mutual support if required during the response", "Resources including required PPE are barely adequate, will be overwhelmed if situation changes to any degree"));

                    break;
                case 3: //code 3
                    questions.Add(new RADeMSQuestion(1, 3, "Operational complexity", "How complex & complicated is the task", true, "Simple operation, driving light vehicle (e.g.~pickup/small rescue truck) with one other member assisting with communications and navigating.", "More complex operation, driving command vehicle with extended or wide body, or other vehicle with more than 1 other member as passengers or pulling small trailer (eg with PWCs, ORVs).", "Very complex operation involving driving vehicle with large trailer, eg command or communications trailer."));
                    questions.Add(new RADeMSQuestion(2, 3, "Activity hazards", "How high are the hazards in the activities", true, "Traffic light, few intersections, good visibility (minor hills, curves, etc) along roadways, roads conducive to traffic moving out of way if needed", "Traffic at medium capacity, flowing well, visibility okay, roads conducive to traffic moving out of way in some sections.", "Traffic heavy, visibility low, difficult for traffic to move out of way."));
                    questions.Add(new RADeMSQuestion(3, 3, "Environmental conditions", "How high are the environmental hazards", true, "Good weather, daylight, dry conditions, temperature well above freezing", "Variable weather, low light, lower but not freezing temperature, light precipitation.", "Poor weather (snow/heavy rain), darkness, freezing temperatures, high wind and/or obscured visibility.."));
                    questions.Add(new RADeMSQuestion(4, 3, "Vulnerability", "How exposed and vulnerable are the team members", true, "Short distance to incident, eg less than 5 km", "Medium distance to incident, eg 5 km to 10 km", "Long distance to incident, eg over 10 km"));
                    questions.Add(new RADeMSQuestion(5, 3, "External influence", "What is the level of pressure due to survivability, media, family, and/or other", true, "Little or no degree of urgency due to either confirmed survivability or confirmed deceased (recovery).~Little or no pressure from family, media or agency.", "Some degree of urgency due to subject survivability factors.~Local publicity.~Family on scene.~Agency and/or press asking questions.", "High degree of urgency due to critical subject survivability factors.~High profile subject.~Family on scene.~Agency, national media influence and/or political pressure to resolve."));
                    questions.Add(new RADeMSQuestion(6, 3, "Personnel Training", "What level of training do personnel have?", false, "Documented high training proficiency above than required by legislation.~Routinely train on vehicle and roadway being used.", "Some documented training proficiency above requirements.~Some training and practicing on vehicle and road being used, or similar vehicle and roadways.", "Minimum training required by legislation, little or no experience on type of vehicle and roadways to be used."));
                    questions.Add(new RADeMSQuestion(7, 3, "Personnel Experience", "What level of experience do personnel have", false, "Extensive experience responding to incident site/area.~>20 similar responses.~Professional experience.", "Some previous responses to site/area.~5-20 similar responses.", "Unfamiliar with the location.~<5 similar responses."));
                    questions.Add(new RADeMSQuestion(8, 3, "Personnel Mental & Physical Preparedness", "How mentally and physically prepared are personnel", false, "Selected driver in good spirits, well-rested and focused on task", "Selected driver positive, coming off work or other activities.", "Approved drivers have been on task through several assignments or coming from strenuous/lengthy activities/work and are showing signs of exhaustion."));
                    questions.Add(new RADeMSQuestion(9, 3, "Planning", "How much planning has there been", false, "Plans (and/or Pre-plans) are in place, including contingencies, all documented.", "Overall plan is in place, with some consultation.", "Basic discussions on overall strategies and tactics.~Nothing in writing for this site."));
                    questions.Add(new RADeMSQuestion(10, 3, "Resources", "What is the level of resources available", false, "Appropriate resources are in place for other assignments letting approved driver focus on driving.", "Basic resources are in place to cover other assignments allowing approved driver to focus on driving.", "Resources are barely adequate, approved driver expected to manage other assignments"));

                    break;
                case 4: //mr
                    questions.Add(new RADeMSQuestion(1, 4, "Operational complexity", "How complex & complicated is the task", true, "Simple operation.~4th class terrain.~Trail approach.~Top-down access, requires only rappelling in.~Simple lower <50 meters in length.~Straight forward evacuation.~One subject.~Minor subject injuries.", "More complex operation.~5th class terrain.~Knot pass raise and/or lower <100 meters in length.~2 or more subjects with non-life-threatening injuries.", "Very complex operation.~Multiple teams.~Convergent volunteers.~Difficult access and egress.~Multi-pitch raise and/or lower >100 meters in length.~Complex evacuation.~Life-threatening injuries."));
                    questions.Add(new RADeMSQuestion(2, 4, "Activity hazards", "How high are the hazards in the activities", true, "Low hazard mountain rescue activities.~ Low angle alpine site.~No site hazards.~Simple terrain.~Hazards are low probability and low severity.", "Moderate hazard mountain rescue activities.~Glaciated alpine or steep alpine.~Challenging terrain.~Site hazards manageable.~Hazard probability high but severity manageable.", "High hazard mountain rescue.~Glaciated & steep alpine.~Avalanche, rock or icefall hazard likely, CDFL involved.~Complex terrain.~Site hazards not easily managed.~Hazards are high probability and high severity."));
                    questions.Add(new RADeMSQuestion(3, 4, "Environmental conditions", "How high are the environmental hazards", true, "Minimal hazards.~Good weather, stable forecast, daylight, warm temperatures and/or good visibility.~Low elevation.", "Moderate hazards.~Uncertain forecast, low light, freezing temperature, moderate precipitation and/or broken visibility.", "Extreme hazards.~Impending forecast, drastic weather system, darkness, cold temperatures, heavy precipitation, high wind and/or obscured visibility.~High elevation."));
                    questions.Add(new RADeMSQuestion(4, 4, "Vulnerability", "How exposed and vulnerable are the team members", true, "Minimal exposure to personnel.~Good terrain to work in.~No overhead hazards.~<1 hour exposure.~1-2 rescuers exposed.", "Moderate exposure to personnel due to terrain/activity/environmental hazards and/or weather.~Manageable overhead hazards.~Safe zones exist to protect rescuers.~1-4 hours exposure.~2-4 rescuers exposed.", "High exposure due to extreme terrain/activity/environmental hazards and/or weather.~High overhead hazards.~Rock fall likely.~No safe-zones to protect rescuers.~>4 hours exposure.~>4 rescuers exposed."));
                    questions.Add(new RADeMSQuestion(5, 4, "External influence", "What is the level of pressure due to survivability, media, family, and/or other", true, "Little or no degree of urgency due to either confirmed survivability or confirmed deceased (recovery).~Little or no pressure from family, media or agency.", "Some degree of urgency due to subject survivability factors.~Local publicity.~Family on scene.~Agency and/or press asking questions.", "High degree of urgency due to critical subject survivability factors.~High profile subject.~Family on scene.~Agency, national media influence and/or political pressure to resolve."));
                    questions.Add(new RADeMSQuestion(6, 4, "Personnel Training", "What level of training do personnel have?", false, "Documented training proficiency appropriate for the activities required.~Routinely train on incident site/area.~Professional level certification (ACMG Alpine Guide).~Instructor level mountain rescue certification.", "Some training proficiency related to the activities required.~Some training on incident site/area.~Mountain rescue training with team leader supervision.", "Little training proficiency related to the activities or location required.~No training on incident site/area.~No training in the terrain features.~Low mountain rescue certification.~New rescue team."));
                    questions.Add(new RADeMSQuestion(7, 4, "Personnel Experience", "What level of experience do personnel have", false, "Extensive experience appropriate for the activities required.~Routinely respond on incident site/area.~>20 similar responses.~Many years of recreational experience.~Professional experience.", "Some experience related to the activities required.~Some previous responses on incident site/area.~5-20 similar responses.~Significant recreational experience.", "Little or no experience related to the activities required.~Completely unfamiliar with the location.~<5 similar responses.~Little or no recreational experience."));
                    questions.Add(new RADeMSQuestion(8, 4, "Personnel Mental & Physical Preparedness", "How mentally and physically prepared are personnel", false, "Personnel are in good spirits, well-rested and exhibit characteristics of a cohesive team.", "Personnel are generally positive although some are tiring.~Adequate physical condition.", "Personnel are negative and question decisions (see note on page 13).~Resources have been on task through several assignments and are showing signs of exhaustion.~Responders have been involved for multi-operational periods.~Unfit/does not exercise on a regular basis."));
                    questions.Add(new RADeMSQuestion(9, 4, "Planning", "How much planning has there been", false, "Plans (and/or Pre-plans) are in place, including contingencies, all documented.", "Overall plan is in place, with some consultation.~Basic notes taken.", "Basic discussions on overall strategies and tactics.~Nothing in writing."));
                    questions.Add(new RADeMSQuestion(10, 4, "Resources", "What is the level of resources available", false, "Appropriate resources and team are in place to cover all anticipated eventualities, contingencies and technical requirements.", "Basic resources and team are in place to cover response.~Back-up is available, but not on site.", "Resources and team are barely adequate.~No back-up or contingency available."));

                    break;
                case 5: //rope
                    questions.Add(new RADeMSQuestion(1, 5, "Operational complexity", "How complex & complicated is the task", true, "Simple operation.~4th class terrain.~Trail approach.~Top-down access.~Requires only rappelling in.~Simple lower <50 meters in length.~Straight forward evacuation.~One subject.~Minor subject injuries.", "More complex operation.~5th class terrain.~Knot pass raise and/or lower <100 meters in length.~2 or more subjects with non-life-threatening injuries.", "Very complex operation.~Multiple teams.~Convergent volunteers.~Difficult access and egress.~Multi-pitch raise and/or lower >100 meters in length.~Complex evacuation.~Life-threatening injuries."));
                    questions.Add(new RADeMSQuestion(2, 5, "Activity hazards", "How high are the hazards in the activities", true, "Simple operation.~4th class terrain.~Trail approach.~Top-down access.~Requires only rappelling in.~Simple lower <50 meters in length.~Straight forward evacuation.~One subject.~Minor subject injuries.", "More complex operation.~5th class terrain.~Knot pass raise and/or lower <100 meters in length.~2 or more subjects with non-life-threatening injuries.", "Very complex operation.~Multiple teams.~Convergent volunteers.~Difficult access and egress.~Multi-pitch raise and/or lower >100 meters in length.~Complex evacuation.~Life-threatening injuries."));
                    questions.Add(new RADeMSQuestion(3, 5, "Environmental conditions", "How high are the environmental hazards", true, "Minimal hazards.~Good weather, stable forecast, daylight, warm temperatures and/or good visibility.~Low elevation.", "Moderate hazards.~Uncertain forecast, low light, freezing temperature, moderate precipitation and/or broken visibility.", "Extreme hazards.~Impending forecast, drastic weather system, darkness, cold temperatures, heavy precipitation, high wind and/or obscured visibility.~ High elevation."));
                    questions.Add(new RADeMSQuestion(4, 5, "Vulnerability", "How exposed and vulnerable are the team members", true, "Minimal exposure to personnel.~Good terrain to work in.~No overhead hazards.~<1 hour exposure.~1-2 rescuers exposed.", "Moderate exposure to personnel due to terrain/activity/environmental hazards and/or weather.~Manageable overhead hazards.~Safe zones exist to protect rescuers.~Short-term exposure to hazards.~1-4 hours exposure.~2-4 rescuers exposed.", "High exposure due to extreme terrain/activity/environmental hazards and/or weather.~High overhead hazards.~Rock fall likely.~No safe-zones to protect rescuers.~Long-term exposure to hazards.~>4 hours exposure.~>4 rescuers exposed."));
                    questions.Add(new RADeMSQuestion(5, 5, "External influence", "What is the level of pressure due to survivability, media, family, and/or other", true, "Little or no degree of urgency due to either confirmed survivability or confirmed deceased (recovery).~Little or no pressure from family, media or agency.", "Some degree of urgency due to subject survivability factors.~Local publicity.~Family on scene.~Agency and/or press asking questions.", "High degree of urgency due to critical subject survivability factors.~High profile subject.~Family on scene.~Agency, national media influence and/or political pressure to resolve."));
                    questions.Add(new RADeMSQuestion(6, 5, "Personnel Training", "What level of training do personnel have?", false, "Documented training proficiency appropriate for the activities required.~Routinely train on incident site.~Rope rescue team member training with team leader supervision.", "Some training proficiency related to the activities required.~Some training on incident site.~Rope rescue team member training with team leader supervision.", "Little training proficiency related to the activities or location required.~No training on incident site.~No training in the terrain features.~Low rope rescue certification.~New rescue team."));
                    questions.Add(new RADeMSQuestion(7, 5, "Personnel Experience", "What level of experience do personnel have", false, "Extensive experience appropriate for the activities required.~Routinely respond to incident site.~>20 similar responses.~Many years of recreational experience.~Professional experience.", "Some experience related to the activities required.~Some previous responses to incident site.~5-20 similar responses.~Significant recreational experience.", "Little or no experience related to the activities required.~Completely unfamiliar with the location.~<5 similar responses.~Little or no recreational experience."));
                    questions.Add(new RADeMSQuestion(8, 5, "Personnel Mental & Physical Preparedness", "How mentally and physically prepared are personnel", false, "Personnel are in good spirits, well-rested and exhibit characteristics of a cohesive team.~Excellent personal fitness.", "Personnel are generally positive although some are tiring.~Adequate physical condition.", "Personnel are negative and question decisions (see note on page 13).~Resources have been on task through several assignments and are showing signs of exhaustion.~Responders have been involved for multi-operational periods.~Unfit/does not exercise on a regular basis."));
                    questions.Add(new RADeMSQuestion(9, 5, "Planning", "How much planning has there been", false, "Plans (and/or Pre-plans) are in place, including contingencies, all documented.", "Overall plan is in place, with some consultation.~Basic notes taken about the site.", "Basic discussions on overall strategies and tactics.~Nothing in writing for this site."));
                    questions.Add(new RADeMSQuestion(10, 5, "Resources", "What is the level of resources available", false, "Appropriate resources are in place to cover all anticipated eventualities, contingencies and technical requirements.", "Basic resources are in place to cover response.~Back-up is available, but not on site.", "Resources are barely adequate.~No back-up or contingency available."));

                    break;
                case 6: //sw
                    questions.Add(new RADeMSQuestion(1, 6, "Operational complexity", "How complex & complicated is the task", true, "Simple operation.~Single tasks.~One subject.~Minor injuries.~Rescue from shore is possible.", "More complex operation.~Multiple single tasks.~Significant non-life-threatening injuries.~Rescuers in the water without anchored rope systems involved.~", "Very complex operation.~Compound tasks.~Life-threatening injuries.~Rescue includes anchored rope systems.~Complex mechanical advantage required."));
                    questions.Add(new RADeMSQuestion(2, 6, "Activity hazards", "How high are the hazards in the activities", true, "Low hazard swiftwater rescue activities.~Shore-based response in low risk environment.~Little or no direct contact with subject.~Indirect contact from shore.~Little to no site hazards.~Hazards are low probability and low severity.", "Moderate hazard swiftwater rescue activities.~Shore or water-based.~Simple and compound rope access.~Tethered swimmer in low-moderate risk swiftwater environments.~Site hazards manageable.~Hazard probability high but severity manageable.", "High hazard swiftwater rescue.~Shore and swim based.~Technical, horizontal aquatic rope access.~Complex rope, advanced rescue protocols.~Rescuers in the water.~Site hazards not easily managed.~Hazards are high probability and high severity."));
                    questions.Add(new RADeMSQuestion(3, 6, "Environmental conditions", "How high are the environmental hazards", true, "Minimal hazards.~Class 1.~Simple waves.~Pool drop.~Velocity <5 km/hr.~Drop <5m/km.~Sand/gravel bottom.~Water temp >20 C.~Water clarity > 2m.~Free flow hydraulics.~No hard hazards.~No entrapment potential.~Clear load.~Stable weather forecast.", "Moderate hazards.~Class 2-3.~Compound waves to 1m.~Staggered rapids.~Velocity 5-12 km/hr.~Drop 6-10m/km.~Boulder bottom.~Water temp 10-20 C.~Water clarity 0.3-1 m.~Potential holding hydraulics.~Some hard hazards.~Surface & suspended load.~Uncertain weather forecast, low light, freezing temperature.", "Extreme hazards.~Site hazards not easily managed.~Class 4+.~Complex waves 2m.~Continuous rapids.~Velocity 10 km+.~Boulder & ledge bottom.~Water temp <10 C.~Water clarity <0.3m.~Entrapment hydraulics.~Frequent hard hazards.~Surface/suspended & bottom load.~Impending weather forecast, darkness, cold temperatures, heavy precipitation, high wind."));
                    questions.Add(new RADeMSQuestion(4, 6, "Vulnerability", "How exposed and vulnerable are the team members", true, "Minimal exposure to personnel.~Good terrain to work in.~No downstream hazards.~No rescuers exposed to hazards.", "Moderate exposure to personnel due to terrain/activity/environmental hazards and/or weather.~Downstream hazards exist but are manageable.~Short-term exposure to hazards.~1 rescuer exposed to hazards.", "High exposure due to extreme terrain/activity/environmental hazards and/or weather.~Dangerous downstream hazards exist.~Medium to long-term exposure to hazards.~>1 rescuer exposed to hazards."));
                    questions.Add(new RADeMSQuestion(5, 6, "External influence", "What is the level of pressure due to survivability, media, family, and/or other", true, "Little or no degree of urgency due to either confirmed survivability or confirmed deceased (recovery).~Little or no pressure from family, media or agency.", "Some degree of urgency due to subject survivability factors.~Local publicity.~Family on scene.~Agency and/or press asking questions.", "High degree of urgency due to critical subject survivability factors.~High profile subject.~Family on scene.~Agency, national media influence and/or political pressure to resolve."));
                    questions.Add(new RADeMSQuestion(6, 6, "Personnel Training", "What level of training do personnel have?", false, "Documented training proficiency appropriate for the activities required.~Skill and knowledge to perform multiple complex specialized rescue tasks in high-risk swiftwater.~Routinely train on incident site/area.~Professional level rescuer certification.", "Some training proficiency related to the activities required.~Skill and knowledge to perform compound tasks in moderate swiftwater environments.~Some training on incident site/area.~All SRT certified rescuers.", "Little training proficiency related to the activities or location required.~Survival, awareness, or basic level swiftwater training only.~No training and/or experience on incident site/area.~Low SRT certification.~New SRT team."));
                    questions.Add(new RADeMSQuestion(7, 6, "Personnel Experience", "What level of experience do personnel have", false, "Extensive experience related to the activities required.~Routinely respond on incident site/area.~>20 similar responses.~Many years of recreational experience.~Professional experience.", "Some experience related to the activities required.~Some previous response on incident site/area.~5-20 similar responses.~Significant recreational experience.", "Little or no experience related to the activities required.~Completely unfamiliar with the location.~<5 similar responses.~Little or no recreational experience."));
                    questions.Add(new RADeMSQuestion(8, 6, "Personnel Mental & Physical Preparedness", "How mentally and physically prepared are personnel", false, "Personnel are in good spirits, well-rested and exhibit characteristics of a cohesive team, excellent physical fitness.", "Personnel are generally positive although some are tiring.~Adequate physical condition.", "Personnel are negative and question decisions (see note on page 13).~Resources have been on task through several assignments and are showing signs of exhaustion.~Responders have been involved for multi-operational periods.~Unfit/does not exercise on a regular basis."));
                    questions.Add(new RADeMSQuestion(9, 6, "Planning", "How much planning has there been", false, "Plans (and/or Pre-plans) are in place, including contingencies, all documented.", "Overall plan is in place, with some consultation.~Basic notes taken.", "Basic discussions on overall strategies and tactics.~Nothing in writing."));
                    questions.Add(new RADeMSQuestion(10, 6, "Resources", "What is the level of resources available", false, "Appropriate resources and team are in place to cover all anticipated eventualities, contingencies and technical requirements.", "Basic resources and team are in place to cover response.~Back-up is available, but not on site.", "Resources and team are barely adequate.~No back-up or contingency available."));

                    break;

                default:
                    return null;
            }
            return questions;
        }
    }
}
