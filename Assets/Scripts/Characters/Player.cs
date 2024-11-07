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
	private bool isHorizontalMove = false;
	private bool hDown = false;
	private bool vDown = false;
	private bool hUp = false;
	private bool vUp = false;
	private Animator animator;

	public int gottenItemNum = 0;
	[Tooltip("현재 가지고 있는 아이템 종류")]
	public ITEM currentGottenItem = ITEM.NONE;

	private Cooker cooker0;
	private Cooker cooker1;

	private Sprite moneySprite;
	private Sprite foodSprite;
	private Sprite trashSprite;

	public GameObject gottenItemShowObject;

	private SpriteRenderer gottenItemShowObjectSpriteRenderer;

	public GameObject bubble;

	private void Start()
	{
		moneySprite = SpriteManager.Instance.moneySprite;
		foodSprite = SpriteManager.Instance.foodSprite;
		trashSprite = SpriteManager.Instance.trashSprite;
		gottenItemShowObjectSpriteRenderer = gottenItemShowObject.AddComponent<SpriteRenderer>();
		gottenItemShowObjectSpriteRenderer.sortingOrder = 8;
		animator = GetComponentInChildren<Animator>();

		cooker0 = GameObject.Find("Cooker0").GetComponent<Cooker>();
		cooker1 = GameObject.Find("Cooker1").GetComponent<Cooker>();
	}

	private void Update()
	{
		// 이동 함수
		if (Time.timeScale == 1)
		{
			Moving();
		}
		// 가진 아이템 보여주기
		GottenItemShow();
	}

	public void GottenItemShow()
	{
		if (gottenItemNum <= 0)
		{
			gottenItemNum = 0;
			currentGottenItem = ITEM.NONE;
		}
		switch (currentGottenItem)
		{
			case ITEM.NONE:
				bubble.SetActive(false);
				gottenItemShowObjectSpriteRenderer.sprite = null;
				return;
			case ITEM.MONEY:
				bubble.SetActive(true);
				gottenItemShowObjectSpriteRenderer.sprite = moneySprite;
				break;
			case ITEM.FOOD:
				bubble.SetActive(true);
				gottenItemShowObjectSpriteRenderer.sprite = foodSprite;
				break;
			case ITEM.TRASH:
				bubble.SetActive(true);
				gottenItemShowObjectSpriteRenderer.sprite = trashSprite;
				break;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.name == "FoodDistributeZone" && GameManager.Instance.isCounterVillianSpawn == false)
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
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.name == "FoodDistributeZone" && GameManager.Instance.isCounterUp == false)
		{
			GameManager.Instance.isSellingFood = false;
		}
	}

	public void Moving()
	{
		// 이동 구현 다시
		horizontal = Input.GetAxisRaw("Horizontal");
		vertical = Input.GetAxisRaw("Vertical");

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

		// Move
		if (isHorizontalMove)
		{
			transform.Translate(new Vector2(horizontal, 0) * Time.deltaTime * GameManager.Instance.playerMoveSpeed);
		}
		else if (!isHorizontalMove)
		{
			transform.Translate(new Vector2(0, vertical) * Time.deltaTime * GameManager.Instance.playerMoveSpeed);
		}

		// Animation
		if (animator.GetInteger("hAxis") != horizontal)
		{
			animator.SetBool("isChange", true);
			animator.SetInteger("hAxis", (int)horizontal);
		}
		else if (animator.GetInteger("vAxis") != vertical)
		{
			animator.SetBool("isChange", true);
			animator.SetInteger("vAxis", (int)vertical);
		}
		else
		{
			animator.SetBool("isChange", false);
		}
	}
}
