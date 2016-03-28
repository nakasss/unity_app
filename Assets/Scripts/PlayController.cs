using UnityEngine;
using System.Collections;


[RequireComponent(typeof(PlayView))]
[RequireComponent(typeof(PlayModel))]
public class PlayController : MonoBehaviour {
	
	[SerializeField] private PlayView view;
	[SerializeField] private PlayModel model;

	private long id = -1;
	public long ID {
		get { return id; }
		set { id = value; }
	}

	private string videoPath = "";
	private static readonly string TEST_VIDEO_PATH_EDITOR = "https://storage.googleapis.com/scopic-mobile-app-storage/videos/sample_movie.ogv";
	private static readonly string TEST_VIDEO_PATH_MOBILE = "https://storage.googleapis.com/scopic-mobile-app-storage/videos/sample_movie.mp4";
	public string VideoPath {
		get {
			return (Application.platform != RuntimePlatform.OSXEditor) ? videoPath : TEST_VIDEO_PATH_EDITOR;
//			return TEST_VIDEO_PATH_EDITOR; // Editor Test
//			return TEST_VIDEO_PATH_MOBILE; // Mobile Test
			//return videoPath;
		}
		set { videoPath = value; }
	}

	private float videoDuration = -1.0f;
	private static readonly float TEST_VIDEO_DURATION = 54.033f;
	public float VideoDuration {
		get {
			return (Application.platform != RuntimePlatform.OSXEditor) ? videoDuration : TEST_VIDEO_DURATION;
//			return TEST_VIDEO_DURATION; // Test
			//return videoDuration;
		}
		set { videoDuration = value; }
	}


	void Start () {
		/*
		 * Test
		 */
//		InitPlay (0);
		// End : Test
	}

	void Update () {
		updateVideoCount ();
		updateUiGroupHandlingCount ();
	}


	/*
	 * Init Play
	 */
	public void InitPlay (long id, string videoPath = null) {
		ID = id;

		if (videoPath != null) {
			VideoPath = videoPath;
		} else {
			VideoPath = model.Api.GetVideoURL (ID);
		}
		VideoDuration = model.Api.GetVideoDuration (ID);
		resetVideoCount ();
		resetUiGroupHandlingCount ();

		view.DisablePlayButton ();
		view.DisableProgressBar ();
		view.SetProgressTimerBySec (0);
		view.SetVideoTitle (model.Api.GetTitle (ID));
		view.SetTotalTimerBySec ((long)VideoDuration);
		view.SetProgress (0.0f);

		model.VideoPlayer.OnVideoReady = OnVideoLoadReady;
		model.VideoPlayer.OnEnd = OnVideoEnd;
		model.VideoPlayer.LoadMovie (VideoPath);
		model.CamManager.UseVRCam ();
	}


	/*
	 * Destory Play
	 */
    [SerializeField] private VRScreenView vrScreenView;
    
	public void DestoryPlay () {
		ID = -1;

		stopVideoCount ();
		stopUiGroupHandlingCount ();
		resetVideoCount ();
		resetUiGroupHandlingCount ();

		model.VideoPlayer.UnLoadMovie ();
		model.CamManager.UseSingleCam ();
        
        vrScreenView.SetDefaultTexture ();
	}


	/*
	 * Play UI Group
	 */
	#region Play UI Group

	/*
	 * UI Group
	 */
	public void OnClickPlayUIGroup () {
		// Click UI Group
	}

	public void OnPointerDownUIGroup () {
		stopUiGroupHandlingCount ();
		resetUiGroupHandlingCount ();
	}

	public void OnPointerUpUIGroup () {
		resetUiGroupHandlingCount ();
		startUiGroupHandlingCount ();
	}

	/*
	 * Non UI Group
	 */
	public void OnClickNonPlayUIGroup () {
		view.ToggleUIGroup ();

		if (view.IsShowUIGroup ()) {
			resetUiGroupHandlingCount ();
			startUiGroupHandlingCount ();
		} else {
			stopUiGroupHandlingCount ();
			resetUiGroupHandlingCount ();
		}
	}

	#endregion Play UI Group


	/*
	 * Top Area
	 */
	#region Top Area

	// Back Icon
	[SerializeField] private UIRootView uiView;

	public void OnClickBackIcon () {
		DestoryPlay ();

		model.CamManager.UseSingleCam ();
		uiView.GoMain ();
	}


	// Viewr Icon
	public void OnValueChangedViewrIcon (bool isOn) {
		if (view.IsViewerIconOn ()) {
			// On
			model.CamManager.VRCam.VRModeEnabled = true;
		} else {
			// Off
			model.CamManager.VRCam.VRModeEnabled = false;
		}
	}

	#endregion Top Area


	/*
	 * Middle Area
	 */
	#region Middle Area

	// Play Button
	public void OnClickPlayButton (bool isOn) {
		if (!view.IsPlayButtonPlay()) {
			model.VideoPlayer.Resume ();
			startVideoCount ();
		} else {
			model.VideoPlayer.Pause ();
			stopVideoCount ();
		}
		view.UpdatePlayButtonBg ();
	}

