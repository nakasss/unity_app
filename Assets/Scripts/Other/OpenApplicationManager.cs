﻿using UnityEngine;
using System;
using System.Collections;


public class OpenApplicationManager {


	public static void OpenFBPage (string pageID) {
		//TODO : Use URL Schene
		string fbPageBaseURL = "https://www.facebook.com/";
		string fbPageURL = fbPageBaseURL + WWW.EscapeURL(pageID);
		//string fbPageDirectURLScheme = "fb://profile/";
		//string fbPageDirectURLScheme = "fb://page/";
		//string fbPageDirectURL = fbPageDirectURLScheme + WWW.EscapeURL(pageID);

		OpenAppByURL(fbPageURL);
		//OpenAppByURL(fbPageDirectURL);
	}

	public static void OpenInstaUserPage (string accountID) {
		//TODO : Use URL Schene
		//string instaPageBaseURL = "https://www.instagram.com/";
		//string instaPageURL = instaPageBaseURL + WWW.EscapeURL(accountID);
		string instaPageURLScheme = "instagram://user?username=";
		string instaPageDirectURL = instaPageURLScheme + WWW.EscapeURL(accountID);

		//OpenAppByURL(instaPageURL);
		OpenAppByURL(instaPageDirectURL);
	}

	public static void OpenInsta (string accountID) {
		//TODO : Use URL Schene
		string instaPageBaseURL = "https://www.instagram.com/";
		string instaPageURL = instaPageBaseURL + WWW.EscapeURL(accountID);
		//string instaPageURLScheme = "https://www.instagram.com/";
		//string instaPageDirectURL = instaPageBaseURL + WWW.EscapeURL(accountID);

		OpenAppByURL(instaPageURL);
	}

	public static void OpenEmailApp (string address, string subject, string body) {
		string escapedBody = Uri.EscapeDataString(body);
		string openEmailURL = "mailto:" + address + "?subject=" + subject + "&body=" + escapedBody;

		OpenAppByURL(openEmailURL);
	}

	public static void OpenPhoneApp (string phoneNum) {
		string phoneURL = "tel:" + phoneNum;

		OpenAppByURL(phoneURL);
	}

	public static void OpenMapApp (string address) {
		string mapURLScheme = "";
		#if UNITY_IPHONE
		mapURLScheme = "http://maps.apple.com/";
		#elif UNITY_ANDROID
		mapURLScheme = "http://maps.google.com/maps";
		#endif

		string mapURL = mapURLScheme + "?q=" + WWW.EscapeURL(address);
		OpenAppByURL(mapURL);
	}

	//Share FB URL
	public static void ShareToFB (string shareURL) {
		//TODO : Use URL Schene
		string fbShareBaseURL = "https://www.facebook.com/sharer/sharer.php?u=";

		string fbURL = fbShareBaseURL + WWW.EscapeURL(shareURL);
		OpenAppByURL(fbURL);
	}

	//Open Browser
	public static void OpenAppByURL (string url) {
		if (String.IsNullOrEmpty(url)) {
			Debug.Log("URL is empty.");
			return;
		}

		Application.OpenURL(url);
	}
}
