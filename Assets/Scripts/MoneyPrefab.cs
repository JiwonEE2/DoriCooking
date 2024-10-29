using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyPrefab : MonoBehaviour
{
	public Player player;
	private void Awake()
	{
		player = GetComponent<Player>();
	}
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
}
