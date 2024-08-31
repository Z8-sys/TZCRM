using System;
using System.Collections.Generic;
using System.Text;


class PastMeeting
{
    public readonly DateTime startTime,
    endTime;
    public readonly string name;

    public PastMeeting(DateTime startTime, DateTime endTime, string name)
    {
        this.startTime = startTime;
        this.endTime = endTime;
        this.name = name;
    }

    public override string ToString()
    {
        return startTime.ToString() + " - " + endTime.ToString() + " - " + name;
    }
}
