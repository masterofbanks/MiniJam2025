using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyStuff : MonoBehaviour
{
    public GameObject bgc;
    public GameObject explostion;
    public GameObject explostion_sfx;
    public Transform bgc_spawnPos;
    public Leaderboard leaderboard;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }

        else if (collision.gameObject.CompareTag("BG"))
        {
            Instantiate(bgc, bgc_spawnPos.position, bgc_spawnPos.rotation);
        }

        else if (collision.gameObject.CompareTag("Numba"))
        {
            Vector3 pos = collision.transform.position;
            Destroy(collision.gameObject);
            StartCoroutine(DeathRoutine(pos));
            
        }
    }

    IEnumerator DeathRoutine(Vector3 t)
    {
        Instantiate(explostion, t, Quaternion.identity);
        Instantiate(explostion_sfx, t, Quaternion.identity);
        GameObject.FindWithTag("GameManager").GetComponent<GameManager>().StartEndingUI();
        yield return leaderboard.SubmitScoreRoutine(PlayerPrefs.GetInt("high_score"));
        
    }
}
