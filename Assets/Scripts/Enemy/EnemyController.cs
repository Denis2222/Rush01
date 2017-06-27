using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {

	private PlayerController	playerController;
	private NavMeshAgent		Agent;
	public	bool 				isFollowing;
	public	Vector3 			followingPosition;
	public	float				moveSpeed;
	public	float				attackSpeed;
	public	RPGEnemy 			RPGEnemy;
	public	float				attackTime;
	public	int					experience;
	public List<GameObject> 	dropItems;
    public float                thinkTime;


	public bool 				isDying;
	public float				dieTime;
	public GameObject			hitText;
	void Start ()
	{
		this.playerController = GameObject.Find ("Player").GetComponent<PlayerController> ();
		this.Agent = GetComponent<NavMeshAgent> ();
		this.RPGEnemy = GetComponent<RPGEnemy> ();
		this.isFollowing = false;
		this.isDying = false;

		IEnumerator routine = initializeData (1.0f);
		this.StartCoroutine (routine);

        this.thinkTime = Time.time;

		InvokeRepeating("ScanRange", 1f, 1f);
	}

	public IEnumerator initializeData(float waitTime)
	{
		while (true)
		{
			if (playerController.RPGPlayer != null) {
				RPGEnemy.setLevel (playerController.RPGPlayer.getLevel());
				break;
			}
			yield return new WaitForSeconds(waitTime);
		}
	}

	void FollowPlayer()
	{
        if (this.isDying == false)
        {
            this.followingPosition = this.playerController.transform.position;
            this.Agent.SetDestination(this.playerController.transform.position);
            this.isFollowing = true;
            GetComponent<Animator>().SetFloat(MovementEnum.MOVEMENT_FORWARD, moveSpeed);
        }
	}

	void UnFollowPlayer()
	{
		Agent.velocity = Vector3.zero;
		Agent.ResetPath();
		GetComponent<Animator> ().SetFloat (MovementEnum.MOVEMENT_FORWARD, 0);
		this.isFollowing = false;
	}

	void rotateToPlayer()
	{
		Vector3 targetDir = playerController.transform.position - transform.position;
		float step = 2f * Time.deltaTime;
		Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
		Debug.DrawRay(transform.position, newDir, Color.red);
		transform.rotation = Quaternion.LookRotation(newDir);
	}

	void attackPlayer() 
	{
        if (this.isDying == false)
        {
            float diff = Time.fixedTime - attackTime;
            if (diff > attackSpeed)
            {
                GetComponent<Animator>().SetBool(MovementEnum.MOVEMENT_ATTACK, true);
                RPGEnemy.Attack(playerController);
                attackTime = Time.fixedTime;
            }
            rotateToPlayer();
        }
	}

	void isAroundPlayer()
	{
        if (this.Agent.remainingDistance <= 2.5f) {
				attackPlayer ();
		} else {
			GetComponent<Animator> ().SetBool (MovementEnum.MOVEMENT_ATTACK, false);
		}
	}

	void OnPausedGame()
	{
        if (!this.isDying)
        {
            GetComponent<Animator>().SetBool(MovementEnum.MOVEMENT_ATTACK, false);
            GetComponent<Animator>().SetFloat(MovementEnum.MOVEMENT_FORWARD, 0);
            Agent.velocity = Vector3.zero;
            Agent.ResetPath();
        }
	}

	void setAnimatorSpeed(int speed)
	{
		Animator animator = GetComponent<Animator> ();
		animator.speed = speed;
	}

	void Update ()
	{
		if (playerController.pauseGame) {
			OnPausedGame ();
			return;
		}
		if (isDying) {
            if (this.GetComponent<CapsuleCollider>().enabled)
                DropItems();
            this.GetComponent<CapsuleCollider>().enabled = false;
            this.GetComponent<NavMeshAgent>().enabled = false;
            float diff = Time.fixedTime - this.dieTime;
            this.transform.position = transform.position + new Vector3(0, -0.001f, 0);
			if (diff > 20f) {
				GameObject.DestroyObject (this.gameObject);
			}
		}
		else if (isFollowing) {
            transform.LookAt(playerController.transform);
            setAnimatorSpeed (1);
			if (playerController.RPGPlayer.getHp () > 0) {
				isAroundPlayer ();
			} else {
				GetComponent<Animator> ().SetBool (MovementEnum.MOVEMENT_ATTACK, false);
			}
		} else if (thinkTime < Time.time){
            print("New Action");
            thinkTime = Time.time + 3 + Random.Range(0f,3f);
			setAnimatorSpeed (5);
			this.followingPosition = this.playerController.transform.position;
			this.Agent.SetDestination (this.transform.position + new Vector3(Random.Range(-30f,30f), 0, Random.Range(-30f, 30f)));
			GetComponent<Animator> ().SetFloat (MovementEnum.MOVEMENT_FORWARD, 0.1f);
        }
        if (this.name == "Skeleton")
        {
            transform.position = Vector3.MoveTowards(transform.position, this.Agent.destination, 2f * Time.deltaTime);
        }
    }


	void ScanRange() {	
		Vector3 dir = this.playerController.transform.position - this.transform.position;
		RaycastHit hit;
		Debug.DrawRay(this.transform.position,dir);
		if (Physics.Raycast(this.transform.position, dir, out hit, 20))
		{
			//print(hit.transform.tag);
			if(hit.transform.tag == "Player" && this.isFollowing == false) {
				//print("FollowPlayer");
				this.FollowPlayer ();
			}
			else if (this.isFollowing == true)
			{
				this.UnFollowPlayer();
			}
		}
		else if (this.isFollowing == true)
		{
			this.UnFollowPlayer();
		}
	}

	public void	DropItems()
	{
		int dropped = Random.Range (0, this.dropItems.Count);
		GameObject newDrop = GameObject.Instantiate (this.dropItems[dropped]);
		newDrop.transform.position = this.transform.position;
		Debug.Log ("The enemy dropped an item :" + newDrop.name);
	}

	public void Die()
	{
		this.UnFollowPlayer ();
		playerController.RPGPlayer.addExp (this.experience);
        GetComponent<Animator>().SetBool(MovementEnum.MOVEMENT_ATTACK, false);
        GetComponent<Animator>().SetFloat(MovementEnum.MOVEMENT_FORWARD, 0);
        Agent.velocity = Vector3.zero;
        GetComponent<Animator> ().SetBool (MovementEnum.MOVEMENT_DEAD, true);
		this.isDying = true;
		this.dieTime = Time.fixedTime;
		Debug.Log ("An enemy is dying ! Well played you won " + this.experience + " points of experience !");
		this.OnPausedGame ();
        
    }

	public void popCurrentHit(int valueDamage)
	{
		GameObject currentHit = GameObject.Instantiate (this.hitText);
		currentHit.GetComponent<Text>().text = " - " + valueDamage.ToString();
		currentHit.GetComponent<Text> ().fontSize = 16;

		currentHit.transform.SetParent(GameObject.Find ("Canvas").GetComponent<Canvas>().transform);
		currentHit.transform.position = new Vector3 (Random.Range(70f,110f), Random.Range(100f,200f), 0f);
	}

	public void takeDamage(int value)
	{
		this.popCurrentHit (value);
		this.RPGEnemy.damage (value);
		if (this.RPGEnemy.getHp () <= 0) {
			this.Die ();	
		}
	}
}
