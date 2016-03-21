using UnityEngine;
using System.Collections;

public class CamerasManager : MonoBehaviour {
	
	[SerializeField] private bool vRCamFirstEnabled = false;


	// Use this for initialization
	void Start () {
		InitCamManager ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	/*
	 * Init
	 */
	public void InitCamManager () {
		if (vRCamFirstEnabled) {
			UseVRCam ();
		} else {
			UseSingleCam ();
		}
	}


	/*
	 * Cam Group
	 */
	public void ToggleCamActive () {
		if (IsSingleCamEnabled ()) {
			DisableSingleCam ();
			EnableVRCam ();
		} else {
			EnableSingleCam ();
			DisableVRCam ();
		}
	}

	public void UseSingleCam () {
		EnableSingleCam ();

		if (IsVRCamEnabled ()) {
			DisableVRCam ();
		}

		if (IsTutorialCamEnabled ()) {
			DisableTutorialSubCam ();
		}
	}

	public void UseVRCam () {
		EnableVRCam ();

		if (IsSingleCamEnabled ()) {
			DisableSingleCam ();
		}

		if (IsTutorialCamEnabled ()) {
			DisableTutorialSubCam ();
		}
	}

	public void UseTutorialCam () {
		EnableTutorialCam ();

		if (IsSingleCamEnabled ()) {
			DisableSingleCam ();
		}
	}


	/*
	 * Single Camera
	 */
	[SerializeField] private GameObject singleCam;

	public void EnableSingleCam () {
		singleCam.SetActive (true);
	}

	public void DisableSingleCam () {
		singleCam.SetActive (false);
	}

	public bool IsSingleCamEnabled () {
		return singleCam.active;
	}


	/*
	 * VR Camera
	 */
	[SerializeField] private GameObject vrCam;
	private Cardboard cardborad; 
	public Cardboard VRCam {
		get {
			if (cardborad == null) {
				cardborad = vrCam.GetComponent<Cardboard> ();
			}
			return cardborad;
		}
	}

	public void EnableVRCam () {
		vrCam.SetActive (true);
	}

	public void DisableVRCam () {
		vrCam.SetActive (false);
	}

	public bool IsVRCamEnabled () {
		return vrCam.active;
	}


	/*
	 * Tutorial Camera
	 */
	[SerializeField] private Camera tutorialSubCam;

	public void EnableTutorialCam () {
		VRCam.VRModeEnabled = false;

		EnableVRCam ();
		EnableTutorialSubCam ();
	}

	public void DisableTutorialCam () {
		DisableTutorialSubCam ();
		DisableVRCam ();
	}

	public void EnableTutorialSubCam () {
		tutorialSubCam.gameObject.SetActive (true);
	}

	public void DisableTutorialSubCam () {
		tutorialSubCam.gameObject.SetActive (false);
	}

	public bool IsTutorialCamEnabled () {
		return tutorialSubCam.gameObject.active;
	}
}
