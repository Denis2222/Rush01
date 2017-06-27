using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(Weapon))]
public class WeaponEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		Weapon myScript = (Weapon)target;
		if(GUILayout.Button("save skin localTransform presets"))
		{
			myScript.localPosition = myScript.skin.transform.localPosition;
			myScript.localRotation = myScript.skin.transform.localRotation;
		}
	}
}
#endif

public class Weapon : Item {

	public List<GameObject> weaponModels;

	private int damage;
	private float attackSpeed;
	public int baseDamage;
	public int variationDamage;
	public int levelUpBoostDamage;
	public int baseAttackSpeed;
	public float variatonAttackSpeed;

	public Vector3 localPosition;
	public Quaternion localRotation;

	public List<AudioClip> sounds;
    public List<AudioClip> soundsHit;

    public List<GameObject> bloods;

    private AudioSource source;

    public bool hit;

    //	private GameObject skin;

    protected void Start () {
//		int rand = Random.Range (0, weaponModels.Count);
//		skin = weaponModels [rand];
//		base.Start ();
		IEnumerator routine = initializeData (0.1f);
		this.StartCoroutine (routine);
        source = GetComponent<AudioSource>();
        hit = false;
    }

	public IEnumerator initializeData(float waitTime)
	{
		while (true)
		{
			if (RPGPlayer.Player != null) {
				int level = RPGPlayer.Player.GetComponent<RPGPlayer> ().getLevel ();
				damage = baseDamage + Random.Range (-variationDamage, variationDamage + 1) + levelUpBoostDamage * level;

				break;
			}
			yield return new WaitForSeconds(waitTime);
		}
	}
	
	void Update () {
		
	}

	public int getDamage()
	{
		return this.damage;
	}

	public float getAttackSpeed()
	{
		return this.attackSpeed;
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && hit == false)
        {
            if (!other.gameObject.GetComponent<EnemyController>().isDying)
            {
                source.clip = soundsHit[Random.Range(0, soundsHit.Count)];
                source.Play();
                print("Weapon have trigger " + other.name);
                hit = true;
                GetComponentInParent<PlayerController>().meleeAttackHit(other);
                GameObject blood = GameObject.Instantiate(bloods[Random.Range(0, bloods.Count)], other.transform.position, Quaternion.identity);
            }
        }
    }

    public void Sound()
    {
        if (!source.isPlaying)
        {
            hit = false;
            source.clip = sounds[Random.Range(0, sounds.Count)];
            source.Play();
        }
    }

    public override bool use()
	{
		RPGPlayer player = RPGPlayer.Player.GetComponent<RPGPlayer> ();
		player.equipWeapon (this);
		player.equipItem (this);
//		skin.transform.localRotation = localRotation;
//		skin.transform.localPosition = localPosition;
		return true;
	}
}
