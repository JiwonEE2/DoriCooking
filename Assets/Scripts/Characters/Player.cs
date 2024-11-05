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

	// �̵� ������ ���� ����
	private float horizontal;
	private float vertical;
	public bool isHorizontalMove = false;
	public bool hDown = false;
	public bool vDown = false;
	public bool hUp = false;
	public bool vUp = false;

	public int gottenItemNum = 0;
	[Tooltip("���� ������ �ִ� ������ ����")]
	public ITEM currentGottenItem = ITEM.NONE;

	public Cooker cooker0;
	public Cooker cooker1;

	public Counter counter;

	private Sprite moneySprite;
	private Sprite foodSprite;
	private Sprite trashSprite;

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
		moneySprite = SpriteManager.Instance.moneySprite;
		foodSprite = SpriteManager.Instance.foodSprite;
		trashSprite = SpriteManager.Instance.trashSprite;
		gottenItemShowObjectSpriteRenderer = gottenItemShowObject.AddComponent<SpriteRenderer>();
		gottenItemShowObjectSpriteRenderer.sortingOrder = 5;
	}

	// Update is called once per frame
	void Update()
	{
		// �̵� �Լ�
		Moving();
		// ������ ó��
		TrashThrow();
		// ���� ������ �����ֱ�
		GottenItemShow();
	}

	public void TrashThrow()
	{
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
		if (collision.CompareTag("MoneySaveZone") && GameManager.Instance.isMoneyBoxVillianSpawn == false)
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
			if (GameManager.Instance.isMoneyBoxVillianSpawn == false)
			{
				UIManager.Instance.enforcePopup.SetActive(true);
				Time.timeScale = 0;
			}
		}
		else if (collision.CompareTag("TrashcanZone"))
		{
			trashcanTimer = 0;
			isTrashcanZone = true;
		}
		// ī���� ������ ���� ���� ����
		else if (collision.gameObject.name == "FoodDistributeZone" && GameManager.Instance.isCounterVillianSpawn == false)
		{
			// ���� �����ֱ�
			GameManager.Instance.isSellingFood = true;
			GameManager.Instance.foodSellTimer = 0;
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		// money�� �ھ��� ��
		if (collision.CompareTag("Money") && GameManager.Instance.isCounterVillianSpawn == false)
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
			// ������ ���� ��
			if (GameManager.Instance.isCookerVillianSpawn[0] == false)
			{
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
		}
		else if (collision.CompareTag("CookZone1"))
		{
			if (GameManager.Instance.isCookerVillianSpawn[1] == false)
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
		}
		else if (collision.CompareTag("FoodSetZone"))
		{
			if (currentGottenItem == ITEM.FOOD)
			{
				// ���� ������ ���� ���� ����
				if (GameManager.Instance.isCounterVillianSpawn == false)
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
		}
		// ���̺� �ݸ����� �ھ��� ��
		else if (collision.GetComponent<TablePrefab>() != null && collision.GetComponent<TablePrefab>().isTableVillianSpawn == false)
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

	public void Moving()
	{
		// �̵� ���� �ٽ�
		horizontal = Input.GetAxis("Horizontal");
		vertical = Input.GetAxis("Vertical");

		hDown = Input.GetButtonDown("Horizontal");
		vDown = Input.GetButtonDown("Vertical");
		hUp = Input.GetButtonUp("Horizontal");
		vUp = Input.GetButtonUp("Vertical");

		if (hDown || vUp)
		{
			isHorizontalMove = true;
		}
		else if (vDown || hUp)
		{
			isHorizontalMove = false;
		}
		else if (hUp || vUp)
		{
			isHorizontalMove = horizontal != 0;
		}
		if (isHorizontalMove)
		{
			transform.Translate(new Vector2(horizontal, 0) * Time.deltaTime * GameManager.Instance.playerMoveSpeed);
		}
		else if (!isHorizontalMove)
		{
			transform.Translate(new Vector2(0, vertical) * Time.deltaTime * GameManager.Instance.playerMoveSpeed);
		}
	}
}
