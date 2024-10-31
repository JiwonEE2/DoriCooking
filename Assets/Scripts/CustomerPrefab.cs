using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CustomerPrefab : MonoBehaviour
{
	public CustomerDataSO customerData;
	public float moveSpeed = 2;

	private SpriteRenderer spriteRenderer;

	public GameObject moneyPrefab;

	public TableDataSO currentTable;

	public Vector2 moneySpawnPoint = new Vector2(1, -4);
	public int foodRequireNum;
	public int foodNum = 0;

	public float eatFoodTime = 0;

	public bool isGetAllFood = false;
	public bool isOccupyTable = false;

	// Start is called before the first frame update
	void Start()
	{
		spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
		spriteRenderer.sortingOrder = 1;
		spriteRenderer.sprite = customerData.sprite;
		foodRequireNum = Random.Range(customerData.minFoodNum, customerData.maxFoodNum + 1);
	}

	// Update is called once per frame
	void Update()
	{
		// �ð��� ������,������ �� ���� �־���, ī���Ϳ� ������ ���� ��
		if (GameManager.Instance.foodSellTimer >= GameManager.Instance.foodSellDuration && foodRequireNum > foodNum && GameManager.Instance.foodCount > 0)
		{
			// ������ �����ش�
			foodNum++;
			GameManager.Instance.foodCount--;
		}

		// ���� �� �ް�, 
		if (foodNum == foodRequireNum && isGetAllFood == false)
		{
			isGetAllFood = true;
		}

		// ������ �� �޾Ҿ��ٸ�,
		if (isGetAllFood)
		{
			// ���̺� ���� �ҾҰ�,
			if (false == isOccupyTable)
			{
				// ���̺��� ���������, ���ְ�,���̺� ����.
				if (TableController.Instance.emptyTableDatas.Count > 0)
				{
					Instantiate(moneyPrefab, moneySpawnPoint, Quaternion.identity);
					isOccupyTable = true;
					GoEmptyTable();
				}
			}
			// ���̺� ������,
			else
			{
				// �� �Ծ����� �԰�
				if (foodNum >= 0)
				{
					EatFood();
				}
				// �ٸԾ����� ġ���.
				else
				{
					// ġ��� �� ���� ��
					TableController.Instance.emptyTableDatas.Add(currentTable);
					TableController.Instance.occupiedTableDatas.Remove(currentTable);
					Destroy(gameObject);
				}
			}
		}
	}

	public void GoEmptyTable()
	{
		// ����ִ� ���̺� �����ϰ� ���ɵ� ���̺� �߰��ϱ�
		currentTable = TableController.Instance.emptyTableDatas[0];
		TableController.Instance.emptyTableDatas.RemoveAt(0);
		TableController.Instance.occupiedTableDatas.Add(currentTable);

		// ���� ������Ʈ ���̺�� ������
		gameObject.transform.position = currentTable.position;
		GameManager.Instance.isCustomerStanding = false;
		GameManager.Instance.customerTimer = 0;
	}

	public void EatFood()
	{
		// foodNum�� ���� �԰� �������
		// �Դ� �ð��� 1�ʸ� ������ ��
		if (eatFoodTime >= 1)
		{
			foodNum -= GameManager.Instance.eatFoodSpeedPerSeceond;
			eatFoodTime = 0;
		}
		else
		{
			eatFoodTime += Time.deltaTime;
		}
	}
}
