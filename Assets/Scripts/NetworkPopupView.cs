using UnityEngine;
using System.Collections;

public class NetworkPopupView : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	/*
	 * Network Popup Windows
	 */
	#region Network Popup Windows

	[SerializeField] private Animator networkWindowAnimator;
	[SerializeField] private PopupCanvasView popupCanvasView;
	private static readonly string NETWORK_WINDOW_ID_KEY = "NetworkWindowId";
	private enum POPUP_WINDOW_ID : int {
		HIDE_ALL = 0,
		WARN_DISCONNECTION = 1,
		WARN_WIFI = 2
	}

	public void HideAllNetworkWindow () {
		networkWindowAnimator.SetInteger (NETWORK_WINDOW_ID_KEY, (int)POPUP_WINDOW_ID.HIDE_ALL);

		if (popupCanvasView.IsShowPopups ()) {
			popupCanvasView.HidePopups ();
		}
	}

	#endregion Network Popup Windows


	/*
	 * WarnNetworkDisconnection
	 */
	#region WarnNetworkDisconnection

	public void ShowNetworkDisconnection () {
		if (!NetworkManager.IsReachableNetwork ()) {
			networkWindowAnimator.SetInteger (NETWORK_WINDOW_ID_KEY, (int)POPUP_WINDOW_ID.WARN_DISCONNECTION);
		} else {
			HideAllNetworkWindow ();
		}

		if (!popupCanvasView.IsShowPopups ()) {
			popupCanvasView.ShowPopups ();
		}
	}

	#endregion WarnNetworkDisconnection


	/*
	 * WarnUseWifi
	 */
	#region WarnUseWifi

	public void ShowWarnWifi () {
		if (!NetworkManager.IsReachableWifi ()) {
			networkWindowAnimator.SetInteger (NETWORK_WINDOW_ID_KEY, (int)POPUP_WINDOW_ID.WARN_WIFI);
		} else {
			HideAllNetworkWindow ();
		}

		if (!popupCanvasView.IsShowPopups ()) {
			popupCanvasView.ShowPopups ();
		}
	}

	#endregion WarnUseWifi
}
