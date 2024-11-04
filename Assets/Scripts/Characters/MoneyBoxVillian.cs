using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyBoxVillian : Villian
{
	private void Awake()
	{
		villianDestroyTime /= 2f;
		transform.position = new Vector2(1, -2);
	}
	protected override void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			villianDestroyTimer += Time.deltaTime;
		}
		if (villianDestroyTimer > villianDestroyTime)
		{
			GameManager.Instance.isMoneyBoxVillianSpawn = false;
			UIManager.Instance.isVillianSpawn = false;
			GameManager.Instance.villianTimer = 0;
			GameManager.Instance.needNewVillianTimerSetting = true;
			Destroy(gameObject);
		}
	}
}
