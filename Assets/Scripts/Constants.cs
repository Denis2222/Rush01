using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants {

	public static string ENEMY_TAG = "Enemy";
	public static string ITEM_TAG = "Item";

	public static Vector3 GetMousePosition()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit[] hits = Physics.RaycastAll (ray);
		foreach (RaycastHit hitInfo in hits)
		{
			if (hitInfo.collider.gameObject.tag == "Floor")
				return (hitInfo.point); 
		}
		return Vector3.zero;
	}

}
