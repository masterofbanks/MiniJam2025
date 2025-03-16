using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class HomeScreenBehavior : MonoBehaviour
{
    public GameObject apps;
    public GameObject bar;
    public float speed;
    public Transform apps_target;
    public Transform bar_target;

    private bool canMoveApps;
    private bool canMoveBar;
    
    // Start is called before the first frame update
    void Start()
    {
        canMoveApps = false;
        canMoveBar = false;
        apps.GetComponentInChildren<BoxCollider2D>().enabled = false;
        bar.GetComponentInChildren<BoxCollider2D>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMoveApps)
        {
            var step = speed * Time.deltaTime; // calculate distance to move
            apps.transform.position = Vector3.MoveTowards(apps.transform.position, apps_target.position, step);
        }

        if (canMoveBar)
        {
            var step = speed * Time.deltaTime; // calculate distance to move
            bar.transform.position = Vector3.MoveTowards(bar.transform.position, bar_target.position, step);
        }

        if (apps.transform.position == apps_target.position)
        {
            apps.GetComponentInChildren<BoxCollider2D>().enabled = true;
        }

        if (bar.transform.position == bar_target.position)
        {
            bar.GetComponentInChildren<BoxCollider2D>().enabled = true;
        }


    }

    public void MoveApps()
    {
        canMoveApps = true;
    }

    public void EndHomeScreen()
    {
        GameObject.FindWithTag("GameManager").GetComponent<GameManager>().Set_InPhoneEvent(false);
        GameObject.FindWithTag("GameManager").GetComponent<GameManager>().CleanUpCrashEvent();
        GameObject.FindWithTag("GameManager").GetComponent<GameManager>().CleanUpDisconnectEvent();
        Destroy(gameObject);
    }

    public void MoveBar()
    {
        canMoveBar = true;
    }
}
