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
    private DateTime date;
    private ClassTableob Classtable;
    private LoopChainTable<GameObject> prefabs;
    public Animator initiallyOpen;
    public Transform parentOfClasstable;
	private int OpenParameterId;
	private Animator Open;

	const string k_OpenTransitionName = "Open";
	const string k_ClosedStateName = "Closed";
    public void Start()
    {
        date = DateTime.Now;
    }
    public void OnEnable()
    {
        Classtable = new ClassTableob(htmlClasstable.text);
        OpenParameterId = Animator.StringToHash (k_OpenTransitionName);
        date = DateTime.Now;
        prefabs = new LoopChainTable<GameObject>(blueDayWindow);
        prefabs.Add(greenDayWindow);
        prefabs.Add(redDayWindow);
		if (initiallyOpen == null)
			return;

		OpenPanel(initiallyOpen);
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
        date = date.AddDays(1);
        GameObject newTable = Instantiate(prefabs.MoveNext(), parentOfClasstable, false);
        TableManager tableManager = newTable.AddComponent<TableManager>();
        tableManager.Initialize(date, Classtable);
        OpenPanel(newTable.GetComponent<Animator>());
    }
    public void YesterDay()
    {
        date = date.AddDays(-1);
        GameObject newTable = Instantiate(prefabs.MoveBack(), parentOfClasstable, false);
        TableManager tableManager = newTable.AddComponent<TableManager>();
        tableManager.Initialize(date, Classtable);
        OpenPanel(newTable.GetComponent<Animator>());
    }
    public void ChangeWeeknum(int newWeeknum)
    {
        date = Classtable.getMondayDate(newWeeknum);
        GameObject newTable = Instantiate(prefabs.MoveBack(), parentOfClasstable, false);
        TableManager tableManager = newTable.AddComponent<TableManager>();
        tableManager.Initialize(date, Classtable);
        OpenPanel(newTable.GetComponent<Animator>());
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
	IEnumerator DisablePanelDeleyed(Animator anim)
	{
		bool closedStateReached = false;
		bool wantToClose = true;
		while (!closedStateReached && wantToClose)
		{
			if (!anim.IsInTransition(0))
				closedStateReached = anim.GetCurrentAnimatorStateInfo(0).IsName(k_ClosedStateName);

			wantToClose = !anim.GetBool(OpenParameterId);

			yield return new WaitForEndOfFrame();
		}

		if (wantToClose)
			anim.gameObject.SetActive(false);
	}
}
