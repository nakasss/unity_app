using UnityEngine;
using System.Collections;

public class MainCanvasView : MonoBehaviour {
	[SerializeField] private Animator mainCanvasAnimator;
	[SerializeField] private NavigationBarView naviBarView;

	private static readonly string MAIN_CANVAS_PAGE_STATE_KEY_NAME = "IsPageOpen";
	/*
	enum MAIN_CANVAS_STATUS : int {
		SHOW_PAGE = 1,
		OPEN_DRAWER = -1
	}
	*/


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	/*
     * Show Page
     */
	public void ShowPages () {
		naviBarView.SetNaviDefault ();
		mainCanvasAnimator.SetBool (MAIN_CANVAS_PAGE_STATE_KEY_NAME, true);
	}

	public bool IsOpenPages () {
		return mainCanvasAnimator.GetBool (MAIN_CANVAS_PAGE_STATE_KEY_NAME);
	}


	/*
	 * Open Drawer Menu
	 */

	public void OpenDrawer () {
		naviBarView.SetNaviXMark ();
		mainCanvasAnimator.SetBool (MAIN_CANVAS_PAGE_STATE_KEY_NAME, false);
	}

	public bool IsOpenDrawer () {
		return !mainCanvasAnimator.GetBool (MAIN_CANVAS_PAGE_STATE_KEY_NAME);
	}
}
