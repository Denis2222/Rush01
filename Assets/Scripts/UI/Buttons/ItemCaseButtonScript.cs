using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.UI;

public class ItemCaseButtonScript : AbstractCaseButton {

	// Use this for initialization
	void Start () {
		base.Update ();
	}

	// Update is called once per frame
	void Update () {
		base.Update ();
	}

	public override void addSpell(Spell spell) {
		
	}

	public override void removeSpell() {
		
	}

	public override void addItem(Item item) {
		if (item == null) {

			removeItem ();
			return;
		}
		this.item = item;
		this.updateIcon ();
	}

	public override void removeItem() {
		this.item = null;
		this.cleanIcon ();
	}
}
