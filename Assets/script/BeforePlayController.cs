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
	[SerializeField] private GameObject mainCamera;

	private float startDisappearPoint = 0.2f;
	private float endDisappearPoint = 0.8f;

	private float minTopAreaHeight;
	private float preferedDescriptionAreaHeight = 0.0f;

	private float minTopAreaHeightRatio = 2.4f;

	// Use this for initialization
	void Start () {
		//Set size
		SetBottomAreaContentsHeight();

		//Set Scroll Event Listener
		//scrollRect.onValueChanged.AddListener(OnScrollPosChanged);
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
		if (float.IsNaN(scrollPos.y)) return;

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


	[SerializeField] private Animator nonViewerAC;
	[SerializeField] private Animator playContentAC;
	[SerializeField] private PlayContentManager playContentManager;
	public void OnPlayButtonClicked () {
		//Enable Play
		nonViewerAC.SetBool("IsOpenPlay", true);
		playContentAC.SetBool("IsOpenPlay", true);
		//Start Load
		playContentManager.InitPlayPage();

		//Set screen orientation
		Screen.orientation = ScreenOrientation.LandscapeLeft;

		StartCoroutine(LoadPlayCam());
	}

	private IEnumerator LoadPlayCam () {
		AsyncOperation async = Application.LoadLevelAdditiveAsync("PlayCam");

		while(!async.isDone){
			yield return new WaitForSeconds(0);
		}

		mainCamera.SetActive(false);
	}
        


	//Facebook Button
	public void OnFBButtonClicked () {
		string shareURL = "http://www.scopic.nl/"; //Test

		OpenApplicationManager.ShareToFB(shareURL);
	}

	//Email Button
	public void OnEmailButtonClicked () {
		string address = "";
		//test
		string subject = "Scopic VR Player";
		//test
		string body = "Scopic made this amazing 360 recruitment video for EY. Check out the link below, and don't forget to look around by moving your smartphone/tablet, or to click-and-drag from on your computer. Enjoy! http://www.scopic.nl/page-ey/";

		OpenApplicationManager.OpenEmailApp(address, subject, body);
	}

}
