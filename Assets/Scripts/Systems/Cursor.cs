using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
	private float cursor = 1;
	public GameObject pausePopup;
	private RectTransform rectTransform;

	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.W))
		{
			cursor = 1;
			rectTransform.anchorMin = new Vector2(0.5f, 0.7f);
			rectTransform.anchorMax = new Vector2(0.5f, 0.7f);
			rectTransform.anchoredPosition = new Vector2(0, 0);
			rectTransform.sizeDelta = new Vector2(250, 150);
		}
		else if (Input.GetKeyDown(KeyCode.S))
		{
			cursor = 0;
			rectTransform.anchorMin = new Vector2(0.5f, 0.3f);
			rectTransform.anchorMax = new Vector2(0.5f, 0.3f);
			rectTransform.anchoredPosition = new Vector2(0, 0);
			rectTransform.sizeDelta = new Vector2(250, 150);
		}
		else if (Input.GetKeyDown(KeyCode.Return))
		{
			if (cursor == 1)
			{
				pausePopup.SetActive(false);
				Time.timeScale = 1;
			}
			else
			{
				// 게임 종료
				Application.Quit();
			}
		}
		print(cursor);
	}
}
