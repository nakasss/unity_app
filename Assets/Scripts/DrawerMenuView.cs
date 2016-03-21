using UnityEngine;
using System.Collections;

public class DrawerMenuView : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	/*
	 * Drawer State
	 */
	[SerializeField] private Animator mainCanvasAnimator;
	private static readonly string MAIN_CANVAS_PAGE_STATE_KEY_NAME = "IsPageOpen";
	enum DRAWER_STATE : int {
		HIDE = 0,
		OPEN = 1
	}

	public int GetDrawerState () {
		bool isOpenPages = mainCanvasAnimator.GetBool (MAIN_CANVAS_PAGE_STATE_KEY_NAME);

		if (isOpenPages) {
			return (int)DRAWER_STATE.HIDE;
		} else {
			return (int)DRAWER_STATE.OPEN;
		}
	}

	public bool IsDrawerOpen () {
		return GetDrawerState () == (int)DRAWER_STATE.OPEN ? true : false;
	}
}
