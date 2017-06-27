using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionVie : Item {

	// Use this for initialization
	void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();
	}

	public override bool use()
	{
		RPGPlayer player = RPGPlayer.Player.GetComponent<RPGPlayer> ();
		Debug.Log ("utilisation potion");
		bool rem = player.addHp ((int)(player.getMaxHp() * 0.3f));

		if (rem)
			player.removeItemToInventory (this);
		return true;
	}
}
