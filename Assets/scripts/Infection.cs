using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Infection : MonoBehaviour
{
    public Image activeImage;
    public float cooldownTime = 5.0f;
    public bool isPressed = true;
    public GameObject mob;

    float cooldownTimer;

    public void Pressed()
    {
        if (activeImage.fillAmount == 1.0f) isPressed = !isPressed;
    }

    void Update()
    {
        if (isPressed && Input.GetMouseButtonDown(0))
        {
            Vector2 mouseposition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 worldMousePos = Camera.main.ScreenToWorldPoint(mouseposition);
            //            Debug.Log(mouseposition);
            Collider2D[] scanArea = Physics2D.OverlapCircleAll(worldMousePos, 3.5f);
            //            Debug.Log(scanArea.Length);
            foreach (Collider2D enemy in scanArea)
            {

                if (enemy.gameObject.tag == "Enemy")
                {

                    int infectionRate = 0;
                    infectionRate = 100 - (enemy.gameObject.GetComponent<Enemy>().enemyHP / 100 * 100);
                    infectionRate = infectionRate - (infectionRate % 10);
                    int infectionChance = Random.Range(0, 50);
                    if (infectionChance <= infectionRate)
                    {
                        Destroy(enemy.gameObject);
                        Instantiate(mob, enemy.gameObject.transform.position, Quaternion.identity);
                    }
                }
            }

            activeImage.fillAmount = 0.0f;
            isPressed = false;
        }

        if (activeImage.fillAmount < 1.0f)
        {
            if (cooldownTimer < cooldownTime)
            {
                cooldownTimer += Time.deltaTime;
                activeImage.fillAmount = cooldownTimer / cooldownTime;
                if (activeImage.fillAmount >= 1.0f)
                {
                    activeImage.fillAmount = 1.0f;
                    cooldownTimer = 0.0f;
                }
            }
        }
    }

    // Test infection radius.
    //	private void OnDrawGizmos() 
    //	{
    //		Vector2 mouseposition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    //		Vector2 worldMousePos = Camera.main.ScreenToWorldPoint(mouseposition);
    //
    //		Gizmos.color = Color.red;
    //		Gizmos.DrawWireSphere (worldMousePos, 3.5f);
    //	}
}
