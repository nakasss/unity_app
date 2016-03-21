using UnityEngine;
using System.Collections;

public class PlayModel : MonoBehaviour {


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	/*
	 * API Interface
	 */
	[SerializeField] private ScopicMobileAPIInterface apiInterface;
	public ScopicMobileAPIInterface Api {
		get { return apiInterface; }
	}


	/*
	 * Video Player
	 */
	[SerializeField] private MovieManager videoPlayer;
	public MovieManager VideoPlayer {
		get { return videoPlayer; }
	}


	/*
	 * Cameras Manager
	 */
	[SerializeField] private CamerasManager camManager;
	public CamerasManager CamManager {
		get { return camManager; }
	}
}
