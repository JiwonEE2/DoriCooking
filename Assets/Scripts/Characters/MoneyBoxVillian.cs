using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyBoxVillian : Villian
{
	private float stealTimer = 0;
	private float stealTime = 30f;

	private void Awake()
	{
		villianDestroyTime /= 2f;
		transform.position = new Vector2(1, -2);
		GameManager.Instance.isMoneyBoxVillianSpawn = true;
	}

	private void Update()
	{
		stealTimer += Time.deltaTime;
		if (stealTimer > stealTime)
		{
			UIManager.Instance.money /= 2;
			Destroy(gameObject);
		}
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		GameManager.Instance.isMoneyBoxVillianSpawn = false;
	}
}
