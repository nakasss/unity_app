using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BeforePlayView : MonoBehaviour {


	// Use this for initialization
	void Start () {
	   
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    
    /*
     * Area State
     */
    [HeaderAttribute ("Area State")]
    [SerializeField] private RectTransform beforePlayContent;
    private static readonly float MIN_TOP_AREA_HEIGHT_RATIO = 2.4f;
    
    public void SetAreaDefaultPosition () {
        float initialTopAreaHeight = beforePlayContent.rect.height;
        float minTopAreaHeight = Screen.width / MIN_TOP_AREA_HEIGHT_RATIO;
        
        float initialDescriptionHeight = initialTopAreaHeight - minTopAreaHeight;
        float initialBottomAreaHeight = initialTopAreaHeight + initialDescriptionHeight;
        
        SetBottomAreaHeight(initialBottomAreaHeight);
        SetDescriptionAreaHeight(initialDescriptionHeight);
		SetTopAreaHeight (0);
		SetBottomAreaTop (0);
    }
    
    /*
     * Top Area
     */
    #region Top Area
    [SerializeField] private RectTransform topArea; //TODO : use cache
    
    public void SetTopAreaHeight (float height) {
        Vector2 sizeDelta = topArea.sizeDelta;
        sizeDelta.y = height;
        topArea.sizeDelta = sizeDelta; 
    }
    
    public float GetTopAreaHeight () {
        return topArea.sizeDelta.y;
    }

    #endregion Top Area
    
    /*
     * Video Info Top Area
     */
    #region Video Info Top Area
    [SerializeField] private CanvasGroup videoInfoCanvasGroup;
    
    public void SetVideoInfoAlpha (float alpha) {
        videoInfoCanvasGroup.alpha = alpha;
    }
    #endregion Video Info Top Area
    
    /*
     * Bottom Area
     */
    #region Bottom Area
    [SerializeField] private RectTransform bottomArea; //TODO : use cache
    
    public void SetBottomAreaHeight (float height) {
        Vector2 sizeDelta = bottomArea.sizeDelta;
        sizeDelta.y = height;
        bottomArea.sizeDelta = sizeDelta; 
    }

	public void SetBottomAreaTop (float topPos) {
		Vector2 anchoredPos = bottomArea.anchoredPosition;
		anchoredPos.y = topPos;
		bottomArea.anchoredPosition = anchoredPos;
	}
    
    public float GetBottomAreaHeight () {
        return bottomArea.sizeDelta.y;
    }

	public float GetBottomAreaTop () {
		return bottomArea.anchoredPosition.y;
	}
    #endregion Bottom Area
    
    /*
     * Description Area
     */
    #region Description Area
    [SerializeField] private RectTransform descriptionArea; //TODO : use cache
    
    public void SetDescriptionAreaHeight (float height) {
        Vector2 sizeDelta = descriptionArea.sizeDelta;
        sizeDelta.y = height;
        descriptionArea.sizeDelta = sizeDelta; 
    }
    
    public float GetDescriptionAreaHeight () {
        return descriptionArea.sizeDelta.y;
    }
    #endregion Description Area
    
    /*
     * Update Contents
     */
    [HeaderAttribute ("Before Play Contents")]
    [SerializeField] private ScopicMobileAPIInterface apiInterface;
    public void UpdateContentsById (long id) {
        // Titles
		SetTopTitle (apiInterface.GetTitle(id));
		SetTopSubtitle (apiInterface.GetSubtitle (id));
        SetBottomTitle(apiInterface.GetTitle(id));
        
        // Thumbnail
		SetThumbnailDefault();
        apiInterface.SetThumbnail(GetThumbnail(), id);
        
        // Description
		//SetDescriptionWithSubtitleAndDuration (apiInterface.GetDescription(id), apiInterface.GetSubtitle(id), (long)apiInterface.GetVideoDuration(id));
		SetDescriptionWithDuration (apiInterface.GetDescription(id), (long)apiInterface.GetVideoDuration(id));
        //SetDescription(apiInterface.GetDescription(id));
        
        // Reset top & bottom size default
        //SetAreaDefaultPosition();
    }
    

    /*
     * Top Title
     */
    #region Top Title
    [SerializeField] private Text topTitle;
    
    public void SetTopTitle (string title) {
        topTitle.text = title;
    }
    #endregion Top Title


	/*
     * Top Subtitle
     */
	#region Top Title
	[SerializeField] private Text topSubtitle;

	public void SetTopSubtitle (string subtitle) {
		if (topSubtitle != null) {
			topSubtitle.text = subtitle;
		}
	}
	#endregion Top Subtitle


	/*
	 * GoPlayButton
	 */
	#region GoPlayButton
	[SerializeField] private Animator goPlayButtonAnimator;
	private static readonly string GO_PLAY_BUTTON_KEY_NAME = "IsPlay";

	public void ToggleGoPlayButton () {
		if (IsShowPlayButton ()) {
			ShowDownloadButton ();
		} else {
			ShowPlayButton ();
		}
	}

	public void ShowPlayButton () {
		goPlayButtonAnimator.SetBool (GO_PLAY_BUTTON_KEY_NAME, true);
	}

	public void ShowDownloadButton () {
		goPlayButtonAnimator.SetBool (GO_PLAY_BUTTON_KEY_NAME, false);
	}

	public bool IsShowPlayButton () {
		return goPlayButtonAnimator.GetBool (GO_PLAY_BUTTON_KEY_NAME);
	}

	#endregion GoPlayButton


	/*
	 * Download Button
	 */
	#region Download Button
	[SerializeField] private Image progressCircle;
	[SerializeField] private Animator downloadButtonAnimator;
	private static readonly string DOWNLOAD_BUTTON_KEY_NAME = "IsDownloading";

	public void ToggleDownloadButton () {
		if (IsShowDownloading ()) {
			ShowBeforeDownloadButton ();
		} else {
			ShowDownloadingButton ();
		}
	}

	public void ShowBeforeDownloadButton () {
		downloadButtonAnimator.SetBool (DOWNLOAD_BUTTON_KEY_NAME, false);
	}

	public void ShowDownloadingButton () {
		downloadButtonAnimator.SetBool (DOWNLOAD_BUTTON_KEY_NAME, true);
	}

	public bool IsShowDownloading () {
		return downloadButtonAnimator.GetBool (DOWNLOAD_BUTTON_KEY_NAME);
	}

	public void SetDownloadingProgress (float progress) {
		progressCircle.fillAmount = progress;
	}

	#endregion Download Button


	/*
	 * File Size Text
	 */
	// TODO : 引数をfloatで扱えるように変更
	#region File Size Text
	[SerializeField] private Text fileSizeLabel;

	public void SetFileSize (string fileSize) {
		fileSizeLabel.text = fileSize;
	}

	#endregion File Size Text
    
    /*
     * Thumbnail
     */
    #region Thumbnail
    [SerializeField] private RawImage thumbnail;
	[SerializeField] private Texture thumbnailDummy;
    
    public RawImage GetThumbnail () {
        return thumbnail;
    }

	public void SetThumbnailDefault () {
		thumbnail.texture = thumbnailDummy;
	}

	public void SetThumbnailTransparent () {
		Color thumbnailColor = thumbnail.color;
		thumbnailColor.a = 0;
		thumbnail.color = thumbnailColor;
	}

	public void SetThumbnailApprent () {
		Color thumbnailColor = thumbnail.color;
		thumbnailColor.a = 1;
		thumbnail.color = thumbnailColor;
	}
		
    #endregion Thumbnail
    
    /*
     * Bottom Title
     */
    #region Bottom Title
    [SerializeField] private Text bottomTitle;
    
    public void SetBottomTitle (string title) {
        bottomTitle.text = title;
    }
    #endregion Bottom Title

	/*
     * Video Duraction Label
     */
	#region Video Duraction Label
	[SerializeField] private Text videoDurationLabel;

	public void SetDuration (long sec) {
		string minutePart = (sec / 60 < 10) ? "0" + (sec / 60).ToString() : (sec / 60).ToString();
		string secPart = (sec % 60 < 10) ? "0" + (sec % 60).ToString() : (sec % 60).ToString();;

		videoDurationLabel.text = minutePart + ":" + secPart;
	}

	#endregion Video Duraction Label
    
    /*
     * Description
     */
    #region Description
    [SerializeField] private Text description;
	private static readonly int MAX_CHAR_NUM_WITHOUT_BEST_FIT = 500;

    public void SetDescription (string descriptionText) {
		if (descriptionText.Length > MAX_CHAR_NUM_WITHOUT_BEST_FIT) {
			description.resizeTextForBestFit = true;
		} else {
			description.resizeTextForBestFit = false;
		}
		Debug.Log ("Description length : " + descriptionText.Length);

        description.text = descriptionText;
    }

	public void SetDescriptionWithDuration (string descriptionText, long sec) {
		string minutePart = (sec / 60 < 10) ? "0" + (sec / 60).ToString() : (sec / 60).ToString();
		string secPart = (sec % 60 < 10) ? "0" + (sec % 60).ToString() : (sec % 60).ToString();;
		string duration = minutePart + ":" + secPart;

		string descriptionWithDuration = descriptionText + "\r\n" + "\r\n" + "(" + duration + ")" + "\r\n";
		SetDescription (descriptionWithDuration);
	}

	public void SetDescriptionWithSubtitleAndDuration (string descriptionText, string subtitle, long sec) {
		string minutePart = (sec / 60 < 10) ? "0" + (sec / 60).ToString() : (sec / 60).ToString();
		string secPart = (sec % 60 < 10) ? "0" + (sec % 60).ToString() : (sec % 60).ToString();;
		string duration = minutePart + ":" + secPart;

		string descriptionWithDuration;
		if (!string.IsNullOrEmpty (subtitle)) {
			subtitle = "<b>" + subtitle + "</b>";
			descriptionWithDuration = subtitle + "\r\n" + "\r\n" + descriptionText + "\r\n" + "\r\n" + "(" + duration + ")";
		} else {
			descriptionWithDuration = descriptionText + "\r\n" + "\r\n" + "(" + duration + ")" + "\r\n";
		}
	
		SetDescription (descriptionWithDuration);
	}

    #endregion Description

    
    /*
     * FB
     */
    #region FB
    #endregion FB
    

    /*
     * Email
     */
    #region Email
    #endregion Email


	/*
     * Delete Button
     */
	#region Delete Button

	[SerializeField] private Animator bottomBtnsWrapperAnimator;
	private static readonly string DELETE_BUTTON_STATE_KEY_NAME = "IsShowDeleteButton";

	public void ShowDeleteButton () {
		bottomBtnsWrapperAnimator.SetBool (DELETE_BUTTON_STATE_KEY_NAME, true);
	}

	public void HideDeleteButton () {
		bottomBtnsWrapperAnimator.SetBool (DELETE_BUTTON_STATE_KEY_NAME, false);
	}

	public bool IsShowDeleteButton () {
		return bottomBtnsWrapperAnimator.GetBool (DELETE_BUTTON_STATE_KEY_NAME);
	}

	#endregion Delete Button

}
