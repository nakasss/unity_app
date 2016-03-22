using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Xml;

public class FirstOpenManager {
	
	private static readonly string FIRST_CONF_DIR_NAME = "scopicfirst.d";
	private static readonly string FIRST_CONF_FILE_NAME = "first_open_conf.xml";
	private static readonly string FIRST_OPEN_XML_ELEMENT = "firstopen";


	public static string ConfDirPath {
		get {
			string dirPath = 
				((Application.platform == RuntimePlatform.OSXEditor) ? 
					Application.streamingAssetsPath : 
					Application.persistentDataPath) +
				"/" + FIRST_CONF_DIR_NAME;

			if (!Directory.Exists (dirPath)) {
				Directory.CreateDirectory (dirPath);
			}

			return dirPath;
		}
	}

	public static string ConfFilePath {
		get {
			return ConfDirPath + "/" + FIRST_CONF_FILE_NAME;
		}
	}


	public static bool IsFirstOpen (bool isDebugMode = false) {
		if (isDebugMode)
			return true;


		bool isFirstOpen = true;

		if (!File.Exists (ConfFilePath)) {
			createFirstConfFile (false);
		} else {
			isFirstOpen = Convert.ToBoolean (getFirstOpenText());

			if (isFirstOpen) {
				setFirstConfFile (!isFirstOpen);
			}
		}

		return isFirstOpen;
	}


	private static string getFirstOpenText () {
		XmlDocument xml = new XmlDocument ();
		xml.Load (ConfFilePath);

		foreach( XmlElement element in xml.DocumentElement ) {
			if (element.Name == FIRST_OPEN_XML_ELEMENT) {
				return element.InnerText;
			}
		}

		return "false";
	}


	private static void createFirstConfFile (bool firstOpen) {
		XmlDocument document = new XmlDocument();

		XmlDeclaration declaration = document.CreateXmlDeclaration ("1.0", "UTF-8", null);
		XmlElement root = document.CreateElement ("root");

		document.AppendChild (declaration);
		document.AppendChild (root);

		XmlElement fistOpenElement = document.CreateElement (FIRST_OPEN_XML_ELEMENT);
		fistOpenElement.InnerText = firstOpen.ToString ();
		root.AppendChild (fistOpenElement);

		document.Save (ConfFilePath);
	}


	private static void setFirstConfFile (bool firstOpen) {
		if (!File.Exists (ConfFilePath)) {
			createFirstConfFile (firstOpen);
			return;
		} 

		XmlDocument xml = new XmlDocument ();
		xml.Load (ConfFilePath);

		foreach( XmlElement element in xml.DocumentElement ) {
			if (element.Name == FIRST_OPEN_XML_ELEMENT) {
				element.InnerText = firstOpen.ToString ();
				xml.Save (ConfFilePath);
				return;
			}
		}
	}
}
