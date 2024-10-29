using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public float moveSpeed = 2;
	public MoneyPrefab moneyPrefab;
	public GameObject gettenMoney;

	[Tooltip("최대 가질 수 있는 아이템 수")]
	public int gettableItemNum = 4;
	private int getItemNum = 0;

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
		if (getItemNum < gettableItemNum)
		{
			if (collision.CompareTag("Money"))
			{
				getItemNum++;
				Instantiate(gettenMoney, transform);
				Destroy(collision.gameObject);
			}
		}
	}
}
