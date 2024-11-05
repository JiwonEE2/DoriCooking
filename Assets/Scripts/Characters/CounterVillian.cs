using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterVillian : Villian
{
	private void Awake()
	{
		transform.position = new Vector2(2, -5);
		GameManager.Instance.isCounterVillianSpawn = true;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		GameManager.Instance.isCounterVillianSpawn = false;
	}
}
