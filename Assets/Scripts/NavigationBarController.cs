using UnityEngine;
using System.Collections;

public class NavigationBarController : MonoBehaviour {
    [SerializeField] private NavigationBarView navigationBarView;
	[SerializeField] private MainCanvasView mainCanvasView;
    [SerializeField] private PagesView pagesView;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    
    /*
     * Click Navi Icon
     */
    #region Click Navi Icon
    enum NAVIICON_STATUS : int {
        DEFAULT = 0,
        X_MARK = 1,
        BACK = 2
    }
    
    public void OnClickNaviIcon () {
        switch (navigationBarView.GetNaviIconStatus()) {
            case (int)NAVIICON_STATUS.DEFAULT:
                OnClickDefaultIcon();
                break;
            case (int)NAVIICON_STATUS.X_MARK:
                OnClickXMarkIcon();
                break;
            case (int)NAVIICON_STATUS.BACK:
                OnClickBackIcon();
                break;
            default:
                OnClickDefaultIcon();
                break;
        }
    }
    #endregion Click Navi Icon
    
    /*
     * Click Default navi icon
     */
    #region Click Default navi icon
    public void OnClickDefaultIcon () {
		mainCanvasView.OpenDrawer ();
    }
    #endregion Click Default navi icon
    
    /*
     * Click X-Mark navi icon
     */
    #region Click X-Mark navi icon
    public void OnClickXMarkIcon () {
		mainCanvasView.ShowPages ();
    }
    #endregion Click X-Mark navi icon
    
    /*
     * Click Back navi icon
     */
    #region Click Back navi icon
    public void OnClickBackIcon () {
        pagesView.GoToTop();
    }
    #endregion Click Back navi icon
}
