using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class NaviBarManager : MonoBehaviour {
	[SerializeField] private Animator nonViewerAC;
	[SerializeField] private Animator naviIconAC;
	[SerializeField] private Animator mainContentAC;

	enum NaviStatus : int {
		Default = 0,
		XMark = 1,
		Back = 2
	}
	enum MainContentPageID : int {
		Main = 0,
		AboutUs = 1,
		Contact = 2,
		BeforePlay = 3
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
			naviIconAC.SetInteger("NavigationStatus", (int)NaviStatus.Default);
			mainContentAC.SetInteger("PageID", (int)MainContentPageID.Main);
		}

	}
}
