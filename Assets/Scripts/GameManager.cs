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
    public float comboNumber;
    public float decreaseFactor;
    public GameObject RefreshButton;

    [Header("Refresh Values")]
    public GameObject black_screen;
    public Transform black_screen_pos;
    public bool refreshing;

    [Header("Phone Event Values")]
    public float time_between_phone_events;

    [Header("Ad Values")]
    public GameObject Ad;

    [Header("Off Values")]
    public GameObject Off;

    [Header("Crash Values")]
    public GameObject CrashNoticeText;
    public GameObject DisconnectNoticeText;
    public GameObject HomeScreen;

    [Header("Enemy Stuff")]
    public GameObject enemy;
    public GameObject warning;
    public float enemySpeed;
    public float time_between_enemy_spawns;
    public Transform[] e_spawn_positions;
    public Transform[] w_spawn_positions;

    [Header("Diver Side Valuues")]
    public SkyDiverController diveScript;
    public float starting_altitude;
    public float current_altitude;
    public float nearMissCount;
    public float falling_velo;
    public int missesToTP;
    public Transform background_target;
    public GameObject deathCam;
    public GameObject gameCam;
    public GameObject deathPlayer;


    [Header("Text Fields")]
    public GameObject SubCountText;
    public GameObject TotalCountText;
    public GameObject Altitude_text;
    public GameObject NearMissCount_text;
    public GameObject ComboText;
    public GameObject TPButton;



    public int RefreshCount;
    private int newSubs;
    private int CurrentSubCount;
    private float t_sub;
    private float t_phone;
    private float t_enemy;
    private bool inPhoneEvent;
    private int index;
    private bool playerDead;
    // Start is called before the first frame update
    void Start()
    {
        diveScript = GameObject.FindWithTag("Player").GetComponent<SkyDiverController>();
        CurrentSubCount = StartSubCount;
        RefreshCount = StartSubCount;
        newSubs = 0;
        t_sub = 0;
        t_phone = 0;
        inPhoneEvent = false;
        CrashNoticeText.SetActive(false);
        DisconnectNoticeText.SetActive(false);
        index = 0;
        current_altitude = starting_altitude;
        comboNumber = 1;
        TPButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        SubCountText.GetComponent<TextMeshPro>().text = "New Subs: " + newSubs.ToString();
        TotalCountText.GetComponent<TextMeshPro>().text = "Total Subs: " + RefreshCount.ToString();
        Altitude_text.GetComponent<TextMeshPro>().text = "Altitude: " + current_altitude.ToString();
        NearMissCount_text.GetComponent<TextMeshPro>().text = "Near Misses: " + nearMissCount.ToString();
        int test = (int)(comboNumber);
        ComboText.GetComponent<TextMeshPro>().text = "Combo: " + test.ToString();


        if (!playerDead)
        {
            Death();
            CalculateAltitude();

            if (t_sub > time_between_increments && !inPhoneEvent)
            {
                CurrentSubCount += (int)(incramentSubs * comboNumber);
                t_sub = 0;
            }

            else if (!inPhoneEvent)
            {
                t_sub += Time.deltaTime;
            }

            if (t_phone > time_between_phone_events && !inPhoneEvent)
            {
                inPhoneEvent = true;
                System.Random rand = new System.Random();
                index = rand.Next(0, 4);
                ChoosePhoneEvent(index);
                t_phone = 0;
            }

            else if (!inPhoneEvent)
            {
                t_phone += Time.deltaTime;
            }

            if (t_enemy > time_between_enemy_spawns)
            {
                SpawnEnemy();
                t_enemy = 0;
            }

            else
            {
                t_enemy += Time.deltaTime;
            }

            if (comboNumber > 2)
            {
                comboNumber = (comboNumber - decreaseFactor * Time.deltaTime);
            }

            if (time_between_enemy_spawns > 0.85f)
            {
                time_between_enemy_spawns -= 0.0025f * Time.deltaTime;

            }

            if (nearMissCount > missesToTP && !inPhoneEvent)
            {
                TPButton.SetActive(true);
            }

            else
            {
                TPButton.SetActive(false);
            }

            if (falling_velo < 100)
                falling_velo += 0.0002f * Time.deltaTime;
        }
        

    }

    private void Death()
    {
        if(current_altitude < 100 || diveScript.health == 0)
        {
            playerDead = true;
            gameCam.SetActive(false);
            deathCam.SetActive(true);
            deathPlayer.GetComponent<Rigidbody2D>().gravityScale = 5;
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
            case 2:
                DisconnectEvent();
                break;
            case 3:
                OffEvent();
                break;
            default:
                AdEvent();
                break;

        }
    }

    public void ChangeComboNumber(string type)
    {
        
        switch (type) {
            case "Near Miss!":
                comboNumber += 2;
                decreaseFactor = 0.5f;
                break;
            case "Super Near Miss!":
                comboNumber += 5;
                decreaseFactor = 0.9f;
                break;

            case "Ultra Near Miss!":
                comboNumber += 10;
                decreaseFactor = 2;
                break;

            default:
                comboNumber *= 1;
                break;
        
        }


    }

    private void AdEvent()
    {
        Instantiate(Ad, black_screen_pos.position, black_screen_pos.rotation);
        RefreshButton.SetActive(false);
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
        if((index == 1 || index == 2) && inPhoneEvent)
        {
            Instantiate(HomeScreen, black_screen_pos.position, black_screen_pos.rotation);

        }
    }

    public void DisconnectEvent()
    {
        DisconnectNoticeText.SetActive(true);
        SubCountText.SetActive(false);
        TotalCountText.SetActive(false);
        RefreshButton.SetActive(false);
    }

    public void CleanUpDisconnectEvent()
    {
        DisconnectNoticeText.SetActive(false);
        SubCountText.SetActive(true);
        TotalCountText.SetActive(true);
        RefreshButton.SetActive(true);
    }

    public void OffEvent()
    {
        Instantiate(Off, black_screen_pos.position, black_screen_pos.rotation);
        RefreshButton.SetActive(false);
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

    public void RestartAltitude()
    {
        if (nearMissCount > missesToTP)
        {
            current_altitude = starting_altitude;
            nearMissCount -= missesToTP;
        }
            
    }
}
