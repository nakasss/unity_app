using UnityEngine;
using System.Collections;

public class UIRootView : MonoBehaviour {


	// Use this for initialization
	void Start () {
		//GoMain ();
		GoSplash();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	/*
	 * UI Scenes
	 */
	#region UI Scenes

	[SerializeField] private Animator uiAnimator;
	[SerializeField] private PlayController playController;

	private static readonly string UI_SCENES_ID_KEY_NAME = "SceneId";
	public enum SCENE_ID : int {
		MAIN = 0,
		PLAY = 1,
		SPLASH = 2
	}


	/*
	 * Splash
	 */
	public void GoSplash () {
		Screen.orientation = ScreenOrientation.Portrait;

		uiAnimator.SetInteger (UI_SCENES_ID_KEY_NAME, (int)SCENE_ID.SPLASH);
	}

	public void DestorySplash () {
		
	}


	/*
	 * Main
	 */
	public void GoMain () {
		Screen.orientation = ScreenOrientation.Portrait;

		uiAnimator.SetInteger (UI_SCENES_ID_KEY_NAME, (int)SCENE_ID.MAIN);
	}


	/*
	 * Play
	 */
	public void GoPlay (long id, string videoPath = null) {
		uiAnimator.SetInteger (UI_SCENES_ID_KEY_NAME, (int)SCENE_ID.PLAY);

		Screen.orientation = ScreenOrientation.LandscapeLeft;

		playController.InitPlay (id, videoPath);
	}


	/*
	 * Get Current Scene ID
	 */
	public int GetCurrentSceneId () {
		return uiAnimator.GetInteger (UI_SCENES_ID_KEY_NAME);
	}

	#endregion UI Scenes
}
