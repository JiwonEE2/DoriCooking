using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : SingletonManager<UIManager>
{
	public float money = 0;
	public TextMeshProUGUI moneyText;
	public TextMeshProUGUI levelText;
	public GameObject pausePopup;
	public GameObject enforcePopup;
	public GameObject villianSign;

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
		villianSign.SetActive(false);
		pausePopup.SetActive(false);
		enforcePopup.SetActive(false);
	}

	private void Update()
	{
		moneyText.text = $"$ {money}";
		levelText.text = GameManager.Instance.level.ToString();

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			// 강화창 열려 있으면 닫고
			if (enforcePopup.activeSelf)
			{
				enforcePopup.SetActive(false);
				Time.timeScale = 1;
			}
			// 아니면 일시정지 창을 연다.
			else
			{
				pausePopup.SetActive(true);
				Time.timeScale = 0;
			}
		}

		// 빌런 스폰 시 UI 활성화 및 소멸시 비활성화
		villianSign.SetActive(isVillianSpawn);
	}
}
