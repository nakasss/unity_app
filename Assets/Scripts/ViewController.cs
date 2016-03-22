using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ViewController : MonoBehaviour {
	// Rect Transformコンポーネントをキャッシュ
	private RectTransform cachedRectTransform;
	public RectTransform CachedRectTransform {
		get {
			if(cachedRectTransform == null)
				{ cachedRectTransform = GetComponent<RectTransform>(); }
			return cachedRectTransform;
		}
	}

	// ビューのタイトルを取得、設定するプロパティ
	public virtual string Title { get { return ""; } set {} }
}
