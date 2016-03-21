using UnityEngine;
using System.Collections;

public class RootCanvasView : MonoBehaviour {
	[SerializeField]
	private Animator rootCanvasAnimator;

	private enum RootStatus : int {
		Show = 0,
		Navigation = 1,
		Hide = 2
	}

	private enum PageStatus : int {
		Main = 0,
		AboutUs = 1,
		Contact = 2,
		BeforePlay = 3
	}

	private enum NaviIconStatus : int {
		Menu = 0,
		Back = 1,
		XMark = 2
	}




	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}




	/*
	 * Animation
	 */
	// -- Root -- 
	public void ShowRoot () {
		rootCanvasAnimator.SetInteger("Root", (int)RootStatus.Show);
	}

	public void HideRoot () {
		rootCanvasAnimator.SetInteger("Root", (int)RootStatus.Hide);
	}

	public void ShowNavigation () {
		rootCanvasAnimator.SetInteger("Root", (int)RootStatus.Navigation);
	}

	// -- Pages -- 
	public void ShowMainPage () {
		rootCanvasAnimator.SetInteger("Pages", (int)PageStatus.Main);	
	}

	public void ShowAboutPage () {
		rootCanvasAnimator.SetInteger("Pages", (int)PageStatus.AboutUs);	
	}

	public void ShowContactPage () {
		rootCanvasAnimator.SetInteger("Pages", (int)PageStatus.Contact);	
	}

	public void ShowBeforePlayPage () {
		rootCanvasAnimator.SetInteger("Pages", (int)PageStatus.BeforePlay);	
	}

	// -- Navi Icon -- 
	public void ShowNaviMenu () {
		rootCanvasAnimator.SetInteger("NaviIcon", (int)NaviIconStatus.Menu);	
	}

	public void ShowNaviBack () {
		rootCanvasAnimator.SetInteger("NaviIcon", (int)NaviIconStatus.Back);
	}

	public void ShowNaviXMark () {
		rootCanvasAnimator.SetInteger("NaviIcon", (int)NaviIconStatus.XMark);
	}

}
