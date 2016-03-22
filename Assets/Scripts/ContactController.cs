using UnityEngine;
using System.Collections;

public class ContactController : MonoBehaviour {


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	/*
	 * On Click Phone Button
	 */
	public void OnClickPhoneButton () {
		string scopicPhoneNum = "+31624343765";

		OpenApplicationManager.OpenPhoneApp(scopicPhoneNum);
	}

	/*
	 * On Click Email Button
	 */
	public void OnClickEmailButton () {
		string scopicAddress = "info@scopic.nl";

		OpenApplicationManager.OpenEmailApp(scopicAddress, "", "");
	}

	/*
	 * On Click Map Button
	 */
	public void OnClickMapButton () {
		string scopicAddress = "129,tolstraat,Amsterdam,1074VJ,Netherlands";

		OpenApplicationManager.OpenMapApp(scopicAddress);
	}
}
