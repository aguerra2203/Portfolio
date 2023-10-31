using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{

    [SerializeField] KillPlayer playerScript;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip hitClip;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = GameObject.Find("Lara").GetComponent<KillPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("HAZARD", gameObject);

        if (collision.tag == "Player")
        {
            playerScript.Kill();
            audioSource.clip = hitClip;
            audioSource.Play();
        }
    }

}
