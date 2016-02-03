using UnityEngine;
using System.Collections;


public class MainSceneLoad : MonoBehaviour {
	private AsyncOperation asyncMainScene;
	private bool isLoadCompleted;
	/*
	 * Loading Event
	 */
    public delegate void SceneLoadComplete();
	public SceneLoadComplete OnComplete;



	// Use this for initialization
	void Start () {
		isLoadCompleted = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	/*
	 * Scene Load Asset
	 */
	public void StartMainSceneLoad () {
		StartCoroutine("LoadSceneAsync", "Main");
	}

	private void OnCompleteLoadScene () {
		isLoadCompleted = true;

		if (OnComplete != null) {
			OnComplete();
		}
	}

	public bool IsLoadCompleted () {
		return isLoadCompleted;
	}

	public void EnableMainScene () {
		if (isLoadCompleted) {
			asyncMainScene.allowSceneActivation = true;
		}
	}

	private IEnumerator LoadSceneAsync (string sceneName) {
        asyncMainScene = Application.LoadLevelAsync(sceneName);
        asyncMainScene.allowSceneActivation = false;
        //asyncs[sceneName] = async;

        while (asyncMainScene.progress < 0.9f) {
            yield return new WaitForEndOfFrame();
        }

        //Call End event
        OnCompleteLoadScene();
    }

}
