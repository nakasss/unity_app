﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class TopPageView : MonoBehaviour {


	// Use this for initialization
	void Start () {
        cellBase.SetActive(false);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    
    /*
     * VideoList
     */
    [HeaderAttribute ("Video List")]
    [SerializeField]
    private ScopicMobileAPIInterface apiInterface;
    
    public void UpdateVideoList () {
        apiInterface.BackFirst();
        Vector2 cellTop = new Vector2(0.0f, 0.0f);
        
         while (apiInterface.HasNextId()) {
            long id = apiInterface.GetVideoId();
            VideoCellBase cell = CreateVideoCellForId(id);
            cell.Top = cellTop;
            
            cell.Title = apiInterface.GetTitle(id);
            apiInterface.SetThumbnail(cell.thumbnailImg, id);
            
            cellTop = cellTop + new Vector2(0.0f, -cell.Height);
        }
        
        setScrollContentHeight(-cellTop.y);
    }
    
    
    /*
     * ScrollContent
     */
    #region ScrollContent
    [SerializeField]
    private ScrollRect videoList;
    
    private void setScrollContentHeight (float height) {
        Vector2 contentHeight = new Vector2(0.0f, height);
        
        videoList.content.sizeDelta = contentHeight;
    }
    
    #endregion ScrollContent
    
    
    /*
     * Cell
     */
    #region Cell
    [SerializeField]
    private GameObject cellBase; // Require "VideoCellBase"
    
    private VideoCellBase CreateVideoCellForId (long id) {
        GameObject newObj = Object.Instantiate (cellBase) as GameObject;
        newObj.name = cellBase.gameObject.name + "_" + id;
        newObj.SetActive(true);
        
        VideoCellBase videoNewCell = newObj.GetComponent<VideoCellBase>();
        videoNewCell.SetCellParent(cellBase.transform.parent);
        videoNewCell.ID = id;
        
        return videoNewCell;
    }
    
    #endregion Cell

}