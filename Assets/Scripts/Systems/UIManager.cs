using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : SingletonManager<UIManager>
{
	public float money = 0;
	public TextMeshProUGUI moneyText;
	public GameObject pausePopup;

	// Start is called before the first frame update
	void Start()
	{
		pausePopup.SetActive(false);
	}

	// Update is called once per frame
	void Update()
	{
		moneyText.text = money.ToString();
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			pausePopup.SetActive(true);
			Time.timeScale = 0;
		}
	}

	public void OnClickResumeButton()
	{
		pausePopup.SetActive(false);
		Time.timeScale = 1;
	}
}
