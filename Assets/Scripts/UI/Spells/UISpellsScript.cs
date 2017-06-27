using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpellsScript : MonoBehaviour {

	public ButtonScript exitButton;
	public GameObject[] contentObjects;
	public UISpellScript[] spellsSlots;
	public Text spellsPoints;

	public Text spellSelectedName;
	public Text spellSelectedLevel;
	public Text spellSelectedDescription;
	public AbstractCaseButton spellInfo;

	public bool visible = false;

	private RPGPlayer player;

	// Use this for initialization
	void Start () {
		this.player = GameObject.Find ("Player").GetComponent<RPGPlayer>();
		foreach (GameObject spell in player.spells) {
			if (player.getLevel () >= spell.GetComponent<Spell> ().startLevel)
				addSpell (spell);
		}
		foreach (GameObject o in contentObjects) {
			o.SetActive (false);
		}
		if (player.spells.Count > 0) {
			selectSpell (player.spells[0].GetComponent<Spell>());
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (exitButton.isSelected) {
			this.player.GetComponent<PlayerController> ().pauseGame = false;
			exitButton.isSelected = false;
			setVisible (false);
		}
		if (this.visible == false)
			return;
		foreach (GameObject spell in player.spells) {
			bool contain = false;
			foreach (UISpellScript spellSlot in spellsSlots) {
				if (spellSlot.spell.Equals(spell)) {
					contain = true;
					break;
				}
			}
			if (contain == false && player.getLevel () >= spell.GetComponent<Spell>().startLevel) {
				addSpell (spell);
			}
		}
		spellsPoints.text = "" + player.getSpellPoints ();
	}

	public void setVisible(bool visible) {
		this.visible = visible;
		foreach (GameObject o in contentObjects) {
				o.SetActive (visible);
		}
	}

	public void selectSpell(Spell spell) {
		if (spell != null) {
			spellSelectedName.text = spell.name;
			spellSelectedLevel.text = "" + spell.getLevel ();
			spellSelectedDescription.text = "" +
			"Damages   : " + spell.getValue () + "\n" +
			"Mana cost : " + spell.mana + "\n" +
			"Heal      : ";
			spellInfo.addSpell (spell);
		} else {
			/*if (player.spells.Count > 0) {
				selectSpell (player.spells [0]);
				return;
			}*/
			spellSelectedName.text = "";
			spellSelectedLevel.text = "0";
			spellSelectedDescription.text = "";
			spellInfo.addSpell (null);
		}
	}

	public void addSpell(GameObject spell) {
		foreach (UISpellScript spellSlot in spellsSlots) {
			if (spellSlot.spell == null) {
				spellSlot.addSpell (spell);
				break ;
			}
		}
	}

	public void removeSpell(Spell spell) {
		foreach (UISpellScript spellSlot in spellsSlots) {
			if (spellSlot.spell != null && spell == spellSlot.spell) {
				spellSlot.removeSpell();
				break ;
			}
		}
	}
}
