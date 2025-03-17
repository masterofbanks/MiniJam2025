using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseBehavior : MonoBehaviour
{

    //components
    public PlayerInputActions PIAs;

    //Input Actions
    private InputAction click;

    private void Awake()
    {
        PIAs = new PlayerInputActions();
    }
    void Start()
    {

    }

    void Update()
    {
        //Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);
    }

    private void Click(InputAction.CallbackContext context)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        if(hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Refresh"))
            {
                //Debug.Log("Hit Refresh");
                GameObject.FindWithTag("GameManager").GetComponent<GameManager>().SetNewSubs();
            }

            else if (hit.collider.gameObject.CompareTag("HomeButton"))
            {
                //Debug.Log("Hit Home Button");
                GameObject.FindWithTag("GameManager").GetComponent<GameManager>().GoHomeScreen();
            }

            else if (hit.collider.gameObject.CompareTag("X"))
            {
                Debug.Log("Hit X");
                GameObject.FindWithTag("Ad").GetComponent<Ad_Behavior>().Clicked_X();

            }

            else if (hit.collider.gameObject.CompareTag("RightButton"))
            {
                hit.collider.gameObject.GetComponentInParent<HomeScreenBehavior>().MoveApps();
            }

            else if (hit.collider.gameObject.CompareTag("Youtube"))
            {
                if(GameObject.FindWithTag("GameManager").GetComponent<GameManager>().GetPhoneGameIndex() == 1)
                    hit.collider.gameObject.GetComponentInParent<HomeScreenBehavior>().EndHomeScreen();
            }

            else if (hit.collider.gameObject.CompareTag("TP"))
            {
                GameObject.FindWithTag("GameManager").GetComponent<GameManager>().RestartAltitude();
            }

            else if (hit.collider.gameObject.CompareTag("Settings"))
            {
                hit.collider.gameObject.GetComponentInParent<HomeScreenBehavior>().MoveBar();
            }

            else if (hit.collider.gameObject.CompareTag("Wifi"))
            {
                if (GameObject.FindWithTag("GameManager").GetComponent<GameManager>().GetPhoneGameIndex() == 2)
                {
                    hit.collider.gameObject.GetComponentInParent<HomeScreenBehavior>().EndHomeScreen();
                }
            }

            else if (hit.collider.gameObject.CompareTag("On"))
            {
                hit.collider.gameObject.GetComponentInParent<OffScreenBehavior>().TurnOn();
            }

            else if (hit.collider.gameObject.CompareTag("Numba"))
            {
                hit.collider.gameObject.GetComponentInParent<OffScreenBehavior>().AddNumber(hit.collider.gameObject.name);
            }

            


        }
    }

    private void OnEnable()
    {
        click = PIAs.UI.Click;
        click.Enable();
        click.performed += Click;
    }


    private void OnDisable()
    {
        click.Disable();
    }
}
