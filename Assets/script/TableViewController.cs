using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(ScrollRect))]
public class TableViewController<T> : ViewController		// ViewControllerクラスを継承
{
	protected List<T> tableData = new List<T>();			// リスト項目のデータを保持
	[SerializeField] private RectOffset padding;			// スクロールさせる内容のパディング
	[SerializeField] private float spacingHeight = 0.0f;	// 各セルの間隔

	// Scroll Rectコンポーネントをキャッシュ
	private ScrollRect cachedScrollRect;
	public ScrollRect CachedScrollRect
	{
		get {
			if(cachedScrollRect == null) { 
				cachedScrollRect = GetComponent<ScrollRect>(); }
			return cachedScrollRect;
		}
	}

	// インスタンスのロード時に呼ばれる
	protected virtual void Awake()
	{
	}
	
	// リスト項目に対応するセルの高さを返すメソッド
	protected virtual float CellHeightAtIndex(int index)
	{
		// 実際の値を返す処理は継承したクラスで実装する
		//return 0.0f;
		return cellBase.GetComponent<RectTransform>().sizeDelta.y;
	}

	// スクロールさせる内容全体の高さを更新するメソッド
	protected void UpdateContentSize()
	{
		// スクロールさせる内容全体の高さを算出する
		float contentHeight = 0.0f;
		for(int i=0; i<tableData.Count; i++)
		{
			contentHeight += CellHeightAtIndex(i);
			if(i > 0) { contentHeight += spacingHeight; }
		}

		// スクロールさせる内容の高さを設定する
		Vector2 sizeDelta = CachedScrollRect.content.sizeDelta;
		sizeDelta.y = padding.top + contentHeight + padding.bottom;
		CachedScrollRect.content.sizeDelta = sizeDelta;
	}

#region セルを作成するメソッドとセルの内容を更新するメソッドの実装
	[SerializeField] private GameObject cellBase;	// コピー元のセル
	private LinkedList<TableViewCell<T>> cells = 
		new LinkedList<TableViewCell<T>>();			// セルを保持

	// インスタンスのロード時Awakeメソッドの後に呼ばれる
	protected virtual void Start()
	{
		// コピー元のセルは非アクティブにしておく
		cellBase.SetActive(false);

#region セルを再利用する処理の実装
		// Scroll RectコンポーネントのOn Value Changedイベントのイベントリスナーを設定する
		CachedScrollRect.onValueChanged.AddListener(OnScrollPosChanged);
#endregion
	}

	// セルを作成するメソッド
	private TableViewCell<T> CreateCellForIndex(int index)
	{
		// コピー元のセルから新しいセルを作成する
		GameObject obj = Instantiate(cellBase) as GameObject;
		obj.SetActive(true);
		TableViewCell<T> cell = obj.GetComponent<TableViewCell<T>>();

		// 親要素の付け替えをおこなうとスケールやサイズが失われるため、変数に保持しておく
		Vector3 scale = cell.transform.localScale;
		Vector2 sizeDelta = cell.CachedRectTransform.sizeDelta;
		Vector2 offsetMin = cell.CachedRectTransform.offsetMin;
		Vector2 offsetMax = cell.CachedRectTransform.offsetMax;

		cell.transform.SetParent(cellBase.transform.parent);

		// セルのスケールやサイズを設定する
		cell.transform.localScale = scale;
		cell.CachedRectTransform.sizeDelta = sizeDelta;
		cell.CachedRectTransform.offsetMin = offsetMin;
		cell.CachedRectTransform.offsetMax = offsetMax;

		// 指定したインデックスのリスト項目に対応するセルとして内容を更新する
		UpdateCellForIndex(cell, index);

		cells.AddLast(cell);

		return cell;
	}

	// セルの内容を更新するメソッド
	private void UpdateCellForIndex(TableViewCell<T> cell, int index)
	{
		// セルに対応するリスト項目のインデックスを設定する
		cell.DataIndex = index;

		if(cell.DataIndex >= 0 && cell.DataIndex <= tableData.Count-1)
		{
			// セルに対応するリスト項目があれば、セルをアクティブにして内容を更新し、高さを設定する
			cell.gameObject.SetActive(true);
			cell.UpdateContent(tableData[cell.DataIndex]);
			cell.Height = CellHeightAtIndex(cell.DataIndex);
		}
		else
		{
			// セルに対応するリスト項目がなかったら、セルを非アクティブにして表示しない
			cell.gameObject.SetActive(false);
		}
	}
#endregion

#region visibleRectの定義とvisibleRectを更新するメソッドの実装
	private Rect visibleRect;								// リスト項目をセルとして表示する範囲を示す矩形
	[SerializeField] private RectOffset visibleRectPadding;	// visibleRectのパディング

