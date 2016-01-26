using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class PlayContentManager : MonoBehaviour {
	private static readonly string baseURL = "https://dl.dropboxusercontent.com/u/62976696/VideoStreamingTest/";
	private string videoName = "sample_scene";
	private float videoDuration = 98.0f; //TODO : It have to ge got by API and it has to be ms.

	[SerializeField] private GameObject tergetScreen;
	[SerializeField] private Toggle playButton;
	[SerializeField] private Slider videoProgressBar;
	[SerializeField] private Text timeLabel;
	[SerializeField] private Text totalTimeLabel;
	[SerializeField] private CanvasGroup playUIGroup;
	[SerializeField] private Canvas rootCanvas;

	private AudioSource audioSource;

	//Timer 
	private float progressTimer = 0.0f;
	//Progress Bar
	private bool isDragging = false; //This valiable is modified in ProgressBarDragManager.cs
	//PlayUIGroup Alpha Animation
	private bool isAlphaAnimating = false;
	private bool isUpAlpha = false;
	private float originAlpha = 0.0f;
	private float destAlpha = 0.0f;
	private AnimationCurve dissolveAnimationCurve = null;
	//Screen Touch
	private float cameraDistance;



	// Use this for initialization
	void Start () {
		//init Playbutton
		DisablePlayButton();
		OffPlayButton();
		DisableProgressBar();
		//init Progress Bar
		SetSliderValue(0.0f);
		videoProgressBar.onValueChanged.AddListener(delegate {DraggingValueChangeCheck();});
		//init Timer Label
		SetTimeText("00:00");
		//Set Total Time
		totalTimeLabel.text = GetTimeTextByDeltaTime(videoDuration);
		//Get AudioSource
		audioSource = tergetScreen.GetComponent<AudioSource>();
		//Get Screen Distance
		cameraDistance = rootCanvas.planeDistance;

		//Start Load Video
		LoadVideo(videoName);
	}
	
	// Update is called once per frame
	void Update () {
		OnScreenTouch();
	}

	//Mainly used for Animation
	void LateUpdate() {
		//Time count up
		TimerCountUp();

		//Update progress bar
		GoProgressBar();

		//UI Group Dissolve Animation
		ChangeUIGroupAlpha();
	}




	/*
	 *  Video UI Manger
	 */
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

	//Play UI Group
	void TogglePlayUIGroupAlpha () {
		if (playUIGroup.interactable) {
			DownUIGroupAlpha();
		} else {
			UpUIGroupAlpha();
		}

		playUIGroup.interactable = !playUIGroup.interactable;
	}
	void DownUIGroupAlpha () {
		if (playUIGroup.alpha == 0.0f) return;

		SetDissolveAnimationCurve();

		originAlpha = playUIGroup.alpha;
		isUpAlpha = false;
		isAlphaAnimating = true;
	}
	void UpUIGroupAlpha () {
		if (playUIGroup.alpha == 1.0f) return;

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
			return;
		}

		float newAlphaValue = 1.0f * dissolveAnimationCurve.Evaluate(Time.time);
		if (isUpAlpha) {
			playUIGroup.alpha = originAlpha + (1.0f * dissolveAnimationCurve.Evaluate(Time.time));
		} else {
			playUIGroup.alpha = originAlpha - (1.0f * dissolveAnimationCurve.Evaluate(Time.time));
		}
	}

	//Screen Touch
	public void OnScreenTouch() {


		if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) || Input.GetMouseButtonUp(0)) {
			//TogglePlayUIGroupAlpha();
			GetTouchedObjectName(Input.mousePosition);
		}
	}

	string GetTouchedObjectName (Vector3 touchedPos) {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
		RaycastHit hit = new RaycastHit();

		string objectName = "";
		if (Physics.Raycast(ray.origin,ray.direction,out hit,Mathf.Infinity)) {
        	objectName = hit.collider.gameObject.name;
        	Debug.Log("Object name : " + objectName);
      	} else {
      		Debug.Log("Object name : can't get");
      	}

      	return objectName;
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
	}

	void PauseMobileMovie () {
		easyMovieTexture.Pause();
	}

	void ResumeMobileMovie () {
		easyMovieTexture.Play();
	}

	void ReplayMobileMovieFromBegin () {
		//Stop Video and Rewind
		easyMovieTexture.Stop();

		//Start
		StartPlayMobileMovie();
	}

	void JumpToMovileMovie (int msec) {
		easyMovieTexture.SeekTo(msec);

		if (!IsPlayingMobileMovie()) {
			//Update first frame
		}
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
	#endif




	/*
	 *  Video Manager for Desktop
	 */
	#if UNITY_EDITOR
	private MovieTexture deskMovie;
	//Load Movie
	IEnumerator LoadDeskMovie (string url) {
		WWW www = new WWW(url);
		deskMovie = www.movie;

		while (!deskMovie.isReadyToPlay) {
	    	yield return null;
	    }

	    //Set Movie
		tergetScreen.GetComponent<Renderer>().material.mainTexture = deskMovie;
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

	void ReplayDeskMovieFromBegin () {
		//Stop Video and Rewind
		deskMovie.Stop();
		audioSource.Stop();
		audioSource.clip = deskMovie.audioClip;

		//Start
		StartPlayDeskMovie();
	}
	#endif


}
