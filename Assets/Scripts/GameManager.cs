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

    [Header("Enemy Stuff")]
    public GameObject enemy;
    public GameObject warning;
    public float enemySpeed;
    public float time_between_enemy_spawns;
    public Transform[] e_spawn_positions;
    public Transform[] w_spawn_positions;

    [Header("Diver Side Valuues")]
    public float starting_altitude;
    public float current_altitude;
    public float nearMissCount;
    public float falling_velo;


    [Header("Text Fields")]
    public GameObject SubCountText;
    public GameObject TotalCountText;
    public GameObject Altitude_text;
    public GameObject NearMissCount_text;

    

    private int RefreshCount;
    private int newSubs;
    private int CurrentSubCount;
    private float t_sub;
    private float t_phone;
    private float t_enemy;
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
        current_altitude = starting_altitude;
    }

    // Update is called once per frame
    void Update()
    {
        
        SubCountText.GetComponent<TextMeshPro>().text = "New Subs: " + newSubs.ToString();
        TotalCountText.GetComponent<TextMeshPro>().text = "Total Subs: " + RefreshCount.ToString();
        Altitude_text.GetComponent<TextMeshPro>().text = "Altitude: " + current_altitude.ToString();
        NearMissCount_text.GetComponent<TextMeshPro>().text = "Near Miss Count: " + nearMissCount.ToString();

        CalculateAltitude();

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

        if(t_enemy > time_between_enemy_spawns)
        {
            SpawnEnemy();
            t_enemy = 0;
        }

        else
        {
            t_enemy += Time.deltaTime;
        }

        
    }

    private void CalculateAltitude()
    {
        
        
        current_altitude = (int)(current_altitude - falling_velo * (float)Time.deltaTime );

    }
    private void SpawnEnemy()
    {
        System.Random rand = new System.Random();
        int randNum = rand.Next(0, 3);
        GameObject e_object = Instantiate(enemy, e_spawn_positions[randNum].position, e_spawn_positions[randNum].rotation);
        GameObject w_object = Instantiate(warning, w_spawn_positions[randNum].position, w_spawn_positions[randNum].rotation);
        e_object.GetComponent<Rigidbody2D>().velocity = new Vector2(0, enemySpeed);
    }


    public void SetNewSubs()
    {
        StartCoroutine(RefreshRoutine());
        newSubs = CurrentSubCount - RefreshCount;
        RefreshCount = CurrentSubCount;
    }

    private void ChoosePhoneEvent(int index)
    {
        //Debug.Log(index);
        switch (index)
        {
            case 0:
                AdEvent();
                break;
            case 1:
                CrashEvent();
                break;
            default:
                AdEvent();
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
