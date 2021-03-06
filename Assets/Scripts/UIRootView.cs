﻿using UnityEngine;
using System.Collections;

public class UIRootView : MonoBehaviour {


	// Use this for initialization
	void Start () {

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
	[SerializeField] private CamerasManager camManager;

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
//		Screen.orientation = ScreenOrientation.LandscapeLeft; //test

		uiAnimator.SetInteger (UI_SCENES_ID_KEY_NAME, (int)SCENE_ID.SPLASH);
	}

	public bool IsInSplash () {
		return GetCurrentSceneId () == (int)SCENE_ID.SPLASH;
	}


	/*
	 * Main
	 */
	public void GoMain () {
		Screen.orientation = ScreenOrientation.Portrait;
//		Screen.orientation = ScreenOrientation.LandscapeLeft; //test

		uiAnimator.SetInteger (UI_SCENES_ID_KEY_NAME, (int)SCENE_ID.MAIN);
	}

	public void GoMainFromPlay () {
		playController.DestoryPlay ();

		GoMain ();
	}

	public bool IsInMain () {
		return GetCurrentSceneId () == (int)SCENE_ID.MAIN;
	}


	/*
	 * Play
	 */
	[SerializeField] private VRScreenView vrScreenView;

	public void GoPlay (long id, string videoPath = null) {
		vrScreenView.UpsideDownForiOS ();
		uiAnimator.SetInteger (UI_SCENES_ID_KEY_NAME, (int)SCENE_ID.PLAY);

		Screen.orientation = ScreenOrientation.LandscapeLeft;
		playController.InitPlay (id, videoPath);
	}

	public bool IsInPlay () {
		return GetCurrentSceneId () == (int)SCENE_ID.PLAY;
	}


	/*
	 * Get Current Scene ID
	 */
	public int GetCurrentSceneId () {
		return uiAnimator.GetInteger (UI_SCENES_ID_KEY_NAME);
	}

	#endregion UI Scenes
}
