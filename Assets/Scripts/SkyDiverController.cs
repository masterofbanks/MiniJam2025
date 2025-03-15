using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkyDiverController : MonoBehaviour
{
    [Header("Diver Physics")]
    public float horizontal_speed;

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
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = med_position.position;
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
        HandleInput();
        var step = horizontal_speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, target, step);
    }

    private void HandleInput()
    {
        if(transform.position == target)
        {
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
