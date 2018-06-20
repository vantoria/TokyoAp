using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuEvents : MonoBehaviour {

    public void setActive(string menu)
    {
        clearMenuExcept(menu);
        GameObject.Find(menu).transform.GetChild(0).gameObject.SetActive(true);
    }
    public void clearMenuExcept(string menu)
    {
        for ( int i= 1; i<7; i++)
        {
            if (menu != "Menu"+i)
            {
                Debug.Log("test"+i);
                GameObject.Find("Menu"+i).transform.GetChild(0).gameObject.SetActive(false);
            }

        }
       
    }
}
