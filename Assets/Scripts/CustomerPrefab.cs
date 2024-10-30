using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerPrefab : MonoBehaviour
{
	public CustomerDataSO customerData;
	public float moveSpeed = 2;

	private SpriteRenderer spriteRenderer;

	public bool isGetFood = false;

	public GameObject moneyPrefab;

	public TableDataSO currentTable;

	public Vector2 moneySpawnPoint = new Vector2(1, -4);
	// Start is called before the first frame update
	void Start()
	{
		spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
		spriteRenderer.sortingOrder = 1;
		spriteRenderer.sprite = customerData.sprite;
	}

	// Update is called once per frame
	void Update()
	{
		if (isGetFood && TableController.Instance.emptyTableDatas.Count > 0)
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
		isGetFood = false;
	}
}
