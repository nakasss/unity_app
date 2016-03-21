using UnityEngine;
using System.Collections;

public class DownloadManager : EasyBgDownloaderCtl {


	// Use this for initialization
	protected override void Start() {
		// Set Video Dir Path
		DestinationDirectoryPath = StreamAssetManager.DirPath;

		base.Start ();
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
	}
}
