using UnityEngine;
using System.Collections;

public class AboutUsManager : MonoBehaviour {


	public void OnScopicWebButtonClicked () {
		OpenApplicationManager.OpenAppByURL("http://www.scopic.nl/");
	}

	public void OnScopicFBButtonClicked () {
		OpenApplicationManager.OpenFBPage("scopicvideo");
	}

	public void OnScopicInstaButtonClicked () {
		OpenApplicationManager.OpenInsta("scopic_vr");
	}
}
