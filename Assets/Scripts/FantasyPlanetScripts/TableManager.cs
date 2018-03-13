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
    private Transform t_Title;
    private Transform t_Table;
    private Button b_Close;
    private Animator a_Open;
    private string[] Weekdays = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
    List<Transform> timePeriod = new List<Transform>();
    List<Transform> Times = new List<Transform>();

    const string OpenTransitionName = "Open";
    const string ClosedStateName = "Closed";
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
    }
    private void catchElement()
    {
        var Window = transform.Find("Window");
        this.t_Title = Window.Find("Title");
        this.t_Table = Window.Find("Table");
        this.b_Close = Window.Find("Close").GetComponent<Button>();
        a_Open = GetComponent<Animator>();
        b_Close.onClick.AddListener(() =>
        {
            CloseCurrent();
        });

        timePeriod.Add(t_Table.Find("Morning"));
        timePeriod.Add(t_Table.Find("Afternoon"));
        timePeriod.Add(t_Table.Find("Evening"));
        foreach (var item in timePeriod)
        {
            Times.Add(item.Find("FirstClass"));
            Times.Add(item.Find("SecondClass"));
        }
    }
    private void fillClassTable()
    {
        t_Title.Find("Date").GetComponent<Text>().text = date.ToString("yyyy/MM/dd");
        t_Title.Find("WeekDay").GetComponent<Text>().text = Weekdays[(int)date.DayOfWeek];
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
    public void CloseCurrent()
    {
        if (a_Open == null || !a_Open.isActiveAndEnabled)
            return;

        a_Open.SetBool(OpenTransitionName, false);
        StartCoroutine(DisablePanelDeleyed(a_Open));
        a_Open = null;

    }
    IEnumerator DisablePanelDeleyed(Animator anim)
    {
        bool closedStateReached = false;
        bool wantToClose = true;
        while (!closedStateReached && wantToClose)
        {
            if (!anim.IsInTransition(0))
                closedStateReached = anim.GetCurrentAnimatorStateInfo(0).IsName(ClosedStateName);

            wantToClose = !anim.GetBool(OpenTransitionName);

            yield return new WaitForEndOfFrame();
        }

        if (wantToClose)
            anim.gameObject.SetActive(false);
    }
}
