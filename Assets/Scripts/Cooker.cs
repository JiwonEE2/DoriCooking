using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooker : MonoBehaviour
{
	// ��ȭ�ϸ� 2�ʷ� �ٱ�
	public float cookDuration = 4f;

	public int foodNum = 0;

	private void Start()
	{
		StartCoroutine(CookCoroutine());
	}

	private IEnumerator CookCoroutine()
	{
		while (true)
		{
			yield return new WaitForSeconds(cookDuration);
			foodNum++;
		}
	}
}
