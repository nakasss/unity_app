using UnityEngine;
using System.Collections;


public class SplashController : MonoBehaviour {
	//Only For Debug
	private bool isFirstOpen = true;

	[SerializeField]
	private SplashView splashView;
	[SerializeField]
	private TutorialController tutorialController;
	[SerializeField]
	private MainSceneLoad mainSceneLoad;




	void Awake () {
		DontDestroyOnLoad(this);
	}

	// Use this for initialization
	void Start () {
		InitController();	
	}
	
	// Update is called once per frame
	void Update () {
	
	}



	public void InitController () {

		if (isFirstOpen) {
			//Go welcome
			splashView.OnShow = GoWelcome;
		} else {
			//Go Main

			//Logo Move when Load complete and after Splash Page show
			mainSceneLoad.OnComplete = MoveNaviTop;
			splashView.OnShow = MoveNaviTop;
			//Go to main when logo moves to Top
			splashView.OnComplete = GoMain;
		}

		mainSceneLoad.StartMainSceneLoad();
	}




	private void MoveNaviTop () {
		if (splashView.IsSplashShow() && mainSceneLoad.IsLoadCompleted()) {
			splashView.LogoMoveToTop();
		}
	}

	public void DisableSplash () {
		gameObject.SetActive(false);
	}

	public void GoMain () {
		if (splashView.IsSplashShow() && mainSceneLoad.IsLoadCompleted()) {
			mainSceneLoad.EnableMainScene();
		}
	}

	private void GoWelcome () {
		//gameObject.SetActive(false);
		tutorialController.ActiveTutorial();
	}


}
