using UnityEngine;
using System.Collections;

public class VRScreenView : MonoBehaviour {

	// Use this for initialization
	void Start () {
		SetDefaultTexture ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	[SerializeField] private Texture defaultTexture;

	public void SetDefaultTexture () {
		SetTexture (defaultTexture);
	}


	public void SetTexture (Texture texture) {
		gameObject.GetComponent<Renderer>().material.mainTexture = texture;
	}


	[SerializeField] private Transform screen;

	public void UpsideDownForiOS () {

		#if UNITY_IPHONE
		if (transform.localScale.y > 0) {
			Vector3 screenScale = transform.localScale;
			screenScale.y = -transform.localScale.y;
			transform.localScale = screenScale;
		}
		#endif
	}
}
