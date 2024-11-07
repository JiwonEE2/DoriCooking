using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooker : MonoBehaviour
{
	private GameObject objectRenderer;
	private SpriteRenderer objectSpriteRenderer1;
	private SpriteRenderer objectSpriteRenderer2;

	public int cookerNum;
	public int foodCount = 0;

	private Player player;

	private void Start()
	{
		player = GameObject.Find("Player").GetComponent<Player>();
		objectRenderer = transform.Find("ObjectRenderer").gameObject;

		objectSpriteRenderer1 = objectRenderer.transform.Find("Renderer0").GetComponent<SpriteRenderer>();
		objectSpriteRenderer2 = objectRenderer.transform.Find("Renderer1").GetComponent<SpriteRenderer>();
		objectSpriteRenderer1.sprite = SpriteManager.Instance.foodSprite;
		objectSpriteRenderer2.sprite = SpriteManager.Instance.foodSprite;
		objectSpriteRenderer1.sortingLayerName = "tableItem";
		objectSpriteRenderer2.sortingLayerName = "tableItem";
		StartCoroutine(CookCoroutine());
	}

	private void Update()
	{
		FoodRender();
	}

	private void FoodRender()
	{
		if (foodCount > 0)
		{
			objectRenderer.SetActive(true);
		}
		else
		{
			objectRenderer.SetActive(false);
		}
	}

	private IEnumerator CookCoroutine()
	{
		while (true)
		{
			if (foodCount < GameManager.Instance.cookerFoodLimit[cookerNum])
			{
				yield return new WaitForSeconds(GameManager.Instance.cookDuration[cookerNum]);
				if (GameManager.Instance.isCookerVillianSpawn[cookerNum] == false)
				{
					foodCount++;
					yield return null;
				}
			}
			else
			{
				yield return null;
			}
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && GameManager.Instance.isCookerVillianSpawn[cookerNum] == false)
		{
			if (player.currentGottenItem == Player.ITEM.FOOD || player.currentGottenItem == Player.ITEM.NONE)
			{
				while (foodCount > 0 && player.gottenItemNum < GameManager.Instance.playerGettableItemCount)
				{
					player.gottenItemNum++;
					foodCount--;
					player.currentGottenItem = Player.ITEM.FOOD;
				}
			}
		}
	}
}
