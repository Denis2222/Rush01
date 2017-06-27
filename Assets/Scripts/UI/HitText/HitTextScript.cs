using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitTextScript : MonoBehaviour {

	private Text text;
	private int maxPoliceSize = 30;
	private int minPoliceSize = 1;
	private float currentSize = 30;

	// Use this for initialization
	void Start () {
		text = this.GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (currentSize <= minPoliceSize) {
			GameObject.Destroy (this.gameObject);
			return;
		}
		currentSize -= 1f;
		setSizeToFont ((int)currentSize);
		this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y + 0.7f, this.transform.position.z);
	}

	public void setSizeToFont(int size) {
		this.text.fontSize = size;
	}

	public void initialize(float damage) {
		this.text.text = "" + damage;
	}
}
