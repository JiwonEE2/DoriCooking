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
					GoEmptyTable();
					isGoTable = true;
					StartCoroutine(MovingCoroutine());
				}
			}
		}

		if (isGoTable && (Vector2)transform.position == currentTable.GetComponent<TablePrefab>().customerPos)
		{
			StopCoroutine(MovingCoroutine());
		}
	}

	public void GoEmptyTable()
	{
		// ����ִ� ���̺� �����ϰ� ���ɵ� ���̺� �߰��ϱ�
		currentTable = TableController.Instance.emptyTables[0];
		TableController.Instance.emptyTables.Remove(currentTable);

		// ���� ������Ʈ ���̺�� ������
		//gameObject.transform.position = currentTable.GetComponent<TablePrefab>().customerPos;
	}

	public void EatFood()
	{
		// foodNum�� ���� �԰� �������
		// �Դ� �ð��� 1�ʸ� ������ ��
		if (eatFoodTime >= 1)
		{
			foodNum -= GameManager.Instance.eatFoodSpeedPerSeceond[currentTable.GetComponent<TablePrefab>().tableNum];
			currentTable.GetComponent<TablePrefab>().trashCount++;
			eatFoodTime = 0;
		}
		else
		{
			eatFoodTime += Time.deltaTime;
		}
	}

	public IEnumerator MovingCoroutine()
	{
		while (isGoTable)
		{
			while ((Vector2)transform.position != currentTable.GetComponent<TablePrefab>().customerPos)
			{
				// �մ��� ��ã�� �˰��� �����ϱ� (��ֹ��� ���� ���� ��Ȳ���� ����ϱ⿡ ������ �����ƺ���)
				// �Ŀ� A* �� �˰��� �־ �� ��
				// 1. ��������� ������������ x,y������ ���� ���Ѵ�.
				float x = currentTable.GetComponent<TablePrefab>().customerPos.x - transform.position.x;
				float y = currentTable.GetComponent<TablePrefab>().customerPos.y - transform.position.y;

				// 2. ������ ������ ������ �� ū ������ ���ҽ�Ų��.
				if (Mathf.Abs(x) > Mathf.Abs(y))
				{
					if (x < 0)
					{
						transform.Translate(new Vector2(-1, 0));
						yield return new WaitForSeconds(1 / moveSpeed);
					}
					else
					{
						transform.Translate(new Vector2(1, 0));
						yield return new WaitForSeconds(1 / moveSpeed);
					}
				}
				else
				{
					if (y < 0)
					{
						transform.Translate(new Vector2(0, -1));
						yield return new WaitForSeconds(1 / moveSpeed);
					}
					else
					{
						transform.Translate(new Vector2(0, 1));
						yield return new WaitForSeconds(1 / moveSpeed);
					}
				}
			}
			// 3. �̶�, ��ֹ��� �ִٸ� �ٸ� ������ ���ҽ�Ų��.

			// 4. �� ������ �������� �����ư��� ������ ���ҽ�Ų��. 

			// 5. �̶����� ��ֹ��� �ִ� �� Ȯ���ϸ� ��ֹ��� �ִٸ� �ٸ� ������ ���ҽ�Ų��.

			// 6. �ٵ� �� ���� �� ������ ���ҽ�ų �� ���ٸ� �ش� ������ �������� �� �ֵ��� �ݴ� ��ֹ��� ���ҽ�Ų��.

			if ((Vector2)transform.position == currentTable.GetComponent<TablePrefab>().customerPos)
			{
				// ���̺� ������,
				// �� �Ծ����� �԰�
				if (foodNum >= 0)
				{
					EatFood();
				}
				// �ٸԾ����� ġ���.
				else
				{
					// ġ��� �� ���� ��
					TableController.Instance.trashedTables.Add(currentTable);
					Destroy(gameObject);
					yield return null;
				}
			}
		}
		yield return null;
	}
}
