using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterVillian : Villian
{
	private void Awake()
	{
		transform.position = new Vector2(2, -5);
	}
}
