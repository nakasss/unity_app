using UnityEngine;
using System.Collections;


public class UIRootController : MonoBehaviour {

	[SerializeField] private UIRootView view;
	[SerializeField] private UIRootModel model;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


		#if UNITY_ANDROID
		// Android Back Button
		OnTouchAndroidBackButton ();
		#endif
	}


	/*
	 * Android Back Button
	 */
	#region Android Back Button

	[SerializeField] private MainCanvasView mainCnavasView;
	[SerializeField] private PagesView pagesView;
	[SerializeField] private VRScreenView vrScreenView;

	private static readonly float ANDROID_BACKBUTTON_TOUCH_INTERVAL_MSEC = 0.3f;
	private static readonly float ANDROID_BACKBUTTON_TOUCH_INTERVAL_MAX = 1.0f;
	private float currentIntervalMsec = 0.0f;

	public void OnTouchAndroidBackButton () {
		if (Application.platform != RuntimePlatform.Android) {
			return;
		}

		if (currentIntervalMsec < ANDROID_BACKBUTTON_TOUCH_INTERVAL_MAX) {
			currentIntervalMsec += Time.deltaTime;
		}

		if (Input.GetKey(KeyCode.Escape)) {
			if (currentIntervalMsec < ANDROID_BACKBUTTON_TOUCH_INTERVAL_MSEC) {
				return;
			} else {
				currentIntervalMsec = 0.0f;
			}

			if (view.IsInPlay ()) {
				view.GoMainFromPlay ();
				model.Cam.UseSingleCam ();
				vrScreenView.SetDefaultTexture ();

			} else if (pagesView.IsInBeforePlay ()) {
				pagesView.GoToTop ();

			} else if (mainCnavasView.IsOpenDrawer ()) {
				mainCnavasView.ShowPages ();

			} else {

				Application.Quit ();
			}

		}
	}

	#endregion Android Back Button
		

	/*
	 * Main
	 */
	#region Main

	[SerializeField] private SplashCanvasView splashCnavasView;

	public void OnChangeToMain () {
		
		if (splashCnavasView != null) {
			splashCnavasView.DestorySplashCanvas ();
		}
	}

	#endregion Main
}
