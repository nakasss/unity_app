using UnityEngine;
using System.Collections;


public class TutorialController : MonoBehaviour {
	private static readonly string turotialMovieURL = "https://storage.googleapis.com/me-nakas-test-bucket-8th-feb/sample-movie/sample_scene.mp4";

	[SerializeField] private TutorialView view;
	[SerializeField] private TutotialModel model;
	[SerializeField]
	private SplashController splashController;
	[SerializeField]
	private MovieManager movieManager;


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

	public delegate void ScreenShow();
	public ScreenShow OnShow;

	public void OnShowTutorial () {

		if (OnShow != null) {
			OnShow ();
		}

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

	public void OnClickGetStartedButton () {
		Debug.Log ("Click Get Started Button");
	}

	#endregion Tutorial


	/*
	public void InitController () {

	}

	public void ActiveTutorial () {
		
		movieManager.OnVideoReady = () => {
			OnCompleteLoadTutorialMovie();
		};
		
		tutorialView.OnComplete = () => {
			OnCompleteLogoAnimation();
		};
		//Start Loading Tutorial Movie
		movieManager.LoadMovie(turotialMovieURL);
		//Show Page
		tutorialView.ShowPage();
	}

	//Move Logo after video load
	private void OnCompleteLoadTutorialMovie () {
		//splashController.DisableSplash();
		tutorialView.LogoMoveTop();
	}

	//Show tutorial after logo moved
	private void OnCompleteLogoAnimation () {
		tutorialView.HideBg();
		tutorialView.ShowTutorial();
	}


	public void ClickGetStartedBtn () {
		//splashController.GoMain();
	}
	*/
}
