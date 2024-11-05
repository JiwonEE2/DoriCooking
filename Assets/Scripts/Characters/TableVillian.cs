using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableVillian : Villian
{
	private void Awake()
	{
		transform.position = GameManager.Instance.destroyedCustomerPosition;
		GameManager.Instance.isTableVillianSpawn[GameManager.Instance.destroyedCustomerTableNum] = true;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		GameManager.Instance.isTableVillianSpawn[GameManager.Instance.destroyedCustomerTableNum] = false;
	}
}
