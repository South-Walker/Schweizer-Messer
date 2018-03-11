using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Assets.Scripts.FantasyPlanetScripts
{
    public class RandomColor
    {
        List<Color> Colors = new List<Color>();
        private int nowindex = -1;
        public RandomColor(int seed)
        {
            initlist();
            randomcolors(seed);
        }
        private void randomcolors(int seed)
        {
            UnityEngine.Random.InitState(seed);
            for (int i = Colors.Count - 1; i >= 0; i--)
            {
                int next = (int)(UnityEngine.Random.value * (i + 1));
                Color temp = Colors[next];
                Colors[next] = Colors[i];
                Colors[i] = temp;
            }
        }
        private void initlist()
        {
            Colors.Add(new Color(1f, 0.752941176470588f, 0.796078431372549f));
            Colors.Add(new Color(0.780392156862745f, 0.0823529411764706f, 0.52156862745098f));
            Colors.Add(new Color(0.545098039215686f, 0f, 0.545098039215686f));
            Colors.Add(new Color(1f, 0f, 1f));
            Colors.Add(new Color(0.6f, 0.196078431372549f, 0.8f));
            Colors.Add(new Color(0.482352941176471f, 0.407843137254902f, 0.933333333333333f));
            Colors.Add(new Color(0.254901960784314f, 0.411764705882353f, 0.882352941176471f));
            Colors.Add(new Color(0.392156862745098f, 0.584313725490196f, 0.929411764705882f));
            Colors.Add(new Color(0.117647058823529f, 0.564705882352941f, 1f));
            Colors.Add(new Color(0f, 0.749019607843137f, 1f));
            Colors.Add(new Color(0.372549019607843f, 0.619607843137255f, 0.627450980392157f));
            Colors.Add(new Color(0.498039215686275f, 1f, 0.666666666666667f));
            Colors.Add(new Color(0.235294117647059f, 0.701960784313725f, 0.443137254901961f));
            Colors.Add(new Color(0.180392156862745f, 0.545098039215686f, 0.341176470588235f));
            Colors.Add(new Color(0f, 0.501960784313725f, 0f));
            Colors.Add(new Color(1f, 0.843137254901961f, 0f));
            Colors.Add(new Color(0.854901960784314f, 0.647058823529412f, 0.125490196078431f));
            Colors.Add(new Color(1f, 0.647058823529412f, 0f));
            Colors.Add(new Color(0.627450980392157f, 0.32156862745098f, 0.176470588235294f));
            Colors.Add(new Color(1f, 0.270588235294118f, 0f));
            Colors.Add(new Color(0.941176470588235f, 0.501960784313725f, 0.501960784313725f));
            Colors.Add(new Color(0.752941176470588f, 0.36078431372549f, 0.36078431372549f));
            Colors.Add(new Color(0.698039215686274f, 0.133333333333333f, 0.133333333333333f));
        }
        public Color Next()
        {
            nowindex++;
            if (nowindex >= Colors.Count)
                nowindex = 0;
            return Colors[nowindex];
        }
    }
    public class ClassTableob
    {
        public List<List<Classob>> ClassTable;
        //这个没想到什么好办法，硬编码了
        public DateTime TermBegin = new DateTime(2018,3,5);
        public ClassTableob(string html, int seed = 0)
        {
            RandomColor rc = new RandomColor(seed);
            ClassTable = new List<List<Classob>>();
            if (ClassTable.Count == 0)
            {
                for (int i = 0; i < 8; i++)
                {
                    List<Classob> list = new List<Classob>();
                    ClassTable.Add(list);
                }
            }
            Regex regex = new Regex("<table[^>]*>.*</Table>");
            Match m = regex.Match(html);
            regex = new Regex("<tr[^>]*>.*?</tr[^>]*>");
            MatchCollection mc = regex.Matches(m.Value);
            string teacher = "";
            string classname = "";
            string date = "";
            string room = "";
            Color thiscolor = rc.Next();
            for (int trnum = 1; trnum < mc.Count; trnum++)
            {
                m = mc[trnum];
                string now = m.Value;
                if (Regex.IsMatch(now, "tr height=24"))
                {
                    thiscolor = rc.Next();
                    regex = new Regex("<td[^>]*rowspan=(?<howmanytimes>\\d+)[^>]*>(?<class>[^<]*)</td><td[^>]*>\\d+</td><td[^>]*>(?<teacher>[^<]*)</td><td[^>]*>(?<date>[^<]*)</td><td [^>]*>(?<room>[^<]*)</td><td[^>]*>[^<]*</td><td[^>]*>[^<]*</td><td[^>]*>[^<]*</td></tr>");
                    m = regex.Match(now);
                    GroupCollection gc = m.Groups;
                    teacher = gc["teach"].Value;
                    classname = gc["class"].Value;
                    date = gc["date"].Value;
                    room = gc["room"].Value;
                    Classob nowclass = new Classob(teacher, classname, date, room, thiscolor);
                    ClassTable[nowclass.weekcode].Add(nowclass);
                }
                else
                {
                    regex = new Regex("<td[^>]*>(?<date>[^<]*)(<td[^>]*>(?<room>[^<]*))?");
                    m = regex.Match(now);
                    GroupCollection gc = m.Groups;
                    date = gc["date"].Value;
                    if (!string.IsNullOrEmpty(gc["room"].Value))
                    {
                        room = gc["room"].Value;
                    }
                    Classob nowclass = new Classob(teacher, classname, date, room, thiscolor);
                    ClassTable[nowclass.weekcode].Add(nowclass);
                }
            }
            sort();
        }
        private void sort()
        {
            foreach (List<Classob> list in ClassTable)
            {
                if (list.Count <= 1)
                    continue;
                list.Sort((x, y) => x.timebegin.CompareTo(y.timebegin));
            }
        }
        public List<List<Classob>> GetThisWeekClassTable(DateTime today)
        {
            List<List<Classob>> result = new List<List<Classob>>();
            int weeknum = getweeknum(today);
            for (int i = 0; i < ClassTable.Count; i++)
            {
                List<Classob> thisday = new List<Classob>();
                for (int k = 0; k < ClassTable[i].Count; k++)
                {
                    Classob now = ClassTable[i][k];
                    if (now.isToday(weeknum))
                        thisday.Add(now);
                }
                result.Add(thisday);
            }
            return result;
        }
        private int getweeknum(DateTime today)
        {
            int days = (today - this.TermBegin).Days;
            return days / 7 + 1;
        }
        public DateTime getMondayDate(int weeknum = 0)
        {
            if (weeknum == 0)
                return TermBegin;
            else
            {
                DateTime result = TermBegin.AddDays(weeknum * 7);
                while ((int)result.DayOfWeek == 1)
                {
                    result = result.AddDays(-1);
                }
                return result;
            }
        }
        private static int getweekcode(DateTime today)
        {
            return (int)today.DayOfWeek;
        }
        public List<Classob> GetClassToday(DateTime today)
        {
            List<Classob> result = new List<Classob>();
            int weekcode = getweekcode(today);
            int weeknum = getweeknum(today);
            foreach (Classob clas in ClassTable[weekcode])
            {
                if (clas.isToday(weeknum))
                    result.Add(clas);
            }
            return result;
        }
    }
    public class Classob
    {
        public string teacher;
        public string classname;
        public int timebegin;
        public int timeend;
        public int datebegin;
        public int dateend;
        public string weekday;
        public int weekcode;
        public string room;
        public string quanorsuang;
        public bool isshuang;
        public bool isdan;
        public Color colorintable;
        public Classob(string thisteacher, string thisclassname, string thisdate, string thisroom, Color thiscolor)
        {
            colorintable = thiscolor;
            isdan = true; isshuang = true;
            teacher = thisteacher;
            classname = thisclassname;
            room = thisroom;
            Regex regex = new Regex("(?<weekday>[^\\s]*)\\s+(?<timebegin>\\d+)-(?<timeend>\\d+)节\\s+(?<datebegin>\\d+)-(?<dateend>\\d+)(?<quanorshuang>.*)$");
            GroupCollection gc = regex.Match(thisdate).Groups;
            timebegin = Convert.ToInt32(gc["timebegin"].Value);
            timeend = Convert.ToInt32(gc["timeend"].Value);
            datebegin = Convert.ToInt32(gc["datebegin"].Value);
            dateend = Convert.ToInt32(gc["dateend"].Value);
            weekday = gc["weekday"].Value;
            quanorsuang = gc["quanorshuang"].Value;
            switch (quanorsuang)
            {
                case "双周":
                    isdan = false;
                    break;
                case "单周":
                    isshuang = false;
                    break;
                default:
                    break;
            }
            switch (weekday)
            {
                case "周一":
                    weekcode = 1;
                    break;
                case "周二":
                    weekcode = 2;
                    break;
                case "周三":
                    weekcode = 3;
                    break;
                case "周四":
                    weekcode = 4;
                    break;
                case "周五":
                    weekcode = 5;
                    break;
                case "周六":
                    weekcode = 6;
                    break;
                default:
                    weekcode = 0;
                    break;
            }
        }
        public bool isToday(int weeknum)
        {
            if (weeknum < datebegin || weeknum > dateend)
                return false;
            if (weeknum % 2 == 1 && isdan)
            {
                return true;
            }
            else if (weeknum % 2 == 0 && isshuang)
            {
                return true;
            }
            return false;
        }
    }
}