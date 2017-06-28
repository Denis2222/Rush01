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
        UIEnemy.Clean();
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
		//Debug.DrawRay(transform.position, newDir, Color.red);
		transform.rotation = Quaternion.LookRotation(newDir);
	}

	void meleeAttack()
	{
		float diff = Time.fixedTime - meleeTime;
		if (diff > attackSpeed) {
			meleeTime = Time.fixedTime;
            if (RPGPlayer.rightHandEquiped != null)
            {
                Weapon rightHand = RPGPlayer.rightHandEquiped.gameObject.GetComponent<Weapon>();
                rightHand.Sound();
            }
            GetComponent<Animator>().SetBool(MovementEnum.MOVEMENT_ATTACK, true);
        }
    }

    //Call from weapon when collide with enemy
    public void meleeAttackHit(Collider hit)
    {
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
            
            //print("Primary Attack");
            bool castHit = false;
            bool castFloor = false;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit[] hits;
        	hits = Physics.RaycastAll(ray, 20.0F);
        	for (int i = 0; i < hits.Length; i++)
        	{
				RaycastHit hit = hits[i];
                //print(hit.transform.tag);

                if (hit.transform.tag == "Enemy")
				{
                    rotateToMouse();
                    UIEnemy.LookThat(hit.transform.gameObject);
                    if (!hit.transform.GetComponent<EnemyController>().isDying)
                    {
                        castHit = true;
                        meleeAttack();
                        break;
                    }
				} else if (hit.transform.tag == "Floor"){
                    castFloor = true; ;
                }
        	}
            if (!castHit)
            {
                
                Vector3 position = Constants.GetMousePosition();
                if (position.x != 0 && position.y != 0 && position.z != 0)
                {
                    //Debug.Log(position);
                    // Debug.Log(Agent.destination);

                    NavMeshHit hit;
                    bool blocked = false;
                    blocked = NavMesh.Raycast(transform.position, position, out hit, NavMesh.AllAreas);
                    Debug.DrawLine(transform.position, position, blocked ? Color.red : Color.green);

                    if (blocked)
                        Debug.DrawRay(hit.position, Vector3.up, Color.red);
                    if (Vector3.Distance(position, this.transform.position) > 2f)
                        {
                        rotateToMouse();
                        Agent.SetDestination(position);
                        Debug.Log("ChangeDestination");
                        GetComponent<Animator>().SetFloat(MovementEnum.MOVEMENT_FORWARD, 1);
                        isMooving = true;
                    }
                    else
                    {
                        GetComponent<Animator>().SetFloat(MovementEnum.MOVEMENT_FORWARD, 0);
                        isMooving = false;
                    }
     

                   
                    //print("Move Forward");
                    
                }
            }
        }
        else
        {
            print("Stop");
            isMooving = false;
            GetComponent<Animator>().SetBool(MovementEnum.MOVEMENT_ATTACK, false);
        }

        if (Input.GetMouseButtonDown(MouseClickEnum.RIGHT_CLICK)) {
			RPGPlayer.useSpell(0);
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
            Camera.transform.position = new Vector3(this.transform.position.x, distanceCamera, this.transform.position.z-(distanceCamera / 6));
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
		if (Input.GetKeyDown (KeyCode.Q))
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
