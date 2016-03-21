using UnityEngine;
using System.Collections;


[RequireComponent(typeof(MediaPlayerCtrl))]
public class MovieManager : MonoBehaviour {
	
	[SerializeField] private GameObject targetScreen;
	[SerializeField] private bool loopEnabled = false;
	[SerializeField] private bool autoPlayEnabled = false;
	[SerializeField] private bool audioEnabled = true;
	[SerializeField] private MediaPlayerCtrl easyMovieTexture;

	public delegate void LoadComplete();
	public LoadComplete OnVideoReady;
	public delegate void MovieEnd();
	public MovieEnd OnEnd;

//	private static readonly string TEST_URL = "https://storage.googleapis.com/scopic-mobile-app-storage/videos/sample_movie.ogv";



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}



	/*
	 *  Common Movie Manager
	 */
	//
	// -- Controller --
	//
	public void LoadMovie (string moviePath = null) {
//		if (!isMobilePlatform() || moviePath == null) {
//			moviePath = TEST_URL;
//		}

		#if UNITY_EDITOR
		StartCoroutine("LoadEditorMovie", moviePath);
		#elif UNITY_IPHONE || UNITY_ANDROID
		LoadMobileMovie(moviePath);
		#endif
	}

	public void Play () {
		#if UNITY_EDITOR
		StartPlayEditorMovie();
		#elif UNITY_IPHONE || UNITY_ANDROID
		StartPlayMobileMovie();
		#endif
	}

	public void Resume () {
		#if UNITY_EDITOR
		ResumeEditorMovie();
		#elif UNITY_IPHONE || UNITY_ANDROID
		ResumeMobileMovie();
		#endif
	}

	public void Pause () {
		#if UNITY_EDITOR
		PauseEditorMovie();
		#elif UNITY_IPHONE || UNITY_ANDROID
		PauseMobileMovie();
		#endif
	}

	public void JumpTo (float sec) {
		int msec = (int)(sec * 1000);

		#if UNITY_EDITOR
		Restart();
		#elif UNITY_IPHONE || UNITY_ANDROID
		JumpToMovileMovie(msec);
		#endif
	}

	public void Rewind () {
		#if UNITY_EDITOR
		RewindEditorMovie();
		#elif UNITY_IPHONE || UNITY_ANDROID
		RewindMobileMovie();
		#endif		
	}

	public void Restart () {
		//Restart Video
		#if UNITY_EDITOR
		ReplayEditorMovieFromBegin();
		#elif UNITY_IPHONE || UNITY_ANDROID
		ReplayMobileMovieFromBegin();
		#endif
	}

	public void UnLoadMovie () {
		#if UNITY_EDITOR
		UnLoadEditorVideo();
		#elif UNITY_IPHONE || UNITY_ANDROID
		UnloadMobileMovie();
		#endif
	}

	//
	// -- Staus update --
	//

	//
	// -- Check status  --
	//
	public bool IsMovieReady () {
		#if UNITY_EDITOR
		return IsEditorMovieReady();
		#elif UNITY_IPHONE || UNITY_ANDROID
		return IsMobileMovieReady();
		#endif
	}

	public bool IsMoviePlaying () {
		#if UNITY_EDITOR
		return IsPlayingEditorMovie();
		#elif UNITY_IPHONE || UNITY_ANDROID
		return IsPlayingMobileMovie();
		#endif
	}

	public bool IsMovieEnd () {
		#if UNITY_EDITOR
		//return IsDeskMovieEnd();
		return false;
		#elif UNITY_IPHONE || UNITY_ANDROID
		return IsEndMobileMovie();
		#endif
	}

	private bool isMobilePlatform () {
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
			return true;
		} else {
			return false;
		}
	}

	private bool isAndroid () {
		return Application.platform == RuntimePlatform.Android ? true : false;
	}

	private bool isIos () {
		return Application.platform == RuntimePlatform.IPhonePlayer ? true : false;
	}


	//
	// -- Event --
	//
	private void OnLoadComplete () {
		if (OnVideoReady != null) {
			OnVideoReady();
		}
	}

	private void OnMovieEnd () {
		if (OnEnd != null) {
			OnEnd();
		}
	}





	/*
	 *  Video Manager for Mobile
	 */
	#if UNITY_IPHONE || UNITY_ANDROID

	//
	// -- Controller --
	//
	private void LoadMobileMovie (string moviePath) {
		//modify easy movie setting
		easyMovieTexture.m_bLoop = loopEnabled;
		easyMovieTexture.m_bAutoPlay = autoPlayEnabled;

		//set Target Material
		if (targetScreen != null) {
			easyMovieTexture.m_TargetMaterial = new GameObject[]{targetScreen};
		}

		//Detect when video is ready//
		easyMovieTexture.OnReady = () => {
			if (!audioEnabled) {
				easyMovieTexture.SetVolume(0.0f);
			}
			OnLoadComplete();
			StartPlayMobileMovie();
		};
		//set end event
		easyMovieTexture.OnEnd = () => {
			OnMovieEnd();
		};

		//Load Movie
		easyMovieTexture.Load(moviePath);
	}

	private void StartPlayMobileMovie () {
		easyMovieTexture.Play();

		//Call when mobile movie end
		if (easyMovieTexture.OnEnd == null) {
			easyMovieTexture.OnEnd = () => {
				//Call OnVideoEnd
				OnMovieEnd();
			};
		}
	}

	private void PauseMobileMovie () {
		easyMovieTexture.Pause();
	}

	private void ResumeMobileMovie () {
		easyMovieTexture.Play();
	}

	private void RewindMobileMovie () {
		//Stop Video and Rewind
		easyMovieTexture.Stop();

		//Reset Call when mobile movie end
		if (easyMovieTexture.OnEnd != null) {
			easyMovieTexture.OnEnd = null;
		}
	}

	private void ReplayMobileMovieFromBegin () {
		//Stop Video and Rewind
		RewindMobileMovie();

		//Start
		StartPlayMobileMovie();
	}

	private void JumpToMovileMovie (int msec) {
		easyMovieTexture.SeekTo(msec);

		if (!IsPlayingMobileMovie()) {
			//Update first frame
		}

		//Call when mobile movie end
		if (easyMovieTexture.OnEnd == null) {
			easyMovieTexture.OnEnd = () => {
				//Call OnVideoEnd
				OnMovieEnd();
			};
		}
	}

	private void UnloadMobileMovie () {
		easyMovieTexture.Stop();
		easyMovieTexture.UnLoad();		
	}

	//
	// -- Status Update --
	//
	private void SetMobileScreenObject (GameObject screen) {
		targetScreen = screen;

		easyMovieTexture.m_TargetMaterial = new GameObject[]{targetScreen};
	}

	private void SetMobileLoop (bool loop) {
		loopEnabled = loop;
		easyMovieTexture.m_bLoop = loop;
	}

	private void SetMobileAutoPlay (bool autoPlay) {
		autoPlayEnabled = autoPlay;
		easyMovieTexture.m_bAutoPlay = autoPlay;
	}

	//
	// -- Status Check --
	//
	private bool IsPlayingMobileMovie () {
		if (easyMovieTexture.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING) {
			return true;
		} else {
			return false;
		}
	}

	private bool IsMobileMovieReady () {
		if (easyMovieTexture.GetCurrentState() != MediaPlayerCtrl.MEDIAPLAYER_STATE.NOT_READY && easyMovieTexture.GetCurrentState() != MediaPlayerCtrl.MEDIAPLAYER_STATE.ERROR) {
			return true;
		} else {
			return false;
		}
	}

	private bool IsEndMobileMovie () {
		if (easyMovieTexture.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.END) {
			return true;
		} else {
			return false;
		}
	}
	#endif




	/*
	 *  Video Manager for Editor
	 */
	#if UNITY_EDITOR
	// -- Controller --
	private MovieTexture editorMovie;
	private AudioSource audioSource;
	// -- Status Check --
	private bool isLoadCompleted = false;

	//
	// -- Controller --
	//
	private IEnumerator LoadEditorMovie (string url) {
		WWW www = new WWW(url);
		editorMovie = www.movie;

		while (!editorMovie.isReadyToPlay) {
	    	yield return null;
	    }

	    if (targetScreen != null) {
	    	targetScreen.GetComponent<Renderer>().material.mainTexture = editorMovie;

			if (audioEnabled) {
				audioSource = targetScreen.GetComponent<AudioSource>();
				audioSource.clip = editorMovie.audioClip;
			}
	    }
		
		//videoDuration = deskMovie.duration;
		editorMovie.loop = loopEnabled;

		if (audioEnabled) {
			audioSource.loop = loopEnabled;
		}

		OnLoadComplete();

		if (autoPlayEnabled) {
			StartPlayEditorMovie();
		}
	}

	private void StartPlayEditorMovie () {
		editorMovie.Play();

		if (audioEnabled) {
			audioSource.Play();
		}
	}

	private void PauseEditorMovie () {
		if (editorMovie.isPlaying) {
			editorMovie.Pause();
			if (audioEnabled) {
				audioSource.Pause();
			}
		}
	}

	private void ResumeEditorMovie () {
		if (!editorMovie.isPlaying) {
			editorMovie.Play();
			if (audioEnabled) {
				audioSource.UnPause();
			}
		}
	}

	private void RewindEditorMovie () {
		editorMovie.Stop();
		if (audioEnabled) {
			audioSource.Stop();
			audioSource.clip = editorMovie.audioClip;
		}
	}

	private void ReplayEditorMovieFromBegin () {
		//Stop Video and Rewind
		RewindEditorMovie();

		//Start
		StartPlayEditorMovie();
	}

	private void UnLoadEditorVideo () {
		editorMovie.Stop();
		editorMovie = null;

		if (audioEnabled) {
			audioSource.Stop();
			audioSource = null;
		}
	}

	//
	// -- Status Update --
	//
	private void SetEditorScreenObject (GameObject screen) {
		targetScreen = screen;

		if (isLoadCompleted) {
			targetScreen.GetComponent<Renderer>().material.mainTexture = editorMovie;

			if (audioEnabled) {
				audioSource = targetScreen.GetComponent<AudioSource>();
				audioSource.clip = editorMovie.audioClip;
			}
		}
	}

	private void SetEditorMovieLoop (bool loop) {
		editorMovie.loop = loop;
		if (audioEnabled) {
			audioSource.loop = loop;
		}
	}

	//
	// -- Status Check --
	//
	private bool IsPlayingEditorMovie () {
		return editorMovie.isPlaying;
	}

	private bool IsEditorMovieReady () {
		return editorMovie.isReadyToPlay;
	}


	#endif
	
}
