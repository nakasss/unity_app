using UnityEngine;
using System.Collections;

public class WelcomeCBCamRotationManager : MonoBehaviour {
	[SerializeField] private Transform cbHeadTransform;

	// Use this for initialization
	void Start () {

		#if UNITY_EDITOR
		
		#elif UNITY_IPHONE || UNITY_ANDROID
		//Set screen orientation
		Screen.orientation = ScreenOrientation.Portrait;
		
		CorrectCamRotation();
		#endif
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void CorrectCamRotation () {
		if (Screen.orientation == ScreenOrientation.Portrait) {
			Vector3 eulerAngles = cbHeadTransform.eulerAngles;
			eulerAngles.z = eulerAngles.z - 270;
			cbHeadTransform.eulerAngles = eulerAngles;
		}
	}

}
