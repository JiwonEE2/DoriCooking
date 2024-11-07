using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerPool : MonoBehaviour
{
	public static CustomerPool pool;
	public CustomerPrefab customerPrefab;

	private void Awake()
	{
		pool = this;
	}

	List<CustomerPrefab> poolList = new();

	public void Push(CustomerPrefab customer)
	{
		poolList.Add(customer);
		customer.gameObject.SetActive(false);
		customer.transform.SetParent(transform, false);
	}

	public CustomerPrefab Pop()
	{
		if (poolList.Count <= 0)
		{
			Push(Instantiate(customerPrefab));
		}
		CustomerPrefab customer = poolList[0];
		poolList.Remove(customer);
		customer.gameObject.SetActive(true);
		// ���߿� ī���͵� ���� �ڽ����� �����Ͽ� ����ϰ� �����Ǹ� ���ڴ�.
		customer.transform.SetParent(null);
		return customer;
	}
}
