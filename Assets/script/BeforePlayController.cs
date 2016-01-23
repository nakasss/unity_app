using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//TODO : This have 6 TODO.
public class BeforePlayController : MonoBehaviour {

	[SerializeField] private RectTransform topArea; //TODO : It can be cached.
	[SerializeField] private RectTransform bottomArea; //TODO : It can be cached.
	[SerializeField] private RectTransform descriptionArea; //TODO : It can be cached.
	[SerializeField] private ScrollRect scrollRect;
	[SerializeField] private CanvasGroup videoInfo;
	private float startDisappearPoint = 0.2f;
	private float endDisappearPoint = 0.8f;

	private float minTopAreaHeight;
	private float preferedDescriptionAreaHeight;

	private float minTopAreaHeightRatio = 2.4f;

	// Use this for initialization
	void Start () {
		//Set size
		SetBottomAreaContentsHeight();

		//Set Scroll Event Listener
		scrollRect.onValueChanged.AddListener(OnScrollPosChanged);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//Set Bottom and Description Area Height
	private void SetBottomAreaContentsHeight () {
		float topAreaHeight = topArea.rect.height;
		float screenWidth = Screen.width;

		//Get Minimum TopArea Height
		minTopAreaHeight = screenWidth / minTopAreaHeightRatio;

		//Get preferred BottomArea Height
		preferedDescriptionAreaHeight = topAreaHeight - minTopAreaHeight;

		//Set BottomArea Height
		Vector2 bottomSD = bottomArea.sizeDelta; //TODO : It can be cached.
		bottomSD.y = topAreaHeight + preferedDescriptionAreaHeight;
		bottomArea.sizeDelta = bottomSD;

		//Set DescriptionArea Height
		Vector2 descriptionSD = descriptionArea.sizeDelta; //TODO : It can be cached.
		descriptionSD.y = preferedDescriptionAreaHeight;
		descriptionArea.sizeDelta = descriptionSD;
	}


	public void OnScrollPosChanged (Vector2 scrollPos) {
		float scrollPer = 1.0f - scrollPos.y;
		float bottomPos = preferedDescriptionAreaHeight * scrollPer;

		//Change TopArea Height
		Vector2 topAreaSD = topArea.sizeDelta; //TODO : It can be cached.
		topAreaSD.y = -bottomPos;
		topArea.sizeDelta = topAreaSD;

		//Change Video Info Alpha
		if (scrollPer <= startDisappearPoint) {
			videoInfo.alpha = 1.0f;
		} else if (scrollPer >= endDisappearPoint) {
			videoInfo.alpha = 0.0f;
		} else {
			float videoInfoAlpha = endDisappearPoint - startDisappearPoint - scrollPer; //0.0 ~ 0.4
			videoInfo.alpha = videoInfoAlpha / (endDisappearPoint - startDisappearPoint);
		}
	}

}
