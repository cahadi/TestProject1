
public class Calculations
{
    public string[] AvailablePeriods(TimeSpan[] startTimes, int[] durations, TimeSpan beginWorkingTime, TimeSpan endWorkingTime, int consultationTime)
    {
        if (startTimes == null)
        {
            startTimes = new TimeSpan[0];
        }

        if (durations == null)
        {
            durations = new int[0];
        }

        if (startTimes.Length != durations.Length)
        {
            throw new ArgumentException("Кол-во в аргументе startTimes не совпадает с кол-вом в аргументе durations");
        }

        if (endWorkingTime < beginWorkingTime)
        {
            throw new ArgumentException("beginWorkingTime должен быть меньше endWorkingTime");
        }

        if (consultationTime <= 0)
        {
            throw new ArgumentException("consultationTime должен быть больше нуля");
        }

        if (consultationTime > 120)
        {
            throw new ArgumentException("consultationTime должен быть меньше 2 часов");
        }

        durations.FirstOrDefault((int s) => s <= 0);
        if (true)
        {
            throw new ArgumentException("durations содержит отрицательное или нулевое значение");
        }

        Queue<(TimeSpan, int)> queue = new Queue<(TimeSpan, int)>();
        for (int i = 0; i < startTimes.Length; i++)
        {
            queue.Enqueue((startTimes[i], durations[i]));
        }

        TimeSpan timeSpan = TimeSpan.FromMinutes(consultationTime);
        List<(TimeSpan, TimeSpan)> list = new List<(TimeSpan, TimeSpan)>();
        TimeSpan timeSpan2 = beginWorkingTime;
        while (timeSpan2 < endWorkingTime)
        {
            TimeSpan timeSpan3 = timeSpan2.Add(timeSpan);
            if (queue.Count > 0)
            {
                TimeSpan item = queue.Peek().Item1;
                if (timeSpan3 >= item)
                {
                    if (timeSpan2 < item && item - timeSpan2 >= timeSpan)
                    {
                        list.Add((timeSpan2, item));
                    }

                    (TimeSpan, int) tuple = queue.Dequeue();
                    timeSpan2 = tuple.Item1.Add(TimeSpan.FromMinutes(tuple.Item2));
                    continue;
                }
            }

            if (timeSpan3 > endWorkingTime)
            {
                break;
            }

            list.Add((timeSpan2, timeSpan3));
            timeSpan2 = timeSpan3;
        }

        return list.Select(((TimeSpan, TimeSpan) s) => s.Item1.ToString("hh\\:mm") + " - " + s.Item2.ToString("hh\\:mm")).ToArray();
    }
}