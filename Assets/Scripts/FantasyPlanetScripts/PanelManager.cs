using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.FantasyPlanetScripts;

public class PanelManager : MonoBehaviour {
    public Dropdown d_Weeknum;
    public Transform t_parentOfClasstable;
    public DateTime Date
    {
        get
        {
            return _date;
        }
        set
        {
            ChangeDateTo(value);
        }
    }
    private DateTime _date;

    private ClassTableob Classtable;
    private LoopChainTable<GameObject> prefabs;
    private Animator a_Open;
    private Queue<GameObject> ClasstablePool = new Queue<GameObject>();

    const string OpenTransitionName = "Open";
    const string ClosedStateName = "Closed";
    
    private void Awake()
    {
        initializeParameter();
    }
    private void initializeParameter()
    {
        var htmlClasstable = Resources.Load<TextAsset>("Data/Newclasstable");
        var g_blueDayWindow = Resources.Load<GameObject>("Prefabs/BlueDayMenu");
        var g_greenDayWindow = Resources.Load<GameObject>("Prefabs/GreenDayMenu");
        var g_redDayWindow = Resources.Load<GameObject>("Prefabs/RedDayMenu");
        Classtable = new ClassTableob(htmlClasstable.text);
        prefabs = new LoopChainTable<GameObject>(g_blueDayWindow);
        prefabs.Add(g_greenDayWindow);
        prefabs.Add(g_redDayWindow);
    }
    public void OnEnable()
    {
        ChangeDateTo(DateTime.Now);
        CreateAndOpenTable(prefabs.MoveNext());
    }
    #region Event
    public void NextDay()
    {
        ChangeDateTo(_date.AddDays(1));
        CreateAndOpenTable(prefabs.MoveNext());
    }
    public void YesterDay()
    {
        ChangeDateTo(_date.AddDays(-1));
        CreateAndOpenTable(prefabs.MoveBack());
    }
    public void ChangeWeeknum(Dropdown dropdown)
    {
        int newweeknum = dropdown.value + 1;
        int oldweeknum = Classtable.getWeeknum(Date);
        if (oldweeknum == newweeknum)
        {
            //其实这个不判断也没事，如果值没有被修改DropDown不会触发ChangeValue的事件，因此不会无限递归
            return;
        }
        else
        {
            ChangeDateTo(Classtable.getMondayDate(newweeknum));
            CreateAndOpenTable(prefabs.MoveNext());
        }
    }
    //只负责收拾下属的table对象，本身的Close由上层Menu负责
    public void Close()
    {
        DisableCurrentTable();
    }
    #endregion
    private void CreateAndOpenTable(GameObject prefab)
    {
        //create
        GameObject newTable = Instantiate(prefab, t_parentOfClasstable, false);
        TableManager tableManager = newTable.AddComponent<TableManager>();
        tableManager.Initialize(_date, Classtable);
        //open
        Animator newAnim = newTable.GetComponent<Animator>();
        if (a_Open == newAnim)
            return;
        newAnim.transform.SetAsLastSibling();
        DisableCurrentTable();
        a_Open = newAnim;
        a_Open.SetBool(OpenTransitionName, true);
        //clean up
        ClasstablePool.Enqueue(newTable);
        CleanUpClasstablePool();
    }
    private void DisableCurrentTable()
    {
        if (a_Open == null || !a_Open.isActiveAndEnabled)
            return;
        a_Open.SetBool(OpenTransitionName, false);
        StartCoroutine(DisableClasstableDeleyed(a_Open));
        a_Open = null;
    }
    //销毁多余课程表,当且仅当ClasstablePool里有三个以上预制体对象时执行延迟销毁
    private void CleanUpClasstablePool()
    {
        if (ClasstablePool.Count > 3)
        {
            GameObject classtable = ClasstablePool.Dequeue();
            Animator anim = classtable.GetComponent<Animator>();
            StartCoroutine(CleanUpClasstableDeleyed(classtable, anim));
        }
    }
    private void ChangeDateTo(DateTime newDate)
    {
        int oldWeeknum = Classtable.getWeeknum(_date);
        _date = newDate;
        int newWeeknum = Classtable.getWeeknum(newDate);
        if (oldWeeknum != newWeeknum)
        {
            d_Weeknum.value = Math.Min(20, newWeeknum) - 1;
            d_Weeknum.value = Math.Max(1, newWeeknum) - 1;
        }
    }
    IEnumerator DisableClasstableDeleyed(Animator anim)
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
    IEnumerator CleanUpClasstableDeleyed(GameObject waitingToClear, Animator anim)
    {
        bool closedStateReached = false;
        bool isActive = waitingToClear.activeSelf;
        while (!closedStateReached && isActive) 
        {
            if (!anim.IsInTransition(0))
                closedStateReached = anim.GetCurrentAnimatorStateInfo(0).IsName(ClosedStateName);
            yield return new WaitForEndOfFrame();
        }
        Destroy(waitingToClear);
    }
}
