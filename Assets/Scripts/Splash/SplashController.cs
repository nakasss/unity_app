using UnityEngine;
using System.Collections;


public class SplashController : MonoBehaviour {

	[SerializeField] private SplashView view;
	[SerializeField] private SplashModel model;

	private static readonly bool DEBUG_MODE = false;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	/*
	 * Init
	 */
	public void Init () {
		
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
			view.MoveLogoToTop ();
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
		splashCnavasView.DestorySplashCanvas ();
	}

	#endregion Logo


}
