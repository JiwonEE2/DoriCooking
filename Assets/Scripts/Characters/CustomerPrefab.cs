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

	private SpriteRenderer gettenObjectSpriteRenderer;
	private Sprite foodSprite;

	// ������ �� ���� �־����� Ȯ���� ����.
	public bool isFoodNeed = true;
	public bool isGoingTable = false;
	// ���̺� �����Ͽ����� Ȯ��. ������ ������ �־ ������ �Ŀ��� ��������Ʈ ǥ�ø� ���� �ʱ� ����.
	private bool isGetTable = false;

	private bool isEatCoroutineStart = false;

	private bool isEatEnd = false;

	// Customer Controller���� �ش� customer�� �����ϱ� ���� ����
	public bool readyDestroy = false;
	// �浹 ������ ����. �ٷ� �ٽ� false�� ���ư��� ������ �ʱ�ȭ�� �ʿ� ����.
	private bool isContact = false;

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

		isFoodNeed = true;
	}

	private void Update()
	{
		// 1. �� �ޱ�
		if (isFoodNeed)
		{
			FoodDistribute();
		}
		// 2. �غ�� ���̺� ã��
		else
		{
			if (isGoingTable == false && TableController.Instance.readyTables.Count > 0)
			{
				// 3. �� ����, 4. �غ�� ���̺�� �̵�
				PayMoney();
				GoTable();
			}
		}

		// ���� ������ ���� ��������Ʈ ǥ���ϱ�
		// ��, ���̺� �����ϱ� ��������. ���̺� �����ϸ� ���̺� ���� ǥ���ϰ� �ȴ�.
		FoodRender();

		// ����߰�, �������� ���� ��ġ�� ������ ���� �� �Ļ� ����
		if (isGoingTable && (Vector2)transform.position == currentTable.GetComponent<TablePrefab>().customerPos)
		{
			StopCoroutine(MovingCoroutine());

			// 5. �Ļ�
			StartEat();
			if (isEatCoroutineStart == false)
			{
				isEatCoroutineStart = true;
				StartCoroutine(EatCoroutine());
			}

			// 6. ����
			if (foodNum <= 0)
			{
				isEatEnd = true;
			}
		}

		// �� ���� �����ϸ� �ȴ�.
		if (isEatEnd)
		{
			isEatEnd = false;
			StopAllCoroutines();
			CustomerDestroy();
		}
	}

	private void FoodDistribute()
	{
		// ���ķ��� �����ϴ� �� ���� Ȯ��
		if (foodNum >= foodRequireNum)
		{
			isFoodNeed = false;
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

	private void StartEat()
	{
		isGetTable = true;
		// �� �Ծ����� �԰�
		// ���̺� ���� ���� ��������Ʈ ǥ��
		currentTable.GetComponent<TablePrefab>().objectSpriteRenderer.sprite = SpriteManager.Instance.foodSprite;
	}

	public void PayMoney()
	{
		GameManager.Instance.customerTimer = 0;
		GameManager.Instance.isCustomerStanding = false;
		for (int i = 0; i < foodRequireNum; i++)
		{
			Instantiate(moneyPrefab, moneySpawnPoint, Quaternion.identity);
		}
	}

	private void GoTable()
	{
		// �غ�� ���̺��� �����ϰ� ���� �� �մ��� ���̺�� ����
		currentTable = TableController.Instance.readyTables[0];
		TableController.Instance.readyTables.Remove(currentTable);

		isGoingTable = true;
		isContact = false;
		StartCoroutine(MovingCoroutine());
	}

	public IEnumerator MovingCoroutine()
	{
		// ���
		while ((Vector2)transform.position != currentTable.GetComponent<TablePrefab>().customerPos)
		{
			// 1. ��������� ������������ x,y������ ���� ���Ѵ�.
			float x = currentTable.GetComponent<TablePrefab>().customerPos.x - transform.position.x;
			float y = currentTable.GetComponent<TablePrefab>().customerPos.y - transform.position.y;

			// �浹�Ͽ��� ���
			if (isContact)
			{
				yield return new WaitForSeconds(1f);
				isContact = false;
			}
			// �浹���� �ʾ��� ���
			else
			{
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
		}
		yield return null;
	}

	public IEnumerator EatCoroutine()
	{
		while (foodNum > 0)
		{
			foodNum--;
			currentTable.GetComponent<TablePrefab>().trashCount++;
			yield return new WaitForSeconds(1f / GameManager.Instance.eatFoodSpeedPerSeceond[currentTable.GetComponent<TablePrefab>().tableNum]);
		}
		yield return null;
	}

	public void CustomerDestroy()
	{
		// �ٸԾ����� ������ �����ϰ� �մ� �������
		currentTable.GetComponent<TablePrefab>().objectSpriteRenderer.sprite = SpriteManager.Instance.trashSprite;
		TableController.Instance.trashedTables.Add(currentTable);
		GameManager.Instance.isCustomerDestoy = true;
		GameManager.Instance.destroyedCustomerPosition = transform.position;
		GameManager.Instance.destroyedCustomerTableNum = currentTable.GetComponent<TablePrefab>().tableNum;
		readyDestroy = true;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		isContact = true;
	}
}
