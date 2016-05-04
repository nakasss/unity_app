using UnityEngine;
using UnityEngine.iOS;
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
		EnableSingleCam ();

		if (IsVRCamEnabled ()) {
			DisableVRCam ();
		}
      	
		if (IsTutorialCamEnabled ()) {
			DisableTutorialCam ();
		}
	}

	public void UseVRCam () {
		EnableVRCam ();

		if (IsSingleCamEnabled ()) {
			DisableSingleCam ();
		}
			
		if (IsTutorialCamEnabled ()) {
			DisableTutorialCam ();
		}
	}

	public void UseTutorialCam (bool instantUse = true) {
		instantUseOrNot = instantUse;
		EnableTutorialCam ();

		if (IsSingleCamEnabled ()) {
			DisableSingleCam ();
		}
        
		if (IsVRCamEnabled ()) {
			DisableVRCam ();
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
	private bool instantUseOrNot = true;
    
	private void EnableTutorialCam () {
		if (vrTutorialCam == null) {
			return;
		}
		
        // Tilt
		#if !UNITY_EDITOR
		#if UNITY_ANDROID || UNITY_IPHONE
		Quaternion rotation = vrTutorialHeaderCam.transform.rotation;
        rotation.z = 1.0f;
		vrTutorialHeaderCam.transform.rotation = rotation;
        #endif
		#endif
        
        vrTutorialCam.SetActive(true);
	}

	private void DisableTutorialCam () {
		if (vrTutorialCam == null) {
			return;
		}

        vrTutorialCam.SetActive(false);
        
        // Back Tilt
		Quaternion rotation = vrTutorialHeaderCam.transform.rotation;
        rotation.z = 0;
		vrTutorialHeaderCam.transform.rotation = rotation;

		if (instantUseOrNot) {
			GameObject.Destroy (vrTutorialCam);
		}
	}

	private bool IsTutorialCamEnabled () {
		if (vrTutorialCam == null) {
			return false;
		} else {
			return vrTutorialCam.activeSelf;
		}
	}
        
    #endregion Tutorial Camera
}
