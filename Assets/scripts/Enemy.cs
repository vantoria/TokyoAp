using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int enemyHP = 60;
    public float speed = 2f;
    public int e_damage = 10;
    public Collider2D[] e_attackHitBoxes;
	public GameObject player;
    public Slider healthSlider;
    public bool attackDistance;
	public float damageTime = 1;
	Animator anim;

   
    float timer = 0;
    float timeback = 0f;
    bool damaged;

	GameObject mPriorityTarget;
	void Start (){

		anim = GetComponent<Animator> ();
		player = GameObject.FindGameObjectWithTag("Player").gameObject;
	}


	public enum State
	{
		Idle = 0,
		Move,     
		Attack,
	}
	public State mState = State.Idle;

	enum PriorityValue
	{
		Mob = 0,
		Partner,
		Player,
		None,
	}
	PriorityValue mPriorityValue = PriorityValue.None;

    public void DropItem()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            if (child.tag == "Item")
            {
                child.gameObject.SetActive(true);
                child.parent = null;
                child.transform.position = transform.position;
                i -= 1;
            }
        }
    }

    public void GetDamage(int damageDeal)
    {
		anim.SetTrigger ("TakeDamage");
        damaged = true;
        enemyHP -= damageDeal;
        healthSlider.value = enemyHP;
        if (enemyHP <= 0.0f)
        {
            DropItem();
            enemyHP = 0;
            Destroy(gameObject);
          
        }
    }
    
    
    void idle()
    {
		anim.SetTrigger ("Stop");
    }

	void OnMouseOver(){
		player.GetComponent<player> ().opponent = this.gameObject;
	}
	void OnMouseExit(){
		player.GetComponent<player> ().opponent = null;
	}


    void move()
    {
		
		anim.SetBool ("Walk", true);
		Vector3 e_position = transform.position;
        Vector3 p_position = player.transform.position;
        float step = speed * Time.deltaTime;
        float dist = Vector3.Distance(p_position, e_position);
        //Debug.Log(dist);
        if (dist <= 5.5f && dist >= 3.1f)
        {
            transform.position = Vector3.MoveTowards(e_position, p_position, step);
        } else if (dist <= 2.5f)
        {
            attackDistance = !attackDistance;
            //Debug.Log("stop");
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }   else
        {
            attackDistance = false;
        }
    }

    private void OnTriggerStay2D(Collider2D target)
    {
		
		
		if (target.tag == "Player" || target.tag == "Partner" || target.tag == "mob")
        {
			if(target.tag == "mob" && (int)mPriorityValue >= (int)PriorityValue.Mob) mPriorityValue = PriorityValue.Mob; 
            else if(target.tag == "Partner" && (int)mPriorityValue >= (int)PriorityValue.Partner) mPriorityValue = PriorityValue.Partner; 
			else if(target.tag == "Player" && (int)mPriorityValue >= (int)PriorityValue.Player) mPriorityValue = PriorityValue.Player; 

            if( (target.tag == "Player" && mPriorityValue == PriorityValue.Player) ||
                (target.tag == "Partner" && mPriorityValue == PriorityValue.Partner) ||
                (target.tag == "mob" && mPriorityValue == PriorityValue.Mob) ) { mState = State.Attack; mPriorityTarget = target.gameObject; }

            if(mState == State.Attack)
            {
				if (timer >= damageTime) {
					timer -= damageTime;
					anim.SetTrigger ("Attack");
					if (target.tag == "Player")
						target.GetComponent<player> ().p_getDamage (e_damage);
					else if (target.tag == "Partner")
						target.GetComponent<partnerAI> ().GetDamage (e_damage);
					else if (target.tag == "mob")
						target.GetComponent<mob> ().GetDamage (e_damage);
				} else {
					timer += Time.deltaTime;
					anim.SetTrigger ("Stop");
				}
            }
         }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
	if (other.tag == "Player" || other.tag == "Partner" || other.tag == "mob")
        {
            timer = 0;
			if ((other.tag == "Player" && mPriorityValue == PriorityValue.Player) ||
			   (other.tag == "Partner" && mPriorityValue == PriorityValue.Partner) ||
			   (other.tag == "mob" && mPriorityValue == PriorityValue.Mob)) 
			{ mState = State.Move; }
        }
    }
    

    private void Update()
    {
		if (mState == State.Idle) {
			anim.SetTrigger ("Stop");
			anim.SetBool ("Walk", false);
		}
        timeback += Time.deltaTime;
	
		if (gameObject != null && mState == State.Move ) {
			if (mState == State.Move)
				transform.position = Vector3.MoveTowards (transform.position, mPriorityTarget.transform.position, speed * Time.deltaTime);       
			move();
		}
        // Debug.Log(enemyHP);     
    }

}




