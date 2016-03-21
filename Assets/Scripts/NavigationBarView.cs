using UnityEngine;
using System.Collections;

public class NavigationBarView : MonoBehaviour {
    

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    
    #region Navi Icon
    [HeaderAttribute ("Navi Icon")]
    [SerializeField] private Animator naviIconAnimator;
    private static readonly string NAVIICON_STATUS_PARAM_NAME = "NavigationStatus";
    enum NAVIICON_STATUS : int {
        DEFAULT = 0,
        X_MARK = 1,
        BACK = 2
    }
    
    /*
     * Get Navi Icon Status
     */
    public int GetNaviIconStatus () {
        return naviIconAnimator.GetInteger(NAVIICON_STATUS_PARAM_NAME);
    }
     
    /*
     * Change To Default
     */
    #region Change To Default
    public void SetNaviDefault () {
        naviIconAnimator.SetInteger(NAVIICON_STATUS_PARAM_NAME, (int)NAVIICON_STATUS.DEFAULT);
    }
    #endregion Change To Default
    
    /*
     * Change To X-Mark
     */
    #region Change To X-Mark
    public void SetNaviXMark () {
        naviIconAnimator.SetInteger(NAVIICON_STATUS_PARAM_NAME, (int)NAVIICON_STATUS.X_MARK);
    }
    #endregion Change To X-Mark
    
    /*
     * Change To Back Arrow
     */
    #region Change To Back Arrow
    public void SetNaviBack () {
        naviIconAnimator.SetInteger(NAVIICON_STATUS_PARAM_NAME, (int)NAVIICON_STATUS.BACK);
    }
    #endregion Change To Back Arrow

    #endregion Navi Icon
}
