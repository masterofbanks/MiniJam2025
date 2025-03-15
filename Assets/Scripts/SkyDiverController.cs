using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkyDiverController : MonoBehaviour
{
    [Header("Diver Physics")]
    public float horizontal_speed;
    public LayerMask EnemyMask;

    [Header("Sky Diving Positions")]
    public Transform left_position;
    public Transform right_position;
    public Transform med_position;

    //components
    private Rigidbody2D rb;
    private InputAction move;
    public PlayerInputActions PIAs;

    //keyValues
    private Vector2 rawDirectionInputs;
    public Vector2 DirectionalInput;
    private Vector3 target;
    private float distanceToEnemy;
    private bool showNearMiss;
    private bool hit;
    public float hitDUration;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = med_position.position;
        distanceToEnemy = 10000;
        showNearMiss = true;
        hit = false;
    }

    private void Awake()
    {
        PIAs = new PlayerInputActions();
    }
    // Update is called once per frame
    void Update()
    {
        rawDirectionInputs = move.ReadValue<Vector2>();
        DirectionalInput = new Vector2(System.Math.Sign(rawDirectionInputs.x), System.Math.Sign(rawDirectionInputs.y));
        HandleMisses();
        ShootRay();
        HandleInput();
        var step = horizontal_speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, target, step);
    }

    private void HandleInput()
    {
        if(transform.position == target)
        {
            showNearMiss = !hit;
            if(target == med_position.position)
            {
                if(DirectionalInput.x == 1)
                {
                    
                    target = right_position.position;
                }

                else if(DirectionalInput.x == -1)
                {
                    

                    target = left_position.position;
                }
            }

            else if ((target == left_position.position && DirectionalInput.x == 1) || (target == right_position.position && DirectionalInput.x == -1))
            {
                target = med_position.position;



            }

        }
    }

    private void HandleMisses()
    {
        if (DirectionalInput.x != 0 && distanceToEnemy < 10f && transform.position != target && showNearMiss)
        {
            showNearMiss = false;
            Debug.Log("near Miss!!");
            Debug.Log(distanceToEnemy);
        }
    }

    private void ShootRay()
    {
        

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, EnemyMask);
        if(hit.collider != null)
        {
            distanceToEnemy = hit.distance;
        }

        else
        {
            distanceToEnemy = 10000;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Player Hit!!");
            StartCoroutine(HitRoutine());
        }
    }

    IEnumerator HitRoutine()
    {
        hit = true;
        yield return new WaitForSeconds(hitDUration);
        hit = false;
    }

   

    private void OnEnable()
    {
        move = PIAs.Player.Move;
        move.Enable();

        
    }

    private void OnDisable()
    {
        move.Disable();
    }
}
