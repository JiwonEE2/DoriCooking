using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnforceCursor : MonoBehaviour
{
	private int vertical = 1;
	private int horizontal = -1;
	private RectTransform rectTransform;

	[Header("레벨업 가격을 설정해 주세요")]
	public float speedCost = 300f;
	public float capacityCost = 300f;
	public float villianCost = 300f;
	public float cookerCost = 200f;
	public float countercost = 500f;
	public float tableCost = 300f;

	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
	}

	private void Update()
	{
		// 키 입력받기
		GetKey();

		// 커서의 이동
		Move();

		// 선택
		Choice();
	}

	public void GetKey()
	{
		if (Input.GetKeyDown(KeyCode.W))
		{
			vertical = 1;
		}
		else if (Input.GetKeyDown(KeyCode.S))
		{
			vertical = 0;
		}
		else if (Input.GetKeyDown(KeyCode.A))
		{
			horizontal--;
			if (horizontal < -1)
			{
				horizontal = -1;
			}
		}
		else if (Input.GetKeyDown(KeyCode.D))
		{
			horizontal++;
			if (horizontal > 1)
			{
				horizontal = 1;
			}
		}
	}

	public void Move()
	{
		switch (vertical)
		{
			case 1:
				switch (horizontal)
				{
					// Speed Up
					case -1:
						rectTransform.anchorMin = new Vector2(0.2f, 0.7f);
						rectTransform.anchorMax = new Vector2(0.2f, 0.7f);
						rectTransform.anchoredPosition = new Vector2(0, 0);
						break;
					// Capacity Up
					case 0:
						rectTransform.anchorMin = new Vector2(0.5f, 0.7f);
						rectTransform.anchorMax = new Vector2(0.5f, 0.7f);
						rectTransform.anchoredPosition = new Vector2(0, 0);
						break;
					// Villian Interaction Speed Up
					case 1:
						rectTransform.anchorMin = new Vector2(0.8f, 0.7f);
						rectTransform.anchorMax = new Vector2(0.8f, 0.7f);
						rectTransform.anchoredPosition = new Vector2(0, 0);
						break;
				}
				break;
			case 0:
				switch (horizontal)
				{
					// Coocker Up
					case -1:
						rectTransform.anchorMin = new Vector2(0.2f, 0.3f);
						rectTransform.anchorMax = new Vector2(0.2f, 0.3f);
						rectTransform.anchoredPosition = new Vector2(0, 0);
						break;
					// Counter Up
					case 0:
						rectTransform.anchorMin = new Vector2(0.5f, 0.3f);
						rectTransform.anchorMax = new Vector2(0.5f, 0.3f);
						rectTransform.anchoredPosition = new Vector2(0, 0);
						break;
					// Table Up
					case 1:
						rectTransform.anchorMin = new Vector2(0.8f, 0.3f);
						rectTransform.anchorMax = new Vector2(0.8f, 0.3f);
						rectTransform.anchoredPosition = new Vector2(0, 0);
						break;
				}
				break;
		}
	}

	public void Choice()
	{
		if (Input.GetKeyDown(KeyCode.Return))
		{
			switch (vertical)
			{
				case 1:
					switch (horizontal)
					{
						// Speed Up
						case -1:
							if (speedCost <= UIManager.Instance.money && UIManager.Instance.isSpeedUp == false)
							{
								UIManager.Instance.money -= speedCost;
								GameManager.Instance.playerMoveSpeed = 4;
								GameManager.Instance.level++;
								UIManager.Instance.isSpeedUp = true;
							}
							break;
						// Capacity Up
						case 0:
							if (capacityCost <= UIManager.Instance.money && UIManager.Instance.isCapacityUp == false)
							{
								UIManager.Instance.money -= capacityCost;
								GameManager.Instance.playerGettableItemCount = 10;
								GameManager.Instance.level++;
								UIManager.Instance.isCapacityUp = true;
							}
							break;
						// Villian Interaction Speed Up
						case 1:
							if (villianCost <= UIManager.Instance.money && UIManager.Instance.isVillianInteractionUp == false)
							{
								UIManager.Instance.money -= villianCost;
								GameManager.Instance.playerVillianInteractionSpeed = 1.5f;
								GameManager.Instance.level++;
								UIManager.Instance.isVillianInteractionUp = true;
							}
							break;
					}
					break;
				case 0:
					switch (horizontal)
					{
						// Coocker Up
						case -1:
							if (cookerCost <= UIManager.Instance.money && UIManager.Instance.cookerUpCount < 2)
							{
								UIManager.Instance.money -= cookerCost;
								GameManager.Instance.cookDuration[UIManager.Instance.cookerUpCount] = 2f;
								GameManager.Instance.cookerFoodLimit[UIManager.Instance.cookerUpCount] = 10;
								GameManager.Instance.level++;
								UIManager.Instance.cookerUpCount++;
							}
							break;
						// Counter Up
						case 0:
							if (countercost <= UIManager.Instance.money && GameManager.Instance.isCounterUp == false)
							{
								UIManager.Instance.money -= countercost;
								GameManager.Instance.foodSellDuration = 0.1f;
								GameManager.Instance.isCounterUp = true;
								GameManager.Instance.level++;
							}
							break;
						// Table Up
						case 1:
							if (tableCost <= UIManager.Instance.money && UIManager.Instance.tableUpCount < 6)
							{
								UIManager.Instance.money -= tableCost;
								GameManager.Instance.eatFoodSpeedPerSeceond[UIManager.Instance.tableUpCount] = 4;
								GameManager.Instance.level++;
								UIManager.Instance.tableUpCount++;
							}
							break;
					}
					break;
			}
		}
	}
}
