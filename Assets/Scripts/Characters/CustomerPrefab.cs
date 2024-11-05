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
	public bool isGoingTable = false;
	private bool isGetTable = false;

	private SpriteRenderer gettenObjectSpriteRenderer;
	private Sprite foodSprite;

	private bool noEmptyTable = false;
	private bool isWaitForCleanCoroutineStart = false;
	private bool isContact = false;

	private bool isEatCoroutineStart = false;

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
		// 음식 가지면 음식 스프라이트 표시하기
		if (foodNum > 0 && isGetTable == false)
		{
			gettenObjectSpriteRenderer.enabled = true;
		}
		else
		{
			gettenObjectSpriteRenderer.enabled = false;
		}

		// 판매할 수 있을 때. 빌런도 없어야 함
		if (GameManager.Instance.isSellingFood && GameManager.Instance.isCounterVillianSpawn == false)
		{
			// 시간도 지났고,음식을 덜 나눠 주었고, 카운터에 음식이 있을 때
			if (GameManager.Instance.foodSellTimer >= GameManager.Instance.foodSellDuration && foodRequireNum > foodNum && GameManager.Instance.foodCount > 0 && isGetAllFood == false)
			{
				// 음식을 나눠준다
				foodNum++;
				GameManager.Instance.foodCount--;
				GameManager.Instance.foodSellTimer = 0;
			}
		}

		// 음식 다 받고, 
		if (foodNum == foodRequireNum && isGetAllFood == false)
		{
			isGetAllFood = true;
		}

		// 음식을 다 받았었다면,
		if (isGetAllFood)
		{
			// 테이블에 가지 았았고,
			if (false == isGoingTable)
			{
				// 테이블이 비어있으면, 돈주고,테이블에 간다.
				if (TableController.Instance.emptyTables.Count > 0)
				{
					if (noEmptyTable == true && isWaitForCleanCoroutineStart == false)
					{
						StartCoroutine(WaitForCleanCoroutine());
					}
					else if (noEmptyTable == false)
					{
						StopCoroutine(WaitForCleanCoroutine());
						GameManager.Instance.customerTimer = 0;
						GameManager.Instance.isCustomerStanding = false;
						Instantiate(moneyPrefab, moneySpawnPoint, Quaternion.identity);
						GoEmptyTable();
						isGoingTable = true;
						StartCoroutine(MovingCoroutine());
					}
				}
				// 테이블이 비어있지 않으면 대기 코루틴 시작
				else
				{
					noEmptyTable = true;
				}
			}
		}

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

	public void GoEmptyTable()
	{
		// 비어있는 테이블 삭제하고 점령된 테이블에 추가하기
		currentTable = TableController.Instance.emptyTables[0];
		TableController.Instance.emptyTables.Remove(currentTable);
	}

	public IEnumerator WaitForCleanCoroutine()
	{
		isWaitForCleanCoroutineStart = true;
		yield return new WaitForSeconds(1f);
		noEmptyTable = false;
	}

	public IEnumerator MovingCoroutine()
	{
		while (isGoingTable)
		{
			// 테이블에 가는 중
			while ((Vector2)transform.position != currentTable.GetComponent<TablePrefab>().customerPos)
			{
				// 손님의 길찾기 알고리즘 구현하기 (장애물이 많지 않은 상황에서 사용하기에 가볍고 괜찮아보임)
				// 후에 A* 등 알고리즘 넣어도 될 듯
				// 1. 출발지에서 목적지까지의 x,y증분을 각각 구한다.
				float x = currentTable.GetComponent<TablePrefab>().customerPos.x - transform.position.x;
				float y = currentTable.GetComponent<TablePrefab>().customerPos.y - transform.position.y;

				if (isContact == false)
				{
					// 2. 증분이 같아질 때까지 더 큰 증분을 감소시킨다.
					if (Mathf.Abs(x) > Mathf.Abs(y))
					{
						if (x < 0)
						{
							// 해당 위치에서 갈 수 없는 경우
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
			// 3. 이때, 장애물이 있다면 다른 증분을 감소시킨다.

			// 4. 두 증분이 같아지면 번갈아가며 증분을 감소시킨다. 

			// 5. 이때에도 장애물이 있는 지 확인하며 장애물이 있다면 다른 증분을 감소시킨다.

			// 6. 근데 두 방향 다 증분을 감소시킬 수 없다면 해당 방향을 빠져나올 수 있도록 반대 장애물을 감소시킨다.

			// 테이블에 갔으면,
			isGetTable = true;
			// 덜 먹었으면 먹고
			// 테이블 위에 음식 스프라이트 표시
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
		// 다먹었으면 쓰레기 생성하고 손님 사라지기
		currentTable.GetComponent<TablePrefab>().objectSpriteRenderer.sprite = SpriteManager.Instance.trashSprite;
		TableController.Instance.trashedTables.Add(currentTable);
		GameManager.Instance.isCustomerDestoy = true;
		GameManager.Instance.destroyedCustomerPosition = transform.position;
		GameManager.Instance.destroyedCustomerTableNum = currentTable.GetComponent<TablePrefab>().tableNum;
		StopAllCoroutines();
		Destroy(gameObject);
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
