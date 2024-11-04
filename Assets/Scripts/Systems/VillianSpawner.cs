using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class VillianSpawner : MonoBehaviour
{
	public enum VILLIAN
	{
		COOKER, COUNTER, MONEYBOX, TABLE
	}
	public VILLIAN selectedVillian;

	[Tooltip("빌런 프리팹")]
	public GameObject villianPrefab;
	private GameObject villian;

	[Tooltip("빌런 삭제 후 생성 최소 시간")]
	public float minTime = 30;
	[Tooltip("빌런 삭제 후 생성 최대 시간")]
	public float maxTime = 60;
	[SerializeField, Tooltip("빌런 생성 예약 시간")]
	private float spawnTime = 0;

	public Vector2 cookerVillianPosition;
	public Vector2 counterVillianPosition;
	public Vector2 tableVillianPosition;
	public Vector2 moneyBoxVillianPosition;

	private void Reset()
	{
		cookerVillianPosition = new Vector2(5, -2);
		counterVillianPosition = new Vector2(2, -5);
		moneyBoxVillianPosition = new Vector2(1, -2);
	}

	private void Update()
	{
		SetTimer();
		SpawnVillian();
	}

	public void SetTimer()
	{
		// 빌런이 삭제되어 새로운 타이머 세팅이 필요하다면
		if (GameManager.Instance.newVillianTimerSetting)
		{
			spawnTime = Random.Range(minTime, maxTime);
			GameManager.Instance.newVillianTimerSetting = false;
		}
	}

	public void SpawnVillian()
	{
		// 무전취식, 전화, 절도 빌런의 생성
		if (GameManager.Instance.villianTimer > spawnTime && UIManager.Instance.isVillianSpawn == false)
		{
			villian = Instantiate(villianPrefab, transform);

			// 셋 중 랜덤으로 하나 골라서 해당 빌런의 포지션을 세팅하기
			selectedVillian = (VILLIAN)Random.Range(0, 3);
			while (selectedVillian == VILLIAN.COUNTER && GameManager.Instance.isCustomerStanding)
			{
				selectedVillian = (VILLIAN)Random.Range(0, 3);
			}
			switch (selectedVillian)
			{
				case VILLIAN.COOKER:
					villian.transform.position = cookerVillianPosition;
					// 손님 막아버리기!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
					break;
				case VILLIAN.COUNTER:
					villian.transform.position = counterVillianPosition;
					break;
				case VILLIAN.MONEYBOX:
					villian.transform.position = moneyBoxVillianPosition;
					GameManager.Instance.isMoneyBoxVillianSpawn = true;
					break;
			}
			UIManager.Instance.isVillianSpawn = true;
		}
		// 취침 빌런의 생성
		if (GameManager.Instance.isCustomerDestoy)
		{
			if (UIManager.Instance.isVillianSpawn)
			{
				GameManager.Instance.isCustomerDestoy = false;
			}
			else if (Random.Range(0, 5) == 0)
			{
				villian = Instantiate(villianPrefab, transform);
				villian.transform.position = GameManager.Instance.destroyedCustomerPosition;
				UIManager.Instance.isVillianSpawn = true;
			}
			else
			{
				GameManager.Instance.isCustomerDestoy = false;
			}
		}
	}
}
