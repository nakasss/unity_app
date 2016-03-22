using UnityEngine;
using System.Collections;

public class DrawerMenuController : MonoBehaviour {
	[SerializeField] private PagesView pagesView;
	[SerializeField] private MainCanvasView mainCanvasView;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	/*
     * On Click Home
     */
	public void OnClickHomeMenu () {
		pagesView.GoToTop ();
		mainCanvasView.ShowPages ();
	}

	/*
	 * On Click About
	 */
	public void OnClickAboutMenu () {
		pagesView.GoToAboutUs ();
		mainCanvasView.ShowPages ();
	}

	/*
	 * On Click Contact
	 */
	public void OnClickContactMenu () {
		pagesView.GoToContact ();
		mainCanvasView.ShowPages ();
	}

	/*
	 * On Click Margin Space in menu
	 */
	public void OnClickMenuArea () {
		mainCanvasView.ShowPages ();
	}
}
