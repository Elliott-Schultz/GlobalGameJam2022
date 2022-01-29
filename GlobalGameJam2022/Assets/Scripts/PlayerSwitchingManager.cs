using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwitchingManager : MonoBehaviour
{
    public GameObject[] players;
    public bool turn;

    private void Awake()
    {
        if (players.Length == 2)
        {
            players[0].GetComponent<PlayerController>().SetNotTurn();
            players[0].SetActive(false);
            players[1].GetComponent<PlayerController>().SetNotTurn();
            players[1].SetActive(false);

            players[0].SetActive(true);
            players[0].GetComponent<PlayerController>().SetTurn();
            
            

            turn = true;
        }
        else
        {
            Debug.Log("Need two players");
        }
    }

    public void Switch()
    {
        Vector2 changeVelocity;
        if(turn)
        {
            if (!players[1].GetComponent<PlayerController>().getIsDead())
            {
                changeVelocity = players[0].GetComponent<Rigidbody2D>().velocity;
                players[0].GetComponent<PlayerController>().SetNotTurn();
                players[0].SetActive(false);

                players[1].SetActive(true);
                players[1].GetComponent<PlayerController>().SetTurn();
                players[1].transform.position = players[0].transform.position;
                players[1].GetComponent<Rigidbody2D>().velocity = changeVelocity;
                if(players[1].GetComponent<PlayerController>().getFacingRight() != players[0].GetComponent<PlayerController>().getFacingRight())
                {
                    players[1].GetComponent<PlayerController>().Flip();
                }

                turn = false;
                Debug.Log("End Player 1 Turn");
            }
        }
        else
        {
            if (!players[0].GetComponent<PlayerController>().getIsDead())
            {
                changeVelocity = players[1].GetComponent<Rigidbody2D>().velocity;
                players[1].GetComponent<PlayerController>().SetNotTurn();
                players[1].SetActive(false);

                players[0].SetActive(true);
                players[0].GetComponent<PlayerController>().SetTurn();
                players[0].transform.position = players[1].transform.position;
                players[0].GetComponent<Rigidbody2D>().velocity = changeVelocity;
                if (players[0].GetComponent<PlayerController>().getFacingRight() != players[1].GetComponent<PlayerController>().getFacingRight())
                {
                    players[0].GetComponent<PlayerController>().Flip();
                }

                turn = true;
                Debug.Log("End Player 2 Turn");
            }
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
