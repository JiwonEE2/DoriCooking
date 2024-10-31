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
		// 시간도 지났고,음식을 덜 나눠 주었고, 카운터에 음식이 있을 때
		if (GameManager.Instance.foodSellTimer >= GameManager.Instance.foodSellDuration && foodRequireNum > foodNum && GameManager.Instance.foodCount > 0)
		{
			// 음식을 나눠준다
			foodNum++;
			GameManager.Instance.foodCount--;
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
			if (false == isOccupyTable)
			{
				// 테이블이 비어있으면, 돈주고,테이블에 간다.
				if (TableController.Instance.emptyTableDatas.Count > 0)
				{
					Instantiate(moneyPrefab, moneySpawnPoint, Quaternion.identity);
					isOccupyTable = true;
					GoEmptyTable();
				}
			}
			// 테이블에 갔으면,
			else
			{
				// 덜 먹었으면 먹고
				if (foodNum >= 0)
				{
					EatFood();
				}
				// 다먹었으면 치운다.
				else
				{
					// 치우는 것 구현 전
					TableController.Instance.emptyTableDatas.Add(currentTable);
					TableController.Instance.occupiedTableDatas.Remove(currentTable);
					Destroy(gameObject);
				}
			}
		}
	}

	public void GoEmptyTable()
	{
		// 비어있는 테이블 삭제하고 점령된 테이블에 추가하기
		currentTable = TableController.Instance.emptyTableDatas[0];
		TableController.Instance.emptyTableDatas.RemoveAt(0);
		TableController.Instance.occupiedTableDatas.Add(currentTable);

		// 게임 오브젝트 테이블로 보내기
		gameObject.transform.position = currentTable.position;
		GameManager.Instance.isCustomerStanding = false;
		GameManager.Instance.customerTimer = 0;
	}

	public void EatFood()
	{
		// foodNum에 따라 먹고 사라지기
		// 먹는 시간이 1초를 지났을 때
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
