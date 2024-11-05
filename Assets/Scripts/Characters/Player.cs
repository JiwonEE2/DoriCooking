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

	// 이동 구현을 위한 변수
	private float horizontal;
	private float vertical;
	public bool isHorizontalMove = false;
	public bool hDown = false;
	public bool vDown = false;
	public bool hUp = false;
	public bool vUp = false;

	public int gottenItemNum = 0;
	[Tooltip("현재 가지고 있는 아이템 종류")]
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

	// 쓰레기 처리
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
		// 이동 함수
		Moving();
		// 쓰레기 처리
		TrashThrow();
		// 가진 아이템 보여주기
		GottenItemShow();
	}

	public void TrashThrow()
	{
		// 쓰레기 처리
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
			// 가진 게 돈일 때만
			if (currentGottenItem == ITEM.MONEY)
			{
				// 가진만큼 매니저에 넣고
				UIManager.Instance.money += gottenItemNum;
				// 내 돈은 0으로
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
		// 카운터 빌런이 없을 때만 가능
		else if (collision.gameObject.name == "FoodDistributeZone" && GameManager.Instance.isCounterVillianSpawn == false)
		{
			// 음식 나눠주기
			GameManager.Instance.isSellingFood = true;
			GameManager.Instance.foodSellTimer = 0;
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		// money랑 박았을 때
		if (collision.CompareTag("Money") && GameManager.Instance.isCounterVillianSpawn == false)
		{
			// 가진게 돈이거나 없다면
			if (currentGottenItem == ITEM.MONEY || currentGottenItem == ITEM.NONE)
			{
				// 가질 수 있는 만큼만 가지고
				if (gottenItemNum < GameManager.Instance.playerGettableItemCount)
				{
					gottenItemNum++;
					Destroy(collision.gameObject);
					// 가진 것을 머니로 바꾸기
					currentGottenItem = ITEM.MONEY;
				}
			}
		}
		// 해당 존의 부모에게서 음식 얻기
		else if (collision.CompareTag("CookZone0"))
		{
			// 가진 게 음식이거나 없을 때
			// 빌런도 없을 때
			if (GameManager.Instance.isCookerVillianSpawn[0] == false)
			{
				if (currentGottenItem == ITEM.FOOD || currentGottenItem == ITEM.NONE)
				{
					// 가질 수 있는 만큼, 요리대에 있는 만큼 음식 얻기
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
					// 가질 수 있는 만큼, 요리대에 있는 만큼 음식 얻기
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
				// 계산대 빌런이 없을 때만 가능
				if (GameManager.Instance.isCounterVillianSpawn == false)
				{
					// 게임매니저의 푸드리밋, 가진 요리 확인하고 넣기
					while (GameManager.Instance.foodCountLimit > GameManager.Instance.foodCount && gottenItemNum > 0)
					{
						// 가진 요리를 전부 foodsetzone에 넣기
						gottenItemNum--;
						GameManager.Instance.foodCount++;
					}
				}
			}
		}
		// 테이블 콜리전과 박았을 때
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
		// 이동 구현 다시
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
