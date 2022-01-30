using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSwitchingManager : MonoBehaviour
{
    public GameObject[] players;
    public bool turn;
    public CameraFollow cameraFollow;

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
            cameraFollow.setTarget(players[0].transform);
            

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
                players[1].GetComponent<PlayerController>().setHitGroundAfterHit(players[0].GetComponent<PlayerController>().getHitGroundAfterHit());
                players[1].GetComponent<PlayerController>().SetTurn();
                players[1].transform.position = players[0].transform.position;
                players[1].GetComponent<Rigidbody2D>().velocity = changeVelocity;
                if(players[1].GetComponent<PlayerController>().getFacingRight() != players[0].GetComponent<PlayerController>().getFacingRight())
                {
                    players[1].GetComponent<PlayerController>().Flip();
                }

                cameraFollow.setTarget(players[1].transform);

                turn = false;
                //Debug.Log("End Player 1 Turn");
            }
            else if(players[0].GetComponent<PlayerController>().getIsDead())
            {
                SceneManager.LoadScene(0);
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
                players[0].GetComponent<PlayerController>().setHitGroundAfterHit(players[1].GetComponent<PlayerController>().getHitGroundAfterHit());
                players[0].GetComponent<PlayerController>().SetTurn();
                players[0].transform.position = players[1].transform.position;
                players[0].GetComponent<Rigidbody2D>().velocity = changeVelocity;
                if (players[0].GetComponent<PlayerController>().getFacingRight() != players[1].GetComponent<PlayerController>().getFacingRight())
                {
                    players[0].GetComponent<PlayerController>().Flip();
                }

                cameraFollow.setTarget(players[0].transform);

                turn = true;
                //Debug.Log("End Player 2 Turn");
            }
            else if (players[1].GetComponent<PlayerController>().getIsDead())
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}
