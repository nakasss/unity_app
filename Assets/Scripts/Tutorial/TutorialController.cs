using UnityEngine;
using System.Collections;


public class TutorialController : MonoBehaviour {
	//private static readonly string turotialMovieURL = "https://storage.googleapis.com/me-nakas-test-bucket-8th-feb/sample-movie/sample_scene.mp4";

	[SerializeField] private TutorialView view;
	[SerializeField] private TutotialModel model;


	// Use this for initialization
	void Start () {
		//InitController();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	/*
	 * Init
	 */
	public void Init () {
		model.Cam.UseTutorialCam ();
	}


	/*
	 * Tutorial Root
	 */
	#region Tutorial Root

	[SerializeField] VRScreenView vrScreenView;
	[SerializeField] Texture tutorialTexture;

	public delegate void ScreenShow();
	public ScreenShow OnShow;

	public void OnShowTutorial () {

		if (OnShow != null) {
			OnShow ();
		}

		// Call them when bg image is ready
		vrScreenView.SetTexture(tutorialTexture);
		view.MoveLogoToTop ();
		view.ShowTutorial ();
		view.HideBg ();
	}

	#endregion Tutorial Root


	/*
	 * Logo
	 */
	#region Logo

	public delegate void LogoAnimation();
	public LogoAnimation OnCompleteLogoMove;

	public void OnMoveLogoInTop () {
		view.SetLogoInTop ();

		if (OnCompleteLogoMove != null) {
			OnCompleteLogoMove ();
		}
	}

	#endregion Logo


	/*
	 * Tutorial
	 */
	#region Tutorial

	[SerializeField] private UIRootView uiView;

	public void OnClickGetStartedButton () {
		vrScreenView.SetDefaultTexture ();
		uiView.GoMain ();
		model.Cam.UseSingleCam ();
	}

	#endregion Tutorial

}
