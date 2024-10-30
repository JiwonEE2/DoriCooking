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
			// �� �ְ�
			Instantiate(moneyPrefab, new Vector2(-6, 2), Quaternion.identity);
			GoEmptyTable();
		}
	}

	public void GoEmptyTable()
	{
		// ����ִ� ���̺� �����ϰ� ���ɵ� ���̺� �߰��ϱ�
		TableController.Instance.ocuppiedTableDatas.Add(TableController.Instance.emptyTableDatas[0]);
		TableController.Instance.emptyTableDatas.RemoveAt(0);

		// ���� ������Ʈ ���̺�� ������
		gameObject.transform.position = TableController.Instance.ocuppiedTableDatas[TableController.Instance.ocuppiedTableDatas.Count - 1].position;
		GameManager.Instance.isCustomerStanding = false;

		// �԰�

		// ġ��� ���̺� ����Ƽ�� �ű��
		isGetFood = false;
	}
}
