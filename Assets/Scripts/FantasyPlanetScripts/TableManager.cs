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
    private Transform title;
    private string[] Weekdays = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
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
        this.title = transform.Find("Window/Title");
        title.Find("Date").GetComponent<Text>().text = date.ToString("yyyy/MM/dd");
        title.Find("WeekDay").GetComponent<Text>().text = Weekdays[(int)date.DayOfWeek];
    }
}
