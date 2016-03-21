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
	 * Camera Manager
	 */
	[SerializeField] private CamerasManager camManager;

	public CamerasManager Cam {
		get { return camManager; }
	}
}
