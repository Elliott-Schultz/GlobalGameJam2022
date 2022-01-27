using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwitchingManager : MonoBehaviour
{
    public GameObject[] players;
    private bool turn;

    private void Awake()
    {
        if (players.Length == 2)
        {
            players[0].SetActive(true);
            players[1].SetActive(false);

            turn = true;
        }
        else
        {
            Debug.Log("Need two players");
        }
    }

    public void Switch()
    {
        if(turn)
        {
            players[1].SetActive(true);
            players[1].transform.position = players[0].transform.position;
            //players[1].GetComponent<Rigidbody2D>().velocity = players[0].GetComponent<Rigidbody2D>().velocity;
            //players[1].GetComponent<PlayerController>().setInputX(players[0].GetComponent<PlayerController>().getInputX());
            //players[1].GetComponent<PlayerController>().setInputX(0);
            players[0].SetActive(false);
            turn = false;
        }
        else
        {
            players[0].SetActive(true);
            players[0].transform.position = players[1].transform.position;
            //players[0].GetComponent<Rigidbody2D>().velocity = players[1].GetComponent<Rigidbody2D>().velocity;
            //players[0].GetComponent<PlayerController>().setInputX(players[1].GetComponent<PlayerController>().getInputX());
            //players[0].GetComponent<PlayerController>().setInputX(0);
            players[1].SetActive(false);
            turn = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
