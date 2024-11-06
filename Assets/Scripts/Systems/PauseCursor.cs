using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseCursor : MonoBehaviour
{
	private float vertical = 1;
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
			vertical = 1;
			rectTransform.anchorMin = new Vector2(0.5f, 0.7f);
			rectTransform.anchorMax = new Vector2(0.5f, 0.7f);
			rectTransform.anchoredPosition = new Vector2(0, 0);
		}
		else if (Input.GetKeyDown(KeyCode.S))
		{
			vertical = 0;
			rectTransform.anchorMin = new Vector2(0.5f, 0.3f);
			rectTransform.anchorMax = new Vector2(0.5f, 0.3f);
			rectTransform.anchoredPosition = new Vector2(0, 0);
		}
		else if (Input.GetKeyDown(KeyCode.Return))
		{
			if (vertical == 1)
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
	}
}
