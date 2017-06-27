using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.UI;

public class SpellCaseButtonScript : AbstractCaseButton {

	// Use this for initialization
	void Start () {
		base.Start();
	}

	// Update is called once per frame
	void Update () {
		base.Update ();
	}

	public override void addSpell(Spell spell) {
		if (spell == null) {
			removeSpell ();
			return;
		}
		this.spell = spell;
		this.GetComponent<Image> ().sprite = spell.icon;
	}

	public override void removeSpell() {
		this.spell = null;
		this.GetComponent<Image> ().sprite = blankSprite;
	}

	public override void addItem(Item item) {
	
	}

	public override void removeItem() {
		
	}
}
