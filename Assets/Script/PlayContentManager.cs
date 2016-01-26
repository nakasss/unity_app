using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class PlayContentManager : MonoBehaviour {
	private static readonly string baseURL = "https://dl.dropboxusercontent.com/u/62976696/VideoStreamingTest/";
	private string videoName = "sample_scene";

	[SerializeField] private GameObject tergetScreen;
	[SerializeField] private Toggle playButton;
	[SerializeField] private Slider videoProgressBar;
	[SerializeField] private Text timeLabel;

	private AudioSource audioSource;
	private float videoDuration = -1.0f;

	//Timer 
	private float progressTimer = 0.0f;

	//Progress Bar
	private bool isDragging = false; //This valiable is modified in ProgressBarDragManager.cs


	// Use this for initialization
	void Start () {
		//init Playbutton
		DisablePlayButton();
		OffPlayButton();
		//init Progress Bar
		SetSliderValue(0.0f);
		videoProgressBar.onValueChanged.AddListener(delegate {DraggingValueChangeCheck();});
		//init Timer Label
		SetTimeText("00:00");
		//Get AudioSource
		audioSource = tergetScreen.GetComponent<AudioSource>();


		LoadVideo(videoName);
	}
	
	// Update is called once per frame
	void Update () {
		TimerCountUp();
		GoProgressBar();
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
		SetTimeTextByDeltaTime(progressTimer);
	}

	void SetTimeTextByDeltaTime (float timer) {
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
		
		SetTimeText(timeText);
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
		Restart();
	}
	// Event Attached to videoProgressBar
	public void DraggingValueChangeCheck () {
		if (!isDragging) return;

		//get progress pointing time
		progressTimer = videoDuration * videoProgressBar.value;

		SetTimeTextByDeltaTime(progressTimer);
	}

	void SetSliderValue (float value) {
		videoProgressBar.value = value;
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
		#elif UNITY_IPHONE || 	UNITY_ANDROID
		
		#endif
	}

	void Resume () {
		#if UNITY_EDITOR
		ResumeDeskMovie();
		#elif UNITY_IPHONE || 	UNITY_ANDROID
		
		#endif
	}

	void Pause () {
		#if UNITY_EDITOR
		PauseDeskMovie();
		#elif UNITY_IPHONE || 	UNITY_ANDROID
		
		#endif
	}

	void JumpTo (int position) {

	}

	void Restart () {
		//Init UI
		progressTimer = 0.0f;
		DisablePlayButton();
		OffPlayButton();
		SetTimeText("00:00");
		SetSliderValue(0.0f);

		//Start Video
		#if UNITY_EDITOR
		ReplayDeskMovieFromBegin();
		#elif UNITY_IPHONE || 	UNITY_ANDROID

		#endif
		
		//Start UI
		EnablePlayButton();
		OnPlayButton();
	}

	bool IsVideoReady () {
		bool flag = true;
		#if UNITY_EDITOR
		flag = deskMovie.isReadyToPlay;
		#elif UNITY_IPHONE || 	UNITY_ANDROID

		#endif
		return flag;
	}

	bool IsVideoPlaying () {
		bool flag = true;
		#if UNITY_EDITOR
		flag = deskMovie.isPlaying;
		#elif UNITY_IPHONE || 	UNITY_ANDROID

		#endif
		return flag;
	}




	/*
	 *  Video Manager for Mobile
	 */
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

			easyMovieTexture.SetVolume(0.0f);
			StartPlayMobileMovie();
		};

		//Play Video
		easyMovieTexture.Play();
	}

	void StartPlayMobileMovie () {
		easyMovieTexture.Play();
	}




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
		videoDuration = deskMovie.duration;
		//deactive loop
		deskMovie.loop = false;
		audioSource.loop = false;

		//Enable Play Button
		OnPlayButton();
		EnablePlayButton();

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
