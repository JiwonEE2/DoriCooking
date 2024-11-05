using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TablePrefab : MonoBehaviour
{
	public int tableNum;
	public int trashCount;
	public Vector2 customerPos;
	public SpriteRenderer objectSpriteRenderer;
	public bool isTableVillianSpawn = false;

	private void Start()
	{
		isTableVillianSpawn = GameManager.Instance.isTableVillianSpawn[tableNum];
		customerPos = new Vector2(transform.position.x, transform.position.y + 1);
		objectSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
		objectSpriteRenderer.sortingOrder = 6;
	}

	private void Update()
	{
		isTableVillianSpawn = GameManager.Instance.isTableVillianSpawn[tableNum];
	}
}
