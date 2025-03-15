using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBehavior : MonoBehaviour
{
    public enum State
    {
        full,
        half,
        empty
    }

    public State state;
    private Animator anime;
    
    // Start is called before the first frame update
    void Start()
    {
        anime = GetComponent<Animator>();
        state = State.full;
    }

    // Update is called once per frame
    void Update()
    {
        anime.SetInteger("state", (int)state);
    }
}
