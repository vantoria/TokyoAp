//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System;

//public class partnerAI_old : MonoBehaviour {

//    public Transform player;
//    int currentWaypoint = 0;
//    public float updateRate = 2f;
//    public Path path;
//    public float speed = 300f;
//    public ForceMode2D fmode;
//    Seeker seeker;
//    public bool pathend = false;
//    Rigidbody2D rb;

//    public float nextWaypointDistance = 3f;

//    // Use this for initialization
//    void Start() {
//        seeker = GetComponent<Seeker>();
//        rb = GetComponent<Rigidbody2D>();

//        if (player == null)
//        {
//            Debug.LogError("no player target found");
//            return;
//        }

//        seeker.StartPath(transform.position, player.position, OnPathComplete);

//        StartCoroutine(UpdatePath());

//	}

//    private IEnumerator UpdatePath()
//    {
//        if (player == null)
//        {
//            yield return false;
//        }
//        seeker.StartPath(transform.position, player.position, OnPathComplete);

//        yield return new WaitForSeconds (1f / updateRate);
//        StartCoroutine(UpdatePath());
//    }

//    public void OnPathComplete(Path p)
//    {
//       // Debug.Log("path initialize");
//        path = p;
//        currentWaypoint = 0;
//    }

//    private void FixedUpdate()
//    {
//        if (player == null)
//        {
//            return;
//        }
//        if (path == null)
//            return;

//        if (currentWaypoint >= path.vectorPath.Count)
//        {
//            if (pathend)
//                return;
//            pathend = true;
//            return;
//        }
//        pathend = false;

//        // find direction
//        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
//        dir *= speed * Time.fixedDeltaTime;

//        if ((player.transform.position - this.transform.position).magnitude <= 3f)
//        {
//            //Debug.Log("Within range");
//            rb.velocity = new Vector3(0, 0, 0);
//        } else
//        {
//            rb.AddForce(dir, fmode);
//        }
        

//        float pddistance = 0f;
//        pddistance = (player.transform.position - this.transform.position).magnitude;
//        //Debug.Log(pddistance);


//        float dist = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
//        if (dist < nextWaypointDistance)
//        {
//            currentWaypoint++;
//            return;
//        }
       
//    }


//    // Update is called once per frame
//    void Update () {

//    }
//}
