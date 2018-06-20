using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class player : MonoBehaviour 
{
    /*character stat*/
    public int playerHp = 100;
    public int maxPlayerHP = 100;

    //limit movement 

    /*character status*/
    public bool moveStatus;
    public bool menuStatus;
    public GameObject DeadScene;

    //android touch system
    bool isTouchAttackMove;
    bool isTouchMoveStatus;
    float pressPeriod = 0.0f;
    bool isAttackMove = false;
    GameObject enemyTarget;
    Vector3 touchPosition;
    Vector3 screenToWorldPointPosition;
    public Rigidbody2D rigid2d;

    //heart animation
    Animator anim;
    public int heartbeattype;
    public Image healthImage;
    public Image healthBackground;
    float heartBeat;

    //status point system
    int mAddStatPoints = 0, mPointsTemp = 0;
    float mExpMaxTimer = 1.0f;
    bool mIsShowExpPanel = false;
    GameObject mGOLevelUpPanel;

    //damage effect
    GameObject mHitEffect;
    GameObject mGetDamageEffect;
    SpriteRenderer sprite;

    // use item
    PlayerStats mPlayerStats;
    ItemUse itemUse;

	public GameObject opponent;
	public GameObject targetopponent;



    private Vector3 NewMP;

    void Start () 
    {
        anim = GetComponent<Animator> ();
        heartbeattype = 1;
        healthImage.GetComponent<Tweener>().Play("heartbeat");
        healthBackground.GetComponent<Tweener>().Play("heartbeat");
        rigid2d = gameObject.GetComponent<Rigidbody2D>();
        mGetDamageEffect = GameObject.FindGameObjectWithTag("GetDamageEffect").gameObject;
        mHitEffect = Resources.Load("Prefabs/HitEffect") as GameObject;
        sprite = GetComponent<SpriteRenderer>();

        itemUse = GetComponent<ItemUse>();
        mPlayerStats = GetComponent<PlayerStats> ();

        mGetDamageEffect.SetActive(false);
    }

    public GameObject ClickSelect()
    {
        Vector2 rayPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero);
		//Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + 0.5f), Vector2.left, Color.green);
        if (hit)
        {
            Debug.Log(hit.transform.name);
            enemyTarget = hit.transform.gameObject;
            return hit.transform.gameObject;
        }
        else
        {
            return null;
        }
    }

    public void move(Vector2 mousePosition)
    {
		
		anim.SetTrigger ("Run");
		transform.position = Vector2.MoveTowards(gameObject.transform.position, mousePosition, 3 * Time.deltaTime);
        if(transform.position.x == mousePosition.x && transform.position.y == mousePosition.y)
        {
            anim.SetTrigger("Stop");
            anim.ResetTrigger("Run");

        }

        //        if (faceLeft) transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y);
        //        else if (faceLeft == false) transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y);
    }


    void Update() 
    {
		

        /*========================== Touch system =============================*/

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            //            if (GetComponent<ItemUse>().stoneExist == true)
            //                moveStatus = false;
            //else if (GetComponent<ItemUse>().stoneExist == false) { moveStatus = true; }
            if (!menuStatus && (gameObject.GetComponent<Infection>().isPressed == false)) 
            {
                if (itemUse.IsStoneCreated && Input.GetMouseButtonUp(0)) itemUse.ThrowStone(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                else
                {
                    if (Input.GetMouseButtonDown(0))
                    { pressPeriod = 0;  }
                    else if (Input.GetMouseButton(0))
                    {  pressPeriod += Time.deltaTime;  }

                    if (Input.GetMouseButtonUp(0) && pressPeriod < 0.5f)
					{ 
						
                        isAttackMove = false;
                        Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        Vector3 temp = transform.localScale;

                        if(transform.position.x > touchPos.x) temp.x = Mathf.Abs(transform.localScale.x);  // Flip left
                        else if(transform.position.x < touchPos.x) temp.x = -Mathf.Abs(transform.localScale.x);  // Flip right
                        transform.localScale = temp;


					

//                        if (ClickSelect () != null && ClickSelect ().name == "crate") {
//                            //Debug.Log ("working");
//                            isAttackMove = true;
//                        }
//                        if (ClickSelect() != null && ClickSelect().name == "enemy" )
//                        {
//                            isAttackMove = true;                                    
//                        }
						if (opponent != null) {
							targetopponent = opponent;
							isAttackMove = true;
						}
                        else
                        {
							
                            moveStatus = true;
                            Vector2 touchPosition = Input.mousePosition;
                            screenToWorldPointPosition = Camera.main.ScreenToWorldPoint(touchPosition);
                        }
                    }                   
                }
            }
        }

        if (moveStatus)
        {
            move(screenToWorldPointPosition);
            Vector2 playerPos = new Vector2(transform.position.x, transform.position.y);
            Vector2 WorldPos = new Vector2(screenToWorldPointPosition.x, screenToWorldPointPosition.y);
            if (playerPos == WorldPos) moveStatus = false;
        }
        else if (isAttackMove)
        {
			if(targetopponent != null)
            {
				
				Vector2 enemyPosition = targetopponent.transform.position;
                Vector2 temp = enemyPosition;
				Vector2 pPos = transform.position;
                           
                temp.x += 1.0f;
                move(temp);
				if (pPos == temp)
                {
					
                    isAttackMove = false;

                    /// attakc
                    LaunchAttack(mPlayerStats.mStatus.strength);

                } 
            }
        }

        #if UNITY_ANDROID

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
        RaycastHit2D Touches = Physics2D.Raycast(Input.GetTouch(0).position, -Vector2.up);
        if (Touches.collider != null)
        {
        if (Touches.collider.tag == "enemy" || Touches.collider.tag == "world_object")
        {
        isTouchAttackMove = true;
        }
        else
        {
        isTouchMoveStatus = true;
        Vector2 TouchPosition = Input.GetTouch(0).position;
        }
        }
        }

        if (isTouchMoveStatus)
        {
        move(touchPosition);
        Vector2 playerPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 WorldPos = new Vector2(screenToWorldPointPosition.x, screenToWorldPointPosition.y);

        Vector3 temp = transform.localScale;
        if (transform.position.x > touchPosition.x) temp.x = Mathf.Abs(transform.localScale.x);  // Flip left
        else if (transform.position.x < touchPosition.x) temp.x = -Mathf.Abs(transform.localScale.x);  // Flip right
        transform.localScale = temp;

        if (playerPos == WorldPos)
        {
        moveStatus = false;
        }
        }   
        else if(isTouchAttackMove)   {
        Vector3 enemyPosition = enemyTarget.transform.position;
        Vector3 temp = enemyPosition;
        Debug.Log(enemyPosition);
        temp.x += 1.0f;
        move(temp);
        if (transform.position == temp)
        {
        isTouchMoveStatus = false;
        LaunchAttack(attackHitBoxes[0], mStatus.strength);
        }

        }




        #endif



        float moveSpeed = mPlayerStats.mStatus.speed;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate((Vector3.right * Time.deltaTime) * moveSpeed);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate((Vector3.left * Time.deltaTime) * moveSpeed);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate((Vector3.up * Time.deltaTime) * moveSpeed);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate((Vector3.down * Time.deltaTime) * moveSpeed);
        }
    }

    // Healing fucntion.
    public void Heal(int amount)
    {
        heartbeat();
        if (playerHp <= maxPlayerHP)
        {
            playerHp += amount;
            if (playerHp > maxPlayerHP) playerHp = maxPlayerHP;
        }

        healthImage.fillAmount = (float)playerHp / 100 ;
    }

    // Item attackup function アイテムを使って、ダメージをアップします。
    public void AttackUp(int amount)
    { LaunchAttack(mPlayerStats.mStatus.strength + amount);  }

    IEnumerator GetDamageEffect() 
    {
        mGetDamageEffect.SetActive(true);

        Image image = mGetDamageEffect.GetComponent<Image>();
        Color c = image.color;
        c.a = 1.0f;

        while (c.a > 0) 
        {
            c.a = c.a - 0.05f;
            image.color = c;
            yield return null;
        }

        mGetDamageEffect.SetActive(false);
    }

    //attack script 攻撃
    private void LaunchAttack(int damageValue)
    {
        
        var IgnoreLayer = (1 << 10);

       // Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + 0.5f), Vector2.left, Color.green);

        RaycastHit2D hit = Physics2D.Raycast((new Vector2(transform.position.x, transform.position.y + 0.5f)), Vector2.left, 3f, IgnoreLayer);

		if (hit.collider != null) {
			if (hit.collider.gameObject.tag == "Enemy") {
				opponent.GetComponent<Enemy> ().GetDamage (damageValue);

			}
			else if (hit.collider.gameObject.tag == "world_object") {
				opponent.GetComponent<Crate> ().Crate_damage (1);
			}

			anim.SetTrigger("Attack");
			Vector3 pos = hit.collider.bounds.center;
			if (transform.localScale.x > 0) pos.x -= 0.75f;
			else pos.x += 0.75f;
			Instantiate (mHitEffect, pos, Quaternion.identity);

		}
//        if (hit.collider.gameObject.tag == "Enemy")
//        {
//            Debug.Log(hit.transform.gameObject.name);
//            
//            hit.transform.gameObject.GetComponent<Enemy>().GetDamage(damageValue);
//
//         
//        }
//		else if (hit.collider.gameObject.tag == "world_object")
//        {
//			Debug.Log(hit.transform.gameObject.name);
//            //Debug.Log ("working1");
//            anim.SetTrigger("Attack");
//
//            hit.transform.gameObject.GetComponent<Crate>().Crate_damage(damageValue);
//
//            Vector3 pos = hit.collider.bounds.center;
//            if (transform.localScale.x > 0) pos.x -= 0.75f;
//            else pos.x += 0.75f;
//            //              Debug.Log("Instantiate");
//            Instantiate(mHitEffect, pos, Quaternion.identity);
//        }
    }

    public void p_getDamage(int value)
    {
        anim.ResetTrigger("Run");
        anim.SetTrigger ("TakeDamage");
        heartbeat();
        value = value - mPlayerStats.mStatus.defense;
        if (value <= 0) value = 1;


        playerHp -= value;
        if (playerHp <= 0.0f) 
        {
            DeadScene.SetActive(true);
        }
        StartCoroutine(GetDamageEffect());
        healthImage.fillAmount = (float)playerHp / 100;
    }
    public void heartbeat()
    {
        if (playerHp >= 70 && heartbeattype != 1) {
            heartbeattype = 1;
            healthImage.GetComponent<Tweener> ().Play ("heartbeat");
            healthBackground.GetComponent<Tweener> ().Play ("heartbeat");
            healthImage.GetComponent<Tweener> ().Stop ("heartbeat1");
            healthBackground.GetComponent<Tweener> ().Stop ("heartbeat1");
            healthImage.GetComponent<Tweener> ().Stop ("heartbeat2");
            healthBackground.GetComponent<Tweener> ().Stop ("heartbeat2");

        } else if (playerHp >= 40 && playerHp < 70 && heartbeattype != 2) {
            heartbeattype = 2;
            healthImage.GetComponent<Tweener> ().Play ("heartbeat1");
            healthBackground.GetComponent<Tweener> ().Play ("heartbeat1");
            healthImage.GetComponent<Tweener> ().Stop ("heartbeat");
            healthBackground.GetComponent<Tweener> ().Stop ("heartbeat");
            healthImage.GetComponent<Tweener> ().Stop ("heartbeat2");
            healthBackground.GetComponent<Tweener> ().Stop ("heartbeat2");
        } else if (playerHp > 0 && playerHp < 40 && heartbeattype != 3) {
            heartbeattype = 3;
            healthImage.GetComponent<Tweener> ().Play ("heartbeat2");
            healthBackground.GetComponent<Tweener> ().Play ("heartbeat2");
            healthImage.GetComponent<Tweener> ().Stop ("heartbeat1");
            healthBackground.GetComponent<Tweener> ().Stop ("heartbeat1");
            healthImage.GetComponent<Tweener> ().Stop ("heartbeat");
            healthBackground.GetComponent<Tweener> ().Stop ("heartbeat");
        } else {
        }
    }
}
