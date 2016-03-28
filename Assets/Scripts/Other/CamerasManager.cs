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
    /*
	public void ToggleCamActive () {
		if (IsSingleCamEnabled ()) {
			DisableSingleCam ();
			EnableVRCam ();
		} else {
			EnableSingleCam ();
			DisableVRCam ();
		}
	}
    */

	public void UseSingleCam () {
		Debug.Log ("Use Single Cam");
		EnableSingleCam ();

		if (IsVRCamEnabled ()) {
			DisableVRCam ();
		}
      	
		if (IsTutorialCamEnabled ()) {
			Debug.Log ("Desable Tutorial Cam");
			DisableTutorialCam ();
		}
	}

	public void UseVRCam () {
		Debug.Log ("Use VR Cam");
		EnableVRCam ();

		if (IsSingleCamEnabled ()) {
			DisableSingleCam ();
		}
			
		if (IsTutorialCamEnabled ()) {
			DisableTutorialCam ();
		}
	}

	public void UseTutorialCam () {
		Debug.Log ("Use Tutorial VR Cam");
		//Screen.orientation = ScreenOrientation.LandscapeLeft;

		EnableTutorialCam ();

		if (IsSingleCamEnabled ()) {
			DisableSingleCam ();
		}
        
		if (IsVRCamEnabled ()) {
			DisableVRCam ();
		}

		//Screen.orientation = ScreenOrientation.Portrait;
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
		return singleCam.activeSelf;
	}


	/*
	 * VR Camera
	 */
	#region VR Camera
	[HeaderAttribute("VR Main Camera")]
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

	private void EnableVRCam () {
		vrCam.SetActive (true);
	}

	private void DisableVRCam () {
		vrCam.SetActive (false);
	}
    
	private bool IsVRCamEnabled () {
		return vrCam.activeSelf;
	}

	#endregion VR Camera


	/*
	 * Tutorial Camera
	 */
     
    #region Tutorial Camera
    
    [HeaderAttribute("Tutorial Camera")]
	[SerializeField] private GameObject vrTutorialCam;
	[SerializeField] private GameObject vrTutorialHeaderCam;
    
	private void EnableTutorialCam () {
		if (vrTutorialCam == null) {
			Debug.Log ("Tutorial Cam is missing");
			return;
		}
		
        // Tilt
        #if UNITY_ANDROID
		Quaternion rotation = vrTutorialHeaderCam.transform.rotation;
        rotation.z = 1.0f;
		vrTutorialHeaderCam.transform.rotation = rotation;
        #endif
        
        vrTutorialCam.SetActive(true);
	}

	private void DisableTutorialCam () {
		Debug.Log ("Yeash Desable Tutorial Cam");
		if (vrTutorialCam == null) {
			Debug.Log ("Tutorial Cam is missing");
			return;
		}

        vrTutorialCam.SetActive(false);
        
        // Back Tilt
		Quaternion rotation = vrTutorialHeaderCam.transform.rotation;
        rotation.z = 0;
		vrTutorialHeaderCam.transform.rotation = rotation;

		GameObject.Destroy (vrTutorialCam);
	}

	private bool IsTutorialCamEnabled () {
		if (vrTutorialCam == null) {
			Debug.Log ("Tutorial Cam is missing");
			return false;
		} else {
			return vrTutorialCam.activeSelf;
		}
	}
        
    #endregion Tutorial Camera
}
