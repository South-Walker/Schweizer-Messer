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
    private bool hasTable = false;

	const string k_OpenTransitionName = "Open";
	const string k_ClosedStateName = "Closed";
    public void Start()
    {
        date = DateTime.Now;
        prefabs = new LoopChainTable<GameObject>(blueDayWindow);
        prefabs.Add(greenDayWindow);
        prefabs.Add(redDayWindow);
        Classtable = new ClassTableob(htmlClasstable.text);
    }
    public void OnEnable()
    {
        OpenParameterId = Animator.StringToHash (k_OpenTransitionName);
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
        date = date.AddDays(1);
        CreateAndOpenTable(prefabs.MoveNext());
    }
    public void YesterDay()
    {
        date = date.AddDays(-1);
        CreateAndOpenTable(prefabs.MoveBack());
    }
    public void ChangeWeeknum(int newWeeknum)
    {
        date = Classtable.getMondayDate(newWeeknum);
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
        TableManager tableManager = newTable.AddComponent<TableManager>();
        if (Classtable == null)
            return;
        tableManager.Initialize(date, Classtable);
        OpenPanel(newTable.GetComponent<Animator>());
        hasTable = true;
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
