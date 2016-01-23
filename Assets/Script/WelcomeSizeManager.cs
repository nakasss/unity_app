using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class WelcomeSizeManager : MonoBehaviour {
	[SerializeField] private GameObject scrollContent;
	private GameObject[] welcomeContents;

	// Use this for initialization
	void Start () {
		float screenWidth = Screen.width;
		welcomeContents = GetWelcomeContents(scrollContent);

		//set ScrollView size
		RectTransform scrollConRect = scrollContent.GetComponent<RectTransform>();
		Vector2 scrollRSD = scrollConRect.sizeDelta;
		scrollRSD.x = screenWidth * scrollContent.transform.childCount;
		scrollConRect.sizeDelta = scrollRSD;

		//set Welcome contents size
		RectTransform previousRect = null;
		foreach (GameObject content in welcomeContents) {
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
	
	// Update is called once per frame
	void Update () {
	
	}

	private GameObject[] GetWelcomeContents(GameObject parent) {
        if(parent == null) return null;

        GameObject[] gameObjects = new GameObject[parent.transform.childCount];
        // 使いやすいようにtransformsからgameObjectを取り出す
        for (int i = 0; i < parent.transform.childCount; i++) {
        	gameObjects[i] = parent.transform.GetChild(i).gameObject;
        }
        
        // 配列に変換してreturn
        return gameObjects;
	}
}
