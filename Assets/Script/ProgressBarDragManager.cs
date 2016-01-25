using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ProgressBarDragManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
	[SerializeField] private PlayContentManager playContentManager;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnBeginDrag(PointerEventData eventData) {
		playContentManager.BeginDraggingValueChangeCheck();
	}

	public void OnDrag(PointerEventData pointerEventData) {
        
    }

	public void OnEndDrag(PointerEventData eventData) {
		playContentManager.EndDraggingValueChangeCheck();
	}
}