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
		// 근데 플레이어가 트리거에 들어왔을 때!
		if (GameManager.Instance.isSellingFood)
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
			if (false == isGoTable)
			{
				// 테이블이 비어있으면, 돈주고,테이블에 간다.
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
		// 비어있는 테이블 삭제하고 점령된 테이블에 추가하기
		currentTable = TableController.Instance.emptyTables[0];
		TableController.Instance.emptyTables.Remove(currentTable);

		// 게임 오브젝트 테이블로 보내기
		//gameObject.transform.position = currentTable.GetComponent<TablePrefab>().customerPos;
	}

	public void EatFood()
	{
		// foodNum에 따라 먹고 사라지기
		// 먹는 시간이 1초를 지났을 때
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
				// 손님의 길찾기 알고리즘 구현하기 (장애물이 많지 않은 상황에서 사용하기에 가볍고 괜찮아보임)
				// 후에 A* 등 알고리즘 넣어도 될 듯
				// 1. 출발지에서 목적지까지의 x,y증분을 각각 구한다.
				float x = currentTable.GetComponent<TablePrefab>().customerPos.x - transform.position.x;
				float y = currentTable.GetComponent<TablePrefab>().customerPos.y - transform.position.y;

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
			// 3. 이때, 장애물이 있다면 다른 증분을 감소시킨다.

			// 4. 두 증분이 같아지면 번갈아가며 증분을 감소시킨다. 

			// 5. 이때에도 장애물이 있는 지 확인하며 장애물이 있다면 다른 증분을 감소시킨다.

			// 6. 근데 두 방향 다 증분을 감소시킬 수 없다면 해당 방향을 빠져나올 수 있도록 반대 장애물을 감소시킨다.

			if ((Vector2)transform.position == currentTable.GetComponent<TablePrefab>().customerPos)
			{
				// 테이블에 갔으면,
				// 덜 먹었으면 먹고
				if (foodNum >= 0)
				{
					EatFood();
				}
				// 다먹었으면 치운다.
				else
				{
					// 치우는 것 구현 전
					TableController.Instance.trashedTables.Add(currentTable);
					Destroy(gameObject);
					yield return null;
				}
			}
		}
		yield return null;
	}
}
