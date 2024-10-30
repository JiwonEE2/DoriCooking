using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Customer Data", menuName = "Scriptable Object/Customer Data", order = 0)]
public class CustomerDataSO : ScriptableObject
{
	public string customerName;
	public int minFoodNum;
	public int maxFoodNum;
	public Sprite sprite;
}
