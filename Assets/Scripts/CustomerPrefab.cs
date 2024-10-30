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
		if (GameManager.Instance.foodSellTimer >= GameManager.Instance.foodSellDuration && foodRequireNum > foodNum && GameManager.Instance.foodNum > 0)
		{
			// 음식을 나눠준다
			foodNum++;
			GameManager.Instance.foodNum--;
		}
		// 음식도 다 나눠주고, 테이블도 비어있을 때
		if (foodNum == foodRequireNum && TableController.Instance.emptyTableDatas.Count > 0)
		{
			// 돈 주고
			Instantiate(moneyPrefab, moneySpawnPoint, Quaternion.identity);
			GoEmptyTable();
		}
	}

	public void GoEmptyTable()
	{
		// 비어있는 테이블 삭제하고 점령된 테이블에 추가하기
		currentTable = TableController.Instance.emptyTableDatas[0];
		TableController.Instance.emptyTableDatas.RemoveAt(0);

		TableController.Instance.ocuppiedTableDatas.Add(currentTable);

		// 게임 오브젝트 테이블로 보내기
		gameObject.transform.position = currentTable.position;
		GameManager.Instance.isCustomerStanding = false;
		GameManager.Instance.customerTimer = 0;

		// 먹고

		// 치우고 테이블 임프티로 옮기고
		//Destroy(gameObject);
	}
}
