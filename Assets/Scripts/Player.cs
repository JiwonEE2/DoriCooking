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
	public int gettenItemNum = 0;
	[Tooltip("현재 가지고 있는 아이템 종류")]
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
			// 가진 게 돈일 때만
			if (currentGettenItem == ITEM.MONEY)
			{
				// 가진만큼 매니저에 넣고
				UIManager.Instance.money += gettenItemNum;
				// 내 돈은 0으로
				gettenItemNum = 0;
				// 가진 아이템도 없음으로 설정
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
		// money랑 박았을 때
		if (collision.CompareTag("Money"))
		{
			// 가진게 돈이거나 없다면
			if (currentGettenItem == ITEM.MONEY || currentGettenItem == ITEM.NONE)
			{
				// 가질 수 있는 만큼만 가지고
				if (gettenItemNum < gettableItemNum)
				{
					gettenItemNum++;
					Destroy(collision.gameObject);
					// 가진 것을 머니로 바꾸기
					currentGettenItem = ITEM.MONEY;
				}
			}
		}
		// 해당 존의 부모에게서 음식 얻기
		else if (collision.CompareTag("CookZone0"))
		{
			// 가진 게 음식이거나 없을 때
			if (currentGettenItem == ITEM.FOOD || currentGettenItem == ITEM.NONE)
			{
				// 가질 수 있는 만큼, 요리대에 있는 만큼 음식 얻기
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
				// 가질 수 있는 만큼, 요리대에 있는 만큼 음식 얻기
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
				// 게임매니저의 푸드리밋 확인하고 넣기
				// 가진 요리를 전부 foodsetzone에 넣기
				GameManager.Instance.foodCount += gettenItemNum;
				gettenItemNum = 0;
				currentGettenItem = ITEM.NONE;
			}
		}
	}
}
