using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class NMNotifierBehavior : MonoBehaviour
{
    private TextMeshPro textComponent;
    public string text;
    public float maxTimeAlive;
    private float t;

    
    // Start is called before the first frame update
    void Start()
    {
        textComponent = GetComponent<TextMeshPro>();
        textComponent.enabled = false;
        t = maxTimeAlive;
    }

    // Update is called once per frame
    void Update()
    {
        textComponent.text = text;
        if(t < maxTimeAlive)
        {
            t += Time.deltaTime;
            textComponent.enabled = true;
        }

        else
        {
            textComponent.enabled = false;
        }
    }

    public void RestartTimeAlive()
    {
        t = 0;
    }
}
