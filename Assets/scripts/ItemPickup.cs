using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ItemPickup : MonoBehaviour 
{
    public enum Type
    {
        Arm,
        Organ,
        Bat,
        Pipe,
        Small_Stone,
        Big_Stone
    };
    public Type mType = Type.Arm;
    public Texture2D mArm, mOrgan, mBat, mPipe, mSmallStone, mBigStone;
    public float untouchableTime = 1.5f;

    List<Texture2D> Texture2DList = new List<Texture2D>();
    bool mIsEnabled = false;
    float mUntouchableTimer = 0.0f;

    new Renderer renderer;

    void OnEnable () 
    {
        renderer = GetComponent<Renderer>();

        if(Texture2DList.Count == 0) AddTextureToList ();
    }

    #if UNITY_EDITOR
    void Update()
    {
        if( !Application.isPlaying ) 
        {
            if(renderer.sharedMaterial.mainTexture == Texture2DList[(int)mType]) return;
            renderer.material = new Material(renderer.sharedMaterial);
            renderer.sharedMaterial.mainTexture = Texture2DList[(int)mType];
        }

        if(mUntouchableTimer < untouchableTime)  { mUntouchableTimer += Time.deltaTime; return; }
        mIsEnabled = true;
    }
    #endif

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!mIsEnabled || other.transform.tag != "Player") return;

        PlayerItem.ItemChange(mType, true);
        Destroy(gameObject);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!mIsEnabled || other.transform.tag != "Player") return;

        PlayerItem.ItemChange(mType, true);
        Destroy(gameObject);
    }

    void AddTextureToList()
    {
        Texture2DList.Add(mArm);
        Texture2DList.Add(mOrgan);
        Texture2DList.Add(mBat);
        Texture2DList.Add(mPipe);
        Texture2DList.Add(mSmallStone);
        Texture2DList.Add(mBigStone);
    }
}
