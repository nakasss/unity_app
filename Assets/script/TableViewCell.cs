using UnityEngine;

public class TableViewCell<T> : ViewController	// ViewControllerクラスを継承する
{
	// セルの内容を更新するメソッド
	public virtual void UpdateContent(T itemData)
	{
		// 実際の処理は継承したクラスで実装する
	}

	// セルに対応するリスト項目のインデックスを保持
	public int DataIndex { get; set; }
	
	// セルの高さを取得、設定するプロパティ
	public float Height
	{
		get { return CachedRectTransform.sizeDelta.y; }
		set {
			Vector2 sizeDelta = CachedRectTransform.sizeDelta;
			sizeDelta.y = value;
			CachedRectTransform.sizeDelta = sizeDelta;
		}
	}
	
	// セルの上端の位置を取得、設定するプロパティ
	public Vector2 Top
	{
		get {
			Vector3[] corners = new Vector3[4];
			CachedRectTransform.GetLocalCorners(corners);
			return CachedRectTransform.anchoredPosition + 
				new Vector2(0.0f, corners[1].y);
		}
		set {
			Vector3[] corners = new Vector3[4];
			CachedRectTransform.GetLocalCorners(corners);
			CachedRectTransform.anchoredPosition = 
				value - new Vector2(0.0f, corners[1].y);
		}
	}
	
	// セルの下端の位置を取得、設定するプロパティ
	public Vector2 Bottom
	{
		get {
			Vector3[] corners = new Vector3[4];
			CachedRectTransform.GetLocalCorners(corners);
			return CachedRectTransform.anchoredPosition + 
				new Vector2(0.0f, corners[3].y);
		}
		set {
			Vector3[] corners = new Vector3[4];
			CachedRectTransform.GetLocalCorners(corners);
			CachedRectTransform.anchoredPosition = 
				value - new Vector2(0.0f, corners[3].y);
		}
	}
}
