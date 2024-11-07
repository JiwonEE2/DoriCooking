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

	public bool isReadyToGetCustomer = true;
	// coroutine 한 번만 시행을 위함
	private bool waitCoroutine = false;

	private Player player;

	private void Start()
	{
		player = GameObject.Find("Player").GetComponent<Player>();

		isTableVillianSpawn = GameManager.Instance.isTableVillianSpawn[tableNum];
		customerPos = new Vector2(transform.position.x, transform.position.y + 1);
		objectSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
		objectSpriteRenderer.sortingLayerName = "table";
	}

	private void Update()
	{
		isTableVillianSpawn = GameManager.Instance.isTableVillianSpawn[tableNum];
		if (trashCount <= 0 && isReadyToGetCustomer == false && waitCoroutine == false)
		{
			StartCoroutine(ReadyCheckCoroutine());
		}
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

	private IEnumerator ReadyCheckCoroutine()
	{
		waitCoroutine = true;
		yield return new WaitForSeconds(1f);
		isReadyToGetCustomer = true;
	}
}
