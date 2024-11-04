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

	[Tooltip("���� ���� �� ���� �ּ� �ð�")]
	public float minTime = 30;
	[Tooltip("���� ���� �� ���� �ִ� �ð�")]
	public float maxTime = 60;
	[SerializeField, Tooltip("���� ���� ���� �ð�")]
	private float spawnTime = 0;

	private void Update()
	{
		SetTimer();
		SpawnVillian();
	}

	public void SetTimer()
	{
		// ������ �����Ǿ� ���ο� Ÿ�̸� ������ �ʿ��ϴٸ�
		if (GameManager.Instance.needNewVillianTimerSetting)
		{
			print("Ÿ�̸� ����");
			spawnTime = Random.Range(minTime, maxTime);
			GameManager.Instance.needNewVillianTimerSetting = false;
		}
	}

	public void SpawnVillian()
	{
		// �������, ��ȭ, ���� ������ ����
		if (GameManager.Instance.villianTimer > spawnTime && UIManager.Instance.isVillianSpawn == false)
		{
			print("���� ����");
			// �� �� �������� �ϳ� ��� �ش� ������ �������� �����ϱ�
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
		// ��ħ ������ ����
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
