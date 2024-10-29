using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
	public CustomerDataSO customerData;

	public float moveSpeed = 2;

	private SpriteRenderer spriteRenderer;
	// Start is called before the first frame update
	void Start()
	{
		spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
		spriteRenderer.sortingOrder = 1;
		spriteRenderer.sprite = customerData.sprite;
	}

	// Update is called once per frame
	void Update()
	{

	}
}
