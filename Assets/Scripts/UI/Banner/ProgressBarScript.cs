using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBarScript : MonoBehaviour {

	private Vector3 baseScale;
	private Vector3 basePosition;
	private RectTransform rect;

	// Use this for initialization
	void Start () {
		rect = this.GetComponent<RectTransform> ();
		baseScale = new Vector3 (this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
		basePosition = new Vector3 (rect.position.x, rect.position.y, rect.position.z);
	}
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdatePurcent(float purcent) {
		float width = rect.sizeDelta.x;

		float purcentscale = (purcent / 100f);
		this.transform.localScale = new Vector3 (purcentscale, this.transform.localScale.y, this.transform.localScale.z);
		rect.position = new Vector3 ((basePosition.x - ((width / 2) - (((width / 2) * purcent) / 100))),basePosition.y, basePosition.z);
	}
}
