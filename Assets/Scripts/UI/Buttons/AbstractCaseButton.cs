using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.UI;

public abstract class AbstractCaseButton : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler {

	public KeyCode keycode = KeyCode.None;

	public bool blocked = false;
	public bool duplicatable = false;
	public bool isSelected = false;
	public bool active = true;
	public bool onDrop = false;
	public Sprite blankSprite;

	public Item item = null;
	public Spell spell = null;

	private bool ondrag = false;
	private Vector3 basePosition;

	// Use this for initialization
	public void Start () {
		if (item != null)
			addItem (item);
		if (spell != null)
			addSpell (spell);
		this.gameObject.SetActive (active);
	}
	
	// Update is called once per frame
	public void Update () {
		if (active != this.gameObject.active)
			this.gameObject.SetActive (active);
		updateIcon ();
	}

	public Item getItem () {
		return (this.item);
	}

	public Spell getSpell() {
		return (this.spell);
	}

	public abstract void addItem (Item item);

	public abstract void removeItem ();

	public abstract void addSpell (Spell spell);

	public abstract void removeSpell ();

	public void updateIcon() {
		if (this.item != null) {
			this.GetComponent<Image> ().sprite = item.icon;
		} else if (this.spell != null) {
			this.GetComponent<Image> ().sprite = spell.icon;
		}
	}

	public void cleanIcon() {
		this.GetComponent<Image> ().sprite = this.blankSprite;
	}

	public int getCaseId() {
		int caseId = -1;
		try
		{
			caseId = Int32.Parse (this.gameObject.name.Substring (4));
		}
		catch (FormatException e)
		{
			Console.WriteLine(e.Message);
		}
		return (caseId);
	}

	public void OnPointerClick(PointerEventData data) {
		isSelected = true;
		int caseId = -1;
		try
		{
			caseId = Int32.Parse (data.selectedObject.name.Substring (4));
		}
		catch (FormatException e)
		{
			Console.WriteLine(e.Message);
		}
		if (caseId == -1)
			return ;
		//Debug.Log("Click On Case Inventory n: " + caseId);
	}

	//Do this when the mouse is clicked over the selectable object this script is attached to.
	public void OnPointerDown(PointerEventData eventData)
	{
		if (blocked == true)
			return;
		basePosition = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z);
		PointerScript.instance.setCurButton (this);
		//NON FONCTIONNEL SUR CETTE VERSION
		//PointerScript.instance.followCursor (this.gameObject);
		this.ondrag = true;
	}

	//Do this when the mouse click on this selectable UI object is released.
	public void OnPointerUp(PointerEventData eventData)
	{
		if (this.blocked)
			return ;
		Item tmpitem = this.item;
		Spell tmpspell = this.spell;

		if (PointerScript.instance.curButton () != null && PointerScript.instance.curButton () != this) {
			AbstractCaseButton newCase = PointerScript.instance.curButton ();

			int button_type = -1;

			if ((newCase is ItemCaseButtonScript))
				button_type = 0;
			if ((newCase is SpellCaseButtonScript))
				button_type = 1;
			if (button_type == 0 && this.item != null || button_type == 1 && this.spell != null || button_type == -1) {
				if (duplicatable == false) {
					this.removeItem ();
					this.removeSpell ();
				}
				if (newCase.getItem () != null)
					this.addItem (newCase.item);
				if (newCase.getSpell () != null)
					this.addSpell (newCase.spell);
				switch (button_type) {
				case 0:
					if (tmpitem != null)
						newCase.addItem (tmpitem);
					break;
				case 1:
					if (tmpspell != null)
						newCase.addSpell (tmpspell);
					break;
				default:
					if (tmpitem != null)
						newCase.addItem (tmpitem);
					else if (tmpspell != null)
						newCase.addSpell (tmpspell);
					break;
				}
			}
		} else if (this.spell != null && this.duplicatable == false && PointerScript.instance.curButton () == null) {
			this.removeSpell ();
		}
		PointerScript.instance.setCurButton (null);
		//NON FONCTIONNEL SUR CETTE VERSION
		//PointerScript.instance.unfollowCursor (this.gameObject);
		this.transform.position = new Vector3 (basePosition.x, basePosition.y, this.transform.position.z);
		this.ondrag = false;
	}

	//Do this when the cursor enters the rect area of this selectable UI object.
	public void OnPointerEnter(PointerEventData eventData)
	{
		UITransporteurScript.instance.setCurCase (this);
		if (this.ondrag)
			return ;
		PointerScript.instance.setCurButton (this);
	}

	//Do this when the cursor exits the rect area of this selectable UI object.
	public void OnPointerExit(PointerEventData eventData)
	{
		UITransporteurScript.instance.setCurCase (null);
		if (this.ondrag)
			return ;
		if (PointerScript.instance.curButton () == this) {
			PointerScript.instance.setCurButton (null);
		}
	}
}
