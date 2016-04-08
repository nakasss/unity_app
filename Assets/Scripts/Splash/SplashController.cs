using UnityEngine;
using System.Collections;


public class SplashController : MonoBehaviour {

	[SerializeField] private SplashView view;
	[SerializeField] private SplashModel model;

	private static readonly bool DEBUG_MODE = false;


	// Use this for initialization
	void Start () {
		Init ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	/*
	 * Init
	 */
	[SerializeField] VRScreenView vrScreenView;

	public void Init () {
		if (NetworkManager.IsReachableNetwork()) {
			model.Api.StartLoadAPI ();
		}
		model.Api.StartLoadAPI ();

		vrScreenView.SetDefaultTexture ();

	}


	/*
	 * Splash Root
	 */
	#region Splash Root

	[SerializeField] private SplashCanvasView splashCnavasView;
	[SerializeField] private NetworkPopupController networkPopupController;
	[SerializeField] private NetworkPopupView networkPopupView;

	public delegate void ScreenShow();
	public ScreenShow OnShow;

	public void OnShowSplash () {
		if (OnShow != null) {
			OnShow ();
		}

		MoveTutorialOrMain ();
	}

	public void MoveTutorialOrMain () {
		// Network Connection Check
		if (!NetworkManager.IsReachableNetwork()) {
			networkPopupView.ShowNetworkDisconnection ();
			networkPopupController.OnRetryNetworkConnect = () => {
				if (!model.Api.IsLoaded ()) {
					model.Api.StartLoadAPI ();
				}
				MoveTutorialOrMain ();
			};
			return;
		}

		if (FirstOpenManager.IsFirstOpen (DEBUG_MODE)) {
			MoveTutorial ();
		} else {
			MoveMain ();
		}
	}

	public void MoveTutorial () {
		splashCnavasView.GoTutorial ();
	}

	public void MoveMain () {
		if (model.Api.IsLoaded ()) {
			view.MoveLogoToTop ();
		} else {
			model.Api.OnLoaded += view.MoveLogoToTop;
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
	}

	#endregion Logo


}
