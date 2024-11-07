using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableController : SingletonManager<TableController>
{
	public List<TablePrefab> readyTables;
	public List<TablePrefab> trashedTables;

	private void Start()
	{
		readyTables.Add(transform.Find("Table0").GetComponent<TablePrefab>());
		readyTables.Add(transform.Find("Table1").GetComponent<TablePrefab>());
		readyTables.Add(transform.Find("Table2").GetComponent<TablePrefab>());
		readyTables.Add(transform.Find("Table3").GetComponent<TablePrefab>());
		readyTables.Add(transform.Find("Table4").GetComponent<TablePrefab>());
		readyTables.Add(transform.Find("Table5").GetComponent<TablePrefab>());
	}

	private void Update()
	{
		for (int i = 0; i < trashedTables.Count; i++)
		{
			if (trashedTables[i].GetComponent<TablePrefab>().trashCount <= 0 && trashedTables[i].GetComponent<TablePrefab>().isReadyToGetCustomer)
			{
				trashedTables[i].GetComponent<TablePrefab>().objectSpriteRenderer.sprite = null;
				readyTables.Add(trashedTables[i]);
				trashedTables.RemoveAt(i);
			}
		}
		for (int i = 0; i < readyTables.Count; i++)
		{
			if (readyTables[i].GetComponent<TablePrefab>().trashCount > 0)
			{
				readyTables[i].GetComponent<TablePrefab>().objectSpriteRenderer.sprite = SpriteManager.Instance.trashSprite;
				trashedTables.Add(readyTables[i]);
				readyTables.RemoveAt(i);
			}
		}
	}
}
