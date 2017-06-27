using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGEnemy : GameUnit {

	void Start ()
	{
		base.Start ();	
		
	}

	public void Attack(PlayerController Controller)
	{
		RPGPlayer Player = Controller.RPGPlayer;
		int hitDamage = base.getDamage ();
		if ((Player.getHp () - hitDamage) <= 0) {
			Controller.Die ();
		} else {
			Player.damage (hitDamage);
		}
	}

	public override void setLevel(int level)
	{
		base.setLevel (level);
		base.updateCompetancePoint(level * 5);
	
		while (this.getcompetancePoint () != 0) {
			int rand = Random.Range (1, 5);
			switch (rand)
			{
				case UpgradeEnum.UPGRADE_STRENGTH:
					this.boostStr ();
				break;

				case UpgradeEnum.UPGRADE_INTEL:
					this.boostIntel ();
				break;

				case UpgradeEnum.UPGRADE_CONST:
					this.boostConstitution();
				break;

				case UpgradeEnum.UPGRADE_AGILITY:
					this.boostAgi();
				break;
			}
		}
	}

	void Update ()
	{
		base.Update ();
	}
}
