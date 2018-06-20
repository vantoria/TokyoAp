using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour 
{
	enum Testss
	{
		asd = 0,
		qwe
	}
	Testss testss = Testss.asd;

	void flip()
	{

	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		Testss flip = Testss.asd;

		if(testss == flip)
		{
			flip ();
		}
	}
}
