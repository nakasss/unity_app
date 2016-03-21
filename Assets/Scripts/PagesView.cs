using UnityEngine;
using System.Collections;

public class PagesView : MonoBehaviour {
    [SerializeField] private Animator pagesAnimator;
    [SerializeField] private NavigationBarView navigationBarView;
    private static readonly string PAGES_STATUS_PARAM_NAME = "PageID";
    enum PAGES_STATUS : int {
        TOP = 0,
        ABOUTUS = 1,
        CONTACT = 2,
        BEFOREPLAY = 3
    }


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    
    /*
     * To Top
     */
    #region To Top
    public void GoToTop () {
        pagesAnimator.SetInteger(PAGES_STATUS_PARAM_NAME, (int)PAGES_STATUS.TOP);
        navigationBarView.SetNaviDefault();
        
        // Init
    }
    #endregion To Top
    
    /*
     * To About Us
     */
    #region To About Us
    public void GoToAboutUs () {
        pagesAnimator.SetInteger(PAGES_STATUS_PARAM_NAME, (int)PAGES_STATUS.ABOUTUS);
        navigationBarView.SetNaviDefault();
        
        // Init
    }
    #endregion To About Us
    
    /*
     * To Contact
     */
    #region To Contact
    public void GoToContact () {
        pagesAnimator.SetInteger(PAGES_STATUS_PARAM_NAME, (int)PAGES_STATUS.CONTACT);
        navigationBarView.SetNaviDefault();
        
        // Init
    }
    #endregion To Contact
    
    /*
     * To Before Play
     */
    #region To Before Play
    [SerializeField] private BeforePlayController beforePlayController;
    
    public void GoToBeforePlay (long id) {
        pagesAnimator.SetInteger(PAGES_STATUS_PARAM_NAME, (int)PAGES_STATUS.BEFOREPLAY);
        navigationBarView.SetNaviBack();
        
        // Init
        beforePlayController.InitBeforePlayById(id);
    }
    #endregion To Before Play

}
