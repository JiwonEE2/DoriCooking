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

	[Tooltip("���� ������")]
	public GameObject villianPrefab;
	private GameObject villian;

	[Tooltip("���� ���� �� ���� �ּ� �ð�")]
	public float minTime = 30;
	[Tooltip("���� ���� �� ���� �ִ� �ð�")]
	public float maxTime = 60;
	[SerializeField, Tooltip("���� ���� ���� �ð�")]
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
		// ������ �����Ǿ� ���ο� Ÿ�̸� ������ �ʿ��ϴٸ�
		if (GameManager.Instance.newVillianTimerSetting)
		{
			spawnTime = Random.Range(minTime, maxTime);
			GameManager.Instance.newVillianTimerSetting = false;
		}
	}

	public void SpawnVillian()
	{
		// �������, ��ȭ, ���� ������ ����
		if (GameManager.Instance.villianTimer > spawnTime && UIManager.Instance.isVillianSpawn == false)
		{
			villian = Instantiate(villianPrefab, transform);

			// �� �� �������� �ϳ� ��� �ش� ������ �������� �����ϱ�
			selectedVillian = (VILLIAN)Random.Range(0, 3);
			while (selectedVillian == VILLIAN.COUNTER && GameManager.Instance.isCustomerStanding)
			{
				selectedVillian = (VILLIAN)Random.Range(0, 3);
			}
			switch (selectedVillian)
			{
				case VILLIAN.COOKER:
					villian.transform.position = cookerVillianPosition;
					// �մ� ���ƹ�����!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
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
		// ��ħ ������ ����
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
