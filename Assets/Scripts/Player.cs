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

	private int x;
	private int y;

	public int gottenItemNum = 0;
	[Tooltip("���� ������ �ִ� ������ ����")]
	public ITEM currentGottenItem = ITEM.NONE;

	public Cooker cooker0;
	public Cooker cooker1;

	public Counter counter;

	[Tooltip("��� ���� �� ǥ�õ� ������(�������� ���Ե� ������ ��ü)")]
	public Sprite moneySprite;
	public Sprite foodSprite;
	public Sprite trashSprite;

	[SerializeField]
	private Sprite showingItemSprite;
	public GameObject gottenItemShowObject;

	[SerializeField]
	private SpriteRenderer gottenItemShowObjectSpriteRenderer;

	// ������ ó��
	public float trashcanTimer = 0;
	public bool isTrashcanZone = false;

	// Start is called before the first frame update
	void Start()
	{
		gottenItemShowObjectSpriteRenderer = gottenItemShowObject.AddComponent<SpriteRenderer>();
		gottenItemShowObjectSpriteRenderer.sortingOrder = 3;
		StartCoroutine(MovingCoroutine());
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKey(KeyCode.W))
		{
			y = 1;
		}
		else if (Input.GetKey(KeyCode.S))
		{
			y = -1;
		}
		else if (Input.GetKey(KeyCode.D))
		{
			x = 1;
		}
		else if (Input.GetKey(KeyCode.A))
		{
			x = -1;
		}

		// ������ ó��
		if (isTrashcanZone)
		{
			trashcanTimer += Time.deltaTime;
			if (trashcanTimer > 1)
			{
				gottenItemNum = 0;
			}
		}
		if (gottenItemNum == 0)
		{
			currentGottenItem = ITEM.NONE;
		}
		GottenItemShow();
	}

	public void GottenItemShow()
	{
		switch (currentGottenItem)
		{
			case ITEM.NONE:
				gottenItemShowObjectSpriteRenderer.sprite = null;
				return;
			case ITEM.MONEY:
				gottenItemShowObjectSpriteRenderer.sprite = moneySprite;
				break;
			case ITEM.FOOD:
				gottenItemShowObjectSpriteRenderer.sprite = foodSprite;
				break;
			case ITEM.TRASH:
				gottenItemShowObjectSpriteRenderer.sprite = trashSprite;
				break;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("MoneySaveZone"))
		{
			// ���� �� ���� ����
			if (currentGottenItem == ITEM.MONEY)
			{
				// ������ŭ �Ŵ����� �ְ�
				UIManager.Instance.money += gottenItemNum;
				// �� ���� 0����
				gottenItemNum = 0;
			}
		}
		else if (collision.CompareTag("EnforceZone"))
		{
			UIManager.Instance.enforcePopup.SetActive(true);
			Time.timeScale = 0;
		}
		else if (collision.CompareTag("TrashcanZone"))
		{
			trashcanTimer = 0;
			isTrashcanZone = true;
		}
		else if (collision.gameObject.name == "FoodDistributeZone")
		{
			// ���� �����ֱ�
			GameManager.Instance.isSellingFood = true;
			GameManager.Instance.foodSellTimer = 0;
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		// money�� �ھ��� ��
		if (collision.CompareTag("Money"))
		{
			// ������ ���̰ų� ���ٸ�
			if (currentGottenItem == ITEM.MONEY || currentGottenItem == ITEM.NONE)
			{
				// ���� �� �ִ� ��ŭ�� ������
				if (gottenItemNum < GameManager.Instance.playerGettableItemCount)
				{
					gottenItemNum++;
					Destroy(collision.gameObject);
					// ���� ���� �ӴϷ� �ٲٱ�
					currentGottenItem = ITEM.MONEY;
				}
			}
		}
		// �ش� ���� �θ𿡰Լ� ���� ���
		else if (collision.CompareTag("CookZone0"))
		{
			// ���� �� �����̰ų� ���� ��
			if (currentGottenItem == ITEM.FOOD || currentGottenItem == ITEM.NONE)
			{
				// ���� �� �ִ� ��ŭ, �丮�뿡 �ִ� ��ŭ ���� ���
				while (cooker0.foodCount > 0 && gottenItemNum < GameManager.Instance.playerGettableItemCount)
				{
					gottenItemNum++;
					cooker0.foodCount--;
					currentGottenItem = ITEM.FOOD;
				}
			}
		}
		else if (collision.CompareTag("CookZone1"))
		{
			if (currentGottenItem == ITEM.FOOD || currentGottenItem == ITEM.NONE)
			{
				// ���� �� �ִ� ��ŭ, �丮�뿡 �ִ� ��ŭ ���� ���
				while (cooker1.foodCount > 0 && gottenItemNum < GameManager.Instance.playerGettableItemCount)
				{
					gottenItemNum++;
					cooker1.foodCount--;
					currentGottenItem = ITEM.FOOD;
				}
			}
		}
		else if (collision.CompareTag("FoodSetZone"))
		{
			if (currentGottenItem == ITEM.FOOD)
			{
				// ���ӸŴ����� Ǫ�帮��, ���� �丮 Ȯ���ϰ� �ֱ�
				while (GameManager.Instance.foodCountLimit > GameManager.Instance.foodCount && gottenItemNum > 0)
				{
					// ���� �丮�� ���� foodsetzone�� �ֱ�
					gottenItemNum--;
					GameManager.Instance.foodCount++;
				}
			}
		}
		// ���̺� �ݸ����� �ھ��� ��
		else if (collision.GetComponent<TablePrefab>() != null)
		{
			if (currentGottenItem == ITEM.NONE || currentGottenItem == ITEM.TRASH)
			{
				while (collision.GetComponent<TablePrefab>().trashCount > 0 && gottenItemNum < GameManager.Instance.playerGettableItemCount)
				{
					gottenItemNum++;
					collision.GetComponent<TablePrefab>().trashCount--;
					currentGottenItem = ITEM.TRASH;
				}
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("TrashcanZone"))
		{
			isTrashcanZone = false;
		}
		else if (collision.gameObject.name == "FoodDistributeZone" && GameManager.Instance.isCounterUp == false)
		{
			GameManager.Instance.isSellingFood = false;
		}
	}

	public IEnumerator MovingCoroutine()
	{
		while (true)
		{
			transform.Translate(new Vector2(x, y));
			x = 0;
			y = 0;
			yield return new WaitForSeconds(1 / GameManager.Instance.playerMoveSpeed);
		}
	}
}
