using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityScript;
using Assets.Scripts.FantasyPlanetScripts;

public class TableManager : MonoBehaviour {
    private DateTime date;
    private List<Classob> classToday;
    private Transform Title;
    private Transform Table;
    private string[] Weekdays = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
    List<Transform> timePeriod = new List<Transform>();
    List<Transform> Times = new List<Transform>();
    // Use this for initialization
    void Start () {
	}
    private void OnEnable()
    {
    }
    // Update is called once per frame
    void Update () {
		
	}
    public void Initialize(DateTime date, ClassTableob classtable)
    {
        this.date = date;
        this.classToday = classtable.GetClassToday(date);
        catchElement();
        fillClassTable();
        Debug.Log("1");
    }
    private void catchElement()
    {
        var Window = transform.Find("Window");
        this.Title = Window.Find("Title");
        this.Table = Window.Find("Table");

        timePeriod.Add(Table.Find("Morning"));
        timePeriod.Add(Table.Find("Afternoon"));
        timePeriod.Add(Table.Find("Evening"));
        foreach (var item in timePeriod)
        {
            Times.Add(item.Find("FirstClass"));
            Times.Add(item.Find("SecondClass"));
        }
    }
    private void fillClassTable()
    {
        Title.Find("Date").GetComponent<Text>().text = date.ToString("yyyy/MM/dd");
        Title.Find("WeekDay").GetComponent<Text>().text = Weekdays[(int)date.DayOfWeek];
        foreach (var classob in classToday)
        {
            int beginPoint = classob.timebegin / 2;
            int endPoint = (classob.timeend - classob.timebegin + 1) / 2;
            var position = Times.GetRange(beginPoint, endPoint);
            foreach (var button in position)
            {
                button.Find("Text").GetComponent<Text>().text = classob.classname;
            }
        }
    }
}
