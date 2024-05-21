using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool PlayerDead = false;
    public Player Player;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("doomguy2").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerDead)
        {
            Player.enabled = false;
        }
    }
}
