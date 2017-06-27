using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public Camera 			Camera;
	private NavMeshAgent	Agent;
	bool					isMooving;
	public bool 			lockedCamera;
    public float            distanceCamera;
	public RPGPlayer		RPGPlayer;
	public bool				pauseGame;
	public bool				isDying;
	public float			dieTime;
	public float			meleeTime;
	public float			attackSpeed;
    private UIEnemy      UIEnemy;

	void Start () {
		Camera = GameObject.Find ("Main Camera").GetComponent<Camera> ();
		Agent = GetComponent<NavMeshAgent> ();
		RPGPlayer = GetComponent<RPGPlayer> ();
        UIEnemy = GameObject.Find("UIEnemy").GetComponent<UIEnemy>();
		isMooving = false;
		//lockedCamera = false;
		pauseGame = false;
		distanceCamera = 50f;
	}

	public void Die () {
		RPGPlayer.setHp (0);
		dieTime = Time.fixedTime;
		pauseGame = true;
		isDying = true;
		GetComponent<Animator> ().SetBool (MovementEnum.MOVEMENT_DEAD, true);
	}

	void rotateToMouse()
	{
		Vector3 targetDir = Constants.GetMousePosition () - transform.position;
		float step = 100f * Time.deltaTime;
		Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
		Debug.DrawRay(transform.position, newDir, Color.red);
		transform.rotation = Quaternion.LookRotation(newDir);
	}

	void meleeAttack()
	{
		float diff = Time.fixedTime - meleeTime;
		if (diff > attackSpeed) {
			/*Collider[] hitColliders = Physics.OverlapSphere (this.transform.position, 1f);
			int i = 0;
			while (i < hitColliders.Length) {
				if (hitColliders [i].gameObject.tag == Constants.ENEMY_TAG) {
					print("zombie touche");
					EnemyController controller = hitColliders [i].gameObject.GetComponent<EnemyController> ();
					if (controller.isDying == false)
					{
                        controller.takeDamage(RPGPlayer.getDamage());
                    }
				}
				i++;
			}*/
			meleeTime = Time.fixedTime;
            if (RPGPlayer.rightHandEquiped != null)
            {
                Weapon rightHand = RPGPlayer.rightHandEquiped.gameObject.GetComponent<Weapon>();
                rightHand.Sound();
            }
            GetComponent<Animator>().SetBool(MovementEnum.MOVEMENT_ATTACK, true);
        }
        //GetComponent<Animator>().SetBool(MovementEnum.MOVEMENT_ATTACK, false);

    }

    //Call from weapon when collide with enemy
    public void meleeAttackHit(Collider hit)
    {
        //print("Hit"+ RPGPlayer.getDamage());
        EnemyController bad = hit.gameObject.GetComponent<EnemyController>();
        bad.takeDamage(RPGPlayer.getDamage());
    }

	void takeItem()
	{
		Debug.Log ("take item");
		Collider[] hitColliders = Physics.OverlapSphere (this.transform.position, 1f);
		int i = 0;
		while (i < hitColliders.Length) {
				if (hitColliders [i].gameObject.tag == Constants.ITEM_TAG) {
					Item it = hitColliders [i].gameObject.GetComponent<Item> ();
					it.take ();
				    Debug.Log ("take weapon");
					RPGPlayer.Player.GetComponent<RPGPlayer> ().addItemToInventory (it);
					break;
				}
			i++;
		}
	}

	void dropItem() 
	{
		RPGPlayer.Player.GetComponent<RPGPlayer> ().dropItem ();
	}

	void catchMovement()
	{

		if (Input.GetMouseButton(MouseClickEnum.LEFT_CLICK)) {
			print("Primary Attack");
            bool castHit = false;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit[] hits;
        	hits = Physics.RaycastAll(ray, 100.0F);
        	for (int i = 0; i < hits.Length; i++)
        	{
				RaycastHit hit = hits[i];
                print(hit.transform.tag);

                if (hit.transform.tag == "Enemy")
				{
                    UIEnemy.LookThat(hit.transform.gameObject);
                    if (!hit.transform.GetComponent<EnemyController>().isDying)
                    {
                        castHit = true;
                        meleeAttack();
                        break;
                    }
				}
        	}
            if (!castHit)
            {
                
                Vector3 position = Constants.GetMousePosition();
                if (position.x != 0 && position.y != 0 && position.z != 0)
                {
                    Debug.Log(Agent.SetDestination(position));
                    GetComponent<Animator>().SetFloat(MovementEnum.MOVEMENT_FORWARD, 1);
                    isMooving = true;
                }
            }
            rotateToMouse();
        }


		if (Input.GetMouseButtonDown(MouseClickEnum.RIGHT_CLICK)) {
			RPGPlayer.useSpell(0);
		}
		if (Input.GetMouseButton (MouseClickEnum.LEFT_CLICK)) {

		} else if (Input.GetKey (KeyCode.Q)) {
			meleeAttack ();
		} else {
			GetComponent<Animator> ().SetBool (MovementEnum.MOVEMENT_ATTACK, false);
		}

	}

	public void StopMovement()
	{
		Agent.velocity = Vector3.zero;
		Agent.ResetPath();
		isMooving = false;
		GetComponent<Animator> ().SetFloat (MovementEnum.MOVEMENT_FORWARD, 0);
	}

	void followCamera()
	{
		//if (lockedCamera) {
            Camera.transform.position = new Vector3(this.transform.position.x, distanceCamera, this.transform.position.z-(distanceCamera / 8));
			Camera.transform.LookAt(transform.position);
		//} else {
            //Camera.transform.position = new Vector3(this.transform.position.x, Camera.transform.position.y, Camera.transform.position.z);
        //}
	}

	void endGame()
	{
		//openEndMenu ();
		Debug.Log ("Game over !");
	}

	void onPauseGame()
	{
		StopMovement ();
		if (isDying) {
			float diff = Time.fixedTime - dieTime;
			if (diff > 3f) {
				GetComponent<Animator> ().SetBool (MovementEnum.MOVEMENT_DEAD, false);
				isDying = false;
				endGame ();
			}
		}
	}

	void Update () {
		if (pauseGame) {
			onPauseGame ();
			return;
		}
		if (Input.GetKeyDown (KeyCode.E))
			this.takeItem ();
		if (Input.GetKeyDown (KeyCode.D))
			this.dropItem ();
        distanceCamera += Input.GetAxis("Mouse ScrollWheel")*10;
		catchMovement ();
		if (Agent.remainingDistance <= 3f && Agent.remainingDistance != 0) {
			StopMovement ();
		}
		followCamera ();
	}
}
