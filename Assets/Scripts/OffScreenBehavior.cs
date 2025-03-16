using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OffScreenBehavior : MonoBehaviour
{
    public GameObject PasscodeText;
    public GameObject InputText;
    public GameObject numberPad;

    public int Passcode;
    public string input;
    
    // Start is called before the first frame update
    void Start()
    {
        PasscodeText.SetActive(false);
        InputText.SetActive(false);
        numberPad.SetActive(false);
        System.Random random = new System.Random();
        Passcode = random.Next(1000, 10000);
        PasscodeText.GetComponent<TextMeshPro>().text = "Passcode: " + Passcode.ToString();
        InputText.GetComponent<TextMeshPro>().text = "Input: " + input;
    }

    // Update is called once per frame
    void Update()
    {
        InputText.GetComponent<TextMeshPro>().text = "Input: " + input;
    }

    public void AddNumber(string name)
    {
        input += name;

        if(input == Passcode.ToString())
        {
            //DO something cool
            CleanUp();
        }

        else if(input.Length == 4)
        {
            Debug.Log("Incorrect Code!");
            input = "";
        }

    }

    public void TurnOn()
    {
        PasscodeText.SetActive(true);
        InputText.SetActive(true);
        numberPad.SetActive(true);
    }

    public void CleanUp()
    {
        GameObject.FindWithTag("GameManager").GetComponent<GameManager>().Set_InPhoneEvent(false);
        GameObject.FindWithTag("GameManager").GetComponent<GameManager>().CleanUpCrashEvent();
        Destroy(gameObject);
    }
}
