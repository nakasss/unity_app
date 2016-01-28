using UnityEngine;
using System.Collections;

public class SplashLogoAnimationManager : MonoBehaviour {

	[SerializeField] private Animator logoAnimator;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnCompleteLogoAnimation () {
		logoAnimator.SetBool("CompleteAllAnimations", true);
	}
}