	// Replay Button
	public void OnClickReplayButton () {
		view.ToggleMiddleAreaButtons ();
		if (view.IsPlayButtonPlay()) {
			view.ShowPauseButton ();
		}

		model.VideoPlayer.Restart ();

		resetVideoCount ();
		startVideoCount ();
		resetUiGroupHandlingCount ();
		startUiGroupHandlingCount ();
	}

	// Facebook Button
	public void OnClickFacebookButton () {
		//string shareURL = "http://www.scopic.nl/"; //Test
		string shareURL = model.Api.GetFbContet (ID);

		OpenApplicationManager.ShareToFB(shareURL);
	}

	// Email Button
	public void OnClickEmailButton () {
		string address = "";
		//test
		string subject = "Scopic VR";
		//test
		string body = model.Api.GetEmailContet (ID);

		OpenApplicationManager.OpenEmailApp (address, subject, body);
	}
		
	#endregion End : Middle Area


	/*
	 * Bottom Area
	 */
	#region Bottom Area

	/*
	 * Progress Bar
	 */
	private bool isProgressBarDragged = false;

	public void OnProgressBarValueChanged () {
		view.SetProgressTimerBySec ((long)(VideoDuration * view.GetCurrentProgress()));
	}

	public void OnBeginDragProgressBar () {
		isProgressBarDragged = true;
	}

	public void OnEndDragProgressBar () {
		float changedTimeSec = VideoDuration * view.GetCurrentProgress ();

		if (Application.platform == RuntimePlatform.OSXEditor) {
			resetVideoCount ();
			view.SetProgress (0.0f);
		} else {
			setVideoCount (changedTimeSec);
		}

		if (model.VideoPlayer.IsMovieReady()) {
			model.VideoPlayer.JumpTo (changedTimeSec);
		}

		isProgressBarDragged = false;
	}

	public bool IsProgressBarDragged () {
		return isProgressBarDragged;
	}

	#endregion End : Bottom Area


	/*
	 * Video Player
	 */
	#region Video Event

	// Ready to play
	public void OnVideoLoadReady () {
		view.EnablePlayButton ();
		view.EnableProgressBar ();
		view.ShowPauseButton ();

		model.VideoPlayer.Play ();

		resetVideoCount ();
		startVideoCount ();
		resetUiGroupHandlingCount ();
		startUiGroupHandlingCount ();
	}

	// End 
	public void OnVideoEnd () {
		stopVideoCount ();

		view.ShowUIGroup ();
		view.ToggleMiddleAreaButtons ();
	}

	#endregion End : Video Event


	/*
	 * Video Internal Timer
	 */
	#region Video Internal Timer

	private float videoTimeCounter = 0.0f;
	private bool isVideoCounterUpdating = false;

	public void OnUpdateVideoCount () {
		if (!IsProgressBarDragged()) {
			view.SetProgress (videoTimeCounter / VideoDuration);
		}

		#if UNITY_EDITOR
		if (videoTimeCounter >= VideoDuration) {
			OnVideoEnd ();
		}
		#endif
	}

	private void updateVideoCount () {
		if (isVideoCounterUpdating) {
			videoTimeCounter += Time.deltaTime;
			OnUpdateVideoCount ();
		}
	}

	private void startVideoCount () {
		isVideoCounterUpdating = true;
	}

	private void stopVideoCount () {
		isVideoCounterUpdating = false;
	}

	private void setVideoCount (float sec) {
		if (VideoDuration < sec || sec < 0)
			return;

		videoTimeCounter = sec;
		OnUpdateVideoCount ();
	}

	private void resetVideoCount () {
		videoTimeCounter = 0.0f;

		OnUpdateVideoCount ();
	}

	#endregion End : Video Internal Timer


	/*
	 * UI Handling Counter
	 */
	#region UI Handling Counter

	private float uiGroupHnadlingCounter = 0.0f;
	private bool isUiGroupCounterUpdating = false;
	private static readonly float MAX_UI_GROUP_HANDLING_TIME = 3.0f;

	public void OnUpdateUIGroupHandlingCount () {
		
	}

	private void updateUiGroupHandlingCount () {
		if (!isUiGroupCounterUpdating)
			return;
		
		uiGroupHnadlingCounter += Time.deltaTime;
		OnUpdateUIGroupHandlingCount ();
		if (uiGroupHnadlingCounter > MAX_UI_GROUP_HANDLING_TIME) {
			stopUiGroupHandlingCount ();
			resetUiGroupHandlingCount ();

			if (model.VideoPlayer.IsMoviePlaying() && !model.VideoPlayer.IsMovieEnd()) {
				view.ToggleUIGroup ();
			}
		}
	}

	private void startUiGroupHandlingCount () {
		isUiGroupCounterUpdating = true;
	}

	private void stopUiGroupHandlingCount () {
		isUiGroupCounterUpdating = false;
	}

	private void resetUiGroupHandlingCount () {
		uiGroupHnadlingCounter = 0.0f;

		OnUpdateUIGroupHandlingCount ();
	}

	#endregion UI Handling Counter
}
