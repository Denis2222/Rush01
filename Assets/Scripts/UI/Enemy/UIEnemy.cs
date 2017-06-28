using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEnemy : MonoBehaviour {

    public GameObject Name;
    public GameObject Blood;
    public RectTransform blood;
    public GameObject Back;


    private EnemyController enemy;
	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        if (enemy) {
            if (enemy.RPGEnemy && enemy.RPGEnemy.getHp() > 0)
            {
                LookThat(enemy.gameObject);
            }
            else
            {
                Name.GetComponent<Text>().text = "";
                Blood.transform.localScale = new Vector2(0, 1f);
                Back.transform.localScale = new Vector2(0, 1f);
                enemy = null;
            }
        }
    }

    public void LookThat(GameObject entity)
    {
        enemy = entity.gameObject.GetComponent<EnemyController>();


        if (enemy)
        {
            this.gameObject.SetActive(true);
            float life = enemy.RPGEnemy.getHp() * 100 / enemy.RPGEnemy.getMaxHp();
            Name.GetComponent<Text>().text = enemy.name + " " + (life).ToString() + "%";
            Back.transform.localScale = new Vector2(1, 1f);
            Blood.transform.localScale = new Vector2(life/100, 1f);
        }
    }

    public void Clean()
    {
        Name.GetComponent<Text>().text = "";
        Blood.transform.localScale = new Vector2(0, 1f);
        Back.transform.localScale = new Vector2(0, 1f);
        enemy = null;
    }
}
