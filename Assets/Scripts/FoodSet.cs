using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSet : MonoBehaviour
{
	private Player player;

	private void Start()
	{
		player = GameObject.Find("Player").GetComponent<Player>();
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && GameManager.Instance.isCounterVillianSpawn == false)
		{
			while (GameManager.Instance.foodCountLimit > GameManager.Instance.foodCount && player.gottenItemNum > 0)
			{
				player.gottenItemNum--;
				GameManager.Instance.foodCount++;
			}
		}
	}
}
