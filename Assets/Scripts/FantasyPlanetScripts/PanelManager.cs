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
    private Animator a_initiallyOpen;
    private Animator a_Open;
    private bool isProducable = true;
    private Queue<GameObject> ClasstablePool = new Queue<GameObject>();

    const string OpenTransitionName = "Open";
    const string ClosedStateName = "Closed";

    public void Start()
    {
        ChangeDateTo(DateTime.Now);
    }
    private void Awake()
    {
        initializeParameter();
        ChangeDateTo(DateTime.Now);
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
        isProducable = true;
    }
    public void Update()
    {
        if (isProducable)
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
    public void Close()
    {
        CloseCurrentTable();
        ChangeDateTo(DateTime.Now);
    }
    #endregion
    private void CreateAndOpenTable(GameObject prefab)
    {
        GameObject newTable = Instantiate(prefab, t_parentOfClasstable, false);
        TableManager tableManager = newTable.AddComponent<TableManager>();
        tableManager.Initialize(_date, Classtable);
        OpenTable(newTable.GetComponent<Animator>());
        ClasstablePool.Enqueue(newTable);
        CleanUpClasstablePool();
        isProducable = false;
    }
    private void OpenTable(Animator newAnim)
    {
        if (a_Open == newAnim)
            return;
        newAnim.transform.SetAsLastSibling();

        CloseCurrentTable();

        a_Open = newAnim;
        a_Open.SetBool(OpenTransitionName, true);
    }
    private void CloseCurrentTable()
    {
        if (a_Open == null || !a_Open.isActiveAndEnabled)
            return;
        a_Open.SetBool(OpenTransitionName, false);
        StartCoroutine(DisablePanelDeleyed(a_Open));
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
    IEnumerator CleanUpClasstableDeleyed(GameObject waitingToClear, Animator anim)
    {
        bool closedStateReached = false;
        bool isAcrive = waitingToClear.activeSelf;
        while (!closedStateReached && isAcrive) 
        {
            if (!anim.IsInTransition(0))
                closedStateReached = anim.GetCurrentAnimatorStateInfo(0).IsName(ClosedStateName);
            yield return new WaitForEndOfFrame();
        }
        Destroy(waitingToClear);
    }
}
