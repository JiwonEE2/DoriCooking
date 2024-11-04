using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : SingletonManager<UIManager>
{
	public float money = 0;
	public TextMeshProUGUI moneyText;
	public GameObject pausePopup;
	public GameObject enforcePopup;

	// 레벨업이.. 테이블6, 조리대2, 계산대1, 속도1, 적재1, 대응1
	public bool isSpeedUp = false;
	public bool isCapacityUp = false;
	public bool isVillianInteractionUp = false;
	public int cookerUpCount = 0;
	public int tableUpCount = 0;

	// 빌런
	public bool isVillianSpawn = false;

	private void Start()
	{
		pausePopup.SetActive(false);
		enforcePopup.SetActive(false);
	}

	private void Update()
	{
		moneyText.text = money.ToString();
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (enforcePopup.activeSelf)
			{
				enforcePopup.SetActive(false);
				Time.timeScale = 1;
			}
			else
			{
				pausePopup.SetActive(true);
				Time.timeScale = 0;
			}
		}
	}

	public void OnClickResumeButton()
	{
		pausePopup.SetActive(false);
		Time.timeScale = 1;
	}

	public void OnClickSpeedUpButton()
	{
		if (isSpeedUp == false)
		{
			GameManager.Instance.playerMoveSpeed = 4;
			GameManager.Instance.level++;
			isSpeedUp = true;
		}
	}

	public void OnClickCapacityUpButton()
	{
		if (isCapacityUp == false)
		{
			GameManager.Instance.playerGettableItemCount = 10;
			GameManager.Instance.level++;
			isCapacityUp = true;
		}
	}

	public void OnClickVillianInterationUpButton()
	{
		if (isVillianInteractionUp == false)
		{
			GameManager.Instance.playerVillianInteractionSpeed = 1.5f;
			GameManager.Instance.level++;
			isVillianInteractionUp = true;
		}
	}

	public void OnClickCookerUpButton()
	{
		if (cookerUpCount < 2)
		{
			GameManager.Instance.cookDuration[cookerUpCount] = 2f;
			GameManager.Instance.cookerFoodLimit[cookerUpCount] = 10;
			GameManager.Instance.level++;
			cookerUpCount++;
		}
	}

	public void OnClickCounterUpButton()
	{
		if (GameManager.Instance.isCounterUp == false)
		{
			GameManager.Instance.foodSellDuration = 0.1f;
			GameManager.Instance.isCounterUp = true;
			GameManager.Instance.level++;
		}
	}

	public void OnClickTableUpButton()
	{
		if (tableUpCount < 6)
		{
			GameManager.Instance.eatFoodSpeedPerSeceond[tableUpCount] = 4;
			GameManager.Instance.level++;
			tableUpCount++;
		}
	}
}
