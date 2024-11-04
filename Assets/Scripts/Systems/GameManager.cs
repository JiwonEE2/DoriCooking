using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class GameManager : SingletonManager<GameManager>
{
	public int level = 1;
	public float[] customerSpawnRate = { 100f, 100f, 90f, 90f, 90f, 80f, 80f, 80f, 60f, 60f, 40f, 40f, 20f };

	public bool isCustomerStanding = false;
	public float customerTimer = 0;

	public float foodSellTimer = 0;
	[Tooltip("���� �Ǹ� �ӵ�")]
	public float foodSellDuration = 0.5f;
	public int foodCount = 0;
	public int foodCountLimit = 99;

	public bool isSellingFood = false;

	[Tooltip("�÷��̾��� �ʴ� �̵� ĭ ��")]
	public float playerMoveSpeed = 2;
	[Tooltip("�÷��̾ �ִ� ���� �� �ִ� ������ ��")]
	public int playerGettableItemCount = 4;
	[Tooltip("�÷��̾��� ���� ���� �ӵ�")]
	public float playerVillianInteractionSpeed = 3f;

	[Tooltip("cooker ���� ���� �ӵ�")]
	public float[] cookDuration = { 4f, 4f };
	[Tooltip("cooker capacity")]
	public int[] cookerFoodLimit = { 6, 6 };

	[Tooltip("���� ��ȭ ����")]
	public bool isCounterUp = false;

	[Tooltip("�ʴ� ���� �Դ� �ӵ�")]
	public int[] eatFoodSpeedPerSeceond = { 2, 2, 2, 2, 2, 2 };

	public float villianTimer = 0;

	[Tooltip("�մ��� �Ҹ� ��ȣ")]
	public bool isCustomerDestoy = false;
	[Tooltip("�Ҹ�� �մ��� ��ġ")]
	public Vector2 destroyedCustomerPosition;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		customerTimer += Time.deltaTime;
		foodSellTimer += Time.deltaTime;
		villianTimer += Time.deltaTime;
	}
}
