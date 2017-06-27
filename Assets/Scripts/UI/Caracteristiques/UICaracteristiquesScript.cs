using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICaracteristiquesScript : MonoBehaviour {

	public ButtonScript exitButton;
	public GameObject[] contentObjects;
	public bool visible = false;

	public UIStatScript Level;
	public UIStatScript statsPoints;

	public UIStatScript Life;
	public UIStatScript Mana;
	public UIStatScript Strength;
	public UIStatScript Agility;
	public UIStatScript Intelligence;
	public UIStatScript Constitution;

	private RPGPlayer player;

	// Use this for initialization
	void Start () {
		this.player = GameObject.Find ("Player").GetComponent<RPGPlayer>();
		foreach (GameObject o in contentObjects) {
			o.SetActive (false);
		}
		Mana.buttonUp.active = false;
		Life.buttonUp.active = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (exitButton.isSelected) {
			this.player.GetComponent<PlayerController> ().pauseGame = false;
			exitButton.isSelected = false;
			setVisible (false);
		}
		_activeInputs ();
		_updateStats ();
		_upStats ();
	}

	public void setVisible(bool visible) {
		this.visible = visible;
		foreach (GameObject o in contentObjects) {
			o.SetActive (visible);
		}
	}

	private void _activeInputs() {
		if (player.getcompetancePoint () > 0) {
			Strength.buttonUp.active = true;
			Strength.buttonUp.gameObject.SetActive(true);
			Agility.buttonUp.active = true;
			Agility.buttonUp.gameObject.SetActive(true);
			Intelligence.buttonUp.active = true;
			Intelligence.buttonUp.gameObject.SetActive(true);
			Constitution.buttonUp.active = true;
			Constitution.buttonUp.gameObject.SetActive (true);
		} else {
			Strength.buttonUp.active = false;
			Agility.buttonUp.active = false;
			Intelligence.buttonUp.active = false;
			Constitution.buttonUp.active = false;
		}
	}

	private void _updateStats() {
		Level.UpdateStats (player.getLevel());
		statsPoints.UpdateStats (player.getcompetancePoint());
		Life.UpdateStats (player.getHp());
		Mana.UpdateStats (player.getMana());
		Strength.UpdateStats (player.getStr());
		Agility.UpdateStats (player.getAgi());
		Intelligence.UpdateStats (player.getIntel());
		Constitution.UpdateStats (player.getConstitution ());
	}

	private void _upStats() {
		if (Strength.buttonUp.isSelected) {
			this.player.boostStr ();
			Strength.buttonUp.isSelected = false;
		}
		if (Agility.buttonUp.isSelected) {
			this.player.boostAgi ();
			Agility.buttonUp.isSelected = false;
		}
		if (Intelligence.buttonUp.isSelected) {
			this.player.boostIntel ();
			Intelligence.buttonUp.isSelected = false;
		}
		if (Constitution.buttonUp.isSelected) {
			this.player.boostConstitution ();
			Constitution.buttonUp.isSelected = false;
		}
	}
}
