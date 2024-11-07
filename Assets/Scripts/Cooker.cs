using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooker : MonoBehaviour
{
	public int cookerNum;
	public int foodCount = 0;

	public SpriteRenderer objectSpriteRenderer1;
	public SpriteRenderer objectSpriteRenderer2;

	private void Start()
	{
		objectSpriteRenderer1.sortingOrder = 6;
		objectSpriteRenderer2.sortingOrder = 6;
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
			objectSpriteRenderer1.sprite = SpriteManager.Instance.foodSprite;
			objectSpriteRenderer2.sprite = SpriteManager.Instance.foodSprite;
		}
		else
		{
			objectSpriteRenderer1.sprite = null;
			objectSpriteRenderer2.sprite = null;
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
				yield return new WaitForEndOfFrame();
			}
		}
	}
}
