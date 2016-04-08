using UnityEngine;
using System.Collections;

public class SplashCanvasView : MonoBehaviour {
	
	[SerializeField] private SplashView splashView;
	[SerializeField] private TutorialController tutorialController;
	[SerializeField] private TutorialView tutorialView;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void GoSplash () {
		splashView.Show ();
		tutorialView.Hide ();
	}


	public void GoTutorial () {
		splashView.Hide ();
		tutorialController.Init ();
		tutorialView.Show ();
	}


	public bool IsInSplash () {
		return splashView.IsShow ();
	}


	public bool IsInTutorial () {
		return tutorialView.IsShow ();
	}


	public void DestorySplashCanvas () {
		Debug.Log ("Destory Splash Canvas");
		Destroy (gameObject);
	}
}
