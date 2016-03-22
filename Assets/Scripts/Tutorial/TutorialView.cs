using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class TutorialView : MonoBehaviour {
	[SerializeField] private GameObject tutorialScrollContent;
	[SerializeField] private Animator tutorialAnimator;

	private GameObject[] tutorialContents;


	// Use this for initialization
	void Start () {
		InitView();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void InitView () {

		UpdateTutorialSize();
	}



	/*
	 * Tutorial Root
	 */
	#region Tutorial Root

	private static readonly string TUTORIAL_PAGE_STATE_KEY_NAME = "IsShow";

	public void Show () {
		tutorialAnimator.SetBool(TUTORIAL_PAGE_STATE_KEY_NAME, true);
	}

	public void Hide () {
		tutorialAnimator.SetBool(TUTORIAL_PAGE_STATE_KEY_NAME, false);
	}

	public bool IsShow () {
		return tutorialAnimator.GetBool (TUTORIAL_PAGE_STATE_KEY_NAME);
	}

	#endregion Tutorial Root


	/*
	 * Logo
	 */
	#region Logo

	private static readonly string TUTORIAL_LOGO_STATE_KEY_NAME = "IsReadyToMove";
	private static readonly string TUTORIAL_LOGO_STATE_TOP_KEY_NAME = "IsLogoInTop";


	public void MoveLogoToTop () {
		tutorialAnimator.SetBool (TUTORIAL_LOGO_STATE_KEY_NAME, true);
	}

	public void SetLogoInTop () {
		if (tutorialAnimator.GetBool (TUTORIAL_LOGO_STATE_KEY_NAME)) {
			tutorialAnimator.SetBool (TUTORIAL_LOGO_STATE_TOP_KEY_NAME, true);
		}
	}

	public bool IsLogoInTop () {
		if (tutorialAnimator.GetBool (TUTORIAL_LOGO_STATE_KEY_NAME)) {
			return tutorialAnimator.GetBool (TUTORIAL_LOGO_STATE_TOP_KEY_NAME);	
		} else {
			return false;
		}
	}

	#endregion Logo


	/*
	 * Tutorial Panels
	 */
	#region Tutorial Panels

	private static readonly string TUTORIAL_PANELS_STATE_TOP_KEY_NAME = "IsSvShow";


	public void ShowTutorial () {
		tutorialAnimator.SetBool (TUTORIAL_PANELS_STATE_TOP_KEY_NAME, true);
	}

	public void HideTutorial () {
		tutorialAnimator.SetBool (TUTORIAL_PANELS_STATE_TOP_KEY_NAME, false);
	}

	public bool IsShowTutorial () {
		return tutorialAnimator.GetBool (TUTORIAL_PANELS_STATE_TOP_KEY_NAME);
	}

	private void UpdateTutorialSize () {
		float screenWidth = Screen.width;
		tutorialContents = GetChildGameObjects(tutorialScrollContent);

		//set ScrollView size
		RectTransform scrollConRect = tutorialScrollContent.GetComponent<RectTransform>();
		Vector2 scrollRSD = scrollConRect.sizeDelta;
		scrollRSD.x = screenWidth * tutorialScrollContent.transform.childCount;
		scrollConRect.sizeDelta = scrollRSD;

		//set Tutorial contents size
		RectTransform previousRect = null;
		foreach (GameObject content in tutorialContents) {
			RectTransform rectT = content.GetComponent<RectTransform>();

			//set size
			Vector2 rectTSD = rectT.sizeDelta;
			rectTSD.x = screenWidth;
			rectT.sizeDelta = rectTSD;

			//set anchored position
			if (previousRect != null) {
				Vector2 anchoredPos = rectT.anchoredPosition;
				anchoredPos.x = previousRect.anchoredPosition.x + screenWidth;
				rectT.anchoredPosition = anchoredPos;
			}

			previousRect = rectT;
		} 
	}

	private GameObject[] GetChildGameObjects (GameObject parent) {
		if(parent == null) return null;

		GameObject[] gameObjects = new GameObject[parent.transform.childCount];
		for (int i = 0; i < parent.transform.childCount; i++) {
			gameObjects[i] = parent.transform.GetChild(i).gameObject;
		}

		return gameObjects;
	}

	#endregion Tutorial Panels


	/*
	 * Background
	 */
	#region Background

	private static readonly string TUTORIAL_BG_STATE_KEY_NAME = "IsBgShow";

	public void ShowBg () {
		tutorialAnimator.SetBool(TUTORIAL_BG_STATE_KEY_NAME, true);
	}

	public void HideBg () {
		tutorialAnimator.SetBool(TUTORIAL_BG_STATE_KEY_NAME, false);
	}

	public bool IsShowBg () {
		return tutorialAnimator.GetBool (TUTORIAL_BG_STATE_KEY_NAME);
	}

	#endregion Background


}
