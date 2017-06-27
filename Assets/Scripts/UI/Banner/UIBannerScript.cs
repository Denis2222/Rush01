using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIBannerScript : MonoBehaviour{

	public AllTypeCaseButton[] cases;

	public UICaracteristiquesScript caracteristiquePanel;
	public UISpellsScript spellsPanel;
	public UIInventoryScript inventoryPanel;

	public ProgressBarScript manaBarre;
	public ProgressBarScript lifeBarre;

	private RPGPlayer player;

	public static UIBannerScript instance;

	// Use this for initialization
	void Start () {
		instance = this;
		this.player = GameObject.Find ("Player").GetComponent<RPGPlayer>();
	}
	
	// Update is called once per frame
	void Update () {
		_hookClickEntry ();
		_hookNumericInput ();
	}

	private void _hookClickEntry() {
		lifeBarre.UpdatePurcent ((float)(100 * player.getHp() / player.getMaxHp()));
		manaBarre.UpdatePurcent ((float)(100 * player.getMana() / player.getMaxMana()));
	}

	private void _hookNumericInput() {
		foreach (AllTypeCaseButton button in cases) {
			if (Input.GetKeyDown (button.keycode)) {
				if (button.spell != null) {
					player.useSpell (button.spell);
				}
				if (button.item != null) {
					button.item.use ();
				}
			}
		}
	}

	public void OnOpenUICarac() {
		this.player.GetComponent<PlayerController> ().pauseGame = true;
		caracteristiquePanel.setVisible (!caracteristiquePanel.visible);
	}

	public void OnOpenUIInventory() {
		this.player.GetComponent<PlayerController> ().pauseGame = true;
		inventoryPanel.setVisible (!inventoryPanel.visible);
	}

	public void OnOpenUISpells() {
		this.player.GetComponent<PlayerController> ().pauseGame = true;
		spellsPanel.setVisible (!spellsPanel.visible);
	}

	public void OnclickCase(ButtonScript button) {
		
	}

	public bool containsItem(Item it) {
		foreach (ItemCaseButtonScript bt in inventoryPanel.cases) {
			if (bt.item == it) {
				return true;		
			}
		}
		foreach (AllTypeCaseButton bt in cases) {
			if (bt.item == it) {
				return true;			
			}
		}
		return false;
	}

	public void removeItem(Item it) {
		foreach (ItemCaseButtonScript bt in inventoryPanel.cases) {
			if (bt.item == it) {
				bt.removeItem ();	
			}
		}
		foreach (AllTypeCaseButton bt in cases) {
			if (bt.item == it) {
				bt.removeItem ();		
			}
		}
	}

	public void addItem(Item it) {
		foreach (ItemCaseButtonScript bt in inventoryPanel.cases) {
			if (bt.item == null) {
				bt.addItem (it);
				return;
			}
		}
		foreach (AllTypeCaseButton bt in cases) {
			if (bt.item == null) {
				bt.addItem (it);
				return;
			}
		}
	}

}
