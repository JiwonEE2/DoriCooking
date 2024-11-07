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

	private void Start()
	{
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
}
