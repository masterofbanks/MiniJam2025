using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Sub Values")]
    public int StartSubCount;
    public float time_between_increments;
    public int incramentSubs;


    [Header("Text Fields")]
    public GameObject SubCountText;

    private int RefreshCount;
    private int newSubs;
    private int CurrentSubCount;
    private float t;
    // Start is called before the first frame update
    void Start()
    {
        CurrentSubCount = StartSubCount;
        RefreshCount = StartSubCount;
        newSubs = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        SubCountText.GetComponent<TextMeshPro>().text = newSubs.ToString();

        if(t > time_between_increments)
        {
            CurrentSubCount += incramentSubs;
            t = 0;
        }

        else
        {
            t += Time.deltaTime;
        }
    }

    public void SetNewSubs()
    {
        newSubs = CurrentSubCount - RefreshCount;
        RefreshCount = CurrentSubCount;
    }
}
