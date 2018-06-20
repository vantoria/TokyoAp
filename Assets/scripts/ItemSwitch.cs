using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSwitch : MonoBehaviour 
{
	public float moveSpeed = 0.5f;
    public float x_Range = 138.0f;
	private Vector2 firstPressPos;
	private Vector2 secondPressPos;
	private Vector2 currentSwipe;

	enum ItemState
	{
		LEFT = 0,
		MIDDLE = 1,
		RIGHT = 2,
	};
	ItemState mCurrItemState = ItemState.LEFT;

	Image mLeftButtonImg, mRightButtonImg;
	List<Transform> mItemListTrans = new List<Transform>();
    Vector3 mRangeResult = Vector3.zero;
    bool mIsAnimate = false;
    bool mIsRightClick = false;

	void Start () 
	{
		int childCount = transform.childCount;
		for(int i = 0; i < childCount; i++)
		{ mItemListTrans.Add(transform.GetChild(i)); }

		mLeftButtonImg = GameObject.FindGameObjectWithTag("ButtonLeft").GetComponent<Image>();
        mRightButtonImg = GameObject.FindGameObjectWithTag("ButtonRight").GetComponent<Image>();

		mLeftButtonImg.enabled = false;
		mRightButtonImg.enabled = true;

	}
	
	void Update () 
	{
        if(!mIsAnimate) return;

        Vector3 pos = transform.localPosition;
        pos.x = pos.x - (Time.deltaTime * moveSpeed * 10.0f);
        if( (mIsRightClick && pos.x <= mRangeResult.x) || (!mIsRightClick && pos.x >= mRangeResult.x)) { pos = mRangeResult; mIsAnimate = false; }
        transform.localPosition = pos;

	}

	public void PressLeft()
	{
		if (mCurrItemState == ItemState.LEFT) return;
		mCurrItemState = (ItemState)((int)mCurrItemState - 1);
		ToggleButtonState(mCurrItemState);

        Vector3 pos = transform.localPosition;
        pos.x = pos.x + x_Range;
        mRangeResult = pos; 
        moveSpeed = -Mathf.Abs(moveSpeed);
        mIsRightClick = false;
	}


	public void PressRight()
	{
		if (mCurrItemState == ItemState.RIGHT) return;
		mCurrItemState = (ItemState)((int)mCurrItemState + 1);
		ToggleButtonState(mCurrItemState);

        Vector3 pos = transform.localPosition;
        pos.x = pos.x - x_Range;
        mRangeResult = pos; 
        moveSpeed = Mathf.Abs(moveSpeed);
        mIsRightClick = true;
	}

	void ToggleButtonState(ItemState state)
	{
		if(state == ItemState.LEFT)
		{
			mLeftButtonImg.enabled = false;
			mRightButtonImg.enabled = true;
		}
		else if(state == ItemState.MIDDLE)
		{
			mLeftButtonImg.enabled = true;
			mRightButtonImg.enabled = true;
		}
		else if(state == ItemState.RIGHT)
		{
			mLeftButtonImg.enabled = true;
			mRightButtonImg.enabled = false;
		}

        mIsAnimate = true;
	}
	public void Swipe()
	{
		     if(Input.GetMouseButtonDown(0))
			     {
			         //save began touch 2d point
			        firstPressPos = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
			     }
		     if(Input.GetMouseButtonUp(0))
			     {
			            //save ended touch 2d point
			        secondPressPos = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
			        
			            //create vector from the two points
			        currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y); 
			            
			        //normalize the 2d vector
			        currentSwipe.Normalize();
			 
			        //swipe left
			        if(currentSwipe.x < 0 && currentSwipe.y > -0.5f  && currentSwipe.y < 0.5f)
				        {
				            Debug.Log("left swipe");
				        }
			        //swipe right
			        if(currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
				        {
				            Debug.Log("right swipe");
				        }
			    }
	}
}
