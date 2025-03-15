using JetBrains.Annotations;
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
    public GameObject RefreshButton;

    [Header("Refresh Values")]
    public GameObject black_screen;
    public Transform black_screen_pos;
    public bool refreshing;

    [Header("Phone Event Values")]
    public float time_between_phone_events;

    [Header("Ad Values")]
    public GameObject Ad;

    [Header("Crash Values")]
    public GameObject CrashNoticeText;
    public GameObject HomeScreen;

    [Header("Text Fields")]
    public GameObject SubCountText;
    public GameObject TotalCountText;

    

    private int RefreshCount;
    private int newSubs;
    private int CurrentSubCount;
    private float t_sub;
    private float t_phone;
    private bool inPhoneEvent;
    private int index;
    // Start is called before the first frame update
    void Start()
    {
        CurrentSubCount = StartSubCount;
        RefreshCount = StartSubCount;
        newSubs = 0;
        t_sub = 0;
        t_phone = 0;
        inPhoneEvent = false;
        CrashNoticeText.SetActive(false);
        index = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        SubCountText.GetComponent<TextMeshPro>().text = "New Subs: " + newSubs.ToString();
        TotalCountText.GetComponent<TextMeshPro>().text = "Total Subs: " + RefreshCount.ToString();

        if(t_sub > time_between_increments && !inPhoneEvent)
        {
            CurrentSubCount += incramentSubs;
            t_sub = 0;
        }

        else if(!inPhoneEvent)
        {
            t_sub += Time.deltaTime;
        }

        if (t_phone > time_between_phone_events && !inPhoneEvent)
        {
            inPhoneEvent = true;
            System.Random rand = new System.Random();
            index = rand.Next(0, 2);
            ChoosePhoneEvent(index);
            t_phone = 0;
        }

        else if(!inPhoneEvent)
        {
            t_phone += Time.deltaTime;
        }

        
    }

    public void SetNewSubs()
    {
        StartCoroutine(RefreshRoutine());
        newSubs = CurrentSubCount - RefreshCount;
        RefreshCount = CurrentSubCount;
    }

    private void ChoosePhoneEvent(int index)
    {
        Debug.Log(index);
        switch (index)
        {
            case 0:
                AdEvent();
                break;
            case 1:
                CrashEvent();
                break;
            default:
                break;

        }
    }

    private void AdEvent()
    {
        Instantiate(Ad, black_screen_pos.position, black_screen_pos.rotation);
    }

    private void CrashEvent()
    {
        CrashNoticeText.SetActive(true);
        SubCountText.SetActive(false);
        TotalCountText.SetActive(false);
        RefreshButton.SetActive(false);

    }

    public void CleanUpCrashEvent()
    {
        CrashNoticeText.SetActive(false);
        SubCountText.SetActive(true);
        TotalCountText.SetActive(true);
        RefreshButton.SetActive(true);
    }

    public void GoHomeScreen()
    {
        if(index == 1 && inPhoneEvent)
        {
            Instantiate(HomeScreen, black_screen_pos.position, black_screen_pos.rotation);

        }
    }

    IEnumerator RefreshRoutine()
    {
        Instantiate(black_screen, black_screen_pos.position, black_screen_pos.rotation);
        refreshing = true;
        yield return new WaitForSeconds(black_screen.GetComponent<DestroySelf>().timeAlive);
        refreshing = false;
    }

    public void Set_InPhoneEvent(bool val)
    {
        inPhoneEvent = val;
    }

    public int GetPhoneGameIndex()
    {
        return index;
    }


}
