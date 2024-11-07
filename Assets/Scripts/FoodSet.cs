using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSet : MonoBehaviour
{
	private Player player;
	private SpriteRenderer foodRenderer;

	private void Start()
	{
		player = GameObject.Find("Player").GetComponent<Player>();
		foodRenderer = transform.Find("FoodRenderer").GetComponent<SpriteRenderer>();
		foodRenderer.sprite = SpriteManager.Instance.foodSprite;
		foodRenderer.sortingLayerName = "tableItem";
	}

	private void Update()
	{
		FoodRender();
	}

	private void FoodRender()
	{
		if (GameManager.Instance.foodCount > 0)
		{
			foodRenderer.enabled = true;
		}
		else
		{
			foodRenderer.enabled = false;
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && GameManager.Instance.isCounterVillianSpawn == false)
		{
			while (GameManager.Instance.foodCountLimit > GameManager.Instance.foodCount && player.gottenItemNum > 0)
			{
				player.gottenItemNum--;
				GameManager.Instance.foodCount++;
			}
		}
	}
}
