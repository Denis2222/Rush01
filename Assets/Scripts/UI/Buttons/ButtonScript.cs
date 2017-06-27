using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScript : MonoBehaviour, IPointerClickHandler {

	public bool isSelected = false;
	public bool active = true;

	// Use this for initialization
	void Start () {
		this.gameObject.SetActive (active);
	}
	
	// Update is called once per frame
	void Update () {
		if (active != this.gameObject.active)
			this.gameObject.SetActive (active);
	}

	public void OnPointerClick(PointerEventData data) {
		isSelected = true;
		Debug.Log("Click");
	}
}
