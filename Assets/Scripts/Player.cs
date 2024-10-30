using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Player : MonoBehaviour
{
	public enum ITEM
	{
		MONEY, FOOD, TRASH, NONE
	}

	public float moveSpeed = 2;

	[Tooltip("�ִ� ���� �� �ִ� ������ ��")]
	public int gettableItemNum = 4;
	public int gettenItemNum = 0;
	[Tooltip("���� ������ �ִ� ������ ����")]
	public ITEM currentGettenItem = ITEM.NONE;

	public Cooker cooker0;
	public Cooker cooker1;

	public Counter counter;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		float x = Input.GetAxis("Horizontal");
		float y = Input.GetAxis("Vertical");
		transform.Translate(new Vector3(x, y) * Time.deltaTime * moveSpeed);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("MoneySaveZone"))
		{
			// ���� �� ���� ����
			if (currentGettenItem == ITEM.MONEY)
			{
				// ������ŭ �Ŵ����� �ְ�
				UIManager.Instance.money += gettenItemNum;
				// �� ���� 0����
				gettenItemNum = 0;
				// ���� �����۵� �������� ����
				currentGettenItem = ITEM.NONE;
			}
		}
		else if (collision.CompareTag("EnforceZone"))
		{
			UIManager.Instance.enforcePopup.SetActive(true);
			Time.timeScale = 0;
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		// money�� �ھ��� ��
		if (collision.CompareTag("Money"))
		{
			// ������ ���̰ų� ���ٸ�
			if (currentGettenItem == ITEM.MONEY || currentGettenItem == ITEM.NONE)
			{
				// ���� �� �ִ� ��ŭ�� ������
				if (gettenItemNum < gettableItemNum)
				{
					gettenItemNum++;
					Destroy(collision.gameObject);
					// ���� ���� �ӴϷ� �ٲٱ�
					currentGettenItem = ITEM.MONEY;
				}
			}
		}
		// �ش� ���� �θ𿡰Լ� ���� ���
		else if (collision.CompareTag("CookZone0"))
		{
			// ���� �� �����̰ų� ���� ��
			if (currentGettenItem == ITEM.FOOD || currentGettenItem == ITEM.NONE)
			{
				// ���� �� �ִ� ��ŭ, �丮�뿡 �ִ� ��ŭ ���� ���
				while (cooker0.foodCount > 0 && gettenItemNum < gettableItemNum)
				{
					gettenItemNum++;
					cooker0.foodCount--;
					currentGettenItem = ITEM.FOOD;
				}
			}
		}
		else if (collision.CompareTag("CookZone1"))
		{
			if (currentGettenItem == ITEM.FOOD || currentGettenItem == ITEM.NONE)
			{
				// ���� �� �ִ� ��ŭ, �丮�뿡 �ִ� ��ŭ ���� ���
				while (cooker1.foodCount > 0 && gettenItemNum < gettableItemNum)
				{
					gettenItemNum++;
					cooker1.foodCount--;
					currentGettenItem = ITEM.FOOD;
				}
			}
		}
		else if (collision.CompareTag("FoodSetZone"))
		{
			if (currentGettenItem == ITEM.FOOD)
			{
				// ���ӸŴ����� Ǫ�帮�� Ȯ���ϰ� �ֱ�
				// ���� �丮�� ���� foodsetzone�� �ֱ�
				GameManager.Instance.foodCount += gettenItemNum;
				gettenItemNum = 0;
				currentGettenItem = ITEM.NONE;
			}
		}
	}
}
