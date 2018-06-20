using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour 
{
	public float maxTime = 1.0f;
	float currTime = 0.0f;

	void Update () 
	{
		if(currTime < maxTime) 
		{
			currTime += Time.deltaTime;
			return;
		}
		Destroy(gameObject);
	}
}
