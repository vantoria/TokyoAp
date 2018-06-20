using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour {

    public GameObject player;
    private Vector3 offset;

	// Use this for initialization
	void Start () {
        offset = transform.position - player.transform.position;
        
    }

    // Update is called once per frame
    void LateUpdate() {

        transform.position = player.transform.position + offset;
    
        // fix camera if the x position is more than 1.5 less than -10 
        // will change according to how long is the map
        //もしカメラのｘが１．5より大きい、それともー１０より小さいなら、カメラが動かないにします。
        //マップのサイズによって、ｘの数が変更できる。

        if (transform.position.x >= 1.5)
        {
            transform.position = new Vector3(1.5f , transform.position.y, transform.position.z);
        } else if (transform.position.x <= -100)
        {
            transform.position = new Vector3(-100f, transform.position.y, transform.position.z);
        }

       // fix camera if the y position is more than 0.1 and less than -.8
       if (transform.position.y >= 0.1)
        {
            transform.position = new Vector3(transform.position.x, 0.1f, transform.position.z);
        } else if (transform.position.y <= -0.8)
        {
            transform.position = new Vector3(transform.position.x, -0.8f, transform.position.z);
        }



    }
}
