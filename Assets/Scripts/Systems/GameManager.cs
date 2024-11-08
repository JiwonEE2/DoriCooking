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
	[Tooltip("계산대 판매 속도")]
	public float foodSellDuration = 0.5f;
	public int foodCount = 0;
	public int foodCountLimit = 99;

	// 판매 자동화를 위함.
	// 강화 전에는 플레이어가 foodDistribute에 trigger 되어야 활성화된다.
	public bool isSellingFood = false;

	[Tooltip("플레이어의 초당 이동 칸 수")]
	public float playerMoveSpeed = 2;
	[Tooltip("플레이어가 최대 가질 수 있는 아이템 수")]
	public int playerGettableItemCount = 4;
	[Tooltip("플레이어의 빌런 대응 속도")]
	public float playerVillianInteractionSpeed = 3f;

	[Tooltip("cooker 음식 생성 속도")]
	public float[] cookDuration = { 4f, 4f };
	[Tooltip("cooker capacity")]
	public int[] cookerFoodLimit = { 6, 6 };

	[Tooltip("계산대 강화 여부")]
	public bool isCounterUp = false;

	[Tooltip("초당 음식 먹는 속도")]
	public float[] eatFoodSpeedPerSeceond = { 2f, 2f, 2f, 2f, 2f, 2f };

	public float villianTimer = 0;
	public bool needNewVillianTimerSetting = true;

	[Tooltip("손님의 소멸 신호")]
	public bool isCustomerDestoy = false;
	[Tooltip("소멸된 손님의 위치")]
	public Vector2 destroyedCustomerPosition;
	[Tooltip("소멸된 손님의 테이블 번호")]
	public int destroyedCustomerTableNum;

	public bool isCounterVillianSpawn = false;
	public bool[] isCookerVillianSpawn = { false, false };
	public bool isMoneyBoxVillianSpawn = false;
	public bool[] isTableVillianSpawn = { false, false, false, false, false, false };

	private void Update()
	{
		customerTimer += Time.deltaTime;
		foodSellTimer += Time.deltaTime;
		villianTimer += Time.deltaTime;
	}
}
