using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

public class PanelManager : MonoBehaviour {
    public GameObject BlueDayWindow;
    public GameObject GreenDayWindow;
    public GameObject RedDayWindow;
    private DateTime date;
    private LoopChainTable<GameObject> prefabs;
    public Animator initiallyOpen;
    public Transform parentOfClasstable;
	private int m_OpenParameterId;
	private Animator m_Open;

	const string k_OpenTransitionName = "Open";
	const string k_ClosedStateName = "Closed";

	public void OnEnable()
	{
		m_OpenParameterId = Animator.StringToHash (k_OpenTransitionName);
        date = DateTime.Now;
        prefabs = new LoopChainTable<GameObject>(BlueDayWindow);
        prefabs.Add(GreenDayWindow);
        prefabs.Add(RedDayWindow);
		if (initiallyOpen == null)
			return;

		OpenPanel(initiallyOpen);
	}
	public void OpenPanel (Animator anim)
	{
		if (m_Open == anim)
			return;
		anim.gameObject.SetActive(true);

		anim.transform.SetAsLastSibling();

		CloseCurrent();
        
		m_Open = anim;
		m_Open.SetBool(m_OpenParameterId, true);
	}
    public void NextDay()
    {
        GameObject nowPrefab = prefabs.MoveNext();
        var newTable = Instantiate(nowPrefab, parentOfClasstable, false);
        OpenPanel(newTable.GetComponent<Animator>());

    }
    public void YesterDay()
    {
        GameObject nowPrefab = prefabs.MoveBack();
        var newTable = Instantiate(nowPrefab, parentOfClasstable, false);
        OpenPanel(newTable.GetComponent<Animator>());
    }
	public void CloseCurrent()
	{
		if (m_Open == null)
			return;

        EventSystem.current.SetSelectedGameObject(null);
        m_Open.SetBool(m_OpenParameterId, false);
		StartCoroutine(DisablePanelDeleyed(m_Open));
		m_Open = null;

	}
	IEnumerator DisablePanelDeleyed(Animator anim)
	{
		bool closedStateReached = false;
		bool wantToClose = true;
		while (!closedStateReached && wantToClose)
		{
			if (!anim.IsInTransition(0))
				closedStateReached = anim.GetCurrentAnimatorStateInfo(0).IsName(k_ClosedStateName);

			wantToClose = !anim.GetBool(m_OpenParameterId);

			yield return new WaitForEndOfFrame();
		}

		if (wantToClose)
			anim.gameObject.SetActive(false);
	}
}
