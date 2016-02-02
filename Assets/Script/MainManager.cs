using UnityEngine;
using System.Collections;

public class MainManager : MonoBehaviour {
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
		//Set screen orientation
		Screen.orientation = ScreenOrientation.Portrait;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void OnVideoCellClick () {
		MoveToBeforePlay();
	}



	void MoveToBeforePlay () {
		mainContentAC.SetInteger("PageID", (int)MainContentPageID.BeforePlay);
		naviIconAC.SetInteger("NavigationStatus", (int)NaviStatus.Back);
	}
}
