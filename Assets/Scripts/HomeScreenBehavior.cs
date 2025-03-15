using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class HomeScreenBehavior : MonoBehaviour
{
    public GameObject apps;
    public float speed;
    public Transform target;

    private bool canMove;
    
    // Start is called before the first frame update
    void Start()
    {
        canMove = false;
        apps.GetComponentInChildren<BoxCollider2D>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            var step = speed * Time.deltaTime; // calculate distance to move
            apps.transform.position = Vector3.MoveTowards(apps.transform.position, target.position, step);
        }

        if(apps.transform.position == target.position)
        {
            apps.GetComponentInChildren<BoxCollider2D>().enabled = true;
        }
    }

    public void MoveApps()
    {
        canMove = true;
    }

    public void EndHomeScreen()
    {
        GameObject.FindWithTag("GameManager").GetComponent<GameManager>().Set_InPhoneEvent(false);
        GameObject.FindWithTag("GameManager").GetComponent<GameManager>().CleanUpCrashEvent();
        Destroy(gameObject);
    }
}
