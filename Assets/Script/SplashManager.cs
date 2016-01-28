using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class SplashManager : MonoBehaviour {
	//Only For Debug
	private bool isFirstOpen = true;

	private bool isScreenShow = false;

	[SerializeField] private Animator splashAnimator;



	void Awake () {
		DontDestroyOnLoad(this);
	}

	// Use this for initialization
	void Start () {
		StartMainSceneLoad();
		
	}
	
	// Update is called once per frame
	void Update () {
		OnSplashReady();
		OnCompleteLogoAnimation();
	}



	/*
	 * Screen Animation Event
	 */
	private void OnScreenShow () {
		isScreenShow = true;
	}

	private void OnSplashReady () {
		if (isMainSceneLoaded && isScreenShow) {
			if (isFirstOpen) {
				MoveToWelcome();
			} else {
				logoAnimator.SetBool("IsReadyToMove", true);
			}
		}
	}


	/*
	 * Logo Animation Asset
	 */
	[SerializeField] private Animator logoAnimator;
	private void OnCompleteLogoAnimation () {
		if (logoAnimator.GetBool("CompleteAllAnimations")) {
			MoveToMain();
		}
	}




	/*
	 * Scene Load Asset
	 */
	private AsyncOperation asyncMainScene;
	private bool isMainSceneLoaded = false;

	private void StartMainSceneLoad () {
		StartCoroutine(LoadingSceneAsync("Main"));
	}

	private void OnCompleteLoadScene () {
		isMainSceneLoaded = true;
	}

	private IEnumerator LoadingSceneAsync (string sceneName) {
        asyncMainScene = Application.LoadLevelAsync(sceneName);
        asyncMainScene.allowSceneActivation = false;
        //asyncs[sceneName] = async;

        while (asyncMainScene.progress < 0.9f) {
            yield return new WaitForEndOfFrame();
        }

        //Call End event
        OnCompleteLoadScene();
    }


    /*
	 * Move To new page
	 */
	[SerializeField] private GameObject welcomeContent;
	[SerializeField] private GameObject welcomeScreen;
	[SerializeField] private GameObject welcomeSubCamera;

    public void MoveToMain () {
		if (isMainSceneLoaded) {
			asyncMainScene.allowSceneActivation = true;
		}
	}

	public void MoveToWelcome () {
		welcomeContent.SetActive(true);
		welcomeScreen.SetActive(true);
		welcomeSubCamera.SetActive(true);
		splashAnimator.SetBool("SplashShow", false);
	}
}
