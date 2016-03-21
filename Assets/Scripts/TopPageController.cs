using UnityEngine;
using System.Collections;

public class TopPageController : MonoBehaviour {
    [SerializeField]
    private TopPageView view;

	[SerializeField] private ScopicMobileAPIInterface apiInterface;
    
    
	// Use this for initialization
	void Start () {
		InitTop ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	/*
	 * Init Top
	 */
	public void InitTop () {
		apiInterface.OnLoaded = view.UpdateVideoList;
		apiInterface.StartLoadAPI();
	}
    
    
    /*
     * VideoList Cell Click Event
     */
    #region VideoList Cell Click Event
    [SerializeField] private PagesView pagesView;
    
    public void OnClickVideoCell (VideoCellBase videoCell) {
        long id = videoCell.ID;

        pagesView.GoToBeforePlay(id);
    }
    #endregion VideoList Cell Click Event

}
