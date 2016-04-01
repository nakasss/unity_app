using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MiniJSON;


public class ScopicMobileAPIInterface : MonoBehaviour {

	[SerializeField] private bool autoLoadStartEnable = true;
    
    private static readonly string API_BASE_URL = "http://104.155.2.128/api/v1/video/";
    
    private bool isLoaded = false;
    private bool isReady = false;
    private Dictionary<long, Dictionary<string, object>> videoInfo;
    private List<long> idList;
    // Iteration Value
    private int currentIdPosition;
    private int currentTextureDownloadPos;
    
    public delegate void APIReady (); // Called when finish load api and download all thumbnail.
    public APIReady OnReady;
    public delegate void APILoaded (); // Called when finish load api.
    public APILoaded OnLoaded;
    

	// Use this for initialization
	void Start () {
        videoInfo = new Dictionary<long, Dictionary<string, object>>();
        idList = new List<long>();
        currentIdPosition = 0;
        currentTextureDownloadPos = 0;
        
		if (autoLoadStartEnable) {
			StartLoadAPI ();	
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    
    /*
     * Get Iteration
     */
    public bool HasNextId () {
        return currentIdPosition < idList.Count;
    }
    
    public void BackFirst () {
        currentIdPosition = 0;
    }
    
    // If iterator reaches end, go back to first.
    private void MoveNextId () {
        currentIdPosition++;
    }
    
    public long GetVideoId () {
        long id = idList[currentIdPosition];
        MoveNextId();
        return id;
    }
     
    
    /*
     * Get Video Info
     */
    public string GetTitle (long id = -1) {
        id = id == -1 ? idList[currentIdPosition] : id;
		return videoInfo.ContainsKey (id) ? (string)videoInfo [id] ["title"] : "";
		//return (string)videoInfo[id]["title"];
    }

	public string GetSubtitle (long id = -1) {
		id = id == -1 ? idList[currentIdPosition] : id;
		return videoInfo.ContainsKey (id) ? (string)videoInfo [id] ["subtitle"] : "";
		//return (string)videoInfo[id]["title"];
	}
    
    public string GetDescription (long id = -1) {
        id = id == -1 ? idList[currentIdPosition] : id;
		return videoInfo.ContainsKey (id) ? (string)videoInfo [id] ["description"] : "";
        //return (string)videoInfo[id]["description"];
    }
    
    public string GetVideoURL (long id = -1) {
        id = id == -1 ? idList[currentIdPosition] : id;
		return videoInfo.ContainsKey (id) ? (string)videoInfo [id] ["video_url"] : "";
        //return (string)videoInfo[id]["video_url"];
    }
    
    public float GetVideoDuration (long id = -1) {
        id = id == -1 ? idList[currentIdPosition] : id;
		return videoInfo.ContainsKey (id) ? float.Parse((string)videoInfo[id]["video_duration"]) : -1.0f;
        //return float.Parse((string)videoInfo[id]["video_duration"]);
    }

	// TODO : return float or long
	public string GetVideoSize (long id = -1) {
		id = id == -1 ? idList[currentIdPosition] : id;
		return videoInfo.ContainsKey (id) ? (string)videoInfo[id]["video_size"] : "";
	}
    
    public void SetThumbnail (RawImage image, long id = -1) {
        id = id == -1 ? idList[currentIdPosition] : id;
        if (videoInfo[id].ContainsKey("thumbnail_texture")) {
            image.texture = videoInfo[id]["thumbnail_texture"] as Texture;
        } else {
            string thumbnail_url = (string)videoInfo[id]["thumbnail_url"];
            StartCoroutine(setTextureToRawImage(thumbnail_url, image, id));
        }
    }
    
    public List<object> GetCategories (long id = -1) {
        id = id == -1 ? idList[currentIdPosition] : id;
        return videoInfo[id]["categories"] as List<object>;
    }
    
    public string GetFbContet (long id = -1) {
        id = id == -1 ? idList[currentIdPosition] : id;
        return (string)videoInfo[id]["fb_content"];
    }
    
    public string GetEmailContet (long id = -1) {
        id = id == -1 ? idList[currentIdPosition] : id;
        return (string)videoInfo[id]["email_content"];
    }
    
    public DateTime GetCreatedTime (long id = -1) {
        id = id == -1 ? idList[currentIdPosition] : id;
        return DateTime.Parse((string)videoInfo[id]["created"]);
    }
    
    public DateTime GetUpdatedTime (long id = -1) {
        id = id == -1 ? idList[currentIdPosition] : id;
        return DateTime.Parse((string)videoInfo[id]["updated"]);
    }
    
    
    /*
     * Set Video Info
     */
    public void StartLoadAPI () {
        StartCoroutine(getVideoInfo());
    }
    
    public IEnumerator setTextureToRawImage (string url, RawImage image, long id) {
		Texture thumbnailTexture = null;

		if (videoInfo [id].ContainsKey ("thumbnail_texture")) {
			thumbnailTexture = videoInfo [id] ["thumbnail_texture"] as Texture;
		} else {
			WWW www = new WWW(url);

			yield return www;

			thumbnailTexture = www.textureNonReadable;
			if (!videoInfo[id].ContainsKey("thumbnail_texture")) {
				videoInfo[id].Add("thumbnail_texture", thumbnailTexture);
			}
		}

		image.texture = thumbnailTexture;
    }
    
    private IEnumerator getVideoInfo () {
        WWW www = new WWW(API_BASE_URL);
        
        while (!www.isDone) {
            yield return null;
        }
        
        IList jsonData = (IList) MiniJSON.Json.Deserialize(www.text);

		if (jsonData != null) {
			Debug.Log ("List size : " + jsonData.Count);
		} else {
			Debug.Log ("Failed To Access to data");
			return false;
		}
			
        foreach (Dictionary<string, object> video in jsonData) {
            long id = (long)video["id"];
            Debug.Log("ID : " + id);
            idList.Add(id);
            
            videoInfo[id] = video;
        }
        
        OnLoadedAPI();
    }
    
    private IEnumerator getAllTexture () {
        BackFirst();
        
        while (HasNextId()) {
            long id = GetVideoId();
            Dictionary<string, object> video = videoInfo[id];
            
            string url = (string)video["thumbnail_url"];
            StartCoroutine(getTextureByUrl(url, id));
        }
        
        BackFirst();
        
        while (currentTextureDownloadPos < videoInfo.Count) {
            yield return null;
        }
        
        OnReadyAPI();
    }
    
    private IEnumerator getTextureByUrl (string url, long id) {
		if (!videoInfo[id].ContainsKey("thumbnail_texture")) {
			WWW www = new WWW(url);

			yield return www;

			Texture texture = string.IsNullOrEmpty (www.error) ? www.textureNonReadable : null;
			if (!videoInfo [id].ContainsKey ("thumbnail_texture")) {
				videoInfo[id].Add("thumbnail_texture", texture);
			}
		}
        Debug.Log(id + " : thumnail Image downloaded By All Downloader");
        currentTextureDownloadPos++;
    }
    
    
    /*
     * Status
     */
    public bool IsLoaded () {
        return isLoaded;
    }
    
    public bool IsReady () {
        return isReady;
    }
    
    
    /*
     * API Event
     */
    private void OnLoadedAPI () {
        Debug.Log("On Loaded");
        //testLoop();
        isLoaded = true;
        StartCoroutine(getAllTexture());
        
        if (OnLoaded != null) {
            OnLoaded();
        }
    }
    
    private void OnReadyAPI () {
        Debug.Log("Get Ready");
        isReady = true;
        
        if (OnReady != null) {
            OnReady();
        }
    }
    
    
    /*
     * Test
     */
    /*
    [SerializeField]
    private RawImage[] image;
    private void testLoop () {
        BackFirst();
        
        while (HasNextId()) {
            long id = GetVideoId();
            
            //Debug.Log("Created Day : " + GetCreatedTime(id).Second);
            //MoveNextId();
        }
        
        BackFirst();
    }
    */
}
