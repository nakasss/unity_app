using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ImageDownloaderTest : MonoBehaviour {
    IEnumerator Start(){

        // wwwクラスのコンストラクタに画像URLを指定
        //string url = "http://www.wanpug.com/illust/illust3038.png";
        string url = "https://dl.dropboxusercontent.com/u/62976696/FeatureImgTest/tomorrowland.jpg";
        WWW www = new WWW(url);

        // 画像ダウンロード完了を待機
        yield return www;

        // webサーバから取得した画像をRaw Imagで表示する
        RawImage rawImage = GetComponent<RawImage>();
        rawImage.texture = www.textureNonReadable;

        //ピクセルサイズ等倍に
        //rawImage.SetNativeSize();
    }

}