using UnityEngine;
using System.Collections;

public class SplashModel : MonoBehaviour {

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
	 * Camera Manager
	 */
	[SerializeField] private CamerasManager camManager;

	public CamerasManager Cam {
		get { return camManager; }
	}
}
