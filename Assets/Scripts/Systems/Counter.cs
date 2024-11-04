using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour
{
	public CustomerPrefab customerPrefab1;
	public CustomerPrefab customerPrefab2;
	public Vector2 spawnPoint = new Vector2(2, -5);
	public float spawnDuration = 1f;

	private CustomerPrefab customer;
	public SpriteRenderer counterOnFoodRenderer;

	private void Start()
	{
		counterOnFoodRenderer.sprite = SpriteManager.Instance.foodSprite;
		counterOnFoodRenderer.sortingOrder = 6;
	}

	private void Update()
	{
		CustomerSpawn();
		if (GameManager.Instance.foodCount > 0)
		{
			counterOnFoodRenderer.enabled = true;
		}
		else
		{
			counterOnFoodRenderer.enabled = false;
		}
	}

	public void CustomerSpawn()
	{
		// 손님 생성될 자리가 비어있고, 손님 삭제된 지 1초가 지났을 때
		if (GameManager.Instance.isCustomerStanding == false && GameManager.Instance.customerTimer >= spawnDuration)
		{
			float ran = Random.value;
			if (ran * 100 <= GameManager.Instance.customerSpawnRate[GameManager.Instance.level - 1])
			{
				customer = Instantiate(customerPrefab1, spawnPoint, Quaternion.identity);
			}
			else
			{
				customer = Instantiate(customerPrefab2, spawnPoint, Quaternion.identity);
			}
			GameManager.Instance.isCustomerStanding = true;
		}
	}
}
