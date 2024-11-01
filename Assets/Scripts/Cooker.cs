using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooker : MonoBehaviour
{
	public int cookerNum;
	public int foodCount = 0;

	private void Start()
	{
		StartCoroutine(CookCoroutine());
	}

	private IEnumerator CookCoroutine()
	{
		while (true)
		{
			if (foodCount < GameManager.Instance.cookerFoodLimit[cookerNum])
			{
				yield return new WaitForSeconds(GameManager.Instance.cookDuration[cookerNum]);
				foodCount++;
			}
			else
			{
				yield return new WaitForEndOfFrame();
			}
		}
	}
}
