using UnityEngine;
using UnityEngine.UI;
using System.Collections;


[RequireComponent (typeof (Toggle))]
public class PlayToggleManager : MonoBehaviour {
	private Toggle toggle;
	[SerializeField] private Image bgImage;

	// Use this for initialization
	void Start () {
		toggle = GetComponent<Toggle>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ChangeToggleBGAlpha (bool isOn) {
		Graphic bgGraphic = toggle.targetGraphic;

		if (toggle.isOn) {
			//Hide
			bgGraphic.color = new Color(bgGraphic.color.r, bgGraphic.color.g, bgGraphic.color.b, 0.0f);
		} else {
			//Show
			bgGraphic.color = new Color(bgGraphic.color.r, bgGraphic.color.g, bgGraphic.color.b, 255.0f);
		}
	}
}
