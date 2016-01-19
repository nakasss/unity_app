using UnityEngine;
using UnityEngine.UI;

public class VideoData {
	public string title;
	public string featureImgURL;
	public string videoURL;
	public string description;
	public RawImage featureImgTexture;
}

public class VideoTableViewCell : TableViewCell<VideoData> {
	[SerializeField] private RawImage featureImg;
	[SerializeField] private Text titleLabel;

	public override void UpdateContent(VideoData newData) {
		titleLabel.text = newData.title;
		//featureImg.texture = newData.featureImgTexture.texture;
	}
}
