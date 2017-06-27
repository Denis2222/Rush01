using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Spell : MonoBehaviour {

	public string name;
	public Sprite icon;
	public int mana;
	public string description;
	private int level = 0;
	public int startLevel;

	protected void Start () {
        this.gameObject.tag = "Spell";
	}
	
	protected void Update () {
		
	}

	public void up()
	{
		this.level += 1;
	}

	public int getLevel()
	{
		return this.level;
	}

	abstract public bool use ();
	abstract public string getUpgrade ();
	abstract public string getValue ();
}
