using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;



[Serializable]
public class FutureMeeting
{
    public DateTime startTime {  get; set; }
    public DateTime endTime { get; set; }
    public DateTime notificationTime { get; set; }


    public string name;

    public FutureMeeting(DateTime startTime, DateTime endTime, DateTime notificationTime, string name)
    {
        this.startTime = startTime;
        this.endTime = endTime;
        this.notificationTime = notificationTime;
        this.name = name;
    }

    public FutureMeeting(string xmlFilePath)
    {
        try
        {
            using (FileStream fileStream = new FileStream(xmlFilePath + ".xml", FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(FutureMeeting));
                FutureMeeting meeting = (FutureMeeting)serializer.Deserialize(fileStream);
                this.startTime = meeting.startTime;
                this.endTime = meeting.endTime;
                this.notificationTime = meeting.notificationTime;
                this.name = meeting.name;
            }
        } catch { }
    }

    public FutureMeeting()
    {
    }

    public void changeMeetingTime(DateTime suggestStartTime, DateTime suggestEndTime)
    {
        if (suggestStartTime > suggestEndTime)
            return;
        if (notificationTime > suggestStartTime) 
            notificationTime = suggestStartTime.AddMinutes(-10);
        startTime = suggestStartTime;
        endTime = suggestEndTime;
    }

    public void changeNotificationTime(DateTime suggestNotificationTime)
    {
        if (startTime < suggestNotificationTime)
            return;
        notificationTime = suggestNotificationTime;
    }

    public override string ToString()
    {
        return startTime.ToString() + " - " + endTime.ToString() + " - " + notificationTime.ToString() + " - " + name;
    }

    public void saveToXml(string filePath)
    {
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(FutureMeeting));
            using (FileStream fileStream = new FileStream(filePath + name.ToString() + ".xml", FileMode.Create))
            {
                serializer.Serialize(fileStream, this);
            }
        } catch { return; }
    }
}

