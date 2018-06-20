using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPUI : MonoBehaviour 
{
    public float barDisplay = 0;
    public Vector2 pos = new Vector2(20, 40);
    public Vector2 size = new Vector2(400, 41.5f);
    public Texture2D progressBarEmpty;
    public Texture2D progressBarFull;
	
    void Update()
    {
        // TO BE DELETED. FOR TESTING PURPOSES.
        if (Input.GetKeyDown(KeyCode.KeypadEnter)) barDisplay = 1.0f;
        else if (Input.GetKeyDown(KeyCode.Keypad9)) barDisplay = 90.0f * 0.01f;
        else if (Input.GetKeyDown(KeyCode.Keypad8)) barDisplay = 80.0f * 0.01f;
        else if (Input.GetKeyDown(KeyCode.Keypad7)) barDisplay = 70.0f * 0.01f;
        else if (Input.GetKeyDown(KeyCode.Keypad6)) barDisplay = 60.0f * 0.01f;
        else if (Input.GetKeyDown(KeyCode.Keypad5)) barDisplay = 50.0f * 0.01f;
        else if (Input.GetKeyDown(KeyCode.Keypad4)) barDisplay = 40.0f * 0.01f;
        else if (Input.GetKeyDown(KeyCode.Keypad3)) barDisplay = 30.0f * 0.01f;
        else if (Input.GetKeyDown(KeyCode.Keypad2)) barDisplay = 20.0f * 0.01f;
        else if (Input.GetKeyDown(KeyCode.Keypad1)) barDisplay = 10.0f * 0.01f;
        else if (Input.GetKeyDown(KeyCode.Keypad0)) barDisplay = 0.0f;
  
    }

	void OnGUI () 
    {
        // OLD LEGACY CODE. USING NEW UNITY UI SEEMS EASIER TO PULL THIS OFF.
        GUIStyle test = new GUIStyle("box");
        test.padding = new RectOffset(0, 0, 0, 0);

        GUI.BeginGroup (new Rect (pos.x, pos.y, size.x, size.y));
        GUI.Box (new Rect (0, 0, size.x, size.y),progressBarEmpty,test);

        // draw the filled-in part:
        GUI.BeginGroup (new Rect (0, 0, size.x * barDisplay, size.y));
        GUI.Box (new Rect (0, 0, size.x, size.y),progressBarFull,test);
        GUI.EndGroup ();

        GUI.EndGroup ();
	}
}
