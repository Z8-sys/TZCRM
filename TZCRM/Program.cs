using System;
using System.Collections.Generic;

namespace TZCRM
{
    internal class Program
    {
        private static Dictionary<int, int> map = new Dictionary<int, int>()
        {
            { 1, 31 },
            { 2, 29 },
            { 3, 31 },
            { 4, 30 },
            { 5, 31 },
            { 6, 30 },
            { 7, 31 },
            { 8, 31 },
            { 9, 30 },
            { 10, 31 },
            { 11, 30 },
            { 12, 31 }
        };
        static void Main(string[] args)
        {
            bool a = true;
            Plan plan = new Plan();
            while (a)
            {
                plan.convertMeeting(DateTime.Now);
                plan.notification(DateTime.Now);
                Console.WriteLine("Choose an option \n 1 - add meeting \n 2 - change duration " +
                    "\n 3 - change notification \n 4 - delete meeting \n 5 - show future list " +
                    "\n 6 - show past \n 7 - export meeting \n 8 - import meeting \n 9 - export day to txt");
                switch (Console.ReadLine())
                {
                    case "1": 
                        {
                            Console.WriteLine("vvedite nachalo sobytiya");
                            DateTime startTime = inputDate();
                            Console.WriteLine("konec");
                            DateTime endTime = inputDate();
                            Console.WriteLine("uvedomleniye");
                            DateTime notificationTime = inputDate();
                            string name = Console.ReadLine();
                            if (startTime == DateTime.MinValue || endTime == DateTime.MinValue || notificationTime == DateTime.MinValue)
                            {
                                Console.WriteLine("nepravilniy vvod");
                            }
                            else
                            {
                                plan.addMeeting(startTime, endTime, notificationTime, name);
                            }
                            break;
                        }
                    case "2":
                        {
                            Console.WriteLine("vvedite nachalo sobytiya kotoroye hotite zamenit");
                            DateTime currentTime = inputDate();
                            Console.WriteLine("vvedite nachalo sobytiya");
                            DateTime startTime = inputDate();
                            Console.WriteLine("konec");
                            DateTime endTime = inputDate();
                            string name = Console.ReadLine();
                            if (startTime == DateTime.MinValue || endTime == DateTime.MinValue || currentTime == DateTime.MinValue)
                            {
                                Console.WriteLine("nepravilniy vvod");
                            }
                            else
                            {
                                plan.changeDurTime(currentTime, startTime, endTime);
                            }
                            break;
                        }
                    case "3":
                        {
                            Console.WriteLine("vvedite nachalo sobytiya kotoroye hotite zamenit");
                            DateTime currentTime = inputDate();
                            Console.WriteLine("vvedite novoe napominaniye");
                            DateTime notificationTime = inputDate();
                            string name = Console.ReadLine();
                            if (currentTime == DateTime.MinValue || notificationTime == DateTime.MinValue)
                            {
                                Console.WriteLine("nepravilniy vvod");
                            }
                            else
                            {
                                plan.changeNotTime(currentTime, notificationTime);
                            }
                            break;
                        }
                    case "4":
                        {
                            Console.WriteLine("vvedite nachalo sobytiya");
                            DateTime startTime = inputDate();
                            plan.deleteMeeting(startTime);
                            break;
                        }
                    case "5":
                        {
                            plan.showFutureList();
                            break;
                        }
                    case "6":
                        {
                            plan.showPastList();
                            break;
                        }
                    case "7":
                        {
                            Console.WriteLine("vvedite nachalo sobytiya");
                            DateTime startTime = inputDate();
                            Console.WriteLine("kuda sohranit");
                            string path = Console.ReadLine();
                            plan.saveMeeting(startTime, path);
                            break;

                        }
                    case "8":
                        {
                            Console.WriteLine("chto importirovat");
                            string path = Console.ReadLine();
                            FutureMeeting meeting = new FutureMeeting(path);
                            if (meeting.startTime == DateTime.MinValue || meeting.endTime == DateTime.MinValue || meeting.notificationTime == DateTime.MinValue)
                            {
                                Console.WriteLine("File povrezhden ili yego net");
                            }
                            else
                            {
                                plan.addMeeting(meeting.startTime, meeting.endTime, meeting.notificationTime, meeting.name);
                            }
                            break;
                        }
                    case "9":
                        {
                            Console.WriteLine("vvedite nachalo sobytiya");
                            DateTime startTime = inputDateWithoutTime();
                            Console.WriteLine("kuda sohranit");
                            string path = Console.ReadLine();
                            plan.exportExactDayTxt(startTime, path);
                            break;
                        }
                    default: { break; }
                }
            }
        }

        private static DateTime inputDate()
        {
            int year, month, day, hour, minute;
            Console.WriteLine("vvedite god");
            year = inputInt();
            if (year == -1)
                return DateTime.MinValue;
            Console.WriteLine("vvedite mesyac");
            month = inputInt();
            if (month > 12 || month < 1)
                return DateTime.MinValue;
            Console.WriteLine("vvedite den");
            day = inputInt();
            if(day < 1 || day > map[month]) 
                return DateTime.MinValue;
            Console.WriteLine("vvedite chas");
            hour = inputInt();
            if (hour < 0 || hour >= 24)
                return DateTime.MinValue;
            Console.WriteLine("vvedite minut");
            minute = inputInt();
            if ((minute < 0) || (minute >= 60))
                return DateTime.MinValue;
            return new DateTime(year, month, day, hour, minute, 0);

        }

        private static DateTime inputDateWithoutTime()
        {
            int year, month, day;
            Console.WriteLine("vvedite god");
            year = inputInt();
            if (year == -1)
                return DateTime.MinValue;
            Console.WriteLine("vvedite mesyac");
            month = inputInt();
            if (month > 12 || month < 1)
                return DateTime.MinValue;
            Console.WriteLine("vvedite den");
            day = inputInt();
            if (day < 1 || day > map[month])
                return DateTime.MinValue;
            return new DateTime(year, month, day, 0, 0, 0);
        }

        private static int inputInt()
        {
            string input = Console.ReadLine();
            if (int.TryParse(input, out int result))
            {
                return result;
            }
            else
            {
                Console.WriteLine($"nelzya '{input}' ");
                return -1;
            }
        }
    }
}
