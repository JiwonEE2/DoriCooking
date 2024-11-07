using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableController : SingletonManager<TableController>
{
	public List<GameObject> emptyTables;
	public List<GameObject> trashedTables;

	private void Update()
	{
		for (int i = 0; i < trashedTables.Count; i++)
		{
			if (trashedTables[i].GetComponent<TablePrefab>().trashCount <= 0)
			{
				trashedTables[i].GetComponent<TablePrefab>().objectSpriteRenderer.sprite = null;
				emptyTables.Add(trashedTables[i]);
				trashedTables.RemoveAt(i);
			}
		}
		for (int i = 0; i < emptyTables.Count; i++)
		{
			if (emptyTables[i].GetComponent<TablePrefab>().trashCount > 0)
			{
				emptyTables[i].GetComponent<TablePrefab>().objectSpriteRenderer.sprite = SpriteManager.Instance.trashSprite;
				trashedTables.Add(emptyTables[i]);
				emptyTables.RemoveAt(i);
			}
		}
	}
}
