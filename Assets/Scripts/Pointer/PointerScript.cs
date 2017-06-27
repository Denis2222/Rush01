using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerScript : MonoBehaviour {

	public static PointerScript instance;

	public AbstractCaseButton onButton = null;

	public List<GameObject> followers =  new List<GameObject>();

	// Use this for initialization
	void Start () {
		PointerScript.instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		foreach (GameObject obj in this.followers) {
			obj.transform.position = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, obj.transform.position.z);
		}
	}

	public void setCurButton(AbstractCaseButton button) {
		this.onButton = button;
	}

	public AbstractCaseButton curButton() {
		return (this.onButton);
	}

	public void followCursor(GameObject obj) {
		this.followers.Add(obj);
	}

	public bool unfollowCursor(GameObject obj) {
		if (this.followers.Contains (obj)) {
			this.followers.Remove (obj);
			return true;
		}
		return false;
	}
}
