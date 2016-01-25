using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class DragTest : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
	[SerializeField] private PlayContentManager pcm;

	// Use this for initialization
	void Start () {
		Debug.Log("Start!");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnBeginDrag(PointerEventData eventData) {
		Debug.Log("Start Drag");
		pcm.isDragging = true;
	}

	public void OnDrag(PointerEventData pointerEventData) {
        //base.OnDrag(pointerEventData);
    }

	public void OnEndDrag(PointerEventData eventData) {
		Debug.Log("End Drag");
		pcm.isDragging = false;
	}
}