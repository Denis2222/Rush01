using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Item : MonoBehaviour {

	public string name;
	public string description;
	public Sprite icon;
	public GameObject skin;

	protected void Start () {
		skin = GameObject.Instantiate (skin, transform.position, Quaternion.identity);
	//	skin.transform.localPosition = Vector3.zero;
		skin.tag = Constants.ITEM_TAG;
		skin.transform.parent = this.gameObject.transform;
	}

	protected void Update () {

	}

	abstract public bool use ();

	public void take()
	{
		this.transform.position = Vector3.zero;
		skin.SetActive (false);
	}
}
