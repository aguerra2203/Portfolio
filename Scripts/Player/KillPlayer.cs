using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    [SerializeField] HUD hud;

    private int numHearts = 3;

    // Start is called before the first frame update
    void Start()
    {
        hud = GameObject.Find("HUD").GetComponent<HUD>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Kill()
    {
 
        if(numHearts > 0)
        {
            numHearts--;
            hud.RemoveHeart();
            if (numHearts == 0)
            {
                GameManager._instance.LoseGame();
            }
        } 
        Debug.Log("Num Hearts: " + numHearts);
    }
}
