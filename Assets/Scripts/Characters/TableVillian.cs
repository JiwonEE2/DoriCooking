using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableVillian : Villian
{
	private int villianSpawnedTableNum;
	private void Awake()
	{
		villianSpawnedTableNum = GameManager.Instance.destroyedCustomerTableNum;
		transform.position = GameManager.Instance.destroyedCustomerPosition;
		GameManager.Instance.isTableVillianSpawn[villianSpawnedTableNum] = true;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		GameManager.Instance.isTableVillianSpawn[villianSpawnedTableNum] = false;
	}
}
