using UnityEngine;
using System.Collections;


public class SplashView : MonoBehaviour {
	
	[SerializeField] private Animator splashAnimator;


	// Use this for initialization
	void Start () {
		//Set Screen Orientation
		Screen.orientation = ScreenOrientation.Portrait;
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	/*
	 * Splash Root
	 */
	#region Splash Root

	private static readonly string SPLASH_PAGE_STATE_KEY_NAME = "IsShow";


	public void Show () {
		splashAnimator.SetBool (SPLASH_PAGE_STATE_KEY_NAME, true);
	}

	public void Hide () {
		splashAnimator.SetBool (SPLASH_PAGE_STATE_KEY_NAME, false);
	}

	public bool IsShow () {
		return splashAnimator.GetBool (SPLASH_PAGE_STATE_KEY_NAME);
	}

	#endregion Splash Root


	/*
	 * Logo
	 */
	#region Logo

	private static readonly string SPLASH_LOGO_STATE_KEY_NAME = "IsReadyToMove";
	private static readonly string SPLASH_LOGO_STATE_TOP_KEY_NAME = "IsLogoInTop";


	public void MoveLogoToTop () {
		splashAnimator.SetBool (SPLASH_LOGO_STATE_KEY_NAME, true);
	}

	public void SetLogoInTop () {
		if (splashAnimator.GetBool (SPLASH_LOGO_STATE_KEY_NAME)) {
			splashAnimator.SetBool (SPLASH_LOGO_STATE_TOP_KEY_NAME, true);
		}
	}

	public bool IsLogoInTop () {
		if (splashAnimator.GetBool (SPLASH_LOGO_STATE_KEY_NAME)) {
			return splashAnimator.GetBool (SPLASH_LOGO_STATE_TOP_KEY_NAME);	
		} else {
			return false;
		}
	}

	#endregion Logo


}
