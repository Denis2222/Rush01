using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.UI;

public class UISpellScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public Text name;
	public Text lblCountLevel;
	public SpellCaseButtonScript spellCase;
	public ButtonScript buttonLevelup;
	public bool active = false;
	public UISpellsScript spellPanel;

	public GameObject spell;

	private RPGPlayer player;

	// Use this for initialization
	void Start () {
		this.player = GameObject.Find ("Player").GetComponent<RPGPlayer>();
		this.gameObject.SetActive (active);
	}
	
	// Update is called once per frame
	void Update () {
		if (active != this.gameObject.active)
			this.gameObject.SetActive (active);

		if (spell != null) {
			lblCountLevel.text = "" + spell.GetComponent<Spell>().getLevel ();
		}

		if (this.buttonLevelup.isSelected) {
			this.player.useSpellPoint(spell);
			this.buttonLevelup.isSelected = false;
		}
	}

	public void addSpell(GameObject spell) {
		this.spell = spell;
		this.name.text = spell.GetComponent<Spell>().name;
		//TODO
		//this.lblCountLevel.text
		spellCase.addSpell (spell.GetComponent<Spell>());
		this.active = true;
		this.gameObject.SetActive (true);
	}

	public void removeSpell() {
		this.active = false;
		this.gameObject.SetActive (false);
		spellCase.removeSpell ();
		this.spell = null;
		this.name.text = "";
	}

	//Do this when the cursor enters the rect area of this selectable UI object.
	public void OnPointerEnter(PointerEventData eventData)
	{
		spellPanel.selectSpell (this.spell.GetComponent<Spell>());
	}

	//Do this when the cursor exits the rect area of this selectable UI object.
	public void OnPointerExit(PointerEventData eventData)
	{
		spellPanel.selectSpell (null);
	}
}
