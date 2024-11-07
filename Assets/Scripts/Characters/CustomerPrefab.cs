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

	public TablePrefab currentTable;

	public Vector2 moneySpawnPoint = new Vector2(1, -4);
	public int foodRequireNum;
	public int foodNum = 0;

	public float eatFoodTime = 0;

	// ������ �� ���� �־����� Ȯ���� ����.
	public bool isGetAllFood = false;
	public bool isGoingTable = false;
	// ���̺� �����Ͽ����� Ȯ��. ������ ������ �־ ������ �Ŀ��� ��������Ʈ ǥ�ø� ���� �ʱ� ����.
	private bool isGetTable = false;

	private SpriteRenderer gettenObjectSpriteRenderer;
	private Sprite foodSprite;

	private bool isContact = false;

	private bool isEatCoroutineStart = false;

	// Customer Controller���� �ش� customer�� �����ϱ� ���� ����
	public bool readyDestroy = false;

	private void Start()
	{
		gettenObjectSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
		foodSprite = SpriteManager.Instance.foodSprite;
		gettenObjectSpriteRenderer.sprite = foodSprite;
		gettenObjectSpriteRenderer.enabled = false;
		spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
		spriteRenderer.sortingOrder = 4;
		spriteRenderer.sprite = customerData.sprite;
		foodRequireNum = Random.Range(customerData.minFoodNum, customerData.maxFoodNum + 1);
		spriteRenderer.sortingOrder = 4;

		StartCoroutine(EatCoroutine());
	}

	private void Update()
	{
		// ���� �Ǹ�
		// ������ �� �ޱ� ������ �Ǹ��ϰ� ���Ŀ��� �� �ʿ� ����.
		// isGetAllFood�� ���⿡�� �ǵ帰��.
		FoodDistribute();

		// ���� ������ ���� ��������Ʈ ǥ���ϱ�
		// ��, ���̺� �����ϱ� ��������. ���̺� �����ϸ� ���̺� ���� ǥ���ϰ� �ȴ�.
		FoodRender();

		// �̰� ����? �Դ°� �����ΰ�
		if (isGoingTable && (Vector2)transform.position == currentTable.GetComponent<TablePrefab>().customerPos)
		{
			StopCoroutine(MovingCoroutine());
			if (isEatCoroutineStart == false)
			{
				isEatCoroutineStart = true;
			}
			if (foodNum <= 0)
			{
				CustomerDestroy();
			}
		}
	}

	private void FoodDistribute()
	{
		// ����, ������ �� �����־��� �� Ȯ��
		if (isGetAllFood == false)
		{
			// ���ķ��� �����ϴ� �� ���� Ȯ��
			if (foodNum >= foodRequireNum)
			{
				isGetAllFood = true;
			}
			// �Ǹ��� �� ���� ��. ������ ����� ��
			else if (GameManager.Instance.isSellingFood && GameManager.Instance.isCounterVillianSpawn == false)
			{
				// �ð��� ������, ī���Ϳ� ������ ���� ��
				if (GameManager.Instance.foodSellTimer >= GameManager.Instance.foodSellDuration && foodRequireNum > foodNum && GameManager.Instance.foodCount > 0)
				{
					// ������ �����ش�
					foodNum++;
					GameManager.Instance.foodCount--;
					GameManager.Instance.foodSellTimer = 0;
				}
			}
		}
		// ������ �� ���� �־��ٸ�
		else
		{
			// ���̺� ���� �ҾҰ�,
			if (false == isGoingTable)
			{
				// �غ�� ���̺��� ������, ���ְ�, ���̺� ����.
				if (TableController.Instance.readyTables.Count > 0)
				{
					GameManager.Instance.customerTimer = 0;
					GameManager.Instance.isCustomerStanding = false;
					for (int i = 0; i < foodRequireNum; i++)
					{
						Instantiate(moneyPrefab, moneySpawnPoint, Quaternion.identity);
					}
					GoEmptyTable();
					isGoingTable = true;
					StartCoroutine(MovingCoroutine());
				}
			}
		}
	}

	private void FoodRender()
	{
		if (foodNum > 0 && isGetTable == false)
		{
			gettenObjectSpriteRenderer.enabled = true;
		}
		else
		{
			gettenObjectSpriteRenderer.enabled = false;
		}
	}

	public void GoEmptyTable()
	{
		// ����ִ� ���̺� �����ϰ� ���ɵ� ���̺� �߰��ϱ�
		currentTable = TableController.Instance.readyTables[0];
		TableController.Instance.readyTables.Remove(currentTable);
	}

	public IEnumerator MovingCoroutine()
	{
		while (isGoingTable)
		{
			// ���̺� ���� ��
			while ((Vector2)transform.position != currentTable.GetComponent<TablePrefab>().customerPos)
			{
				// �մ��� ��ã�� �˰��� �����ϱ� (��ֹ��� ���� ���� ��Ȳ���� ����ϱ⿡ ������ �����ƺ���)
				// �Ŀ� A* �� �˰��� �־ �� ��
				// 1. ��������� ������������ x,y������ ���� ���Ѵ�.
				float x = currentTable.GetComponent<TablePrefab>().customerPos.x - transform.position.x;
				float y = currentTable.GetComponent<TablePrefab>().customerPos.y - transform.position.y;

				if (isContact == false)
				{
					// 2. ������ ������ ������ �� ū ������ ���ҽ�Ų��.
					if (Mathf.Abs(x) > Mathf.Abs(y))
					{
						if (x < 0)
						{
							// �ش� ��ġ���� �� �� ���� ���
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
				else
				{
					yield return null;
				}
			}
			// 3. �̶�, ��ֹ��� �ִٸ� �ٸ� ������ ���ҽ�Ų��.

			// 4. �� ������ �������� �����ư��� ������ ���ҽ�Ų��. 

			// 5. �̶����� ��ֹ��� �ִ� �� Ȯ���ϸ� ��ֹ��� �ִٸ� �ٸ� ������ ���ҽ�Ų��.

			// 6. �ٵ� �� ���� �� ������ ���ҽ�ų �� ���ٸ� �ش� ������ �������� �� �ֵ��� �ݴ� ��ֹ��� ���ҽ�Ų��.

			// ���̺� ������,
			isGetTable = true;
			// �� �Ծ����� �԰�
			// ���̺� ���� ���� ��������Ʈ ǥ��
			currentTable.GetComponent<TablePrefab>().objectSpriteRenderer.sprite = SpriteManager.Instance.foodSprite;
			yield return null;
		}
		yield return null;
	}

	public IEnumerator EatCoroutine()
	{
		while (true)
		{
			if (isEatCoroutineStart && foodNum > 0)
			{
				foodNum--;
				currentTable.GetComponent<TablePrefab>().trashCount++;
				yield return new WaitForSeconds(1f / GameManager.Instance.eatFoodSpeedPerSeceond[currentTable.GetComponent<TablePrefab>().tableNum]);
			}
			else
			{
				yield return null;
			}
		}
	}

	public void CustomerDestroy()
	{
		// �ٸԾ����� ������ �����ϰ� �մ� �������
		currentTable.GetComponent<TablePrefab>().objectSpriteRenderer.sprite = SpriteManager.Instance.trashSprite;
		TableController.Instance.trashedTables.Add(currentTable);
		GameManager.Instance.isCustomerDestoy = true;
		GameManager.Instance.destroyedCustomerPosition = transform.position;
		GameManager.Instance.destroyedCustomerTableNum = currentTable.GetComponent<TablePrefab>().tableNum;
		StopAllCoroutines();
		readyDestroy = true;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		isContact = true;
		StartCoroutine(StayCoroutine());
	}

	public IEnumerator StayCoroutine()
	{
		yield return new WaitForSeconds(1f);
		isContact = false;
	}
}
