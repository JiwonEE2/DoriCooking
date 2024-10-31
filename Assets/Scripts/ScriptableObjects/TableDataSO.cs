using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Table Data", menuName = "Scriptable Object/Table Data", order = 1)]
public class TableDataSO : ScriptableObject
{
	public int tableNum;
	public Vector2 position;
	public bool isEmpty;
	public int trashCount;
}
