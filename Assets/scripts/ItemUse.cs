using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUse : MonoBehaviour 
{
    public int armHeal = 10;
    public int organHeal = 25;
    public int batAttack = 10;
    public int pipeAttack = 15;
    public int smallStoneAttack = 2;
    public int bigStoneAttack = 5;

    public int smallStoneMob = 3;
    public GameObject smallStoneGO;
    public GameObject bigStoneGO;
    public float stoneSpeed = 1.0f;

    GameObject mCreatedStone;
    bool mIsStoneCreated = false;
    enum StoneType
    {
        Small_Stone = 0,
        Big_Stone
    };
    StoneType stone = StoneType.Small_Stone;

    // Hold to remove item.
    bool mIsButtonPressed = false;
    float mCurrHoldDuration = 0.0f;
    float mHoldDuration = 2.0f;
    string mHoldType = "";
    bool mIsRemoved = false;

    player mPlayerController;

    void Start()
    {
        mPlayerController = GameObject.FindGameObjectWithTag("Player").GetComponent<player>();
    }

    void Update()
    {
        if (mIsStoneCreated) 
        {
            Vector3 temp = Camera.main.ScreenToWorldPoint (Input.mousePosition);
            Vector3 currPos = mCreatedStone.transform.position;
            currPos.x = temp.x;
            currPos.y = temp.y;
            mCreatedStone.transform.position = currPos;
        }

        // For Hold event.
        if (mIsButtonPressed) 
        {
            mCurrHoldDuration += Time.deltaTime;
            if (mCurrHoldDuration >= mHoldDuration) Remove (mHoldType);
        }
    }

    public void Activate(string sType)
    {
        // If item has been removed, effect does not activate.
        if(mIsRemoved) { mIsRemoved = false; return; }

        ItemPickup.Type type = (ItemPickup.Type) System.Enum.Parse( typeof( ItemPickup.Type ), sType );

        List<PlayerItem.CurrItems> currItemList = PlayerItem.currItemList;
        Debug.Log (" currItemList.Count " +  currItemList.Count);

        for (int i = 0; i < currItemList.Count; i++)
        {
            if (mIsStoneCreated && currItemList [i].type == type) 
            {
                Destroy(mCreatedStone);
                mIsStoneCreated = false;

                if(stone == StoneType.Small_Stone) PlayerItem.ItemChange(ItemPickup.Type.Small_Stone, true);
                else if(stone == StoneType.Big_Stone) PlayerItem.ItemChange(ItemPickup.Type.Big_Stone, true);
            }
            else if(currItemList[i].type == type && currItemList[i].total != 0)
            {
                if(type == ItemPickup.Type.Arm) Heal(armHeal);
                else if(type == ItemPickup.Type.Organ) Heal(organHeal);
                else if(type == ItemPickup.Type.Bat) AttackUp(batAttack);
                else if(type == ItemPickup.Type.Pipe) AttackUp(pipeAttack);
                else if(type == ItemPickup.Type.Small_Stone) Small_Stone();
                else if(type == ItemPickup.Type.Big_Stone) Big_Stone();
                PlayerItem.ItemChange(type, false);
            }
        }
    }

    // OnPointerDown Event
    public void OnPointerDown(string sType)
    {
        mIsButtonPressed = true;
        mHoldType = sType;
    }

    // OnPointerUp Event
    public void OnPointerUp()
    {
        mIsButtonPressed = false;
        mCurrHoldDuration = 0.0f;
    }

    public bool IsStoneCreated
    { get { return this.mIsStoneCreated; } }

    public void ThrowStone(Vector3 pos)
    { StartCoroutine (IEThrowStone (pos)); }

    void Remove(string sType)
    {
        ItemPickup.Type type = (ItemPickup.Type) System.Enum.Parse( typeof( ItemPickup.Type ), sType );

        List<PlayerItem.CurrItems> currItemList = PlayerItem.currItemList;
        for (int i = 0; i < currItemList.Count; i++)
        {
            if(currItemList[i].type == type && currItemList[i].total != 0)
            {
                PlayerItem.ItemChange(type, false);
                mIsButtonPressed = false;
                mCurrHoldDuration = 0.0f;
                mIsRemoved = true;
                return;
            }
        }
    }

    void Heal(int amount)
    { mPlayerController.Heal(amount); }

    void AttackUp(int amount)
    { mPlayerController.AttackUp(amount); }

    void Small_Stone()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
        mCreatedStone = Instantiate (smallStoneGO, pos, Quaternion.identity);
        mIsStoneCreated = true;
        stone = StoneType.Small_Stone;
    }

    void Big_Stone()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
        mCreatedStone = Instantiate (bigStoneGO, pos, Quaternion.identity);
        mIsStoneCreated = true;
        stone = StoneType.Big_Stone;
    }

    IEnumerator IEThrowStone(Vector3 pos)
    {
        Destroy(mCreatedStone);
        mIsStoneCreated = false;
        float step = stoneSpeed * Time.deltaTime;
        pos.z = 0.0f;

        GameObject tempGO;
        if(stone == StoneType.Small_Stone) tempGO = smallStoneGO;
        else tempGO = bigStoneGO;

        GameObject go = Instantiate (tempGO, transform.position, Quaternion.identity);
        while(go.transform.position != pos)
        {
            go.transform.position = Vector2.MoveTowards(go.transform.position, pos,  step);
            yield return null;
        }

        List<GameObject> nearestMobList = new List<GameObject>();
        if(stone == StoneType.Small_Stone) nearestMobList = mob.GetNearestMobs(pos, smallStoneMob);
        else if(stone == StoneType.Big_Stone) nearestMobList = mob.GetNearestMobs(pos, mob.mobList.Count);


        for (int i = 0; i < nearestMobList.Count; i++) {
            if (nearestMobList [i]!= null){
                nearestMobList [i].GetComponent<mob> ().MoveToPoint (pos);
            }
            Destroy (go);
        }
    }
}
