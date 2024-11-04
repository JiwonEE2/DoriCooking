using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Villian : MonoBehaviour
{
	public float villianDestroyTimer = 0;
	public float villianDestroyTime = 3f;
	public bool isMoneyBoxVillian = false;

	private void Awake()
	{
		if (GameManager.Instance.isMoneyBoxVillianSpawn)
		{
			isMoneyBoxVillian = true;
			villianDestroyTime = 1.5f;
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			villianDestroyTimer += Time.deltaTime;
		}
		if (villianDestroyTimer > villianDestroyTime)
		{
			if (isMoneyBoxVillian)
			{
				GameManager.Instance.isMoneyBoxVillianSpawn = false;
				UIManager.Instance.isVillianSpawn = false;
				GameManager.Instance.villianTimer = 0;
				GameManager.Instance.newVillianTimerSetting = true;
				Destroy(gameObject);
			}
			else
			{
				UIManager.Instance.isVillianSpawn = false;
				GameManager.Instance.villianTimer = 0;
				GameManager.Instance.newVillianTimerSetting = true;
				Destroy(gameObject);
			}
		}
	}
}
