using UnityEngine;
using System.Collections;


public class ContactPageManager : MonoBehaviour {


	public void OnPhoneIconClicked () {
		string scopicPhoneNum = "+31624343765";

		OpenApplicationManager.OpenPhoneApp(scopicPhoneNum);
	}

	public void OnEmailIconClicked () {
		string scopicAddress = "info@scopic.nl";

		OpenApplicationManager.OpenEmailApp(scopicAddress, "", "");
	}

	public void OnMapIconClicked () {
		string scopicAddress = "129,tolstraat,Amsterdam,1074VJ,Netherlands";

		OpenApplicationManager.OpenMapApp(scopicAddress);
	}
}
