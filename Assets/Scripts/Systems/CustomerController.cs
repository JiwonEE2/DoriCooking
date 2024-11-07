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

	public List<CustomerPrefab> customers;

	private void Update()
	{
		CustomerSpawn();
		CustomerDestroy();
	}

	private void CustomerSpawn()
	{
		// �մ� ������ �ڸ��� ����ְ�, �մ� ������ �� 1�ʰ� ������ ��
		if (GameManager.Instance.isCustomerStanding == false && GameManager.Instance.customerTimer >= spawnDuration && GameManager.Instance.isCounterVillianSpawn == false)
		{
			float ran = Random.value;
			if (ran * 100 <= GameManager.Instance.customerSpawnRate[GameManager.Instance.level - 1])
			{
				customer = Instantiate(customerPrefab1, spawnPoint, Quaternion.identity);
				customers.Add(customer);
			}
			else
			{
				customer = Instantiate(customerPrefab2, spawnPoint, Quaternion.identity);
				customers.Add(customer);
			}
			GameManager.Instance.isCustomerStanding = true;
		}
	}

	private void CustomerDestroy()
	{
		// �մ��� �ڸ����� �� �ٸ����� ���⿡�� ����
		for (int i = 0; i < customers.Count; i++)
		{
			if (customers[i].readyDestroy)
			{
				Destroy(customers[i].gameObject);
				customers.Remove(customers[i]);
			}
		}
	}
}
