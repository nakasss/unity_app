using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NavigationContentManager : MonoBehaviour {
	[SerializeField] private Animator nonViewerAC;
	[SerializeField] private Animator naviIconAC;
	[SerializeField] private Animator mainContentAC;
	enum MainContentPageID : int {
		Main = 0,
		AboutUs = 1,
		Contact = 2,
		BeforePlay = 3
	}
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


	public void OnClickHome () {

		ChangeNaviIconDefault();
		ChangeMainContentDefault();

		mainContentAC.SetInteger("PageID", (int)MainContentPageID.Main);
	}

	public void OnClickAbout () {

		ChangeNaviIconDefault();
		ChangeMainContentDefault();

		mainContentAC.SetInteger("PageID", (int)MainContentPageID.AboutUs);
	}

	public void OnClickContact () {

		ChangeNaviIconDefault();
		ChangeMainContentDefault();
		
		mainContentAC.SetInteger("PageID", (int)MainContentPageID.Contact);
	}

	public void OnClickOtherScreen () {
		ChangeNaviIconDefault();
		ChangeMainContentDefault();

		Debug.Log("Click");
	}


	//Back Navi Icon Defalut
	private void ChangeNaviIconDefault () {
		naviIconAC.SetInteger("NavigationStatus", (int)NaviStatus.Default);
	}
	//Back NonViewer Defalut
	private void ChangeMainContentDefault () {
		nonViewerAC.SetBool("IsOpenNavigation", false);
	}
}
