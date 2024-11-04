using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableVillian : Villian
{
	private void Awake()
	{
		transform.position = GameManager.Instance.destroyedCustomerPosition;
	}
}
