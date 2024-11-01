using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TablePrefab : MonoBehaviour
{
	public int tableNum;
	public int trashCount;
	public Vector2 customerPos;
	public SpriteRenderer objectSpriteRenderer;

	private void Start()
	{
		customerPos = new Vector2(transform.position.x, transform.position.y + 1);
		objectSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
	}
}
