using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
	public CustomerPrefab customerPrefab1;
	public CustomerPrefab customerPrefab2;
	public Vector2 spawnPoint = new Vector2(-5, 1);

	private CustomerPrefab customer;
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		// 손님 생성될 자리가 비어있으면 생성
		if (GameManager.Instance.isCustomerStanding == false)
		{
			float ran = Random.value;
			if (ran * 100 <= GameManager.Instance.customerSpawnRate[GameManager.Instance.level])
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
