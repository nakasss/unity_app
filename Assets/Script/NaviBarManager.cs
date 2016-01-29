using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class NaviBarManager : MonoBehaviour {
	[SerializeField] private Animator nonViewerAC;
	[SerializeField] private Animator naviIconAC;

	enum NaviStatus : int {
		Default = 0,
		XMark = 1,
		Back = 2
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	// Called in navi icon evetn trigger
	private int navistatus = 0;
	public void OnNaviIconClicked () {
		int currentStatus = naviIconAC.GetInteger("NavigationStatus");
		if ((int)NaviStatus.Default == currentStatus) {
			naviIconAC.SetInteger("NavigationStatus", (int)NaviStatus.XMark);
			nonViewerAC.SetBool("IsOpenNavigation", true);
		} else if ((int)NaviStatus.XMark == currentStatus) {
			naviIconAC.SetInteger("NavigationStatus", (int)NaviStatus.Default);
			nonViewerAC.SetBool("IsOpenNavigation", false);
		} else if ((int)NaviStatus.Back == currentStatus) {
			//Back To Main
			Debug.Log("Navi Back Click");
			naviIconAC.SetInteger("NavigationStatus", (int)NaviStatus.Default);
		}

	}
}
