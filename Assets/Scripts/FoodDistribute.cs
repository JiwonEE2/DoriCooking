using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodDistribute : MonoBehaviour
{
	private Player player;

	private void Start()
	{
		player = GameObject.Find("Player").GetComponent<Player>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && GameManager.Instance.isCounterVillianSpawn == false)
		{
			// ���� �����ֱ�
			GameManager.Instance.isSellingFood = true;
			GameManager.Instance.foodSellTimer = 0;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && GameManager.Instance.isCounterUp == false)
		{
			// ���� �����ֱ�
			GameManager.Instance.isSellingFood = false;
		}
	}
}
