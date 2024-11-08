using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trashcan : MonoBehaviour
{
	[Tooltip("개당 버리는 시간")]
	public float trashThrowTime = 1f;
	private float timer = 0;

	private Player player;

	private void Start()
	{
		player = GameObject.Find("Player").GetComponent<Player>();
	}

	private void Update()
	{
		if (timer >= trashThrowTime)
		{
			timer = 0;
			player.gottenItemNum--;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			timer = 0;
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			timer += Time.deltaTime;
		}
	}
}
