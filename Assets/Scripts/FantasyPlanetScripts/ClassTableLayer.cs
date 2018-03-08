using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using System;
using System.Text.RegularExpressions;
using System.Drawing;
using System.IO;
using UnityEngine;
using System.Drawing.Imaging;

public class ClassTableLayer : MonoBehaviour,IGUILayer {
    public TextAsset ClassTable;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool getFlag()
    {
        return true;
    }
    public void resetData()
    {

    }
    public void Draw()
    {
        ClassTableDrawer classTableDrawer = new ClassTableDrawer();
        Stream sImg = classTableDrawer.DrawClassTableInStream(ClassTable.text);
        byte[] bImg = new byte[sImg.Length];
        for (int i = 0; i < bImg.Length; i++)
        {
            bImg[i] = (byte)sImg.ReadByte();
        }
        Texture2D tImg = new Texture2D(ClassTableDrawer.classtablewidth, ClassTableDrawer.classtableheight);
        tImg.LoadImage(bImg);
        GUI.DrawTexture(new Rect(0, 0, ClassTableDrawer.classtablewidth, ClassTableDrawer.classtableheight), tImg);
    }
}
public static class ExpandGraphics
{
    public static void FillSmoothClassRectangle(this System.Drawing.Graphics g, RectangleF r, System.Drawing.Color color)
    {
        //这几个常量可以进一步封装，本质上是四个半圆，半径相同，理论上可以只用五个变量（4个圆心，1个半径）
        float beginx1 = r.X;
        float beginx2 = r.X + r.Width - r.Width / 3;
        float endx1 = r.X + r.Width / 3;
        float endx2 = r.X + r.Width;

        g.FillBetweenFunctions(
            x => { return r.Y + r.Width / 3f - (float)Math.Sqrt(r.Width * r.Width / 9f - (x - r.X - r.Width / 3f) * (x - r.X - r.Width / 3f)); },
            x => { return r.Y + r.Height - r.Width / 3f + (float)Math.Sqrt(r.Width * r.Width / 9f - (x - r.X - r.Width / 3f) * (x - r.X - r.Width / 3f)); },
            beginx1, endx1, color);
        g.FillBetweenFunctions(
            x => { return r.Y; },
            x => { return r.Y + r.Height; },
            endx1, beginx2, color);
        g.FillBetweenFunctions(
            x => { return r.Y + r.Width / 3f - (float)Math.Sqrt(r.Width * r.Width / 9f - (x - r.X - r.Width * 2 / 3f) * (x - r.X - r.Width * 2 / 3f)); },
            x => { return r.Y + r.Height - r.Width / 3f + (float)Math.Sqrt(r.Width * r.Width / 9f - (x - r.X - r.Width * 2 / 3f) * (x - r.X - r.Width * 2 / 3f)); },
            beginx2, endx2, color);
    }
    public static void FillBetweenFunctions(this System.Drawing.Graphics g, Func<float, float> funa, Func<float, float> funb, float beginx, float endx, System.Drawing.Color color)
    {
        float y1, y2, y3, y4;
        for (float i = beginx; i < endx; i += 0.5f)
        {
            y1 = funa(i);
            y2 = funa(i + 1);
            y3 = funb(i);
            y4 = funb(i + 1);
            g.DrawLine(new Pen(color), i, y1, i, y3);
        }
    }
    public static void DrawFunction(this System.Drawing.Graphics g, Func<float, float> function, float beginx, float endx)
    {
        float y1, y2;
        for (float i = beginx; i < endx; i += 0.5f)
        {
            y1 = function(i);
            y2 = function(i + 1);
            g.DrawLine(Pens.Black, i, y1, i + 1, y2);
        }
    }
}
public class ClassTableDrawer
{
    public DateTime now;
    static string[] dates = new string[] { "7", "8", "9", "10", "11", "12", "13" };
    static string[] nums = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
    static string[] weekday = new string[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
    static string[] day = new string[] { "上午", "下午", "晚上" };
    static System.Drawing.Color background = System.Drawing.Color.White;
    public static int classtablewidth = 1280 / 5 * 3;
    public static int classtableheight = 1056;
    static int weekdaywidth = classtablewidth / 8;
    static int weekdayheight = 60;
    static int dinnertimeheight = 30;
    static int dinnertimewidth = classtablewidth;
    static int classwidth = weekdaywidth;
    static int classheight = 78;
    static int daywidth = weekdaywidth / 2;
    static int dayheight = classheight * 4;
    static int width = 1280 / 5 * 3;
    static int clogheight = 30;
    static int height = classtableheight + clogheight;
    static string clogo = "©肖南行South-Walker";
    public Bitmap ClassTableImage = null;
    private System.Drawing.Graphics g = null;
    private Pen mypen = new Pen(System.Drawing.Color.FromArgb(208, 208, 208), 2);
    private System.Drawing.Font biggerfont = new System.Drawing.Font("方正卡通简体", 12, System.Drawing.FontStyle.Regular, GraphicsUnit.Point);
    private System.Drawing.Font smallerfont = new System.Drawing.Font("方正卡通简体", 20, System.Drawing.FontStyle.Bold, GraphicsUnit.Point);
    public ClassTableDrawer()
    {
        now = DateTime.Now;
        if (now.DayOfWeek == 0)
            now = now.AddDays(1);
    }
    public ClassTableDrawer(DateTime nowdate)
    {
        now = nowdate;
        if (now.DayOfWeek == 0)
            now = now.AddDays(1);
    }
    public Bitmap DrawClassTable(string html)
    {
        ClassTableImage = new Bitmap(width, height);
        g = System.Drawing.Graphics.FromImage(ClassTableImage);
        g.Clear(background);
        initdate();
        drawingbase();
        drawclasses(html);
        return ClassTableImage;
    }
    public Stream DrawClassTableInStream(string html)
    {
        DrawClassTable(html);
        MemoryStream stream = new MemoryStream();
        ClassTableImage.Save(stream, ImageFormat.Png);
        stream.Seek(0, SeekOrigin.Begin);
        return stream;
    }
    public void AddDays(double i)
    {
        now = now.AddDays(i);
    }
    private void drawingbase()
    {
        //横线
        g.DrawLine(mypen, 0, 0, classtablewidth, 0);
        g.DrawLine(mypen, 0, classtableheight, classtablewidth, classtableheight);
        g.DrawLine(mypen, 0, weekdayheight, classtablewidth, weekdayheight);
        g.DrawLine(mypen, 0, weekdayheight + classheight * 4, classtablewidth, weekdayheight + classheight * 4);
        g.DrawLine(mypen, 0, weekdayheight + classheight * 4 + dinnertimeheight, classtablewidth, weekdayheight + classheight * 4 + dinnertimeheight);
        g.DrawLine(mypen, 0, weekdayheight + classheight * 8 + dinnertimeheight, classtablewidth, weekdayheight + classheight * 8 + dinnertimeheight);
        g.DrawLine(mypen, 0, weekdayheight + classheight * 8 + dinnertimeheight * 2, classtablewidth, weekdayheight + classheight * 8 + dinnertimeheight * 2);
        for (int i = 1; i < 5; i++)
        {
            g.DrawLine(mypen, weekdaywidth / 2, weekdayheight + i * classheight,
                classtablewidth, weekdayheight + i * classheight);

            g.DrawLine(mypen, weekdaywidth / 2, weekdayheight + dinnertimeheight + (i + 4) * classheight,
                classtablewidth, weekdayheight + dinnertimeheight + (i + 4) * classheight);

            g.DrawLine(mypen, weekdaywidth / 2, weekdayheight + dinnertimeheight * 2 + (i + 8) * classheight,
                classtablewidth, weekdayheight + dinnertimeheight * 2 + (i + 8) * classheight);
        }
        //竖线
        for (int i = 0; i <= 8; i++)
        {
            if (i == 0 || i == 8)
                g.DrawLine(mypen, i * weekdaywidth, 0, i * weekdaywidth, classtableheight);
            else
            {
                g.DrawLine(mypen, i * weekdaywidth, 0, i * weekdaywidth, weekdayheight + classheight * 4);
                g.DrawLine(mypen, i * weekdaywidth, weekdayheight + classheight * 4 + dinnertimeheight,
                    i * weekdaywidth, weekdayheight + classheight * 8 + dinnertimeheight);
                g.DrawLine(mypen, i * weekdaywidth, weekdayheight + classheight * 8 + dinnertimeheight * 2,
                    i * weekdaywidth, classtableheight);
            }
        }
        g.DrawLine(mypen, daywidth, weekdayheight, daywidth, weekdayheight + 4 * classheight);
        g.DrawLine(mypen, daywidth, weekdayheight + 4 * classheight + dinnertimeheight, daywidth, weekdayheight + 8 * classheight + dinnertimeheight);
        g.DrawLine(mypen, daywidth, weekdayheight + 8 * classheight + dinnertimeheight * 2, daywidth, classtableheight);


        StringFormat centerStringFormat = new StringFormat();
        centerStringFormat.Alignment = StringAlignment.Center;
        centerStringFormat.LineAlignment = StringAlignment.Center;

        StringFormat leftStringFormat = new StringFormat();
        leftStringFormat.Alignment = StringAlignment.Near;
        leftStringFormat.LineAlignment = StringAlignment.Near;

        StringFormat rightStringFormat = new StringFormat();
        rightStringFormat.Alignment = StringAlignment.Far;
        rightStringFormat.LineAlignment = StringAlignment.Far;
        //月份
        g.DrawString(DateTime.Now.Month + "月", smallerfont, Brushes.Black, new Rectangle(0, weekdayheight / 3, weekdaywidth, weekdayheight * 2 / 3), centerStringFormat);
        //星期几
        for (int i = 0; i < weekday.Length; i++)
        {
            Rectangle now = new Rectangle(weekdaywidth * (i + 1), 0, weekdaywidth, weekdayheight / 3);
            g.DrawString(weekday[i], biggerfont, Brushes.Black, now, leftStringFormat);
            now = new Rectangle(weekdaywidth * (i + 1), weekdayheight / 3, weekdaywidth, weekdayheight * 2 / 3);
            g.DrawString(dates[i], smallerfont, Brushes.Black, now, centerStringFormat);
        }

        //时段
        for (int i = 0; i < day.Length; i++)
        {
            Rectangle now = new Rectangle(0, weekdayheight + dayheight * i + dinnertimeheight * i, daywidth, dayheight / 2);
            g.DrawString(day[i].Substring(0, 1), smallerfont, Brushes.Black, now, centerStringFormat);
            now = new Rectangle(0, weekdayheight + dayheight * i + dinnertimeheight * i + dayheight / 2, daywidth, dayheight / 2);
            g.DrawString(day[i].Substring(1, 1), smallerfont, Brushes.Black, now, centerStringFormat);
        }

        //节数
        for (int i = 1; i < nums.Length; i++)
        {
            int ynow = 0;
            ynow = (i - 1) / 4 * dinnertimeheight + (i - 1) * classheight + weekdayheight;
            Rectangle now = new Rectangle(weekdaywidth / 2, ynow, weekdaywidth / 2, classheight);
            g.DrawString(nums[i], biggerfont, Brushes.Black, now, centerStringFormat);
        }
    }
    private void drawclasses(string html)
    {
        StringFormat centerStringFormat = new StringFormat();
        centerStringFormat.Alignment = StringAlignment.Center;
        centerStringFormat.LineAlignment = StringAlignment.Center;

        StringFormat leftStringFormat = new StringFormat();
        leftStringFormat.Alignment = StringAlignment.Near;
        leftStringFormat.LineAlignment = StringAlignment.Near;

        StringFormat rightStringFormat = new StringFormat();
        rightStringFormat.Alignment = StringAlignment.Far;
        rightStringFormat.LineAlignment = StringAlignment.Far;

        List<List<Classob>> thisweek = getclasstable(html).GetThisWeekClassTable(now);

        for (int i = 0; i < thisweek.Count; i++)
        {
            List<Classob> nowlist = thisweek[i];
            for (int k = 0; k < nowlist.Count; k++)
            {
                drawaclass(g, nowlist[k]);
            }
        }
        //clogo
        g.DrawString(clogo, biggerfont, Brushes.Black, new Rectangle(0, classtableheight, classtablewidth, clogheight), rightStringFormat);
    }
    private void drawaclass(System.Drawing.Graphics g, Classob aclass)
    {
        Pen mypen = new Pen(System.Drawing.Color.Black, 2);
        Rectangle rect = getclassrect(aclass);
        //    g.FillRectangle(Brushes.White, rect);//直角时涂白使用

        StringFormat centerStringFormat = new StringFormat();
        centerStringFormat.Alignment = StringAlignment.Center;
        centerStringFormat.LineAlignment = StringAlignment.Center;

        //  g.FillRectangle(new SolidBrush(aclass.colorintable), rect);//这个不是圆角
        g.FillSmoothClassRectangle(rect, aclass.colorintable);//这个是个扩展方法
        System.Drawing.Font font = new System.Drawing.Font("方正卡通简体", 12, System.Drawing.FontStyle.Bold, GraphicsUnit.Point);
        g.DrawString(aclass.classname + "\r\n" + aclass.room, font, Brushes.Black, rect, centerStringFormat);
        //   g.DrawRectangle(mypen, rect);//直角时描边使用
    }
    private Rectangle getclassrect(Classob aclass)
    {
        int begin = aclass.timebegin;
        int week = aclass.weekcode + 1;
        int x = week * weekdaywidth;
        int y;
        int end = aclass.timeend;
        int length = (end - begin + 1) * classheight;
        if (begin <= 4)
            y = weekdayheight + (begin - 1) * classheight;
        else if (begin <= 8)
            y = weekdayheight + dinnertimeheight + (begin - 1) * classheight;
        else
            y = weekdayheight + dinnertimeheight * 2 + (begin - 1) * classheight;
        return new Rectangle(x, y, classwidth, length);
    }
    private void initdate()
    {
        int weekdaynow = Convert.ToByte(now.DayOfWeek);
        DateTime beginofweek = now.AddDays(weekdaynow * -1);
        for (int i = 0; i < dates.Length; i++)
        {
            dates[i] = beginofweek.Day.ToString();
            beginofweek = beginofweek.AddDays(1);
        }
    }
    private ClassTableob getclasstable(string html)
    {
        ClassTableob table = new ClassTableob(html, new DateTime(2018, 3, 5));
        return table;
    }
}
public class RandomColor
{
    List<System.Drawing.Color> Colors = new List<System.Drawing.Color>();
    private int nowindex = -1;
    public RandomColor(int seed)
    {
        initlist();
        randomcolors(seed);
    }
    private void randomcolors(int seed)
    {
        System.Random r = new System.Random(seed);
        for (int i = Colors.Count - 1; i >= 0; i--)
        {
            int next = r.Next(0, i + 1);
            System.Drawing.Color temp = Colors[next];
            Colors[next] = Colors[i];
            Colors[i] = temp;
        }
    }
    private void initlist()
    {
        Colors.Add(System.Drawing.Color.FromArgb(255, 192, 203));
        Colors.Add(System.Drawing.Color.FromArgb(199, 21, 133));
        Colors.Add(System.Drawing.Color.FromArgb(139, 0, 139));
        Colors.Add(System.Drawing.Color.FromArgb(255, 0, 255));
        Colors.Add(System.Drawing.Color.FromArgb(153, 50, 204));
        Colors.Add(System.Drawing.Color.FromArgb(123, 104, 238));
        Colors.Add(System.Drawing.Color.FromArgb(65, 105, 225));
        Colors.Add(System.Drawing.Color.FromArgb(100, 149, 237));
        Colors.Add(System.Drawing.Color.FromArgb(30, 144, 255));
        Colors.Add(System.Drawing.Color.FromArgb(0, 191, 255));
        Colors.Add(System.Drawing.Color.FromArgb(95, 158, 160));
        Colors.Add(System.Drawing.Color.FromArgb(127, 255, 170));
        Colors.Add(System.Drawing.Color.FromArgb(60, 179, 113));
        Colors.Add(System.Drawing.Color.FromArgb(46, 139, 87));
        Colors.Add(System.Drawing.Color.FromArgb(0, 128, 0));
        Colors.Add(System.Drawing.Color.FromArgb(255, 215, 0));
        Colors.Add(System.Drawing.Color.FromArgb(218, 165, 32));
        Colors.Add(System.Drawing.Color.FromArgb(255, 165, 0));
        Colors.Add(System.Drawing.Color.FromArgb(160, 82, 45));
        Colors.Add(System.Drawing.Color.FromArgb(255, 69, 0));
        Colors.Add(System.Drawing.Color.FromArgb(240, 128, 128));
        Colors.Add(System.Drawing.Color.FromArgb(192, 92, 92));
        Colors.Add(System.Drawing.Color.FromArgb(178, 34, 34));
    }
    public System.Drawing.Color Next()
    {
        nowindex++;
        if (nowindex >= Colors.Count)
            nowindex = 0;
        return Colors[nowindex];
    }
}
public struct Classob
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
    public System.Drawing.Color colorintable;
    public Classob(string thisteacher, string thisclassname, string thisdate, string thisroom, System.Drawing.Color thiscolor)
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
public struct ClassTableob
{
    public List<List<Classob>> ClassTable;
    public DateTime TermBegin;
    public ClassTableob(string html, DateTime begin, int randomSeed = 10150111)
    {
        RandomColor rc = new RandomColor(randomSeed);
        ClassTable = new List<List<Classob>>();
        TermBegin = begin;
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
        System.Drawing.Color thiscolor = rc.Next();
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
    public string GetStringToday(DateTime today, bool istomorrow = false)
    {
        string result = "";
        if (istomorrow)
        {
            result += today.Year + "年" + today.Month + "月" + today.Day + "日(明天)\r\n";
        }
        else
        {
            result += today.Year + "年" + today.Month + "月" + today.Day + "日(今天)\r\n";
        }
        List<Classob> todayclasses = GetClassToday(today);
        for (int i = 0; i < todayclasses.Count; i++)
        {
            result += todayclasses[i].classname + "\r\n";
            result += todayclasses[i].room + "\r\n";
            result += todayclasses[i].timebegin + "-" + todayclasses[i].timeend + "节\r\n";
        }
        if (todayclasses.Count == 0)
            result += "今天并没有安排课程！\r\n";
        return result;
    }
}