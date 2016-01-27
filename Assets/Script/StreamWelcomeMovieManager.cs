using UnityEngine;
using System.Collections;

public class StreamWelcomeMovieManager : MonoBehaviour {
	private static readonly string baseURL = "https://dl.dropboxusercontent.com/u/62976696/VideoStreamingTest/";
	private string videoName = "sample_scene";

	[SerializeField] private GameObject tergetScreen;


	void Start () {
		LoadVideo(videoName);
	}
	
	void Update () {
	
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
		//Play in Editor
		StartCoroutine(LoadDeskMovie(url));
		#elif UNITY_IPHONE || 	UNITY_ANDROID
		//Play in Mobile platform
		LoadMobileMovie(url);
		#endif
	}



	/*
	 *  Video Manager for Mobile
	 */
	 #if UNITY_IPHONE || UNITY_ANDROID
	[SerializeField] private MediaPlayerCtrl easyMovieTexture;
	void LoadMobileMovie (string url) {
		//modify easy movie setting
		easyMovieTexture.m_bLoop = true;
		easyMovieTexture.m_bAutoPlay = false; //TODO : Force to true!!!

		//set Target Material
		easyMovieTexture.m_TargetMaterial = new GameObject[]{tergetScreen};

		//Load Movie
		easyMovieTexture.Load(url);

		//Set Delegate On Video Ready
		easyMovieTexture.OnReady = () => {
			easyMovieTexture.SetVolume(0.0f);
			StartPlayMobileMovie();
		};
	}

	void StartPlayMobileMovie () {
		easyMovieTexture.Play();
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
		//deactive loop
		deskMovie.loop = true;

		//StartPlay
		StartPlayDeskMovie();
	}

	void StartPlayDeskMovie () {
		deskMovie.Play();
	}

	void PauseDeskMovie () {
		if (deskMovie.isPlaying) {
			deskMovie.Pause();
		}
	}

	void ResumeDeskMovie () {
		if (!deskMovie.isPlaying) {
			deskMovie.Play();
		}
	}

	void ReplayDeskMovieFromBegin () {
		//Stop Video and Rewind
		deskMovie.Stop();

		//Start
		StartPlayDeskMovie();
	}
	#endif

}