using UnityEngine;
using System.IO;
using System.Collections;

public class TestScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log ("Search File : " + StreamAssetManager.DirPath);
		System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(StreamAssetManager.DirPath);
		System.IO.FileInfo[] files = di.GetFiles("*", System.IO.SearchOption.AllDirectories);
		foreach (FileInfo file in files) {
			Debug.Log ("File Name : " + file.Name);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
