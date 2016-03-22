using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class PlayDownloadSample : MonoBehaviour {
	[SerializeField] private Button streamButton;
	[SerializeField] private Button downloadButton;
	[SerializeField] private Slider progressBar;
	[SerializeField] private Animator sampleDownloadAnimator;
	[SerializeField] private Animator playControllerAnimator;
	[SerializeField] private PlayContentManager playController;
    [SerializeField] private EasyBgDownloaderCtl ebdCrl;

	private string footageDirName = "DownloadTest";
	private string videoName = "Biotherm_2880x1440_60fps";
    private string downloadingURL = "";

	// Use this for initialization
	void Start () {
		string dirPath = "";
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
			videoName = videoName + ".mp4";
			dirPath = Application.persistentDataPath + "/" + footageDirName;
		} else {
			videoName = videoName + ".ogv";
			dirPath = Application.streamingAssetsPath + "/" + footageDirName;
		}

		if (!Directory.Exists(dirPath)) {
			Directory.CreateDirectory(dirPath);
		}
		string filePath = dirPath + "/" + videoName;

		if (File.Exists(filePath)) {
			//Direct Play
			sampleDownloadAnimator.SetInteger("DSStatus", 2);
		} else {
			//Switch Page
			sampleDownloadAnimator.SetInteger("DSStatus", 1);
		}
        
        ebdCrl.OnComplete = FinishDownload;
        ebdCrl.DestinationDirectoryPath = dirPath;

		//Screen.orientation = ScreenOrientation.LandscapeLeft;
        //playController.InitPlayPage();
	}
	
	// Update is called once per frame
	void Update () {
	   if (!string.IsNullOrEmpty(downloadingURL) && ebdCrl.IsRunning(downloadingURL)) {
           progressBar.value = ebdCrl.GetProgress(downloadingURL);
       }
	}


	public void OnClickStreamButton () {
		//playController.InitPlayPage(null);
		sampleDownloadAnimator.SetInteger("DSStatus", 0);
		playControllerAnimator.SetBool("IsOpenPlay", true);
	}

	public void OnClickDownloadButton () {
		sampleDownloadAnimator.SetInteger("DSStatus", 3);
		string baseURL = "https://storage.googleapis.com/me-nakas-test-bucket-8th-feb/sample-movie/";
		string videoDownloadName = "Biotherm_2880x1440_60fps";
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
			videoDownloadName = videoDownloadName + ".mp4";
		} else {
			videoDownloadName = videoDownloadName + ".ogv";
		}
		string url = baseURL + videoDownloadName;

		//StartCoroutine(DownloadFootage(url));
        downloadingURL = url;
        ebdCrl.StartDL(url, "Biotherm");
	}

	public void OnDirectPlayButton () {
		string dirPath = "";
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
			dirPath = Application.persistentDataPath + "/" + footageDirName;
		} else {
			dirPath = Application.streamingAssetsPath + "/" + footageDirName;
		}

		if (!Directory.Exists(dirPath)) {
			Directory.CreateDirectory(dirPath);
		}
		string filePath = dirPath + "/" + videoName;
		LoadFootage(filePath);
	}
    
    public void OnDeleteButton () {
        string dirPath = "";
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
			videoName = videoName + ".mp4";
			dirPath = Application.persistentDataPath + "/" + footageDirName;
		} else {
			videoName = videoName + ".ogv";
			dirPath = Application.streamingAssetsPath + "/" + footageDirName;
		}

		if (!Directory.Exists(dirPath)) {
			Directory.CreateDirectory(dirPath);
		}
		string filePath = dirPath + "/" + videoName;

		if (File.Exists(filePath)) {
			//Delete
			File.Delete(filePath);
		}
        sampleDownloadAnimator.SetInteger("DSStatus", 1);
    }


	/*
	 * Download Video
	 */
	// Download test
	private void LoadFootage (string filePath) {
		string absolutePath = "file://" + filePath;
        Debug.Log("Dest Path : " + absolutePath);

		//playController.InitPlayPage(absolutePath);
		sampleDownloadAnimator.SetInteger("DSStatus", 0);
		playControllerAnimator.SetBool("IsOpenPlay", true);
	}
    
    private void FinishDownload (string requestURL, string destPath) {
        downloadingURL = "";
        LoadFootage(destPath);
    }


	// Download test
	private IEnumerator DownloadFootage (string url) {
		WWW www = new WWW(url);
		

		while (!www.isDone) {
			Debug.Log("Progress : " + Mathf.CeilToInt(www.progress*100) + "%");
			progressBar.value = www.progress;
            yield return null;
        }


        if (!string.IsNullOrEmpty(www.error)) {
        	Debug.Log("File download error");
        } else {
        	Debug.Log("Download Done");
        	SaveFile(www);
        	
        }
	}

	private void SaveFile (WWW www) {
		Debug.Log("Saving File");
		
		#if UNITY_EDITOR
		string dirPath = Application.streamingAssetsPath + "/" + footageDirName;
		#elif UNITY_IPHONE || UNITY_ANDROID
		string dirPath = Application.persistentDataPath + "/" + footageDirName;
		#endif
		
		if (!System.IO.Directory.Exists(dirPath)) {
			Directory.CreateDirectory(dirPath);
		}

		string fileName = Path.GetFileName(www.url);
        System.IO.File.WriteAllBytes(dirPath + "/" + fileName, www.bytes);


        //StatVideo
        LoadFootage(dirPath + "/" + fileName);
	}
}
