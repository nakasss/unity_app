using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class PlayContentManager : MonoBehaviour {
	private static readonly string baseURL = "https://dl.dropboxusercontent.com/u/62976696/VideoStreamingTest/";
	private string videoName = "sample_scene";
	private float videoDuration = 98.0f; //TODO : It have to ge got by API and it has to be ms.

	private bool isPlayActive = false;
	private Texture dummyTexture = null;

	[SerializeField] private GameObject tergetScreen;
	[SerializeField] private Toggle playButton;
	[SerializeField] private CanvasGroup playButtonWrapper;
	[SerializeField] private CanvasGroup videoEndButtonsWrapper;
	[SerializeField] private Slider videoProgressBar;
	[SerializeField] private Text timeLabel;
	[SerializeField] private Text totalTimeLabel;
	[SerializeField] private CanvasGroup playUIGroup;
	[SerializeField] private Animator playButtonAnimator;
	[SerializeField] private Animator endVideoButtonsAnimator;
	[SerializeField] private Animator nonViewerAC;
	[SerializeField] private Animator playContentAC;
	[SerializeField] private Toggle viewerButton;
	[SerializeField] private Cardboard cbMain;

	//Timer
	private float progressTimer = 0.0f;
	//Progress Bar
	private bool isDragging = false; //This valiable is modified in ProgressBarDragManager.cs
	//Screen Touch
	private bool isScreenTouchBlocked = false;

	//PlayUIGroup Alpha Animation
	private bool isAlphaAnimating = false;
	private bool isUpAlpha = false;
	private float originAlpha = 0.0f;
	private AnimationCurve dissolveAnimationCurve = null;
	private float playUIshowingDuration = 3.0f;
	private float screenHoldingTime = 0.0f;
	private bool isUIControlling = false;
	//PlayButton

	//EndVideoButtons



	// Use this for initialization
	void Start () {
		//Add Event Listener
		videoProgressBar.onValueChanged.AddListener(delegate {DraggingValueChangeCheck();});

		//get Dummy Texture
		dummyTexture = tergetScreen.GetComponent<Renderer>().material.mainTexture;

	}
	
	// Update is called once per frame
	void Update () {

	}

	//Mainly used for Animation
	void LateUpdate() {
		if (!isPlayActive) return;

		//Time count up
		TimerCountUp();

		//Update progress bar
		GoProgressBar();

		//Count Screen Holding Time
		CountScreenHoldingTime();
		//Called Screen Touched
		OnScreenTouch();
		//UI Group Dissolve Animation
		ChangeUIGroupAlpha();

		#if UNITY_EDITOR
		//Detect Desk Movie End
		DetectDeskMovieEnd();
		#endif
	}





	public void InitPlayPage () {
		progressTimer = 0.0f;
		//Progress Bar
		isDragging = false; //This valiable is modified in ProgressBarDragManager.cs
		//Screen Touch
		isScreenTouchBlocked = false;

		//PlayUIGroup Alpha Animation
		isAlphaAnimating = false;
		isUpAlpha = false;
		originAlpha = 0.0f;
		dissolveAnimationCurve = null;
		playUIshowingDuration = 3.0f;
		screenHoldingTime = 0.0f;
		isUIControlling = false;


		//Disable Whole Play Area
		playUIGroup.alpha = 0.0f;
		playUIGroup.interactable = false;
		playUIGroup.blocksRaycasts = false;
		//init Playbutton
		DisablePlayButton();
		OffPlayButton();
		DisableProgressBar();
		//init Progress Bar
		SetSliderValue(0.0f);
		//init Timer Label
		SetTimeText("00:00");
		//Set Total Time
		totalTimeLabel.text = GetTimeTextByDeltaTime(videoDuration);

		//Start Load Video
		LoadVideo(videoName);


		isPlayActive = true;
	}

	public void DiscardPlayPage () {
		OffPlayButton();
		tergetScreen.GetComponent<Renderer>().material.mainTexture = dummyTexture;

		isPlayActive = false;
		UnLoadVideo();
	}



	/*
	 *  Video UI Manger
	 */
	 //Play UI Group
	void CountScreenHoldingTime () {
		if (!playUIGroup.interactable) return;

		if (!isUIControlling) {
			if (screenHoldingTime > playUIshowingDuration) {
				TogglePlayUIGroupAlpha();
				screenHoldingTime = 0.0f;
				return;
			}

			if (IsVideoPlaying()) {
				screenHoldingTime += Time.deltaTime;
			}
		}
	}
	 //This is modified in Event Trigger from Editor
	public void OnStartUIControlling () {
		isUIControlling = true;
		//reset holding timer
		screenHoldingTime = 0.0f;
	}
	 //This is modified in Event Trigger from Editor
	public void OnEndUIControlling () {
		isUIControlling = false;
	}
	void TogglePlayUIGroupAlpha () {
		if (playUIGroup.interactable) {
			DownUIGroupAlpha();
			screenHoldingTime = 0.0f;
		} else {
			UpUIGroupAlpha();
		}

		playUIGroup.interactable = !playUIGroup.interactable;
		playUIGroup.blocksRaycasts = !playUIGroup.blocksRaycasts;
	}
	void DownUIGroupAlpha () {
		if (playUIGroup.alpha <= 0.0f) return;

		SetDissolveAnimationCurve();

		originAlpha = playUIGroup.alpha;
		isUpAlpha = false;
		isAlphaAnimating = true;
	}
	void UpUIGroupAlpha () {
		if (playUIGroup.alpha >= 1.0f) return;

		SetDissolveAnimationCurve();

		originAlpha = playUIGroup.alpha;
		isUpAlpha = true;
		isAlphaAnimating = true;
	}
	void SetDissolveAnimationCurve () {
		float animationDuration = 0.2f;
		float key1InTangent = 0.0f;
		float key1OutTangent = 0.1f;
		float key2InTangent = 0.0f;
		float key2OutTangent = 0.0f;

		Keyframe keyFrame1 = new Keyframe(Time.time, 0.0f, key1InTangent, key1OutTangent);
		Keyframe keyFrame2 = new Keyframe(Time.time + animationDuration, 1.0f, key2InTangent, key2OutTangent);
		dissolveAnimationCurve = new AnimationCurve(keyFrame1, keyFrame2);
	}
	void ChangeUIGroupAlpha () {
		if (!isAlphaAnimating || dissolveAnimationCurve == null) {
			return;
		}

		if (Time.time >= dissolveAnimationCurve.keys[dissolveAnimationCurve.length-1].time) {
			//Finish Animation
			isAlphaAnimating = false;
			//Adjust Alpha
			playUIGroup.alpha = isUpAlpha ? 1.0f : 0.0f;

			return;
		}

		float newAlphaValue = 1.0f * dissolveAnimationCurve.Evaluate(Time.time);
		if (isUpAlpha) {
			playUIGroup.alpha = originAlpha + (1.0f * dissolveAnimationCurve.Evaluate(Time.time));
		} else {
			playUIGroup.alpha = originAlpha - (1.0f * dissolveAnimationCurve.Evaluate(Time.time));
		}
	}
	//Screen Touch Manager
	//Called In LateUpate
	void OnScreenTouch() {
		if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) || Input.GetMouseButtonUp(0)) {
			if (isScreenTouchBlocked) {
				isScreenTouchBlocked = false;
			} else {
				TogglePlayUIGroupAlpha();
			}
		}
	}
	//This is called in Event Trigger from Editor
	public void BlockScreenTouch () {
		isScreenTouchBlocked = true;
	}

	// Middle Buttons
	void ToggleMiddleButtonsWithoutAnimation () {

		if (playButtonWrapper.interactable) {
			//Deactivate Play Button
			playButtonWrapper.alpha = 0.0f;
			playButtonWrapper.interactable = false;
			playButtonWrapper.blocksRaycasts = false;

			//Activate End Video Button
			videoEndButtonsWrapper.alpha = 1.0f;
			videoEndButtonsWrapper.interactable = true;
			videoEndButtonsWrapper.blocksRaycasts = true;
		} else {
			//Activate Play Button
			playButtonWrapper.alpha = 1.0f;
			playButtonWrapper.interactable = true;
			playButtonWrapper.blocksRaycasts = true;

			//Deactivate End Video Button
			videoEndButtonsWrapper.alpha = 0.0f;
			videoEndButtonsWrapper.interactable = false;
			videoEndButtonsWrapper.blocksRaycasts = false;
		}
	}
	void ToggleMiddleButtonsWithAnimation () {
		if (playButtonWrapper.interactable) {
			playButtonAnimator.SetBool("IsAppear", false);
			endVideoButtonsAnimator.SetBool("IsShow", true);
		} else {
			playButtonAnimator.SetBool("IsAppear", true);
			endVideoButtonsAnimator.SetBool("IsShow", false);
		}
	}

	// Play Button
	// Play Button Event Method
	void OnPlayButtonChanged (bool isOn) {
		if (isOn) {
			//Resume
			Resume();
		} else {
			//Pause
			Pause();
		}
	}
	void EnablePlayButton () {
		playButton.enabled = true;
	}
	void DisablePlayButton () {
		playButton.enabled = false;
	}
	void OnPlayButton () {
		playButton.isOn = true;	
	}
	void OffPlayButton () {
		playButton.isOn = false;	
	}

	// Replay Button
	public void OnReplayButtonClicked () {
		ToggleMiddleButtonsWithAnimation();
		TogglePlayUIGroupAlpha();

		Restart();
	}

	//Facebook Button
	public void OnFBButtonClicked () {
		string shareURL = "http://www.scopic.nl/"; //Test

		OpenApplicationManager.ShareToFB(shareURL);
	}

	//Email Button
	public void OnEmailButtonClicked () {
		string address = "";
		//test
		string subject = "Scopic VR Player";
		//test
		string body = "Scopic made this amazing 360 recruitment video for EY. Check out the link below, and don't forget to look around by moving your smartphone/tablet, or to click-and-drag from on your computer. Enjoy! http://www.scopic.nl/page-ey/";

		OpenApplicationManager.OpenEmailApp(address, subject, body);
	}



	// Time Label
	void TimerCountUp () {
		if (!IsVideoPlaying() || isDragging) return;

		progressTimer += Time.deltaTime;
		SetTimeText(GetTimeTextByDeltaTime(progressTimer));
	}
	string GetTimeTextByDeltaTime (float timer) {
		string timeText = "";
		float minute = timer / 60.0f;
		float second = timer % 60.0f;

		//generate time string
		if (minute < 10.0f) {
			timeText += "0";
		}
		timeText += ((int)minute).ToString() + ":";

		if (second < 10.0f) {
			timeText += "0";
		}
		timeText += ((int)second).ToString();
		
		return timeText;
	}

	void SetTimeText (string text) {
		timeLabel.text = text;	
	}

	// Progress Bar
	void GoProgressBar () {
		if (!IsVideoPlaying() || isDragging || videoDuration == -1.0f) return;

		//set progress with Timer
		float progressValue = progressTimer / videoDuration;
		SetSliderValue(progressValue);
	}
	//Called in ProgressBarDragManager.cs
	public void BeginDraggingValueChangeCheck () {
		isDragging = true;
	}
	//Called in ProgressBarDragManager.cs
	public void EndDraggingValueChangeCheck () {
		isDragging = false;

		//Toggle Middle Button
		//if (IsVideoEnd()) {}
		if (!playButtonWrapper.interactable) {
			ToggleMiddleButtonsWithAnimation();
		}
		

		//Restart on Editor
		JumpTo(progressTimer);
	}
	// Event Attached to videoProgressBar
	public void DraggingValueChangeCheck () {
		if (!isDragging) return;

		//get progress pointing time
		progressTimer = videoDuration * videoProgressBar.value;

		SetTimeText(GetTimeTextByDeltaTime(progressTimer));
	}
	void EnableProgressBar () {
		videoProgressBar.enabled = true;
	}
	void DisableProgressBar () {
		videoProgressBar.enabled = false;
	}
	void SetSliderValue (float value) {
		videoProgressBar.value = value;
	}


	//Back Arrow Icon
	public void OnBackArrowClicked () {
		//Set screen orientation
		Screen.orientation = ScreenOrientation.Portrait;
		
		DiscardPlayPage();
		nonViewerAC.SetBool("IsOpenPlay", false);
		playContentAC.SetBool("IsOpenPlay", false);
	}

	//Viewer Icon Toggle
	public void OnViewerIconClicked (bool isOn) {
		//Toggel VR mode
		cbMain.VRModeEnabled = viewerButton.isOn;
	}





	/*
	 *  Common Video Manager
	 */
	void LoadVideo (string video) {
		string url = "";
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
			url = baseURL + video + ".mp4";
		} else {
			url = baseURL + video + ".ogv";
		}

		#if UNITY_EDITOR
		StartCoroutine(LoadDeskMovie(url));
		#elif UNITY_IPHONE || UNITY_ANDROID
		LoadMobileMovie(url);
		#endif
	}

	void Resume () {
		#if UNITY_EDITOR
		ResumeDeskMovie();
		#elif UNITY_IPHONE || UNITY_ANDROID
		ResumeMobileMovie();
		#endif
	}

	void Pause () {
		#if UNITY_EDITOR
		PauseDeskMovie();
		#elif UNITY_IPHONE || UNITY_ANDROID
		PauseMobileMovie();
		#endif
	}

	void JumpTo (float sec) {
		//sec to msec
		int msec = (int)(sec * 1000);

		#if UNITY_EDITOR
		Restart();
		#elif UNITY_IPHONE || UNITY_ANDROID
		JumpToMovileMovie(msec);
		#endif
	}

	void Rewind () {
		#if UNITY_EDITOR
		RewindDesktMovie();
		#elif UNITY_IPHONE || UNITY_ANDROID
		RewindMobileMovie();
		#endif		
	}

	void Restart () {
		//Init UI
		progressTimer = 0.0f;
		DisablePlayButton();
		OffPlayButton();
		SetTimeText("00:00");
		SetSliderValue(0.0f);

		//Restart Video
		#if UNITY_EDITOR
		ReplayDeskMovieFromBegin();
		#elif UNITY_IPHONE || UNITY_ANDROID
		ReplayMobileMovieFromBegin();
		#endif
		
		//Start UI
		EnablePlayButton();
		OnPlayButton();
	}

	void UnLoadVideo () {
		#if UNITY_EDITOR
		UnLoadDeskVideo();
		#elif UNITY_IPHONE || UNITY_ANDROID
		UnloadMobileMovie();
		#endif
	}

	bool IsVideoReady () {
		#if UNITY_EDITOR
		return deskMovie.isReadyToPlay;
		#elif UNITY_IPHONE || UNITY_ANDROID
		return IsMobileMovieReady();
		#endif
	}

	bool IsVideoPlaying () {
		#if UNITY_EDITOR
		return deskMovie.isPlaying;
		#elif UNITY_IPHONE || UNITY_ANDROID
		return IsPlayingMobileMovie();
		#endif
	}

	bool IsVideoEnd () {
		#if UNITY_EDITOR
		return IsDeskMovieEnd();
		#elif UNITY_IPHONE || UNITY_ANDROID
		return IsEndMobileMovie();
		#endif
	}

	void OnVideoEnd () {
		//Rewind Video
		Rewind();

		OffPlayButton();

		if (playButtonWrapper.interactable) {
			ToggleMiddleButtonsWithAnimation();
		}

		if (!playUIGroup.interactable) {
			TogglePlayUIGroupAlpha();
		}
	}




	/*
	 *  Video Manager for Mobile
	 */
	#if UNITY_IPHONE || UNITY_ANDROID
	[SerializeField] private MediaPlayerCtrl easyMovieTexture;
	void LoadMobileMovie (string url) {
		//modify easy movie setting
		easyMovieTexture.m_bLoop = false;
		easyMovieTexture.m_bAutoPlay = false;

		//set Target Material
		easyMovieTexture.m_TargetMaterial = new GameObject[]{tergetScreen};

		//Load Movie
		easyMovieTexture.Load(url);

		//Detect when video is ready//
		easyMovieTexture.OnReady = () => {
			//Enable Play Button
			OnPlayButton();
			EnablePlayButton();
			EnableProgressBar();

			StartPlayMobileMovie();
		};
	}

	void StartPlayMobileMovie () {
		easyMovieTexture.Play();

		//Call when mobile movie end
		if (easyMovieTexture.OnEnd == null) {
			easyMovieTexture.OnEnd = () => {
				//Call OnVideoEnd
				OnVideoEnd();
			};
		}
	}

	void PauseMobileMovie () {
		easyMovieTexture.Pause();
	}

	void ResumeMobileMovie () {
		easyMovieTexture.Play();
	}

	void RewindMobileMovie () {
		//Stop Video and Rewind
		easyMovieTexture.Stop();

		//Reset Call when mobile movie end
		if (easyMovieTexture.OnEnd != null) {
			easyMovieTexture.OnEnd = null;
		}
	}

	void ReplayMobileMovieFromBegin () {
		//Stop Video and Rewind
		RewindMobileMovie();

		//Start
		StartPlayMobileMovie();
	}

	void JumpToMovileMovie (int msec) {
		easyMovieTexture.SeekTo(msec);

		if (!IsPlayingMobileMovie()) {
			//Update first frame
		}

		//Call when mobile movie end
		if (easyMovieTexture.OnEnd == null) {
			easyMovieTexture.OnEnd = () => {
				//Call OnVideoEnd
				OnVideoEnd();
			};
		}
	}

	void UnloadMobileMovie () {
		easyMovieTexture.Stop();
		easyMovieTexture.UnLoad();		
	}

	bool IsPlayingMobileMovie () {
		if (easyMovieTexture.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING) {
			return true;
		} else {
			return false;
		}
	}

	bool IsMobileMovieReady () {
		if (easyMovieTexture.GetCurrentState() != MediaPlayerCtrl.MEDIAPLAYER_STATE.NOT_READY && easyMovieTexture.GetCurrentState() != MediaPlayerCtrl.MEDIAPLAYER_STATE.ERROR) {
			return true;
		} else {
			return false;
		}
	}

	bool IsEndMobileMovie () {
		if (easyMovieTexture.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.END) {
			return true;
		} else {
			return false;
		}
	}
	#endif




	/*
	 *  Video Manager for Desktop
	 */
	#if UNITY_EDITOR
	private MovieTexture deskMovie;
	private AudioSource audioSource;
	private float timeUntilEnd = 0.0f;
	private bool isDeskMovieEndDetected = false;
	//Load Movie
	IEnumerator LoadDeskMovie (string url) {
		WWW www = new WWW(url);
		deskMovie = www.movie;

		timeUntilEnd = 0.0f;
		isDeskMovieEndDetected = false;

		while (!deskMovie.isReadyToPlay) {
	    	yield return null;
	    }

	    //Set Movie
		tergetScreen.GetComponent<Renderer>().material.mainTexture = deskMovie;
		//Get AudioSource
		audioSource = tergetScreen.GetComponent<AudioSource>();
		//Set Audio
		audioSource.clip = deskMovie.audioClip;
		//Get Duration
		//videoDuration = deskMovie.duration;
		//deactive loop
		deskMovie.loop = false;
		audioSource.loop = false;

		//Enable Play Button
		OnPlayButton();
		EnablePlayButton();
		EnableProgressBar();

		//StartPlay
		StartPlayDeskMovie();
	}

	void StartPlayDeskMovie () {
		deskMovie.Play();
		audioSource.Play();
	}

	void PauseDeskMovie () {
		if (deskMovie.isPlaying) {
			deskMovie.Pause();
			audioSource.Pause();
		}
	}

	void ResumeDeskMovie () {
		if (!deskMovie.isPlaying) {
			deskMovie.Play();
			audioSource.UnPause();
		}
	}

	void RewindDesktMovie () {
		deskMovie.Stop();
		audioSource.Stop();
		audioSource.clip = deskMovie.audioClip;
	}

	void ReplayDeskMovieFromBegin () {
		//Stop Video and Rewind
		RewindDesktMovie();

		if (!playButtonWrapper.interactable) {
			ToggleMiddleButtonsWithAnimation();
		}

		//Start
		StartPlayDeskMovie();
	}

	bool IsDeskMovieEnd () {

		if (Mathf.Round(progressTimer) >= videoDuration) {
			return true;
		} else {
			return false;
		}
	}

	void DetectDeskMovieEnd () {
		if (!deskMovie.isPlaying) return;

		if (IsDeskMovieEnd() && !isDeskMovieEndDetected) {
			timeUntilEnd = 0.0f;
			isDeskMovieEndDetected = true;
		}

		//Call VideoEnd After 0.5 sec after rough detect Video end
		if (isDeskMovieEndDetected) {
			timeUntilEnd += Time.deltaTime;

			if (timeUntilEnd >= 1f) {
				isDeskMovieEndDetected = false;
				timeUntilEnd = 0.0f;
				OnVideoEnd();				
			}
		}
	}

	void UnLoadDeskVideo () {
		deskMovie.Stop();
		deskMovie = null;
	}
	#endif


}
