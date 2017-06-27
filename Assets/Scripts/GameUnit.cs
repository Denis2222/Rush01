using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUnit : MonoBehaviour {

	const int MULTIPLICATEURINTELMANA = 2;
	const int VARIATIONDAMAGE = 5; 
	private int hp;
	protected int exp = 0;
	protected int mana;
	private float timeRegenMana = 0;
	private int competancePoint = 0;
	private int level = 0;

	public Weapon weaponEquip = null;
	[SerializeField]
	protected int str, agi, intel ,constitution, hpBase, regenMana, manaBase;

	protected void Start () {
		this.hp = this.getMaxHp();
		this.mana = this.getMaxMana();
	}
	
	protected void Update () {
		if (this.timeRegenMana >= 1) {
			this.timeRegenMana = 0;
			if (this.mana < this.getMaxMana ()) {
				if (this.mana + this.regenMana <= this.getMaxMana ())
					this.mana += this.regenMana;
				else
					this.mana = getMaxMana ();
			}
		}
		this.timeRegenMana += Time.deltaTime;
	}

	public virtual void setLevel(int l)
	{
		this.exp = (int)(Mathf.Exp((l - -40) / 8.7f) - 111);
	}

	public void setHp(int value)
	{
		this.hp = value;
	}

	public int getHp()
	{
		return this.hp;
	}

	public int getMaxHp()
	{
		return this.hpBase + this.constitution;
	}

	public int getMana()
	{
		return this.mana;
	}

	public int getMaxMana()
	{
		return this.manaBase + this.intel * MULTIPLICATEURINTELMANA;
	}

	public int getStr()
	{
		return this.str;
	}

	public int getAgi()
	{
		return this.agi;
	}

	public int getIntel()
	{
		return this.intel;
	}

	public int getConstitution()
	{
		return this.constitution;
	}

	public int getcompetancePoint() {
		return this.competancePoint;
	}

	public int getLevel()
	{
		return (int)Mathf.Max (Mathf.Floor (8.7f * Mathf.Log ((float)this.exp + 111f) + -40f), 1f);
	}

	virtual public int getDamage()
	{
		return Mathf.Max (Random.Range (str - VARIATIONDAMAGE, str + VARIATIONDAMAGE), 1) + (weaponEquip != null ? weaponEquip.getDamage () : 0);
	}

	public int getMinDamage()
	{
		return this.str - VARIATIONDAMAGE;
	}

	public int getMaxDamage()
	{
		return this.str + VARIATIONDAMAGE;
	}

	public void equipWeapon(Weapon weap)
	{
		this.weaponEquip = weap;
	}

	public Weapon disequipWeapon()
	{
		return this.weaponEquip;
	}

	public void damage(int damage)
	{
		this.hp -= damage;
	}

	public bool addHp(int hp)
	{
		if (this.hp >= getMaxHp ())
			return false;
		if (this.hp + hp > getMaxHp ()) {
			this.hp = getMaxHp ();
		} else
			this.hp += hp;
		return true;
	}

	public void addExp(int exp)
	{
		this.exp += exp;
		if (this.getLevel() > this.level) {
			this.levelUp ();
		}
	}

	virtual protected void levelUp()
	{
		this.level++;
		this.competancePoint += 5;
	}

	public void boostStr()
	{
		if (this.competancePoint > 0) {
			this.str++;
			this.competancePoint--;
		}
	}
	public void boostAgi()
	{
		if (this.competancePoint > 0) {
			this.agi++;
			this.competancePoint--;
		}
	}
	public void boostIntel()
	{
		if (this.competancePoint > 0) {
			this.intel++;
			this.competancePoint--;
			this.mana += MULTIPLICATEURINTELMANA;
		}
	}
	public void boostConstitution()
	{
		if (this.competancePoint > 0) {
			this.constitution++;
			this.competancePoint--;
			this.hp++;
		}
	}

	public void updateCompetancePoint(int points)
	{
		this.competancePoint = points;
	}
}
