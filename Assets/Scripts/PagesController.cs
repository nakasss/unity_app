using UnityEngine;
using System.Collections;

public class PagesController : MonoBehaviour {
	[SerializeField] private PagesView view;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	/*
     * On Click Pages Screen
     */
	[SerializeField] private MainCanvasView mainCanvasView;
	[SerializeField] private DrawerMenuView drawerMenuView;

	public void OnClickPagesScreen () {
		if (drawerMenuView.IsDrawerOpen ()) {
			mainCanvasView.ShowPages ();
		}
	}

}
