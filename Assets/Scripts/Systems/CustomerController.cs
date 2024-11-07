using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerController : MonoBehaviour
{
	public CustomerPrefab customerPrefab1;
	public CustomerPrefab customerPrefab2;
	public Vector2 spawnPoint = new Vector2(2, -5);
	public float spawnDuration = 1f;

	private CustomerPrefab customer;

	public List<CustomerPrefab> customers1;
	public List<CustomerPrefab> customers2;

	private void Update()
	{
		CustomerSpawn();
		CustomerDestroy();
	}

	private void CustomerSpawn()
	{
		// 손님 생성될 자리가 비어있고, 손님 삭제된 지 1초가 지났을 때
		if (GameManager.Instance.isCustomerStanding == false && GameManager.Instance.customerTimer >= spawnDuration && GameManager.Instance.isCounterVillianSpawn == false)
		{
			float ran = Random.value;
			if (ran * 100 <= GameManager.Instance.customerSpawnRate[GameManager.Instance.level - 1])
			{
				customer = Instantiate(customerPrefab1, spawnPoint, Quaternion.identity);
				customers1.Add(customer);
			}
			else
			{
				customer = Instantiate(customerPrefab2, spawnPoint, Quaternion.identity);
				customers2.Add(customer);
			}
			GameManager.Instance.isCustomerStanding = true;
		}
	}

	private void CustomerDestroy()
	{
		// 손님이 자리에서 밥 다먹으면 여기에서 삭제
		for (int i = 0; i < customers1.Count; i++)
		{
			if (customers1[i].readyDestroy)
			{
				Destroy(customers1[i].gameObject);
				customers1.Remove(customers1[i]);
			}
		}
		for (int i = 0; i < customers2.Count; i++)
		{
			if (customers2[i].readyDestroy)
			{
				Destroy(customers2[i].gameObject);
				customers2.Remove(customers2[i]);
			}
		}
	}
}
