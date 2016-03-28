using UnityEngine;
using System.Collections;

public class AboutUsController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	/*
	 * On Click Web Jump Button
	 */
	public void OnClickWebButton () {
		OpenApplicationManager.OpenAppByURL("http://www.scopic.nl/");
	}


	/*
	 * On Click Facebook Jump Button
	 */
	public void OnClickFacebookButton () {
		OpenApplicationManager.OpenFBPage("scopicvr");
	}


	/*
	 * On Click Instagram Jump Button
	 */
	public void OnClickInstagramButton () {
		OpenApplicationManager.OpenInstaUserPage ("scopic_vr");
		//OpenApplicationManager.OpenInsta("scopic_vr");
	}


	/*
	 * On Click Youtube
	 */
	public void OnClickYoutubeButton () {
		//https://www.youtube.com/channel/UCdztiGE_nB2hQ3jl-8Ok77g
		OpenApplicationManager.OpenAppByURL("https://www.youtube.com/channel/UCdztiGE_nB2hQ3jl-8Ok77g");
	}


	/*
	 * On Click Pinterest
	 */
	public void OnClickPinterestButton () {
		//https://www.pinterest.com/scopic_vr/
		OpenApplicationManager.OpenAppByURL("https://www.pinterest.com/scopic_vr/");
	}


	/*
	 * On Click Linkedin
	 */
	public void OnClickLinkedinButton () {
		//https://www.linkedin.com/company/scopic-the-image-brewery
		OpenApplicationManager.OpenAppByURL("https://www.linkedin.com/company/scopic-the-image-brewery");
	}


	/*
	 * On Click G+
	 */
	public void OnClickGPlusButton () {
		//https://plus.google.com/b/102929003774592137270
		OpenApplicationManager.OpenAppByURL("https://plus.google.com/b/102929003774592137270");
	}


	/*
	 * On Click Vimeo
	 */
	public void OnClickVimeoButton () {
		//https://vimeo.com/scopic
		OpenApplicationManager.OpenAppByURL("https://vimeo.com/scopic");
	}
}
