using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;


class Plan
{
    private SortedList<DateTime, FutureMeeting> timeLine = new SortedList<DateTime, FutureMeeting>();
    private SortedList<DateTime, FutureMeeting> notificationStack = new SortedList<DateTime, FutureMeeting>();
    private List<PastMeeting> pastList = new List<PastMeeting>();

    public bool addMeeting(DateTime startTime, DateTime endTime, DateTime notificationTime, string Name)
    {
        endTime = endTime.AddMilliseconds(1); //отмечаем конец события одной миллисекундой чтобы можно было их отличать на временной линии
        if (timeLine.ContainsKey(startTime) || timeLine.ContainsKey(endTime) || (startTime > endTime))
            return false;
        while (notificationStack.ContainsKey(notificationTime))
            notificationTime = notificationTime.AddMilliseconds(1);//в отсортированный список нельзя иметь один и тот же ключ делаем их разными
        FutureMeeting meeting = new FutureMeeting(startTime, endTime, notificationTime, Name);
        timeLine.Add(startTime, meeting);
        timeLine.Add(endTime, meeting);
        if (timeLine.IndexOfKey(startTime) != 0) //начало события идет всегда либо первым либо после конца
            if (timeLine.Keys[timeLine.IndexOfKey(startTime) - 1].Millisecond != 1)
            {
                Console.WriteLine("Vstrecha uzhe zaplanirovana na " + timeLine.Keys[timeLine.IndexOfKey(startTime)]);
                timeLine.Remove(startTime);
                timeLine.Remove(endTime);
                return false;

            }

        if (timeLine.Keys[timeLine.IndexOfKey(startTime) + 1] != endTime) //конец всегда следующий после начала
        {
            Console.WriteLine("Vstrecha uzhe zaplanirovana na " + timeLine.Keys[timeLine.IndexOfKey(startTime)]);
            timeLine.Remove(startTime);
            timeLine.Remove(endTime);
            return false;
        }
        notificationStack.Add(notificationTime, meeting);
        return true;
    }


    public void convertMeeting(DateTime time)
    {
        if (timeLine.Count == 0) return;
        if (timeLine.Keys[1] < time)
        {
            FutureMeeting meeting = timeLine.Values[0];
            PastMeeting pastMeeting = new PastMeeting(meeting.startTime, meeting.endTime, meeting.name);
            timeLine.RemoveAt(0); //самые первые события переносят конвертацию в прошедшее
            timeLine.RemoveAt(0);
            pastList.Add(pastMeeting);
        }
    }

    public void showPastList()
    {
        foreach(PastMeeting meeting in pastList)
        {
            Console.WriteLine(meeting.ToString());
        }
    }

    public void showFutureList()
    {
        FutureMeeting meeting;
        for (int i = 0; i < timeLine.Count; i+=2)
        {
            meeting = timeLine.Values[i];
            Console.WriteLine(meeting.ToString());
        }
    }

    public bool notification(DateTime date)
    {
        if (notificationStack.Count > 0 )
            if (date > notificationStack.Keys.First())
            {
                Console.WriteLine("Notification - " + notificationStack.Values[0]);
                notificationStack.RemoveAt(0);
                return true;
            }
        return false;
    }

    public void changeDurTime(DateTime currentTime, DateTime startTime, DateTime endTime)
    {
        if (timeLine.ContainsKey(currentTime))
        {
            FutureMeeting meeting = timeLine[currentTime];
            deleteMeeting(currentTime);
            if (addMeeting(startTime, endTime, meeting.notificationTime, meeting.name))
                return;
            else
            {
                notificationStack.Add(meeting.notificationTime, meeting);
                timeLine.Add(meeting.startTime, meeting);
                timeLine.Add(meeting.endTime, meeting);
            }
        }
    }

    public void changeNotTime(DateTime startTime, DateTime newTime)
    {
        if (notificationStack.ContainsKey(newTime))
            return;
        DateTime currentTime = timeLine[startTime].notificationTime;
        FutureMeeting meeting = notificationStack[currentTime];
        while (notificationStack.ContainsKey(newTime)) newTime = newTime.AddMilliseconds(1);
        meeting.changeNotificationTime(newTime);
        if (notificationStack.ContainsKey(currentTime))
            notificationStack.Remove(currentTime);
        notificationStack.Add(newTime, meeting);
    }

    public void saveMeeting(DateTime startTime, string path)
    {
        if (timeLine.ContainsKey(startTime))
            timeLine[startTime].saveToXml(path);
    }

    public void deleteMeeting(DateTime startTime)
    {
        if (timeLine.ContainsKey(startTime))
        {
            notificationStack.Remove(timeLine[startTime].notificationTime);
            timeLine.Remove(timeLine[startTime].endTime);
            timeLine.Remove(startTime);
        }
    }


    public void exportExactDayTxt(DateTime day, string path)
    {
        if (timeLine.Count == 0) return;
        string output = "";
        DateTime nextDay = day.AddDays(1);
        timeLine.Add(day, null);
        int i = timeLine.IndexOfKey(day);
        timeLine.RemoveAt(i);
        if (i % 2 != 0) i += 1;
        if (i >= timeLine.Count) return;
        for (int j = i; j < timeLine.Count && timeLine.Keys[j] < nextDay; j += 2)
        {
            output += (timeLine.Values[j].ToString() + " \n");
        }

        try
        {
            File.WriteAllText(path + "day" + day.Day.ToString() + ".txt", output);
        }
        catch { return; }
    }
}