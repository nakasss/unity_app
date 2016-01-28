using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WelcomeManager : MonoBehaviour {
	//sample Video
	private string videoName = "sample_scene";

	[SerializeField] private StreamWelcomeMovieManager streamWelcomeVideo;
	[SerializeField] private Animator scrollViewAnimator;
	[SerializeField] private Animator logoAnimator;
	[SerializeField] private Animator bgAnimator;
	[SerializeField] private SplashManager splashManager;

	// Use this for initialization
	void Start () {
		//Load Video
		streamWelcomeVideo.LoadVideo(videoName);		
	}
	
	// Update is called once per frame
	void Update () {
		OnFinishLoadVideo();

		OnFinishLoboAnimation();
	}




	//Event 
	private void OnFinishLoadVideo () {
		if (streamWelcomeVideo.IsReadyToPlay() && !scrollViewAnimator.GetBool("IsReadyToShow")) {
			logoAnimator.SetBool("IsReadyToMove", true);
			streamWelcomeVideo.PlayVideo();
		}
	}
	private void OnFinishLoboAnimation () {
		if (logoAnimator.GetBool("CompleteAllAnimations")) {
			scrollViewAnimator.SetBool("IsReadyToShow", true);
			bgAnimator.SetBool("IsMovieStart", true);
		}
	}
}
