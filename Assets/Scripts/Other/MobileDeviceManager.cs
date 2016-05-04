using UnityEngine;
using UnityEngine.iOS;
using System.Collections;


public class MobileDeviceManager {

	public static bool IsiPhone6Later () {
		if (UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhone6 ||
		    UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhone6Plus ||
		    UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhone6S ||
		    UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhone6SPlus ||
			UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhoneUnknown) {
//			Debug.Log ("Later iPhone");
			return true;
		} else {
//			Debug.Log ("Before iPhone");
			return false;
		}
	}
}