	// visibleRectを更新するためのメソッド
	private void UpdateVisibleRect()
	{
		// visibleRectの位置はスクロールさせる内容の基準点からの相対位置
		visibleRect.x = 
			CachedScrollRect.content.anchoredPosition.x + visibleRectPadding.left;
		visibleRect.y = 
			-CachedScrollRect.content.anchoredPosition.y + visibleRectPadding.top;

		// visibleRectのサイズはスクロールビューのサイズ+パディング
		visibleRect.width = CachedRectTransform.rect.width + 
			visibleRectPadding.left + visibleRectPadding.right;
		visibleRect.height = CachedRectTransform.rect.height + 
			visibleRectPadding.top + visibleRectPadding.bottom;
	}
#endregion

#region テーブルビューの表示内容を更新する処理の実装
	protected void UpdateContents()
	{
		UpdateContentSize();	// スクロールさせる内容のサイズを更新する
		UpdateVisibleRect();	// visibleRectを更新する

		if(cells.Count < 1)
		{
			// セルが1つもない場合、visibleRectの範囲に入る最初のリスト項目を探して、
			// それに対応するセルを作成する
			Vector2 cellTop = new Vector2(0.0f, -padding.top);
			for(int i=0; i<tableData.Count; i++)
			{
				float cellHeight = CellHeightAtIndex(i);
				Vector2 cellBottom = cellTop + new Vector2(0.0f, -cellHeight);
				if((cellTop.y <= visibleRect.y && 
					cellTop.y >= visibleRect.y - visibleRect.height) || 
				   (cellBottom.y <= visibleRect.y && 
				   	cellBottom.y >= visibleRect.y - visibleRect.height))
				{
					TableViewCell<T> cell = CreateCellForIndex(i);
					cell.Top = cellTop;
					break;
				}
				cellTop = cellBottom + new Vector2(0.0f, spacingHeight);
			}

			// visibleRectの範囲内に空きがあればセルを作成する
			FillVisibleRectWithCells();
		}
		else
		{
			// すでにセルがある場合、先頭のセルから順に対応するリスト項目の
			// インデックスを設定し直し、位置と内容を更新する
			LinkedListNode<TableViewCell<T>> node = cells.First;
			UpdateCellForIndex(node.Value, node.Value.DataIndex);
			node = node.Next;
			
			while(node != null)
			{
				UpdateCellForIndex(node.Value, node.Previous.Value.DataIndex + 1);
				node.Value.Top = 
					node.Previous.Value.Bottom + new Vector2(0.0f, -spacingHeight);
				node = node.Next;
			}

			// visibleRectの範囲内に空きがあればセルを作成する
			FillVisibleRectWithCells();
		}
	}

	// visibleRectの範囲内に表示される分のセルを作成するメソッド
	private void FillVisibleRectWithCells()
	{
		// セルがなければ何もしない
		if(cells.Count < 1)
		{
			return;
		}

		// 表示されている最後のセルに対応するリスト項目の次のリスト項目があり、
		// かつ、そのセルがvisibleRectの範囲内に入るようであれば、対応するセルを作成する
		TableViewCell<T> lastCell = cells.Last.Value;
		int nextCellDataIndex = lastCell.DataIndex + 1;
		Vector2 nextCellTop = lastCell.Bottom + new Vector2(0.0f, -spacingHeight);

		while(nextCellDataIndex < tableData.Count && 
			nextCellTop.y >= visibleRect.y - visibleRect.height)
		{
			TableViewCell<T> cell = CreateCellForIndex(nextCellDataIndex);
			cell.Top = nextCellTop;

			lastCell = cell;
			nextCellDataIndex = lastCell.DataIndex + 1;
			nextCellTop = lastCell.Bottom + new Vector2(0.0f, -spacingHeight);
		}
	}
#endregion

#region セルを再利用する処理の実装
	private Vector2 prevScrollPos;	// 前回のスクロール位置を保持

	// スクロールビューがスクロールされたときに呼ばれる
	public void OnScrollPosChanged(Vector2 scrollPos)
	{
		// visibleRectを更新する
		UpdateVisibleRect();
		// スクロールした方向によって、セルを再利用して表示を更新する
		ReuseCells((scrollPos.y < prevScrollPos.y)? 1: -1);

		prevScrollPos = scrollPos;
	}

	// セルを再利用して表示を更新するメソッド
	private void ReuseCells(int scrollDirection)
	{
		if(cells.Count < 1)
		{
			return;
		}

		if(scrollDirection > 0)
		{
			// 上にスクロールしている場合、visibleRectの範囲より上にあるセルを
			// 順に下に移動して内容を更新する
			TableViewCell<T> firstCell = cells.First.Value;
			while(firstCell.Bottom.y > visibleRect.y)
			{
				TableViewCell<T> lastCell = cells.Last.Value;
				UpdateCellForIndex(firstCell, lastCell.DataIndex + 1);
				firstCell.Top = lastCell.Bottom + new Vector2(0.0f, -spacingHeight);

				cells.AddLast(firstCell);
				cells.RemoveFirst();
				firstCell = cells.First.Value;
			}

			// visibleRectの範囲内に空きがあればセルを作成する
			FillVisibleRectWithCells();
		}
		else if(scrollDirection < 0)
		{
			// 下にスクロールしている場合、visibleRectの範囲より下にあるセルを
			// 順に上に移動して内容を更新する
			TableViewCell<T> lastCell = cells.Last.Value;
			while(lastCell.Top.y < visibleRect.y - visibleRect.height)
			{
				TableViewCell<T> firstCell = cells.First.Value;
				UpdateCellForIndex(lastCell, firstCell.DataIndex - 1);
				lastCell.Bottom = firstCell.Top + new Vector2(0.0f, spacingHeight);

				cells.AddFirst(lastCell);
				cells.RemoveLast();
				lastCell = cells.Last.Value;
			}
		}
	}
#endregion
}
