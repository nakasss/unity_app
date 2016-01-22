using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class VideoData {
	public string title;
	public string featureImgURL;
	public string videoURL;
	public string description;
	public RawImage featureImgTexture;
}

public class VideoTableViewCell : TableViewCell<VideoData> {
	[SerializeField] private Text titleLabel;
	[SerializeField] private RawImage featureImg;
	[SerializeField] private AspectRatioFitter fImgAspectFitter;

	public override void UpdateContent(VideoData newData) {
		titleLabel.text = newData.title;

		if (!string.IsNullOrEmpty(newData.featureImgURL)) {
			StartCoroutine(SetFeaturedImg(newData.featureImgURL));
		}
	}


	//画像ダウンロード
	IEnumerator SetFeaturedImg (string filename) {
		string baseURL = "https://dl.dropboxusercontent.com/u/62976696/FeatureImgTest/";
		WWW www = new WWW(baseURL + filename);

		yield return www;

		SetAspectByImg(www.textureNonReadable);
		featureImg.texture = www.textureNonReadable;
	}

	private void SetAspectByImg (Texture img) {
		float aspect = (float)img.width / (float)img.height;;
		fImgAspectFitter.aspectRatio = aspect;
	}
}
