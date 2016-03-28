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
}
