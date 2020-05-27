using Microsoft.VisualStudio.TestTools.UnitTesting;
using DomainLibrary.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using DataLayer;
using TrainingTest;
using Shouldly;

namespace DomainLibrary.Domain.Tests
{
    [TestClass()]
    public class TrainingManagerTests
    {
        [TestInitialize]
        public void Initialize()
        {
            TrainingManager TM = new TrainingManager(new UnitOfWork(new TrainingContextTest()));

            TM.AddCyclingTraining(new DateTime(2020, 4, 21, 16, 00, 00), 40, new TimeSpan(1, 20, 00), 30, null, TrainingType.Endurance, null, BikeType.RacingBike);
            TM.AddCyclingTraining(new DateTime(2020, 4, 18, 18, 00, 00), 40, new TimeSpan(1, 42, 00), null, null, TrainingType.Recuperation, null, BikeType.RacingBike);
            TM.AddCyclingTraining(new DateTime(2020, 4, 19, 16, 45, 00), null, new TimeSpan(1, 0, 00), null, 219, TrainingType.Interval, "5x5 min 270", BikeType.IndoorBike);
            TM.AddRunningTraining(new DateTime(2020, 4, 17, 12, 30, 00), 5000, new TimeSpan(0, 27, 17), null, TrainingType.Endurance, null);
            TM.AddRunningTraining(new DateTime(2020, 4, 19, 12, 30, 00), 5000, new TimeSpan(0, 25, 48), null, TrainingType.Endurance, null);
            TM.AddRunningTraining(new DateTime(2020, 3, 17, 11, 0, 00), 5000, new TimeSpan(0, 28, 10), null, TrainingType.Interval, "3x700m");
            TM.AddRunningTraining(new DateTime(2020, 3, 17, 11, 0, 00), 8000, new TimeSpan(0, 42, 10), null, TrainingType.Endurance, null);
        }

        [TestMethod()]
        public void AddCyclingTrainingTest()
        {
            TrainingManager TM = new TrainingManager(new UnitOfWork(new TrainingContextTest(true)));
            var cyclingCount = TM.GetAllCyclingSessions().Count();
            cyclingCount.ShouldBe(3);
            TM.AddCyclingTraining(new DateTime(2020, 4, 19, 16, 45, 00), null, new TimeSpan(1, 0, 00), null, 219, TrainingType.Interval, "5x5 min 270", BikeType.IndoorBike);
            cyclingCount = TM.GetAllCyclingSessions().Count();
            cyclingCount.ShouldBe(4);
        }

        [TestMethod]
        public void AddCyclingTraining_ShouldThrowDomainException_WhenDateInFuture()
        {
            TrainingManager TM = new TrainingManager(new UnitOfWork(new TrainingContextTest(true)));

            try
            {
                TM.AddCyclingTraining(new DateTime(2020, 10, 19, 16, 45, 00), null, new TimeSpan(1, 0, 00), null, 219, TrainingType.Interval, "5x5 min 270", BikeType.IndoorBike);
            }
            catch(DomainException ex)
            {
                StringAssert.Contains(ex.Message, "Training is in the future");
                return;
            }
            Assert.Fail("The expected exception was not thrown");
        }

        [TestMethod]
        public void AddCyclingTraining_ShouldThrowDomainException_WhenDistanceInvalid()
        {
            TrainingManager TM = new TrainingManager(new UnitOfWork(new TrainingContextTest(true)));

            try
            {
                TM.AddCyclingTraining(new DateTime(2020, 4, 19, 16, 45, 00), -1000, new TimeSpan(1, 0, 00), null, 219, TrainingType.Interval, "5x5 min 270", BikeType.IndoorBike);
            }
            catch (DomainException ex)
            {
                StringAssert.Contains(ex.Message, "Distance invalid value");
                return;
            }

            try
            {
                TM.AddCyclingTraining(new DateTime(2020, 4, 19, 16, 45, 00), 600, new TimeSpan(1, 0, 00), null, 219, TrainingType.Interval, "5x5 min 270", BikeType.IndoorBike);
            }
            catch (DomainException ex)
            {
                StringAssert.Contains(ex.Message, "Distance invalid value");
                return;
            }

            Assert.Fail("The expected exception was not thrown");
        }

        [TestMethod]
        public void AddCyclingTraining_ShouldThrowDomainException_WhenTimeInvalid()
        {
            TrainingManager TM = new TrainingManager(new UnitOfWork(new TrainingContextTest(true)));

            try
            {
                TM.AddCyclingTraining(new DateTime(2020, 4, 19, 16, 45, 00), null, new TimeSpan(-1), null, 219, TrainingType.Interval, "5x5 min 270", BikeType.IndoorBike);
            }
            catch (DomainException ex)
            {
                StringAssert.Contains(ex.Message, "Time invalid value");
                return;
            }

            try
            {
                TM.AddCyclingTraining(new DateTime(2020, 4, 19, 16, 45, 00), null, new TimeSpan(21,0,0), null, 219, TrainingType.Interval, "5x5 min 270", BikeType.IndoorBike);
            }
            catch (DomainException ex)
            {
                StringAssert.Contains(ex.Message, "Time invalid value");
                return;
            }

            Assert.Fail("The expected exception was not thrown");
        }

