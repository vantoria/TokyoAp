using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItem : MonoBehaviour 
{
	// -----------------------------ITEMS------------------------------------
	public class CurrItems
	{
		public ItemPickup.Type type;
		public int total;

		public CurrItems(ItemPickup.Type type, int total)
		{
			this.type = type;
			this.total = total;
		}
	}
	public static List<CurrItems> currItemList = new List<CurrItems>();

	void Start () 
	{ ItemInitialization(); }

	void ItemInitialization()
	{
		// Add ItemType to List.
		for (int i = 0; i < System.Enum.GetValues(typeof(ItemPickup.Type)).Length; i++)
		{ currItemList.Add(new CurrItems((ItemPickup.Type)i, 0)); }

		// Set all item value to 0 in UI.
		Transform canvas = GameObject.FindGameObjectWithTag("Canvas").transform;
		for (int i = 0; i < canvas.childCount; i++)
		{
			Transform canvasChild = canvas.GetChild(i);
			if (canvasChild.name == "Item_Button")
			{
				foreach (Transform child in canvasChild)
				{ child.FindChild("Total").GetComponent<Text>().text = currItemList[i].total.ToString(); }
			}
		}
	}

	static void updateItemUiValue(ItemPickup.Type type)
	{
		int val = (int)type;

		Transform canvasChild = GameObject.FindGameObjectWithTag("ItemButton").transform.GetChild(val);
		canvasChild.FindChild("Total").GetComponent<Text>().text = currItemList[val].total.ToString();
	}

	static void addItem(int i)
	{
		if (currItemList[i].total > 9) return;
		currItemList[i].total += 1;
	}

	static void subtractItem(int i)
	{
		if (currItemList[i].total == 0) return;
		currItemList[i].total -= 1;
	}

	public static void ItemChange(ItemPickup.Type type, bool isAdd)
	{
		for (int i = 0; i < currItemList.Count; i++)
		{
			if (currItemList[i].type == type)
			{
				if (isAdd) addItem(i);
				else subtractItem(i);
				updateItemUiValue(type);
				return;
			}
		}
	}
}
