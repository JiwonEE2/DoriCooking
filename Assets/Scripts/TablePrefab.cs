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

	private Player player;

	private void Start()
	{
		player = GameObject.Find("Player").GetComponent<Player>();

		isTableVillianSpawn = GameManager.Instance.isTableVillianSpawn[tableNum];
		customerPos = new Vector2(transform.position.x, transform.position.y + 1);
		objectSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
		objectSpriteRenderer.sortingOrder = 6;
	}

	private void Update()
	{
		isTableVillianSpawn = GameManager.Instance.isTableVillianSpawn[tableNum];
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && isTableVillianSpawn == false)
		{
			if (player.currentGottenItem == Player.ITEM.TRASH || player.currentGottenItem == Player.ITEM.NONE)
			{
				while (trashCount > 0 && player.gottenItemNum < GameManager.Instance.playerGettableItemCount)
				{
					player.gottenItemNum++;
					trashCount--;
					player.currentGottenItem = Player.ITEM.TRASH;
				}
			}
		}
	}
}
