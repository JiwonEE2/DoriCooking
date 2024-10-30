using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : SingletonManager<GameManager>
{
	public int level;
	public float[] customerSpawnRate = { 100f, 100f, 90f, 90f, 90f, 80f, 80f, 80f, 60f, 60f, 40f, 40f, 20f };

	public bool isCustomerStanding = false;
	public float customerTimer = 0;

	public float foodSellTimer = 0;
	public float foodSellDuration = 0.5f;
	public int foodCount = 0;
	public int foodCountLimit = 99;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		customerTimer += Time.deltaTime;
		foodSellTimer += Time.deltaTime;
	}
}
