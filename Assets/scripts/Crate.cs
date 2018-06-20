using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour {

    public GameObject itemDrop;
    public float CrateHP = 4.0f;
	public GameObject player;

	void Start(){
		player = GameObject.FindGameObjectWithTag("Player").gameObject;
	}

    public void Crate_damage(int DamageDeal)
    {
        CrateHP -= DamageDeal;
        if (CrateHP <= 0)
        {
			Destroy (gameObject);
            Instantiate(itemDrop);
        }
    }

	void OnMouseOver(){
		player.GetComponent<player> ().opponent = this.gameObject;
	}
	void OnMouseExit(){
		player.GetComponent<player> ().opponent = null;
	}
}
