using UnityEngine;
using System.Collections;


public class TutorialController : MonoBehaviour {
	[SerializeField]
	private TutorialView tutorialView;
	[SerializeField]
	private SplashController splashController;
	[SerializeField]
	private MovieManager movieManager;


	// Use this for initialization
	void Start () {
		InitController();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void InitController () {

	}

	public void ActiveTutorial () {
		
		movieManager.OnComplete = () => {
			OnCompleteLoadTutorialMovie();
		};
		
		tutorialView.OnComplete = () => {
			OnCompleteLogoAnimation();
		};
		//Start Loading Tutorial Movie
		movieManager.LoadMovie("https://dl.dropboxusercontent.com/u/62976696/VideoStreamingTest/sample_scene.mp4");
		//Show Page
		tutorialView.ShowPage();
	}

	//Move Logo after video load
	private void OnCompleteLoadTutorialMovie () {
		splashController.DisableSplash();
		tutorialView.LogoMoveTop();
	}

	//Show tutorial after logo moved
	private void OnCompleteLogoAnimation () {
		tutorialView.HideBg();
		tutorialView.ShowTutorial();
	}


	public void ClickGetStartedBtn () {
		splashController.GoMain();
	}
}
