using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enforce : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && GameManager.Instance.isMoneyBoxVillianSpawn == false)
		{
			UIManager.Instance.enforcePopup.SetActive(true);
			Time.timeScale = 0;
		}
	}
}
