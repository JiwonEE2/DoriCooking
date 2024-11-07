using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyBox : MonoBehaviour
{
	private Player player;

	private void Start()
	{
		player = GameObject.Find("Player").GetComponent<Player>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && GameManager.Instance.isMoneyBoxVillianSpawn == false)
		{
			if (player.currentGottenItem == Player.ITEM.MONEY)
			{
				UIManager.Instance.money += 10 * player.gottenItemNum;
				player.gottenItemNum = 0;
			}
		}
	}
}
