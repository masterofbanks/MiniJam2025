using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ad_Behavior : MonoBehaviour
{
    public GameObject X_Object;
    public Transform[] spawnPositions;
    public int numClicks;

    private int s_index;
    private Transform spawnPos;
    private int num_Xs;
    private GameObject X;
    // Start is called before the first frame update
    void Start()
    {
        System.Random rand = new System.Random();
        s_index = rand.Next(spawnPositions.Length);
        spawnPos = spawnPositions[s_index];
        X = Instantiate(X_Object, spawnPos.position, spawnPos.rotation, transform);
        num_Xs = 0;
    }

    private void Update()
    {
        if (num_Xs > numClicks)
        {
            GameObject.FindWithTag("GameManager").GetComponent<GameManager>().Set_InPhoneEvent(false);

            Destroy(gameObject);
        }
    }

    public void Clicked_X()
    {
        if (s_index == spawnPositions.Length - 1)
        {
            s_index = 0;
        }

        else
        {
            s_index++;
        }
        num_Xs++;
        X.transform.position = spawnPositions[s_index].position;

            
    }

    
}
