using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.PyroParticles;

public class Heal : Spell {

	public GameObject halo;
	public int valueHeal;
	public int LevelUpBoost;

	protected void Start () {
		base.Start ();
	}

	protected void Update () {
		base.Update ();
	}

	public override bool use()
	{
		if (RPGPlayer.Player.GetComponent<RPGPlayer> ().addHp (this.getHeal ())) {
			GameObject g = GameObject.Instantiate (halo, RPGPlayer.Player.transform.position, Quaternion.LookRotation (Constants.GetMousePosition () - RPGPlayer.Player.transform.position));
			g.transform.SetParent (RPGPlayer.Player.transform);
			Destroy (g, 3.0f);
			return true;
		}
		return false;
	}

	public override string getValue()
	{
		return "[" + valueHeal + "]";
	}

	public override string getUpgrade()
	{
		return "[" + (valueHeal + LevelUpBoost * this.getLevel()) +  "]";
	}

	public int getHeal()
	{
		return (valueHeal + LevelUpBoost * this.getLevel ());
	}
}
