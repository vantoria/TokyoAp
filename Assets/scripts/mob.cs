using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mob : MonoBehaviour 
{
	Animator anim;
	public static List<GameObject> mobList = new List<GameObject>();

	public int mobHP = 100;
	public int attackDMG = 10;
	public int attackTimer = 2;
	public float timer = 0;

	Vector2 MobTarget;
	Vector2 randompoint;
	private Collider2D tempEnemyTarget;

	bool mIsStoneEffect = false;

	public enum State
	{
		Idle,
		//ScanMoveTarget,        
		RandomMove,     
		MoveToRandom,
		Attack,
	}
	public State mState;

	void Awake()
	{
		anim = GetComponent<Animator> ();
		mobList.Add(this.gameObject);
	}

	struct NearestMobs
	{
		public GameObject go;
		public float length;

		public NearestMobs(GameObject go, float length)
		{
			this.go = go;
			this.length = length;
		}
	}

	public static List<GameObject> GetNearestMobs(Vector3 pos, int total)
	{
		int count = mobList.Count;
		if(total >= count)  return mobList;

		// Calculate the sqrtMagnitude between all mobs and param pos. Put them into tempMobList.
		List<NearestMobs> tempMobList = new List<NearestMobs>();
		for(int i = 0; i < mobList.Count; i++)
		{
			float nearestLength = (mobList[i].transform.position - pos).sqrMagnitude;
			tempMobList.Add( new NearestMobs(mobList[i], nearestLength) );
		}

		// Find the shortest length between all mobs. Put them into nearestList
		List<GameObject> neareastList = new List<GameObject>();
		count = total;
		while(count > 0)
		{
			NearestMobs nearestMob = tempMobList[0];
			for(int i = 1; i < tempMobList.Count; i++)
			{
				if(tempMobList[i].length < nearestMob.length) nearestMob = tempMobList[i]; 
			}

			tempMobList.Remove(nearestMob); 
			neareastList.Add(nearestMob.go);
			count -= 1;

		}

		// Test
		//		for(int i = 0; i < total; i++)
		//		{
		//            Debug.Log("i" + neareastList[i].transform.position);
		//			Debug.Log((neareastList[i].transform.position - pos).sqrMagnitude);
		//		}
		return neareastList;
	}

	public void MoveToPoint(Vector3 pos)
	{
		FlipImage (pos);
		mState = State.MoveToRandom;
		randompoint = pos;
		mIsStoneEffect = true;
	}

	public void GetDamage(int damageDeal)
	{
		
		anim.SetTrigger ("TakeDamage");
		mobHP -= damageDeal;
		if (mobHP <= 0)
		{
			mobHP = 0;
			Destroy(gameObject);
		}
	}

	//private void ScanMoveTarget()
	//{
	//    Collider2D[] hitTarget;
	//    hitTarget = Physics2D.OverlapCircleAll(gameObject.transform.position, 10);
	//    foreach (Collider2D c in hitTarget)
	//    {
	//        if (c.tag == "Enemy") move(c.transform.position);
	//        else mState = State.Idle;
	//    } 
	//}

	private void Idle()
	{
		gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
	}

	private void randomPoint()
	{
		randompoint = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height)));
		mState = State.MoveToRandom;
		FlipImage (randompoint);
	}

	void FlipImage(Vector3 pos)
	{
		Vector3 temp = transform.localScale;
		if(transform.position.x > pos.x) temp.x = Mathf.Abs(transform.localScale.x);  // Flip left
		else if(transform.position.x < pos.x) temp.x = -Mathf.Abs(transform.localScale.x);  // Flip right
		transform.localScale = temp;
	}

	private void move(Vector2 target)
	{
		anim.SetBool ("Moving", true);
		transform.position = Vector2.MoveTowards(gameObject.transform.position, target, 3 * Time.deltaTime);
	}



	private void OnTriggerEnter2D(Collider2D hit)
	{
		//if(mIsStoneEffect) return;
		if (hit.tag == "Enemy")            
		{

			tempEnemyTarget = hit;
			mState = State.Attack;

		} 
	}
	private void attack()
	{
		if (timer >= attackTimer)
		{
			anim.SetTrigger ("Stop");
			anim.SetTrigger ("Attack");
    
			timer -= attackTimer;
			gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			if(tempEnemyTarget != null) tempEnemyTarget.GetComponent<Enemy>().GetDamage(attackDMG);
		}
		else timer += Time.deltaTime;
	}

	private void OnTriggerExit2D(Collider2D hit)
	{
		if (hit.tag == "Enemy") timer = 0;
	}

	//    IEnumerable CharacterPartern()
	//    {
	//        if (mState == State.RandomMove) randomPoint();
	//        else if (mState == State.MoveToRandom)
	//        {
	//            Vector2 currentMob = new Vector2(transform.position.x, transform.position.y);
	//            if (currentMob == randompoint) mState = State.RandomMove;
	//            yield return 0;
	//            move(randompoint);
	//        }
	//        else if (mState == State.Attack) attack();
	//        else if (mState == State.Idle) Idle();
	//    }


	void Update (){
		//        StartCoroutine(CharacterPartern());     
		if (mState == State.RandomMove) randomPoint();
		else if (mState == State.MoveToRandom)
		{
			Vector2 currentMob = new Vector2(transform.position.x, transform.position.y);
			if (currentMob == randompoint) { mState = State.RandomMove; mIsStoneEffect = false; }
			move(randompoint);
		}
		else if (mState == State.Attack) attack();
		else if (mState == State.Idle) Idle();
	}
}
