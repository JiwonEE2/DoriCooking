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

	public GameObject currentTable;

	public Vector2 moneySpawnPoint = new Vector2(1, -4);
	public int foodRequireNum;
	public int foodNum = 0;

	public float eatFoodTime = 0;

	public bool isGetAllFood = false;
	public bool isGoTable = false;

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
		// �ٵ� �÷��̾ Ʈ���ſ� ������ ��!
		if (GameManager.Instance.isSellingFood)
		{
			// �ð��� ������,������ �� ���� �־���, ī���Ϳ� ������ ���� ��
			if (GameManager.Instance.foodSellTimer >= GameManager.Instance.foodSellDuration && foodRequireNum > foodNum && GameManager.Instance.foodCount > 0 && isGetAllFood == false)
			{
				// ������ �����ش�
				foodNum++;
				GameManager.Instance.foodCount--;
				GameManager.Instance.foodSellTimer = 0;
			}
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
			if (false == isGoTable)
			{
				// ���̺��� ���������, ���ְ�,���̺� ����.
				if (TableController.Instance.emptyTables.Count > 0)
				{
					GameManager.Instance.customerTimer = 0;
					GameManager.Instance.isCustomerStanding = false;
					Instantiate(moneyPrefab, moneySpawnPoint, Quaternion.identity);
					isGoTable = true;
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
					//TableController.Instance.emptyTableDatas.Add(currentTable);
					//TableController.Instance.occupiedTableDatas.Remove(currentTable);
					Destroy(gameObject);
				}
			}
		}
	}

	public void GoEmptyTable()
	{
		// ����ִ� ���̺� �����ϰ� ���ɵ� ���̺� �߰��ϱ�
		currentTable = TableController.Instance.emptyTables[0];

		// ���� ������Ʈ ���̺�� ������
		gameObject.transform.position = currentTable.GetComponent<TablePrefab>().customerPos;
	}

	public void EatFood()
	{
		// foodNum�� ���� �԰� �������
		// �Դ� �ð��� 1�ʸ� ������ ��
		if (eatFoodTime >= 1)
		{
			foodNum -= GameManager.Instance.eatFoodSpeedPerSeceond;
			currentTable.GetComponent<TablePrefab>().trashCount++;
			eatFoodTime = 0;
		}
		else
		{
			eatFoodTime += Time.deltaTime;
		}
	}
}
