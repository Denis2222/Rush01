using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryScript : MonoBehaviour {

	public ButtonScript exitButton;
	public GameObject[] contentObjects;
	public bool visible = false;

	public ItemCaseButtonScript[] cases;
	public ItemCaseButtonScript infosCase;
	public Text lblInfos;

	private RPGPlayer player;

	public static string LBL_EMPTY_SELECTION = "None selected object.";

	// Use this for initialization
	void Start () {
		this.player = GameObject.Find ("Player").GetComponent<RPGPlayer>();
		foreach (GameObject o in contentObjects) {
			o.SetActive (false);
		}
		lblInfos.text = LBL_EMPTY_SELECTION;
		//infosCase.//clean Image
	}
	
	// Update is called once per frame
	void Update () {
		if (exitButton.isSelected) {
			this.player.GetComponent<PlayerController> ().pauseGame = false;
			exitButton.isSelected = false;
			setVisible (false);
		}
		_checkOnClickCase ();
	}

	private void _checkOnClickCase() {
		foreach (ItemCaseButtonScript c in cases) {
			if (c.isSelected) {
				if (c.item == null) {
					infosCase.removeItem ();
					lblInfos.text = LBL_EMPTY_SELECTION;
				} else {
					infosCase.addItem (c.item);
					lblInfos.text = c.item.name;
				}
				c.isSelected = false;
			}
		}
	}

	public void setVisible(bool visible) {
		this.visible = visible;
		foreach (GameObject o in contentObjects) {
			o.SetActive (visible);
		}
	}
}
