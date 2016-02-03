using UnityEngine;
using System.Collections;


public class SplashView : MonoBehaviour {
	private bool isSplashShow;
	private bool isLogoInTop;

	[SerializeField]
	private Animator splashAnimator;


	// Use this for initialization
	void Start () {
		//Set Screen Orientation
		Screen.orientation = ScreenOrientation.Portrait;

		InitView();	
	}
	
	// Update is called once per frame
	void Update () {
	
	}



	public void InitView () {
		isSplashShow = false;
		isLogoInTop = false;
	}



	/*
	 * UI manager
	 */
	public void HidePage () {
		splashAnimator.SetBool("IsShow", false);
	}

	public void LogoMoveToTop () {
		splashAnimator.SetBool("IsReadyToMove", true);
	}


	/*
	 * UI status
	 */
	public bool IsSplashShow () {
		return isSplashShow;
	}

	public bool IsLogoInTop () {
		return isLogoInTop;
	}


	/*
	 * UI event
	 */
	public delegate void ScreenShow();
	public delegate void LogoAnimation();

	public ScreenShow OnShow;
	public LogoAnimation OnComplete;


	//called in animator
	private void OnSplashScreenShow () {
		isSplashShow = true;

		if (OnShow != null) {
			OnShow();			
		}
	}

	//called in animator
	private void OnLogoAnimationComplete () {
		isLogoInTop = true;

		if (OnComplete != null) {
			OnComplete();			
		}	
	}
}
