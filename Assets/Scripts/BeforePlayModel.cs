using UnityEngine;
using System.Collections;

public class BeforePlayModel : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	/*
	 * API Interface
	 */
	[SerializeField] private ScopicMobileAPIInterface apiInterface;

	public ScopicMobileAPIInterface Api {
		get { return apiInterface; }
	}


	/*
	 * Downloader
	 */
	#region Downloader
	[SerializeField] private DownloadManager downloader;

	public EasyBgDownloaderCtl Downloader {
		get { return downloader; }	
	}
	#endregion Downloader
}
