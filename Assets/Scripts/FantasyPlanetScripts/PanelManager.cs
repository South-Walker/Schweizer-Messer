using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.FantasyPlanetScripts;

public class PanelManager : MonoBehaviour {
    public GameObject blueDayWindow;
    public GameObject greenDayWindow;
    public GameObject redDayWindow;
    public TextAsset htmlClasstable;
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
    public Animator initiallyOpen;
    public Transform parentOfClasstable;
	private int OpenParameterId;
	private Animator Open;
    private bool hasTable = false;
    private Queue<GameObject> ClasstablePool = new Queue<GameObject>();

	const string OpenTransitionName = "Open";
	const string ClosedStateName = "Closed";
    public void Start()
    {
        _date = DateTime.Now;
        prefabs = new LoopChainTable<GameObject>(blueDayWindow);
        prefabs.Add(greenDayWindow);
        prefabs.Add(redDayWindow);
        Classtable = new ClassTableob(htmlClasstable.text);
    }
    public void OnEnable()
    {
        OpenParameterId = Animator.StringToHash (OpenTransitionName);
		if (initiallyOpen == null)
			return;
        OpenPanel(initiallyOpen);
	}
    public void Update()
    {
        if (!hasTable)
            CreateAndOpenTable(prefabs.MoveNext());
    }
    public void OpenPanel (Animator anim)
	{
		if (Open == anim)
			return;
		anim.gameObject.SetActive(true);

		anim.transform.SetAsLastSibling();

		CloseCurrent();
        
		Open = anim;
		Open.SetBool(OpenParameterId, true);
	}
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
        ChangeDateTo(Classtable.getMondayDate(dropdown.value));
        CreateAndOpenTable(prefabs.MoveNext());
    }
	public void CloseCurrent()
	{
		if (Open == null)
			return;

        EventSystem.current.SetSelectedGameObject(null);
        Open.SetBool(OpenParameterId, false);
		StartCoroutine(DisablePanelDeleyed(Open));
		Open = null;

	}
    private void CreateAndOpenTable(GameObject prefab)
    {
        GameObject newTable = Instantiate(prefab, parentOfClasstable, false);
        ClasstablePool.Enqueue(newTable);
        CleanUpClasstablePool();
        TableManager tableManager = newTable.AddComponent<TableManager>();
        if (Classtable == null)
            return;
        tableManager.Initialize(_date, Classtable);
        OpenPanel(newTable.GetComponent<Animator>());
        hasTable = true;
    }
    //销毁多余课程表
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
        _date = newDate;
    }
    IEnumerator DisablePanelDeleyed(Animator anim)
    {
        bool closedStateReached = false;
        bool wantToClose = true;
        while (!closedStateReached && wantToClose)
        {
            if (!anim.IsInTransition(0))
                closedStateReached = anim.GetCurrentAnimatorStateInfo(0).IsName(ClosedStateName);

            wantToClose = !anim.GetBool(OpenParameterId);

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
