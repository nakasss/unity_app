using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//TODO : This have 6 TODO.
public class BeforePlayController : MonoBehaviour {
    
    [SerializeField] private BeforePlayView view;
	[SerializeField] private BeforePlayModel model;

	private long id = -1;
	public long ID {
		get { return id; }
		set { id = value; }
	}

	public string VideoURL {
		get { return model.Api.GetVideoURL (ID); }
	}


	// Use this for initialization
	void Start () {
		/*
		 * Test
		 */
		//Debug.Log("Downloader Dir : " + model.Downloader.DestinationDirectoryPath);
		// End : Test
	}
	
	// Update is called once per frame
	void Update () {

		// Download Progress Update
		if (ID != -1 && !string.IsNullOrEmpty(VideoURL) && model.Downloader.IsRunning(VideoURL)) {
			OnUpdateDownloadProgress (VideoURL);
		}
	}
    
    
    /*
     * Init BeforePlay Page
     */
    public void InitBeforePlayById (long videoId) {
        ID = videoId;

		model.Downloader.OnComplete = OnCompleteDownload;
		model.Downloader.OnError = OnErrorDownload;
        
        view.UpdateContentsById(ID);
        view.SetAreaDefaultPosition();

		if (StreamAssetManager.FileExists (VideoURL) && !model.Downloader.IsRunning (VideoURL)) {
			view.ShowPlayButton ();	
		} else {
			view.ShowDownloadButton ();

			if (model.Downloader.IsRunning (VideoURL)) {
				view.SetDownloadingProgress (0.0f);
				view.ShowDownloadingButton ();
			} else {
				view.ShowBeforeDownloadButton ();
			}
		}
    }


	/*
     * Destory BeforePlay Page 
     */
	public void DestoryBeforePlay () {
		ID = -1;

		model.Downloader.OnComplete = null;
	}
    
    
    /*
     * Content Scroll Event
     */
    #region Content Scroll Event
    private float DISAPPEAR_START_PERCENTAGE = 0.2f;
	private float DISAPPEAR_END_PERCENTAGE = 0.8f;
    
    public void OnContentScrolled (Vector2 scrollPos) {
        if (float.IsNaN(scrollPos.y)) return;
        
        float topHeightPer = 1.0f - scrollPos.y;
		float topHeight = view.GetDescriptionAreaHeight() * topHeightPer;
        
        // Change Top Area Height
		view.SetTopAreaHeight(-topHeight);
        
        // Change Video Info Alpha
		if (topHeightPer <= DISAPPEAR_START_PERCENTAGE) {
            view.SetVideoInfoAlpha(1.0f);
		} else if (topHeightPer >= DISAPPEAR_END_PERCENTAGE) {
			view.SetVideoInfoAlpha(0.0f);
		} else {
			float baseAlpha = DISAPPEAR_END_PERCENTAGE - DISAPPEAR_START_PERCENTAGE - topHeightPer; //0.0 ~ 0.4
			float videoInfoAlpha = baseAlpha / (DISAPPEAR_END_PERCENTAGE - DISAPPEAR_START_PERCENTAGE);
            view.SetVideoInfoAlpha(videoInfoAlpha);
		}
    }
    #endregion Content Scroll Event
    

    /*
     * Play button click
     */
    #region Play button click

	[SerializeField] private UIRootView uiView;
	[SerializeField] private PlayView playView;

    public void OnClickPlayButton () {
        // Go To Play Button
        Debug.Log("ID : ");

		string requestUrl = model.Api.GetVideoURL (ID);
		string downloadedVideoPath = "file://" + StreamAssetManager.GetFilePath (requestUrl);
		float videoDuration = (float)model.Api.GetVideoDuration (ID);

		uiView.GoPlay (ID, downloadedVideoPath);

		/*
		uiView.GoPlay ();
		playContentManager.InitPlayPage(requestUrl, videoDuration);
		playView.SetVideoTitle (apiInterface.GetTitle(ID));
		//playContentManager.InitPlayPage(null, 0);

		//StartCoroutine(LoadPlayCam());
		Screen.orientation = ScreenOrientation.LandscapeLeft;
		*/
    }

    #endregion Play button click


	/*
	 * Download Button
	 */
	#region Download Button

	public void OnClickStartDownloadButton () {
		string requestURL = VideoURL;
		string naviTitle = model.Api.GetTitle (ID);

		model.Downloader.StartDL (requestURL, naviTitle);

		view.SetDownloadingProgress (0.0f);
		view.ShowDownloadingButton ();
	}

	public void OnClickStopDownloadButton () {
		string requestURL = VideoURL;
		model.Downloader.StopDL (requestURL);
		StreamAssetManager.DeleteFile (requestURL);

		view.SetDownloadingProgress (0.0f);
		view.ShowBeforeDownloadButton ();
	}

	#endregion Download Button

    
    /*
     * Facebook button click
     */
    #region Facebook button click
    public void OnClickFacebookButton () {
        string shareURL = "http://www.scopic.nl/"; //Test

		OpenApplicationManager.ShareToFB(shareURL);
    }
    #endregion Facebook button click
    

    /*
     * Email button click
     */
    #region Email button click
    public void OnClickEmailButton () {
        string address = "";
		//test
		string subject = "Scopic VR Player";
		//test
		string body = "Scopic made this amazing 360 recruitment video for EY. Check out the link below, and don't forget to look around by moving your smartphone/tablet, or to click-and-drag from on your computer. Enjoy! http://www.scopic.nl/page-ey/";

		OpenApplicationManager.OpenEmailApp(address, subject, body);
    }
    #endregion Email button click


	/*
     * File Delete Button
     */
	#region File Delete Button
	public void OnClickFileDeleteButton () {
		string filePath = VideoURL;
		if (!string.IsNullOrEmpty(filePath)) {
			StreamAssetManager.DeleteFile (filePath);
		}

		view.ShowDownloadButton ();
		view.ShowBeforeDownloadButton ();
	}
	#endregion File Delete Button


	/*
	 * Downloader Event
	 */
	#region Downloader Event

	public void OnUpdateDownloadProgress (string videoURL) {
		view.SetDownloadingProgress (model.Downloader.GetProgress(VideoURL));
	}

	public void OnCompleteDownload (string requestURL, string filePath) {
		if (VideoURL == requestURL) {
			view.ShowPlayButton ();
		}
	}

	public void OnErrorDownload (string requestURL, EasyBgDownloaderCtl.DOWNLOAD_ERROR errorCode, string errorMessage) {
		if (VideoURL == requestURL) {
			view.ShowBeforeDownloadButton ();
		}
	}


	#endregion Downloader Event

}
