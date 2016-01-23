using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ScrollRect))]
public class PagingScrollViewController : 
	ViewController, IBeginDragHandler, IEndDragHandler
// ViewControllerクラスを継承して、IBeginDragHandlerインターフェイスと
// IEndDragHandlerインターフェイスを実装する
{
	[SerializeField] private float animationDuration = 0.3f;
	[SerializeField] private float key1InTangent = 0.0f;
	[SerializeField] private float key1OutTangent = 0.1f;
	[SerializeField] private float key2InTangent = 0.0f;
	[SerializeField] private float key2OutTangent = 0.0f;

	[SerializeField] private PageControl pageControl;	// 関連付けるページコントロール
	[SerializeField] private CanvasGroup startButtonArea;

#region OnBeginDrag関数、OnEndDrag関数の実装
	// ScrollRectコンポーネントをキャッシュ
	private ScrollRect cachedScrollRect;
	public ScrollRect CachedScrollRect
	{
		get {
			if(cachedScrollRect == null)
				{ cachedScrollRect = GetComponent<ScrollRect>(); }
			return cachedScrollRect;
		}
	}

	private bool isAnimating = false;		// アニメーション中フラグ
	private Vector2 destPosition;			// 最終的なスクロール位置
	private Vector2 initialPosition;		// 自動スクロール開始時のスクロール位置
	private AnimationCurve animationCurve;	// 自動スクロールのアニメーションカーブ
	private int currentPageIndex = 0;
	private int prevPageIndex = 0;			// 前のページのインデックスを保持

	//Get Started Buttonアニメーション
	private bool isDissolveAnimating = false;		// ボタンのディゾルブアニメーション中フラグ
	private float destAlpha = 0.0f;
	private AnimationCurve buttonAnimationCurve;
	private int prevPageIndexForButton = 0;

	// ドラッグが開始された時に呼ばれる
	public void OnBeginDrag(PointerEventData eventData) {
		// アニメーション中フラグをリセットする
		isAnimating = false;
	}

	// ドラッグが終了した時に呼ばれる
	public void OnEndDrag(PointerEventData eventData) {
		RectTransform scrollContent = CachedScrollRect.content;

		// スクロールビューの現在の動きを止める
		CachedScrollRect.StopMovement();

		float pageWidth = Screen.width;

		// 現在のスクロール位置からフィットさせるページのインデックスを算出する
		currentPageIndex = Mathf.RoundToInt(-(CachedScrollRect.content.anchoredPosition.x) / pageWidth);

		if(currentPageIndex == prevPageIndex && Mathf.Abs(eventData.delta.x) >= 4) {
			// 一定以上の速度でドラッグしていた場合、その方向に1ページ進める
			CachedScrollRect.content.anchoredPosition += new Vector2(eventData.delta.x, 0.0f);
			currentPageIndex += (int)Mathf.Sign(-eventData.delta.x);
		}

		// 先頭や末尾のページの場合、それ以上先にスクロールしないようにする
		if(currentPageIndex < 0) {
			currentPageIndex = 0;
		} else if(currentPageIndex > scrollContent.transform.childCount - 1) {
			currentPageIndex = scrollContent.transform.childCount-1;
		}

		prevPageIndexForButton = prevPageIndex;
		prevPageIndex = currentPageIndex;  // 現在のページのインデックスを保持しておく

		// 最終的なスクロール位置を算出する
		float destX = currentPageIndex * pageWidth;
		destPosition = new Vector2(-destX, CachedScrollRect.content.anchoredPosition.y);

		// 開始時のスクロール位置を保持しておく
		initialPosition = CachedScrollRect.content.anchoredPosition;

		// アニメーションカーブを作成する
		Keyframe keyFrame1 = new Keyframe(Time.time, 0.0f, key1InTangent, key1OutTangent);
		Keyframe keyFrame2 = new Keyframe(Time.time + animationDuration, 1.0f, key2InTangent, key2OutTangent);
		animationCurve = new AnimationCurve(keyFrame1, keyFrame2);

		// アニメーション中フラグをセットする
		isAnimating = true;

		// ページコントロールの表示を更新する
		pageControl.SetCurrentPage(currentPageIndex);
	}
#endregion

#region 自動スクロールアニメーションの実装
	// 毎フレームUpdateメソッドの後に呼ばれる
	void LateUpdate() {
		if(isAnimating) {
			if(Time.time >= animationCurve.keys[animationCurve.length-1].time) {
				// アニメーションカーブの最後のキーフレームを過ぎたら、アニメーションを終了する
				CachedScrollRect.content.anchoredPosition = destPosition;
				isAnimating = false;

				ChangeStartButtonAlpha();

				return;
			}

			// アニメーションカーブから現在のスクロール位置を算出してスクロールビューを移動させる
			Vector2 newPosition = initialPosition + (destPosition - initialPosition) * animationCurve.Evaluate(Time.time);
			CachedScrollRect.content.anchoredPosition = newPosition;
		}

		// Get Started Button ディゾルブ
		if (isDissolveAnimating) {
			if (Time.time >= buttonAnimationCurve.keys[buttonAnimationCurve.length-1].time) {
				isDissolveAnimating = false;
			}

			//ディゾルブ変更
			float newAlpha = destAlpha * buttonAnimationCurve.Evaluate(Time.time);
			startButtonArea.alpha = newAlpha;
		} 
	}
#endregion
	
	// Change Get Start Button Alpha
	void ChangeStartButtonAlpha () {
		int pageControlNum = CachedScrollRect.content.transform.childCount;

		if (currentPageIndex == pageControlNum - 1 && prevPageIndexForButton == pageControlNum - 2) {
			// Appear button
			Keyframe keyFrame1 = new Keyframe(Time.time, 0.0f, key1InTangent, key1OutTangent);
			Keyframe keyFrame2 = new Keyframe(Time.time + animationDuration, 1.0f, key2InTangent, key2OutTangent);
			buttonAnimationCurve = new AnimationCurve(keyFrame1, keyFrame2);

			destAlpha = 1.0f;
			isDissolveAnimating = true;
		} else if (currentPageIndex == pageControlNum - 2 && prevPageIndexForButton == pageControlNum - 1) {
			// Disappear button
			Keyframe keyFrame1 = new Keyframe(Time.time, 0.0f, key1InTangent, key1OutTangent);
			Keyframe keyFrame2 = new Keyframe(Time.time + animationDuration, 1.0f, key2InTangent, key2OutTangent);
			buttonAnimationCurve = new AnimationCurve(keyFrame1, keyFrame2);

			destAlpha = 0.0f;
			isDissolveAnimating = true;
		}
	}


	// インスタンスのロード時Awakeメソッドの後に呼ばれる
	void Start() {
		pageControl.SetNumberOfPages(CachedScrollRect.content.transform.childCount);	// ページ数を5に設定する
		pageControl.SetCurrentPage(0);		// ページコントロールの表示を初期化する
	}

	// 毎フレーム呼ばれる
	void Update() {
	
	}

}
