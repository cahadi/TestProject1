using SF2022User01Lib;
using System.Net;

namespace TestProject1
{
    public class Tests
    {
        Calculations calculations;
        [SetUp]
        public void Setup()
        {
            calculations = new Calculations();
        }

        [Test]
        public void Calculations_AvailablePeriods_With_Null_StartTimes_And_Durations()
        {
            TimeSpan beginWorkingTime = new TimeSpan(9, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(18, 0, 0);
            int consultationTime = 30;

            string[] result = calculations.AvailablePeriods(null, null, beginWorkingTime, 
                endWorkingTime, consultationTime);

            Assert.AreEqual("09:00 - 18:00", string.Join(", ", result));
        }
        
        [Test]
        public void Calculations_AvailablePeriods_With_Different_Lengths_Of_StartTimes_And_Durations()
        {
            var startTimes = new TimeSpan[] { TimeSpan.Parse("09:00"), 
                TimeSpan.Parse("09:30"), TimeSpan.Parse("10:00") };
            var durations = new int[] { 60, 30, 45 };
            var beginWorkingTime = TimeSpan.Parse("08:00");
            var endWorkingTime = TimeSpan.Parse("12:00");
            var consultationTime = 15;

            var result = calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, 
                endWorkingTime, consultationTime);

            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual(result[0], "08:00 - 09:00");
            Assert.AreEqual(result[1], "09:30 - 10:00");
            Assert.AreEqual(result[2], "10:45 - 12:00");
        }

        [Test]
        public void Calculations_AvailablePeriods_With_End_Working_Time_Less_Than_BeginWorkingTime()
        {
            TimeSpan[] startTimes = new TimeSpan[] { TimeSpan.Parse("10:00"), TimeSpan.Parse("11:00") };
            int[] durations = new int[] { 60, 60 };
            TimeSpan beginWorkingTime = TimeSpan.Parse("20:00");
            TimeSpan endWorkingTime = TimeSpan.Parse("10:00");
            int consultationTime = 30;

            Assert.Throws<ArgumentException>(() => calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime));
        }

        [Test]
        public void Calculations_AvailablePeriods_With_Negative_Or_Zero_Duration()
        {
            TimeSpan[] startTimes = new TimeSpan[] { TimeSpan.Parse("10:00"), TimeSpan.Parse("11:00") };
            int[] durations = new int[] { -60, 60 };
            TimeSpan beginWorkingTime = TimeSpan.Parse("08:00");
            TimeSpan endWorkingTime = TimeSpan.Parse("18:00");
            int consultationTime = 30;

            Assert.Throws<ArgumentException>(() => calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime));
        }

        [Test]
        public void Calculations_AvailablePeriods_Wit_hConsultation_Time_Less_Or_Equal_To_Zero()
        {
            TimeSpan[] startTimes = null;
            int[] durations = null;
            TimeSpan beginWorkingTime = TimeSpan.Parse("08:00");
            TimeSpan endWorkingTime = TimeSpan.Parse("18:00");
            int consultationTime = 0;

            Assert.Throws<ArgumentException>(() => calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime));
        }

        [Test]
        public void Calculations_AvailablePeriods_With_Consultation_Time_Greater_Than_Two_Hours()
        {
            TimeSpan[] startTimes = null;
            int[] durations = null;
            TimeSpan beginWorkingTime = TimeSpan.Parse("08:00");
            TimeSpan endWorkingTime = TimeSpan.Parse("18:00");
            int consultationTime = 180;

            Assert.Throws<ArgumentException>(() => calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime));
        }

        [Test]
        public void Calculations_AvailablePeriods_With_No_Appointments()
        {
            TimeSpan[] startTimes = null;
            int[] durations = null;
            TimeSpan beginWorkingTime = TimeSpan.Parse("08:00");
            TimeSpan endWorkingTime = TimeSpan.Parse("18:00");
            int consultationTime = 30;

            string[] result = calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            Assert.IsNotNull(result);
            Assert.AreEqual(10, result.Length);
        }

        [Test]
        public void Calculations_AvailablePeriods_With_One_Appointment()
        {
            TimeSpan[] startTimes = new TimeSpan[] { TimeSpan.Parse("10:00") };
            int[] durations = new int[] { 60 };
            TimeSpan beginWorkingTime = TimeSpan.Parse("08:00");
            TimeSpan endWorkingTime = TimeSpan.Parse("18:00");
            int consultationTime = 30;

            string[] result = calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            Assert.IsNotNull(result);
            Assert.AreEqual(11, result.Length);
            Assert.AreEqual("08:00 - 10:00", result[0]);
            Assert.AreEqual("10:30 - 11:00", result[2]);
            Assert.AreEqual("17:00 - 18:00", result[9]);
        }

        [Test]
        public void Calculations_AvailablePeriods_With_Multiple_Appointments()
        {
            TimeSpan[] startTimes = new TimeSpan[] { TimeSpan.Parse("10:00"), TimeSpan.Parse("14:00"), TimeSpan.Parse("16:00") };
            int[] durations = new int[] { 60, 120, 30 };
            TimeSpan beginWorkingTime = TimeSpan.Parse("08:00");
            TimeSpan endWorkingTime = TimeSpan.Parse("18:00");
            int consultationTime = 30;

            string[] result = calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            Assert.IsNotNull(result);
            Assert.AreEqual(7, result.Length);
            Assert.AreEqual("08:00 - 10:00", result[0]);
            Assert.AreEqual("12:00 - 14:00", result[2]);
            Assert.AreEqual("14:30 - 16:00", result[3]);
            Assert.AreEqual("16:30 - 17:00", result[5]);
            Assert.AreEqual("17:30 - 18:00", result[6]);
        }

        [Test]
        public void Calculations_AvailablePeriods_With_All_Day_Appointments()
        {
            TimeSpan[] startTimes = new TimeSpan[] { TimeSpan.Parse("08:00") };
            int[] durations = new int[] { 600 };
            TimeSpan beginWorkingTime = TimeSpan.Parse("08:00");
            TimeSpan endWorkingTime = TimeSpan.Parse("18:00");
            int consultationTime = 30;

            string[] result = calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Length);
        }
    }
}