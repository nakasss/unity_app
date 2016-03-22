using UnityEngine;
using System.Collections;


public class SplashController : MonoBehaviour {

	[SerializeField] private SplashView view;
	[SerializeField] private SplashModel model;

	private static readonly bool DEBUG_MODE = true;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	/*
	 * Init
	 */
	[SerializeField] VRScreenView vrScreenView;

	public void Init () {
		vrScreenView.SetDefaultTexture ();	
	}


	/*
	 * Splash Root
	 */
	#region Splash Root
	[SerializeField] private SplashCanvasView splashCnavasView;

	public delegate void ScreenShow();
	public ScreenShow OnShow;

	public void OnShowSplash () {
		if (OnShow != null) {
			OnShow ();
		}

		if (FirstOpenManager.IsFirstOpen (DEBUG_MODE)) {
			splashCnavasView.GoTutorial ();
		} else {

			if (model.Api.IsLoaded ()) {
				view.MoveLogoToTop ();
			} else {
				model.Api.OnLoaded += view.MoveLogoToTop;
			}

			/*
			if (DEBUG_MODE) {
				if (model.Api.IsLoaded ()) {
					view.MoveLogoToTop ();
				} else {
					model.Api.OnLoaded += view.MoveLogoToTop;
				}
			} else {
				if (model.Api.IsReady ()) {
					view.MoveLogoToTop ();
				} else {
					model.Api.OnReady += view.MoveLogoToTop;
				}
			}
			*/
		}
	}

	#endregion Splash Root



	/*
	 * Logo
	 */
	#region Logo

	[SerializeField] private UIRootView uiView;

	public delegate void LogoAnimation();
	public LogoAnimation OnCompleteLogoMove;


	public void OnMoveLogoInTop () {
		view.SetLogoInTop ();

		if (OnCompleteLogoMove != null) {
			OnCompleteLogoMove ();
		}

		uiView.GoMain ();
		//splashCnavasView.DestorySplashCanvas ();
	}

	#endregion Logo


}
