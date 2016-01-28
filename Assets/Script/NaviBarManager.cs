using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class NaviBarManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	// Called in navi icon evetn trigger
	[SerializeField] private Animator naviIconAC;
	private int navistatus = 0;
	public void OnNaviIconClicked () {
		switch (navistatus) {
			case 0:
				naviIconAC.SetInteger("NavigationStatus", 0);
				break;
			case 1:
				naviIconAC.SetInteger("NavigationStatus", 1);
				break;
			case 2:
				naviIconAC.SetInteger("NavigationStatus", 2);
				break;
			default:
				naviIconAC.SetInteger("NavigationStatus", 0);
				break;
		}

		navistatus++;
		if (navistatus > 2) {
			navistatus = 0;
		}
	}
}
