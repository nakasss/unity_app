using UnityEngine;
using System.Collections;

public class PlayCamManager : MonoBehaviour {
	[SerializeField]
	private Cardboard cb;

	public static bool isVRModeOn = false;
	private bool currentMode = false;

	// Use this for initialization
	void Start () {
		currentMode = isVRModeOn;
	}
	
	// Update is called once per frame
	void Update () {
		OnVrModeChange();
	}

	private void OnVrModeChange () {
		if (currentMode != isVRModeOn) {
			cb.VRModeEnabled = isVRModeOn;
			currentMode = isVRModeOn;
		}
	}


	public static void SetVRMode (bool isOn) {
		isVRModeOn = isOn;
	}

	public static bool GetVRMode () {
		return isVRModeOn;
	}
}
