using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public float moveSpeed = 2;
	public GameObject gettenMoney;

	[SerializeField]
	private List<GameObject> gettenMoneys;

	[Tooltip("최대 가질 수 있는 아이템 수")]
	public int gettableItemNum = 4;
	private int gettenItemNum = 0;

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

		if (collision.CompareTag("Money"))
		{
			if (gettenItemNum < gettableItemNum)
			{
				gettenItemNum++;
				gettenMoneys.Add(gettenMoney);
				//Instantiate(gettenMoney, transform);
				Destroy(collision.gameObject);
			}
		}
		else if (collision.CompareTag("MoneySaveZone"))
		{
			UIManager.Instance.money += gettenMoneys.Count;
			gettenMoneys.Clear();
		}
		else if (collision.CompareTag("EnforceZone"))
		{
			UIManager.Instance.enforcePopup.SetActive(true);
			Time.timeScale = 0;
		}
	}
}
