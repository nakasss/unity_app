using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ClickTest : MonoBehaviour, IPointerClickHandler {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void OnPointerClick(PointerEventData eventData) {
        //Debug.Log("Particluar Touched By IPointer!");
    }
}
