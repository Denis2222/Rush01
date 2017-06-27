using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.PyroParticles;

public class FireBall : Spell {

	public GameObject ball;
	public int damage;
	public int variationDamage;
	public int LevelUpBoost;

	protected void Start () {
		base.Start ();
	}
	
	protected void Update () {
		base.Update ();
	}

	public override bool use()
	{
		Vector3 pos = RPGPlayer.Player.transform.position;
		pos.y += 4;
		ball.GetComponent<FireProjectileScript> ().FireBall = this;

		GameObject newball = GameObject.Instantiate(ball, pos, Quaternion.LookRotation(Constants.GetMousePosition() - RPGPlayer.Player.transform.position));
        newball.gameObject.tag = "Spell";
        foreach (Transform t in newball.transform)
        {
            t.gameObject.tag = "Spell";
        }


        RPGPlayer.Player.GetComponent<PlayerController> ().StopMovement ();
		RPGPlayer.Player.transform.rotation = Quaternion.LookRotation(Constants.GetMousePosition() - RPGPlayer.Player.transform.position);
		return true;
	}

	public override string getValue()
	{
		return "[" + (damage - variationDamage + LevelUpBoost * this.getLevel()) + " - " + (damage + variationDamage + LevelUpBoost * this.getLevel()) + "]";
	}

	public override string getUpgrade()
	{
		return "[" + (damage - variationDamage + LevelUpBoost * this.getLevel()) + " - " + (damage + variationDamage + LevelUpBoost * this.getLevel()) +  "]";
	}

	public int getDamage()
	{
		return (damage + Random.Range (-variationDamage, (variationDamage + 1)) + LevelUpBoost * this.getLevel ());
	}
}
