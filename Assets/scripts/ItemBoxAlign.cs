using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ItemBoxAlign : MonoBehaviour 
{
    public float offset = 1.0f;

    #if UNITY_EDITOR
	void Update () 
    {
        if( !Application.isPlaying ) 
        {
            Vector3 pos = transform.GetChild(0).transform.localPosition;

            for (int i = 1; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                RectTransform rectTranScript = child.GetComponent<RectTransform>();
                float width = rectTranScript.rect.width;
                float xScale = rectTranScript.transform.localScale.x;

                width = width * xScale;
                pos.x = pos.x + width + offset;
                child.transform.localPosition = pos;
            }
        }
	}
    #endif
}
