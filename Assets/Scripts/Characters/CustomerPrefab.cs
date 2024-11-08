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

	// 음식을 다 나눠 주었는지 확인을 위함.
	public bool isGetAllFood = false;
	public bool isGoingTable = false;
	// 테이블에 도착하였는지 확인. 음식을 가지고 있어도 도착한 후에는 스프라이트 표시를 하지 않기 위함.
	private bool isGetTable = false;

	private bool isEatCoroutineStart = false;

	// Customer Controller에서 해당 customer를 삭제하기 위한 변수
	public bool readyDestroy = false;

	// 충돌 판정을 위함. 바로 다시 false로 돌아가기 때문에 초기화할 필요 없다.
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

		StartCoroutine(EatCoroutine());
	}

	private void Update()
	{
		// 1. 밥 받기
		if (isGetAllFood == false)
		{
			FoodDistribute();
		}
		// 2. 빈 테이블 찾고, 3. 돈 내고, 4. 빈 테이블로 이동
		else if (isGoingTable == false && TableController.Instance.readyTables.Count > 0)
		{
			GoEmptyTable();
		}

		// 음식 가지면 음식 스프라이트 표시하기
		// 단, 테이블에 도착하기 전까지만. 테이블에 도착하면 테이블 위에 표시하게 된다.
		FoodRender();

		// 출발했고, 목적지와 현재 위치가 같으면 세팅 후 식사 시작
		if (isGoingTable && (Vector2)transform.position == currentTable.GetComponent<TablePrefab>().customerPos)
		{
			StopCoroutine(MovingCoroutine());
			StartEat();

			if (foodNum <= 0)
			{
				CustomerDestroy();
			}
		}
	}

	private void FoodDistribute()
	{
		// 음식량이 충족하는 지 먼저 확인
		if (foodNum >= foodRequireNum)
		{
			isGetAllFood = true;
		}
		// 판매할 수 있을 때. 빌런도 없어야 함
		else if (GameManager.Instance.isSellingFood && GameManager.Instance.isCounterVillianSpawn == false)
		{
			// 시간도 지났고, 카운터에 음식이 있을 때
			if (GameManager.Instance.foodSellTimer >= GameManager.Instance.foodSellDuration && foodRequireNum > foodNum && GameManager.Instance.foodCount > 0)
			{
				// 음식을 나눠준다
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
		// 덜 먹었으면 먹고
		// 테이블 위에 음식 스프라이트 표시
		currentTable.GetComponent<TablePrefab>().objectSpriteRenderer.sprite = SpriteManager.Instance.foodSprite;
		isEatCoroutineStart = true;
	}

	public void GoEmptyTable()
	{
		GameManager.Instance.customerTimer = 0;
		GameManager.Instance.isCustomerStanding = false;
		for (int i = 0; i < foodRequireNum; i++)
		{
			Instantiate(moneyPrefab, moneySpawnPoint, Quaternion.identity);
		}

		isGoingTable = true;
		// 준비된 테이블에서 삭제하고 현재 이 손님의 테이블로 설정
		currentTable = TableController.Instance.readyTables[0];
		TableController.Instance.readyTables.Remove(currentTable);
		StartCoroutine(MovingCoroutine());
	}

	public IEnumerator MovingCoroutine()
	{
		// 출발
		while (isGoingTable)
		{
			// 테이블에 가는 중
			while ((Vector2)transform.position != currentTable.GetComponent<TablePrefab>().customerPos)
			{
				// 1. 출발지에서 목적지까지의 x,y증분을 각각 구한다.
				float x = currentTable.GetComponent<TablePrefab>().customerPos.x - transform.position.x;
				float y = currentTable.GetComponent<TablePrefab>().customerPos.y - transform.position.y;

				// 충돌하지 않았을 경우
				if (isContact == false)
				{
					// 2. 증분이 같아질 때까지 더 큰 증분을 감소시킨다.
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
				// 충돌하였을 경우
				else
				{
					yield return new WaitForSeconds(1f);
					isContact = false;
				}
			}

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
		// 다먹었으면 쓰레기 생성하고 손님 사라지기
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
	}
}
