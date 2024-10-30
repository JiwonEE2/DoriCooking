using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooker : MonoBehaviour
{
	// ��ȭ�ϸ� 2�ʷ� �ٱ�
	public float cookDuration = 4f;
	public int foodCount = 0;
	public int foodCountLimit = 6;

	private void Start()
	{
		StartCoroutine(CookCoroutine());
	}

	private IEnumerator CookCoroutine()
	{
		while (true)
		{
			if (foodCount < foodCountLimit)
			{
				yield return new WaitForSeconds(cookDuration);
				foodCount++;
			}
			else
			{
				yield return new WaitForEndOfFrame();
			}
		}
	}
}