        [TestMethod]
        public void AddCyclingTraining_ShouldThrowDomainException_WhenAverageSpeedInvalid()
        {
            TrainingManager TM = new TrainingManager(new UnitOfWork(new TrainingContextTest(true)));

            try
            {
                TM.AddCyclingTraining(new DateTime(2020, 4, 19, 16, 45, 00), null, new TimeSpan(1, 0, 00), -2, 219, TrainingType.Interval, "5x5 min 270", BikeType.IndoorBike);
            }
            catch (DomainException ex)
            {
                StringAssert.Contains(ex.Message, "Average speed invalid value");
                return;
            }

            try
            {
                TM.AddCyclingTraining(new DateTime(2020, 4, 19, 16, 45, 00), null, new TimeSpan(1, 0, 00), 75, 219, TrainingType.Interval, "5x5 min 270", BikeType.IndoorBike);
            }
            catch (DomainException ex)
            {
                StringAssert.Contains(ex.Message, "Average speed invalid value");
                return;
            }

            Assert.Fail("The expected exception was not thrown");
        }

        [TestMethod]
        public void AddCyclingTraining_ShouldThrowDomainException_WhenAverageWattageInvalid()
        {
            TrainingManager TM = new TrainingManager(new UnitOfWork(new TrainingContextTest(true)));

            try
            {
                TM.AddCyclingTraining(new DateTime(2020, 4, 19, 16, 45, 00), null, new TimeSpan(1, 0, 00), null, -123, TrainingType.Interval, "5x5 min 270", BikeType.IndoorBike);
            }
            catch (DomainException ex)
            {
                StringAssert.Contains(ex.Message, "Average watt invalid value");
                return;
            }

            try
            {
                TM.AddCyclingTraining(new DateTime(2020, 4, 19, 16, 45, 00), null, new TimeSpan(1, 0, 00), null, 965, TrainingType.Interval, "5x5 min 270", BikeType.IndoorBike);
            }
            catch (DomainException ex)
            {
                StringAssert.Contains(ex.Message, "Average watt invalid value");
                return;
            }

            Assert.Fail("The expected exception was not thrown");
        }

        [TestMethod()]
        public void GenerateMonthlyCyclingReportTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void AddRunningTrainingTest()
        {
            TrainingManager TM = new TrainingManager(new UnitOfWork(new TrainingContextTest(true)));
            var runningCount = TM.GetAllRunningSessions().Count();
            runningCount.ShouldBe(4);
            TM.AddRunningTraining(new DateTime(2020, 3, 17, 11, 0, 00), 8000, new TimeSpan(0, 42, 10), null, TrainingType.Endurance, null);
            runningCount = TM.GetAllRunningSessions().Count();
            runningCount.ShouldBe(5);
        }

        [TestMethod()]
        public void GenerateMonthlyRunningReportTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void RemoveTrainingsTest()
        {
            TrainingManager TM = new TrainingManager(new UnitOfWork(new TrainingContextTest(true)));
            TM.RemoveTrainings(new List<int> { 1 }, new List<int> { 2, 3, 4 });

            TM.GetAllRunningSessions().Count.ShouldBe(1);
            TM.GetAllCyclingSessions().Count.ShouldBe(2);
        }

        [TestMethod()]
        public void GenerateMonthlyTrainingsReportTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetPreviousRunningSessionsTest()
        {
            TrainingManager TM = new TrainingManager(new UnitOfWork(new TrainingContextTest(true)));
            RunningSession runningSession1 = new RunningSession(new DateTime(2020, 3, 17, 11, 0, 00), 5000, new TimeSpan(0, 28, 10), null, TrainingType.Interval, "3x700m");
            RunningSession runningSession2 = new RunningSession(new DateTime(2020, 3, 17, 11, 0, 00), 8000, new TimeSpan(0, 42, 10), null, TrainingType.Endurance, null);

            List<RunningSession> runningsessions = TM.GetPreviousRunningSessions(2);
            runningsessions[0].ShouldBe(runningSession1);
            runningsessions[1].ShouldBe(runningSession2);
        }

        [TestMethod()]
        public void GetPreviousCyclingSessionsTest()
        {
            TrainingManager TM = new TrainingManager(new UnitOfWork(new TrainingContextTest(true)));
            CyclingSession cyclingSession1 = new CyclingSession(new DateTime(2020, 4, 18, 18, 00, 00), 40, new TimeSpan(1, 42, 00), null, null, TrainingType.Recuperation, null, BikeType.RacingBike);
            CyclingSession cyclingSession2 = new CyclingSession(new DateTime(2020, 4, 19, 16, 45, 00), null, new TimeSpan(1, 0, 00), null, 219, TrainingType.Interval, "5x5 min 270", BikeType.IndoorBike);
            cyclingSession1.Id = 2;

            //TM.GetPreviousCyclingSessions(1).ShouldBe(cyclingSession1);
            //TM.GetPreviousCyclingSessions(2).ShouldContain(cyclingSession2);
        }

        [TestMethod()]
        public void GetAllRunningSessionsTest()
        {
            TrainingManager TM = new TrainingManager(new UnitOfWork(new TrainingContextTest(true)));
            TM.GetAllRunningSessions().Count().ShouldBe(4);
        }

        [TestMethod()]
        public void GetAllCyclingSessionsTest()
        {
            TrainingManager TM = new TrainingManager(new UnitOfWork(new TrainingContextTest(true)));
            TM.GetAllCyclingSessions().Count().ShouldBe(3);
        }
    }
}