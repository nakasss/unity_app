using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class TutorialView : MonoBehaviour {
	[SerializeField] private GameObject tutorialScrollContent;
	[SerializeField] private Animator tutorialAnimator;

	private GameObject[] tutorialContents;

	public delegate void LogoAnimation();
	public LogoAnimation OnComplete;
	private bool isLogoInTop;


	// Use this for initialization
	void Start () {
		InitView();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void InitView () {
		//Init ui status
		isLogoInTop = false;

		//init tutorial size
		InitTutorialSize();
	}


	/*
	 * UI manager
	 */
	// -- Root --  
	public void ShowPage () {
		tutorialAnimator.SetBool("IsShow", true);
	}
	// -- TutorialScrollView -- 
	public void ShowTutorial () {
		tutorialAnimator.SetBool("IsSvShow", true);
	}

	private void InitTutorialSize () {
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

	// -- Logo -- 
	public void LogoMoveTop () {
		tutorialAnimator.SetBool("IsReadyToMove", true);
	}

	// -- Background -- 
	public void HideBg () {
		tutorialAnimator.SetBool("IsBgHide", true);
	}



	/*
	 * UI status
	 */
	public bool IsLogoInTop () {
		return isLogoInTop;
	}
	


	/*
	 * UI event
	 */
	//called in animator
	private void OnLogoAnimationComplete () {
		isLogoInTop = true;

		if (OnComplete != null) {
			OnComplete();			
		}	
	}

}
