using System;
using System.Collections.Generic;
using System.Text;

public struct Time
{
    public int minutes, hours,
        day, month, year;
    public Time(int minutes, int hours,
        int day, int month, int year)
    {
        this.minutes = minutes;
        this.hours = hours;
        this.day = day;
        this.month = month;
        this.year = year;
    }
}