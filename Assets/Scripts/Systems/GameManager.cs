using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : SingletonManager<GameManager>
{
	public int level;
	public float[] customerSpawnRate = { 100f, 100f, 90f, 90f, 90f, 80f, 80f, 80f, 60f, 60f, 40f, 40f, 20f };

	public bool isCustomerStanding = false;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
}
