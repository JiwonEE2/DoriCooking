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

	[Tooltip("최대 가질 수 있는 아이템 수")]
	public int gettableItemNum = 4;
	public int gottenItemNum = 0;
	[Tooltip("현재 가지고 있는 아이템 종류")]
	public ITEM currentGottenItem = ITEM.NONE;

	public Cooker cooker0;
	public Cooker cooker1;

	public Counter counter;

	[Tooltip("들고 있을 때 표시될 아이템(렌더러가 포함된 아이템 객체)")]
	public Sprite moneySprite;
	public Sprite foodSprite;
	public Sprite trashSprite;

	[SerializeField]
	private Sprite showingItemSprite;
	public GameObject gottenItemShowObject;

	[SerializeField]
	private SpriteRenderer gottenItemShowObjectSpriteRenderer;

	// Start is called before the first frame update
	void Start()
	{
		gottenItemShowObjectSpriteRenderer = gottenItemShowObject.AddComponent<SpriteRenderer>();
		gottenItemShowObjectSpriteRenderer.sortingOrder = 3;
	}

	// Update is called once per frame
	void Update()
	{
		float x = Input.GetAxis("Horizontal");
		float y = Input.GetAxis("Vertical");
		transform.Translate(new Vector3(x, y) * Time.deltaTime * moveSpeed);
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
			UIManager.Instance.enforcePopup.SetActive(true);
			Time.timeScale = 0;
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		// money랑 박았을 때
		if (collision.CompareTag("Money"))
		{
			// 가진게 돈이거나 없다면
			if (currentGottenItem == ITEM.MONEY || currentGottenItem == ITEM.NONE)
			{
				// 가질 수 있는 만큼만 가지고
				if (gottenItemNum < gettableItemNum)
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
			if (currentGottenItem == ITEM.FOOD || currentGottenItem == ITEM.NONE)
			{
				// 가질 수 있는 만큼, 요리대에 있는 만큼 음식 얻기
				while (cooker0.foodCount > 0 && gottenItemNum < gettableItemNum)
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
				// 가질 수 있는 만큼, 요리대에 있는 만큼 음식 얻기
				while (cooker1.foodCount > 0 && gottenItemNum < gettableItemNum)
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
}
