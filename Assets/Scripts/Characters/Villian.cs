using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villian : MonoBehaviour
{
	public float villianDestroyTimer = 0;
	public float villianDestroyTime = 3f;

	protected virtual void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			villianDestroyTimer += Time.deltaTime;
		}
		if (villianDestroyTimer > villianDestroyTime)
		{
			UIManager.Instance.isVillianSpawn = false;
			GameManager.Instance.villianTimer = 0;
			GameManager.Instance.needNewVillianTimerSetting = true;
			Destroy(gameObject);
		}
	}
}
