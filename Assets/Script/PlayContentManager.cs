using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class PlayContentManager : MonoBehaviour {
	private static readonly string targetURL = "https://dl.dropboxusercontent.com/u/62976696/VideoStreamingTest/sample_scene.ogv";

	[SerializeField] private GameObject tergetScreen;
	[SerializeField] private Toggle playButton;
	[SerializeField] private Slider videoProgressBar;
	[SerializeField] private Text timeLabel;

	private AudioSource audioSource;
	private float videoDuration = -1.0f;

	//Timer 
	private float timer = 0.0f;
	private float minutesNum = 0.0f;
	private float secondsNum = 0.0f;

	//Progress Bar
	[HideInInspector] public bool isDragging = false; //This valiable is modified in ProgressBarDragManager.cs


	// Use this for initialization
	void Start () {
		//init Playbutton
		DisablePlayButton();
		OffPlayButton();
		//init Progress Bar
		SetSliderValue(0.0f);
		//init Timer Label
		SetTimeText("00:00");
		audioSource = tergetScreen.GetComponent<AudioSource>();

		LoadVideo(targetURL);
	}
	
	// Update is called once per frame
	void Update () {
		TimerCountUp();
		//GoProgressBar();

		if (isDragging) {
			Debug.Log("Dragging");
		}
	}



	bool IsVideoReady () {
		bool flag = deskMovie.isReadyToPlay;
		return flag;
	}

	bool IsVideoPlaying () {
		bool flag = deskMovie.isPlaying;
		return flag;
	}



	/*
	 *  Video UI Manger
	 */
	// Play Button
	// Play Button Event Method
	void OnPlayButtonChanged (bool isOn) {
		if (isOn) {
			//Resume
			ResumeDeskMovie();
		} else {
			//Pause
			PauseDeskMovie();
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
		if (!IsVideoPlaying()) return;

		string timeText = "";
		float deltaTime = Time.deltaTime;
		timer += deltaTime;
		secondsNum += deltaTime;

		if (secondsNum > 60.0f) {
			minutesNum += 1.0f;
			secondsNum = 0.0f;
		}

		//generate time string
		if (minutesNum < 10.0f) {
			timeText += "0";
		}
		timeText += ((int)minutesNum).ToString() + ":";

		if (secondsNum < 10.0f) {
			timeText += "0";
		}
		timeText += ((int)secondsNum).ToString();
		
		SetTimeText(timeText);
	}

	void SetTimeText (string text) {
		timeLabel.text = text;	
	}

	// Progress Bar
	void GoProgressBar () {
		if (!IsVideoPlaying() || videoDuration == -1.0f) return;

		//set progress with Timer
		float progressValue = timer / videoDuration;
		SetSliderValue(progressValue);
	}

	void SetSliderValue (float value) {
		videoProgressBar.value = value;
	}




	/*
	 *  Common Video Manager
	 */
	void LoadVideo (string url) {
		StartCoroutine(LoadDeskMovieByWWW(url));
	}



	/*
	 *  Video Manager for Desktop
	 */
	private MovieTexture deskMovie;
	//Load Movie
	IEnumerator LoadDeskMovieByWWW (string url) {
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
}
