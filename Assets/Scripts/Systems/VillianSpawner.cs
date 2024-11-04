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

	public GameObject cookerVillianPrefab;
	public GameObject counterVillianPrefab;
	public GameObject moneyBoxVillianPrefab;
	public GameObject tableVillianPrefab;

	[Tooltip("빌런 삭제 후 생성 최소 시간")]
	public float minTime = 30;
	[Tooltip("빌런 삭제 후 생성 최대 시간")]
	public float maxTime = 60;
	[SerializeField, Tooltip("빌런 생성 예약 시간")]
	private float spawnTime = 0;

	private void Update()
	{
		SetTimer();
		SpawnVillian();
	}

	public void SetTimer()
	{
		// 빌런이 삭제되어 새로운 타이머 세팅이 필요하다면
		if (GameManager.Instance.needNewVillianTimerSetting)
		{
			print("타이머 세팅");
			spawnTime = Random.Range(minTime, maxTime);
			GameManager.Instance.needNewVillianTimerSetting = false;
		}
	}

	public void SpawnVillian()
	{
		// 무전취식, 전화, 절도 빌런의 생성
		if (GameManager.Instance.villianTimer > spawnTime && UIManager.Instance.isVillianSpawn == false)
		{
			print("빌런 생성");
			// 셋 중 랜덤으로 하나 골라서 해당 빌런의 포지션을 세팅하기
			selectedVillian = (VILLIAN)Random.Range(0, 3);
			while (selectedVillian == VILLIAN.COUNTER && GameManager.Instance.isCustomerStanding)
			{
				selectedVillian = (VILLIAN)Random.Range(0, 3);
			}
			switch (selectedVillian)
			{
				case VILLIAN.COOKER:
					Instantiate(cookerVillianPrefab, transform);
					break;
				case VILLIAN.COUNTER:
					Instantiate(counterVillianPrefab, transform);
					break;
				case VILLIAN.MONEYBOX:
					Instantiate(moneyBoxVillianPrefab, transform);
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
				Instantiate(tableVillianPrefab, transform);
				UIManager.Instance.isVillianSpawn = true;
			}
			else
			{
				GameManager.Instance.isCustomerDestoy = false;
			}
		}
	}
}
