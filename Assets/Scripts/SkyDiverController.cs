using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkyDiverController : MonoBehaviour
{
    [Header("Diver Physics")]
    public float horizontal_speed;




    //components
    private Rigidbody2D rb;
    private InputAction move;
    public PlayerInputActions PIAs;

    //keyValues
    private Vector2 rawDirectionInputs;
    private Vector2 DirectionalInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
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
        rb.velocity = new Vector2(DirectionalInput.x * horizontal_speed, 0);
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
