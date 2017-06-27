using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITransporteurScript : MonoBehaviour {

	public GameObject[] contentObjects;
	public bool visible = false;

	public AllTypeCaseButton slot;
	public Text lblDescription;

	public static UITransporteurScript instance;
	// Use this for initialization
	void Start () {
		instance = this;
		foreach (GameObject o in contentObjects) {
			o.SetActive (false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setVisible(bool visible) {
		this.visible = visible;
		foreach (GameObject o in contentObjects) {
			o.SetActive (visible);
		}
	}

	public void setCurCase(AbstractCaseButton c) {

		Vector3 mousePosition = Input.mousePosition;
		this.transform.position = new Vector3 (mousePosition.x + 10f, mousePosition.y - 10f, mousePosition.z);

		if (c == null) {
			slot.addItem (null);
			slot.addSpell (null);
			lblDescription.text = "";
			setVisible (false);
		} else {
			if (slot.spell == c.spell && slot.spell != null)
				return ;
			if (slot.item == c.item && slot.item != null)
				return ;
			if (c.spell != null) {
				slot.addSpell (c.spell);
				lblDescription.text = c.spell.description;
			} else if (c.item != null) {
				slot.addItem (c.item);
				lblDescription.text = c.item.description;
			}
			setVisible (true);
		}
	}
}
