using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkyDiverController : MonoBehaviour
{
    [Header("Diver Physics")]
    public float horizontal_speed;
    public LayerMask EnemyMask;
    public GameObject[] hearts;
    public int health;
    private int starting_Health;

    [Header("Near Miss Values")]
    public float nearMiss;
    public float superNearMiss;
    public float ultraNearMiss;
    public GameObject NearMissText;
    

    [Header("Sky Diving Positions")]
    public Transform left_position;
    public Transform right_position;
    public Transform med_position;

    [Header("Sound Effects")]
    public GameObject impact;
    public GameObject dash;


    //components
    private Rigidbody2D rb;
    private InputAction move;
    public PlayerInputActions PIAs;
    private Animator anime;

    //keyValues
    private Vector2 rawDirectionInputs;
    public Vector2 DirectionalInput;
    private Vector3 target;
    private float distanceToEnemy;
    private bool showNearMiss;
    private bool hit;
    private bool facingRight;
    private float hitDuration = 0.2f;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = med_position.position;
        distanceToEnemy = 10000;
        showNearMiss = true;
        hit = false;
        starting_Health = hearts.Length * 2;
        health = starting_Health;
        facingRight = true;
        anime = GetComponent<Animator>();
    }

    private void Awake()
    {
        PIAs = new PlayerInputActions();
    }
    // Update is called once per frame
    void Update()
    {
        anime.SetBool("hit", hit);
        rawDirectionInputs = move.ReadValue<Vector2>();
        DirectionalInput = new Vector2(System.Math.Sign(rawDirectionInputs.x), System.Math.Sign(rawDirectionInputs.y));
        HandleMisses();
        ShootRay();
        HandleInput();
        Flip();
        HandleHealth();
        var step = horizontal_speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, target, step);
    }

    private void Flip()
    {
        if (facingRight && System.Math.Sign(DirectionalInput.x) == -1.0f)
        {
            transform.localScale = new Vector3(-1 * transform.localScale.x, 1, 1);
            facingRight = !facingRight;
        }

        else if (!facingRight && System.Math.Sign(DirectionalInput.x) == 1.0f)
        {
            transform.localScale = new Vector3(-1 * transform.localScale.x, 1, 1);
            facingRight = !facingRight;
        }
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
                    Instantiate(dash, transform.position, transform.rotation);
                    target = right_position.position;
                }

                else if(DirectionalInput.x == -1)
                {
                    Instantiate(dash, transform.position, transform.rotation);

                    target = left_position.position;
                }
            }

            else if ((target == left_position.position && DirectionalInput.x == 1) || (target == right_position.position && DirectionalInput.x == -1))
            {
                Instantiate(dash, transform.position, transform.rotation);
                target = med_position.position;



            }

        }
    }

    private void HandleHealth()
    {
        int i = health - 1;
        int s = i % 2;
        int i_heart = i / 2;

        GameObject target_heart = hearts[i_heart];
        if(health != 0)
        {
            if (i_heart < starting_Health / 2 - 1)
            {
                hearts[i_heart + 1].GetComponent<HeartBehavior>().state = HeartBehavior.State.empty;

            }

            if (s == 0)
            {
                hearts[i_heart].GetComponent<HeartBehavior>().state = HeartBehavior.State.half;
            }
        }

        else
        {
            hearts[0].GetComponent<HeartBehavior>().state = HeartBehavior.State.empty;
        }
    }

    private void HandleMisses()
    {
        if (DirectionalInput.x != 0 && distanceToEnemy < nearMiss && transform.position != target && showNearMiss)
        {
            showNearMiss = false;
            Debug.Log(distanceToEnemy);
            NearMissText.GetComponent<NMNotifierBehavior>().RestartTimeAlive();

            if (distanceToEnemy > superNearMiss)
            {
                Debug.Log("near Miss!!");
                NearMissText.GetComponent<NMNotifierBehavior>().text = "Near Miss!";
                GameObject.FindWithTag("GameManager").GetComponent<GameManager>().nearMissCount++;
                GameObject.FindWithTag("GameManager").GetComponent<GameManager>().ChangeComboNumber("Near Miss!");

            }

            else if(distanceToEnemy > ultraNearMiss)
            {
                Debug.Log("Super Near Miss!!");
                NearMissText.GetComponent<NMNotifierBehavior>().text = "Super Near Miss!";
                GameObject.FindWithTag("GameManager").GetComponent<GameManager>().nearMissCount+=3;
                GameObject.FindWithTag("GameManager").GetComponent<GameManager>().ChangeComboNumber("Super Near Miss!");
            }

            else if(distanceToEnemy > 0.2f)
            {
                Debug.Log("Ultra Near Miss!!");
                NearMissText.GetComponent<NMNotifierBehavior>().text = "Ultra Near Miss!";
                GameObject.FindWithTag("GameManager").GetComponent<GameManager>().nearMissCount += 5;
                GameObject.FindWithTag("GameManager").GetComponent<GameManager>().ChangeComboNumber("Ultra Near Miss!");
            }


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
            Destroy(collision.gameObject);
            Instantiate(impact, transform.position, transform.rotation);
            StartCoroutine(HitRoutine());
        }
    }

    IEnumerator HitRoutine()
    {
        hit = true;
        GameObject.FindWithTag("GameManager").GetComponent<GameManager>().nearMissCount = 0;
        if (health > 0)
        {
            health--;

        }
        yield return new WaitForSeconds(hitDuration);
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
