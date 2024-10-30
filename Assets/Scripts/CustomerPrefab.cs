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
		// �ð��� ������,������ �� ���� �־���, ī���Ϳ� ������ ���� ��
		if (GameManager.Instance.foodSellTimer >= GameManager.Instance.foodSellDuration && foodRequireNum > foodNum && GameManager.Instance.foodNum > 0)
		{
			// ������ �����ش�
			foodNum++;
			GameManager.Instance.foodNum--;
		}
		// ���ĵ� �� �����ְ�, ���̺� ������� ��
		if (foodNum == foodRequireNum && TableController.Instance.emptyTableDatas.Count > 0)
		{
			// �� �ְ�
			Instantiate(moneyPrefab, moneySpawnPoint, Quaternion.identity);
			GoEmptyTable();
		}
	}

	public void GoEmptyTable()
	{
		// ����ִ� ���̺� �����ϰ� ���ɵ� ���̺� �߰��ϱ�
		currentTable = TableController.Instance.emptyTableDatas[0];
		TableController.Instance.emptyTableDatas.RemoveAt(0);

		TableController.Instance.ocuppiedTableDatas.Add(currentTable);

		// ���� ������Ʈ ���̺�� ������
		gameObject.transform.position = currentTable.position;
		GameManager.Instance.isCustomerStanding = false;
		GameManager.Instance.customerTimer = 0;

		// �԰�

		// ġ��� ���̺� ����Ƽ�� �ű��
		//Destroy(gameObject);
	}
}
