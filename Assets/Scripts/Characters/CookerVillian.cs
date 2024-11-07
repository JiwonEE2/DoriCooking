using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CookerVillian : Villian
{
	private int cookerNum;

	private void Awake()
	{
		switch (Random.Range(0, 4))
		{
			case 0:
				transform.position = new Vector2(4, Random.Range(-1, -3));
				cookerNum = 0;
				break;
			case 1:
				transform.position = new Vector2(7, Random.Range(-1, -3));
				cookerNum = 0;
				break;
			case 2:
				transform.position = new Vector2(8, Random.Range(-1, -3));
				cookerNum = 1;
				break;
			case 3:
				transform.position = new Vector2(11, Random.Range(-1, -3));
				cookerNum = 1;
				break;
		}
		GameManager.Instance.isCookerVillianSpawn[cookerNum] = true;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		GameManager.Instance.isCookerVillianSpawn[cookerNum] = false;
	}
}
