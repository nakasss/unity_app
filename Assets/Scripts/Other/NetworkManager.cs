using UnityEngine;
using System.Collections;

public class NetworkManager {

	public static bool IsReachableNetwork () {
		return (IsReachableWifi () || IsReachableCarrierNetwork ());
	}

	public static bool IsReachableWifi () {
		return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
	}

	public static bool IsReachableCarrierNetwork () {
		return Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork;
	}
}
