using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySarAssistModels.Assignments;
using MySarAssistModels.Clues;
using MySarAssistModels.RADeMS;
using System;
using System.Collections.Generic;

namespace MySarAssistUnitTests
{
    [TestClass]
    public class DebriefSerializationTests
    {
        [TestMethod]
        public void TestRoundTripSerialization()
        {
            var taskGuid = Guid.NewGuid();
            var asgGuid = Guid.NewGuid();

            var debrief = new AssignmentDebrief
            {
                TaskID = taskGuid,
                AssignmentID = asgGuid,
                AssignmentNumber = 4,
                TeamName = "Team 4",
                WhatYourTeamDid = "Grid search of Sector B. Encountered heavy brush.",
                Gaps = "Steep ravine between waypoints 3 and 4 was not searched.",
                Hazards = "Active wasp nest at waypoint 2; loose scree.",
                Comments = "Searchers exhausted; need hydration for next task.",
                DebriefPOD = 65.5,
                DebriefSpacing = 5.0,
                DebriefTravelSpeedPerHour = 1.2,
                DebriefRangeOfDetection = 10.0,
                TracksProvided = true,
                Deviation = true,
                Weather = "Light rain, 12C, decreasing visibility",
                IsComplete = true,
                IsExported = false
            };

            var clue = new Clue
            {
                TaskID = taskGuid,
                AssignmentID = asgGuid,
                ClueNumber = 1,
                Description = "Blue ballcap found near drainage",
                Latitude = 49.2827,
                Longitude = -123.1207,
                LocationFound = "Waypoint 3",
                DateFound = DateTime.UtcNow,
                RecordedBy = "GSTL Smith"
            };

            var radems = new RADeMSScore
            {
                CategoryID = 2,
                Comment = "Fatigued searchers after 4 hours in heavy brush"
            };

            var package = new AssignmentDebriefPackage(debrief, new[] { clue }, new[] { radems }, "1.2.0");

            string json = AssignmentDebriefPackageTools.ToJson(package, indented: true);
            Assert.IsFalse(string.IsNullOrWhiteSpace(json));

            var restored = AssignmentDebriefPackageTools.FromJson(json);
            Assert.IsNotNull(restored);
            Assert.AreEqual("1.0", restored.FormatVersion);
            Assert.AreEqual("MySAR Assist", restored.Generator);
            Assert.AreEqual("1.2.0", restored.AppVersion);
            Assert.AreEqual(taskGuid, restored.TaskID);
            Assert.AreEqual(asgGuid, restored.AssignmentID);
            Assert.AreEqual(4, restored.AssignmentNumber);
            Assert.AreEqual("Team 4", restored.TeamName);

            Assert.IsNotNull(restored.Debrief);
            Assert.AreEqual("Grid search of Sector B. Encountered heavy brush.", restored.Debrief.WhatYourTeamDid);
            Assert.AreEqual(65.5, restored.Debrief.DebriefPOD);
            Assert.IsTrue(restored.Debrief.TracksProvided);
            Assert.IsTrue(restored.Debrief.IsComplete);

            Assert.AreEqual(1, restored.Clues.Count);
            Assert.AreEqual("Blue ballcap found near drainage", restored.Clues[0].Description);
            Assert.AreEqual(49.2827, restored.Clues[0].Latitude);

            Assert.AreEqual(1, restored.RademsScores.Count);
            Assert.AreEqual(2, restored.RademsScores[0].CategoryID);
        }

        [TestMethod]
        public void TestStripPhotos()
        {
            var debrief = new AssignmentDebrief
            {
                AssignmentNumber = 2,
                TeamName = "Team 2",
                TrackImageBytes = "LARGE_BASE64_TRACK_IMAGE_DATA_SAMPLE"
            };

            var clueWithPhoto = new Clue
            {
                ClueNumber = 1,
                Description = "Shoeprint",
                ImageBytes = "LARGE_BASE64_CLUE_PHOTO_DATA_SAMPLE"
            };

            var originalPackage = new AssignmentDebriefPackage(debrief, new[] { clueWithPhoto });

            var strippedPackage = AssignmentDebriefPackageTools.StripPhotos(originalPackage);

            // Stripped copy must have null photo bytes
            Assert.IsNotNull(strippedPackage.Debrief);
            Assert.IsNull(strippedPackage.Debrief.TrackImageBytes);
            Assert.AreEqual(1, strippedPackage.Clues.Count);
            Assert.IsNull(strippedPackage.Clues[0].ImageBytes);

            // Original package must NOT be mutated
            Assert.IsNotNull(originalPackage.Debrief?.TrackImageBytes);
            Assert.IsNotNull(originalPackage.Clues[0].ImageBytes);
        }

        [TestMethod]
        public void TestQrCodeThresholdEvaluation()
        {
            var debrief = new AssignmentDebrief
            {
                AssignmentNumber = 1,
                TeamName = "Alpha 1",
                WhatYourTeamDid = "Completed trail sweep of North ridge.",
                Comments = "All clear."
            };

            var textOnlyPackage = new AssignmentDebriefPackage(debrief);

            // Text only debrief is well under 2048 bytes
            Assert.IsTrue(AssignmentDebriefPackageTools.CanFitInQrCode(textOnlyPackage, 2048));
            int byteSize = AssignmentDebriefPackageTools.EstimateByteSize(textOnlyPackage);
            Assert.IsTrue(byteSize < 1500, $"Expected byte size < 1500, got {byteSize}");

            // Add simulated photo (3KB of data)
            debrief.TrackImageBytes = new string('A', 3000);
            var heavyPackage = new AssignmentDebriefPackage(debrief);

            // Should exceed QR code capacity
            Assert.IsFalse(AssignmentDebriefPackageTools.CanFitInQrCode(heavyPackage, 2048));

            // Stripping photos should bring it back within QR threshold
            var stripped = AssignmentDebriefPackageTools.StripPhotos(heavyPackage);
            Assert.IsTrue(AssignmentDebriefPackageTools.CanFitInQrCode(stripped, 2048));
        }
    }
}
