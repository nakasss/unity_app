using UnityEngine;
using System.Collections;

public class StreamWelcomeMovieManager : MonoBehaviour {
	private MovieTexture movieTexture;
	private static readonly string url = "https://dl.dropboxusercontent.com/u/62976696/VideoStreamingTest/sample_scene.ogv";

	// Use this for initialization
	void Start () {
		StartCoroutine(LoadMovie());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator LoadMovie() {
		WWW www = new WWW(url);
		movieTexture = www.movie;

	    while (!movieTexture.isReadyToPlay) {
	      yield return null;
	    }

	    GetComponent<Renderer>().material.mainTexture = movieTexture;
	    //GetComponent<AudioSource>().clip = movieTexture.audioClip;
	    movieTexture.loop = true;
	    movieTexture.Play();
    	//GetComponent<AudioSource>().Play();
	}
}