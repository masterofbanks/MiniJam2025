using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyStuff : MonoBehaviour
{
    public GameObject bgc;
    public GameObject explostion;
    public GameObject explostion_sfx;
    public Transform bgc_spawnPos;
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
            Instantiate(explostion, collision.gameObject.transform.position, collision.gameObject.transform.rotation);
            Instantiate(explostion_sfx, collision.gameObject.transform.position, collision.gameObject.transform.rotation);
            GameObject.FindWithTag("GameManager").GetComponent<GameManager>().StartEndingUI();
            Destroy(collision.gameObject);
        }
    }
}
