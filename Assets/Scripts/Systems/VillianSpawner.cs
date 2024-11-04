using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class VillianSpawner : MonoBehaviour
{
	public enum VILLIAN
	{
		COOKER, COUNTER, TABLE, MONEYBOX
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

	[Tooltip("���� ���� ����")]
	public bool isVillianSpawn = false;
	//[Tooltip("��ħ ���� ���� ����")]
	//public bool isTableVillianSpawn = false;
	[Tooltip("���� ���� ���� ����")]
	public bool isMoneyBoxVillianSpawn = false;

	public Vector2 cookerVillianPosition;
	public Vector2 counterVillianPosition;
	public Vector2 tableVillianPosition;
	public Vector2 moneyBoxVillianPosition;

	private float destroyTimer = 0;

	private void Reset()
	{
		cookerVillianPosition = new Vector2(5, 2);
		counterVillianPosition = new Vector2(2, -5);
		moneyBoxVillianPosition = new Vector2(1, -2);
	}

	private void Start()
	{
		spawnTime = Random.Range(minTime, maxTime);
	}
	private void Update()
	{
		SpawnVillian();
		DestroyVillian();
	}

	public void SpawnVillian()
	{
		// �������, ��ȭ, ���� ������ ����
		if (GameManager.Instance.villianTimer > spawnTime && isVillianSpawn == false)
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
					villian.transform.position = counterVillianPosition;
					// �մ� ���ƹ�����!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
					break;
				case VILLIAN.COUNTER:
					villian.transform.position = counterVillianPosition;
					break;
				case VILLIAN.MONEYBOX:
					villian.transform.position = moneyBoxVillianPosition;
					isMoneyBoxVillianSpawn = true;
					break;
			}
			isVillianSpawn = true;
		}
		// ��ħ ������ ����
		if (GameManager.Instance.isCustomerDestoy)
		{
			if (isVillianSpawn)
			{
				GameManager.Instance.isCustomerDestoy = false;
			}
			else if (Random.Range(0, 5) == 0)
			{
				villian = Instantiate(villianPrefab, transform);
				villian.transform.position = GameManager.Instance.destroyedCustomerPosition;
				isVillianSpawn = true;
			}
			else
			{
				GameManager.Instance.isCustomerDestoy = false;
			}
		}
	}

	public void DestroyVillian()
	{
		// ���� ������ ���
		if (isMoneyBoxVillianSpawn)
		{
			// ���� �÷��̾ ���� �ð� �̻� ��ȣ�ۿ� �Ѵٸ�
			if (GameManager.Instance.playerVillianInteractionSpeed / 2f < destroyTimer)
			{
				Destroy(villian);
				isMoneyBoxVillianSpawn = false;
				isVillianSpawn = false;
				GameManager.Instance.villianTimer = 0;
			}
		}
		else if (isVillianSpawn)
		{
			if (GameManager.Instance.playerVillianInteractionSpeed < destroyTimer)
			{
				Destroy(villian);
				isVillianSpawn = false;
				GameManager.Instance.villianTimer = 0;
			}
		}
	}
}
