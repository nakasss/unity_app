using UnityEngine;
using System.Collections;

public class NetworkPopupController : MonoBehaviour {

	[SerializeField] private NetworkPopupView view;


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

	public delegate void NetworkPopupHide();
	public NetworkPopupHide OnHide;

	public void OnHideNetworkPopups () {
		if (OnHide != null) {
			OnHide ();

			OnHide = null;
		}
	}

	#endregion Network Popup Windows


	/*
	 * WarnNetworkDisconnection
	 */
	#region WarnNetworkDisconnection

	public delegate void RetryNetworkConnect();
	public RetryNetworkConnect OnRetryNetworkConnect;

	public void OnClickNetworkDisconnectionRetryButton () {
		OnHide = () => {
			if (OnRetryNetworkConnect != null) {
				OnRetryNetworkConnect();
			}
		};

		view.HideAllNetworkWindow ();
	}

	#endregion WarnNetworkDisconnection
}
