using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour
{
	public MoneyPrefab moneyPrefab;
	public float duration = 2;

	// Start is called before the first frame update
	void Start()
	{
		StartCoroutine(MoneySpawnCoroutine());
	}

	// Update is called once per frame
	void Update()
	{
	}

	private IEnumerator MoneySpawnCoroutine()
	{
		while (true)
		{
			yield return new WaitForSeconds(duration);
			Instantiate(moneyPrefab, new Vector2(-6, 2), Quaternion.identity);
		}
	}
}
