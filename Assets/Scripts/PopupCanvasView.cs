using UnityEngine;
using System.Collections;

public class PopupCanvasView : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	[SerializeField] private Animator popupCanvasAnimator;
	private static readonly string POPUP_CANVAS_ANIMATOR_SHOW_KEY = "IsShow";

	public void ShowPopups () {
		popupCanvasAnimator.SetBool (POPUP_CANVAS_ANIMATOR_SHOW_KEY, true);
	}


	public void HidePopups () {
		popupCanvasAnimator.SetBool (POPUP_CANVAS_ANIMATOR_SHOW_KEY, false);
	}

	public bool IsShowPopups () {
		return popupCanvasAnimator.GetBool (POPUP_CANVAS_ANIMATOR_SHOW_KEY);
	}
}
