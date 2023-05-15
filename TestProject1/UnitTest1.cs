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

            TimeSpan[] startTimes = new TimeSpan[] { };
            int[] durations = new int[] { };

            Assert.Throws<ArgumentException>(() => calculations.AvailablePeriods(startTimes, durations, beginWorkingTime,
                endWorkingTime, consultationTime));
        }

        [Test]
        public void Calculations_AvailablePeriods_With_Different_Lengths_Of_StartTimes_And_Durations()
        {
            var startTimes = new TimeSpan[] { TimeSpan.Parse("09:00"), TimeSpan.Parse("09:30"), TimeSpan.Parse("10:00") };
            var durations = new int[] { 60, 30, 45 };
            var beginWorkingTime = TimeSpan.Parse("08:00");
            var endWorkingTime = TimeSpan.Parse("12:00");
            var consultationTime = 15;

            Assert.That(() => calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime),
                Throws.ArgumentException.With.Message.Contains("durations содержит отрицательное или нулевое значение"));
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
/**/        public void Calculations_AvailablePeriods_With_Negative_Or_Zero_Duration()
        {
            TimeSpan[] startTimes = new TimeSpan[] { TimeSpan.Parse("10:00"), TimeSpan.Parse("11:00") };
            int[] durations = new int[] { -60, 60 };
            TimeSpan beginWorkingTime = TimeSpan.Parse("08:00");
            TimeSpan endWorkingTime = TimeSpan.Parse("18:00");
            int consultationTime = 30;

            Assert.Throws<ArgumentException>(() => calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime));
        }

        [Test]
        public void Calculations_AvailablePeriods_With_Consultation_Time_Less_Or_Equal_To_Zero()
        {
            TimeSpan[] startTimes = null;
            int[] durations = null;
            TimeSpan beginWorkingTime = TimeSpan.Parse("08:00");
            TimeSpan endWorkingTime = TimeSpan.Parse("18:00");
            int consultationTime = 0;

            Assert.Throws<ArgumentException>(() => calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime));
        }

        [Test]
/**/        public void Calculations_AvailablePeriods_With_Consultation_Time_Greater_Than_Two_Hours()
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

            ArgumentException ex = Assert.Throws<ArgumentException>(() => calculations.AvailablePeriods(startTimes, durations, beginWorkingTime,
            endWorkingTime, consultationTime));

            string[] result = ex.Message.Split(';');

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
        }

        [Test]
        public void Calculations_AvailablePeriods_With_One_Appointment()
        {
            TimeSpan[] startTimes = new TimeSpan[] { TimeSpan.Parse("10:00") };
            int[] durations = new int[] { 60 };
            TimeSpan beginWorkingTime = TimeSpan.Parse("08:00");
            TimeSpan endWorkingTime = TimeSpan.Parse("18:00");
            int consultationTime = 30;

            Assert.That(() => calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime),
                Throws.ArgumentException.With.Message.Contains("durations содержит отрицательное или нулевое значение"));
        }

        [Test]
        public void Calculations_AvailablePeriods_With_Multiple_Appointments()
        {
            TimeSpan[] startTimes = new TimeSpan[] { TimeSpan.Parse("10:00"), TimeSpan.Parse("14:00"), TimeSpan.Parse("16:00") };
            int[] durations = new int[] { 60, 120, 30 };
            TimeSpan beginWorkingTime = TimeSpan.Parse("08:00");
            TimeSpan endWorkingTime = TimeSpan.Parse("18:00");
            int consultationTime = 30;

            Assert.That(() => calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime),
                Throws.ArgumentException.With.Message.Contains("durations содержит отрицательное или нулевое значение"));

        }

        [Test]
        public void Calculations_AvailablePeriods_With_All_Day_Appointments()
        {
            TimeSpan[] startTimes = new TimeSpan[] { TimeSpan.Parse("08:00") };
            int[] durations = new int[] { 600 };
            TimeSpan beginWorkingTime = TimeSpan.Parse("08:00");
            TimeSpan endWorkingTime = TimeSpan.Parse("18:00");
            int consultationTime = 30;

            Assert.That(() => calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime),
                Throws.ArgumentException.With.Message.Contains("durations содержит отрицательное или нулевое значение"));
        }
    }
}