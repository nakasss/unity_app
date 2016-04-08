using UnityEngine;
using System;
using System.Collections;
using System.IO;

public class StreamAssetManager {
	private static readonly string STREAM_ASSET_DIR_NAME = "scopic_videos";


	/*
	 * Dir Path
	 */
	public static string DirPath {
		get {
			string dirPath = "";
			if (Application.platform == RuntimePlatform.Android) {
				dirPath = Application.temporaryCachePath + "/" + STREAM_ASSET_DIR_NAME;
			} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
				//dirPath = Application.persistentDataPath + "/" + STREAM_ASSET_DIR_NAME;
				dirPath = Application.temporaryCachePath + "/" + STREAM_ASSET_DIR_NAME;
			} else {
				dirPath = Application.streamingAssetsPath + "/" + STREAM_ASSET_DIR_NAME;
			}

			if (!Directory.Exists (dirPath)) {
				Directory.CreateDirectory (dirPath);
			}

			return dirPath;
		}
	}


	/*
	 * File Path
	 */
	public static string GetFilePath (string fileNameOrPath) {
		//return DirPath + "/" + Uri.UnescapeDataString (Path.GetFileName (fileNameOrPath));
		return DirPath + "/" + Path.GetFileName (fileNameOrPath);
	}


	/*
	 * File Check
	 */
	public static bool FileExists (string fileNameOrPath) {
		//string filePath = DirPath + "/" + Uri.UnescapeDataString (Path.GetFileName (fileNameOrPath));
		string filePath = DirPath + "/" + Path.GetFileName (fileNameOrPath);

		return File.Exists (filePath);
	}


	/*
	 * File Delete
	 */
	public static void DeleteFile (string fileNameOrPath) {
		//string filePath = DirPath + "/" + Uri.UnescapeDataString (Path.GetFileName (fileNameOrPath));
		string filePath = DirPath + "/" + Path.GetFileName (fileNameOrPath);

		if (File.Exists (filePath)) {
			File.Delete (filePath);
		}
	}

	/*
	 * File List
	 */
	public static string[] GetAllFiles () {
		string[] files = Directory.GetFiles (DirPath, "*", System.IO.SearchOption.AllDirectories);
		return files;
	}

}
