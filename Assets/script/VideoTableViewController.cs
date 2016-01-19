﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


[RequireComponent(typeof(ScrollRect))]
public class VideoTableViewController : TableViewController<VideoData> {

	private void LoadData() {
		tableData = new List<VideoData>() {
			new VideoData {title = "TOMORROWLAND"},
			new VideoData {title = "VOLVO OCEAN RACE"},
			new VideoData {title = "EY BUILDING A BETTER WORKING WORLD IN VR"},
			new VideoData {title = "TOMORROWLAND"},
			new VideoData {title = "VOLVO OCEAN RACE"},
			new VideoData {title = "EY BUILDING A BETTER WORKING WORLD IN VR"},
			new VideoData {title = "TOMORROWLAND"},
			new VideoData {title = "VOLVO OCEAN RACE"},
			new VideoData {title = "EY BUILDING A BETTER WORKING WORLD IN VR"}
		};

		UpdateContents();
	}

	/*
	// リスト項目に対応するセルの高さを返すメソッド
	protected override float CellHeightAtIndex(int index)
	{
		if(index >= 0 && index <= tableData.Count-1)
		{
			if(tableData[index].price >= 1000)
			{
				// 価格が1000以上のアイテムを表示するセルの高さを返す
				return 240.0f;
			}
			if(tableData[index].price >= 500)
			{
				// 価格が500以上のアイテムを表示するセルの高さを返す
				return 160.0f;
			}
		}
		return 128.0f;
	}
	*/

	// インスタンスのロード時に呼ばれる
	protected override void Awake()
	{
		// ベースクラスのAwakeメソッドを呼ぶ
		base.Awake();
	}

	// インスタンスのロード時Awakeメソッドの後に呼ばれる
	protected override void Start()
	{
		// ベースクラスのStartメソッドを呼ぶ
		base.Start();

		// リスト項目のデータを読み込む
		LoadData();

/*
#region アイテム一覧画面をナビゲーションビューに対応させる
		if(navigationView != null)
		{
			// ナビゲーションビューの最初のビューとして設定する
			navigationView.Push(this);
		}
#endregion
*/

	}

/*
#region アイテム一覧画面をナビゲーションビューに対応させる
	// ナビゲーションビューを保持
	[SerializeField] private NavigationViewController navigationView;

	// ビューのタイトルを返す
	public override string Title { get { return "SHOP"; } }
#endregion

#region アイテム詳細画面に遷移させる処理の実装
	// アイテム詳細画面のビューを保持
	[SerializeField] private ShopDetailViewController detailView;

	// セルが選択されたときに呼ばれるメソッド
	public void OnPressCell(ShopItemTableViewCell cell)
	{
		if(navigationView != null)
		{
			// 選択されたセルからアイテムのデータを取得して、アイテム詳細画面の内容を更新する
			detailView.UpdateContent(tableData[cell.DataIndex]);
			// アイテム詳細画面に遷移する
			navigationView.Push(detailView);
		}
	}
#endregion
*/
}
