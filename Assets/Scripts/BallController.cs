﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BallController : MonoBehaviour {
    private GameObject ball;
    private Rigidbody rb;
    private Vector3 force;
    private GameObject player;
    private Renderer mr;
    public Text txtScore, txtLives;

    public AudioSource[] sounds;
    public AudioSource blockHit;

    private float ballScale, playerForce, playerRotation, blockForce, blockRotation;
    private Vector3 oppositeForce;

    void Start () {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                ballScale = 4f;
                playerForce = 3f;
                playerRotation = 1f;
                blockForce = 50f;
                blockRotation = 10f;
                break;
            case 1:

                break;
            case 2:
                break;
            default:
                break;
        }
        ball = this.gameObject;
        ball.transform.localScale = new Vector3(ballScale * Random.Range(.5f, 1f), ballScale * Random.Range(.5f, 1f), ballScale * Random.Range(.5f, 1f));
        //ball.transform.localScale *= ballScale;

        rb = ball.GetComponent<Rigidbody>();
        mr = ball.GetComponent<Renderer>();

        player = GameObject.Find("Player").gameObject;

        sounds = GetComponents<AudioSource>();
        blockHit = null;
        //blockHit = sounds[0];
    }

    void Update () {
        //if (transform.position.z != 0f) transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
        if (transform.position.z > Mathf.Abs(.5f)) transform.position = new Vector3(transform.position.x, transform.position.y, 0f);

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("BC with " + other.tag + " @ " + gameObject.transform.position);

        if (other.tag.Equals("block"))
        {
            //blockHit.Play();
            //mr.material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            //mr.material.color = other.GetComponent<Renderer>().material.color * .5f;
            //mr.material.color -= other.GetComponent<Renderer>().material.color;
            mr.material.color *= other.GetComponent<Renderer>().material.color;

            int points = (int) other.GetComponent<Rigidbody>().mass;

            force = new Vector3(Random.Range(-1 * blockForce, blockForce), 0f, 0) * 2f;
            //force = new Vector3(blockForce, 0f, 0) * 2f; //need to find and set magnitude from edge

            rb.AddForce(force, ForceMode.Acceleration);

            rb.velocity *= .9f;
            //rb.angularVelocity = rb.angularVelocity * Random.Range(-1 * blockRotation, blockRotation);

            GameController.score += points;
            txtScore.text = "Score: " + GameController.score;

            Destroy(other.gameObject);
        }

        if (other.tag.Equals("player"))
        {
            oppositeForce = player.GetComponent<Rigidbody>().velocity;
            Debug.Log("player velocity: " + oppositeForce + "  ball velocity: " + rb.velocity);

            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            force = new Vector3(0f, 10f, 0f) * playerForce;
            rb.AddForce(force, ForceMode.Impulse);

            GameController.score++;
            txtScore.text = "Score: " + GameController.score;
        }

        if (other.tag.Equals("edge"))
        {
            GameController.score += -1;
            txtScore.text = "Score: " + GameController.score;
        }

        if (other.tag.Equals("top"))
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (other.tag.Equals("bottom"))
        {
            mr.material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

            rb.transform.localScale *= .5f;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.useGravity = false;
            //rb.isKinematic = true;
        }
    }

    private void OnBecameInvisible()
    {
        GameController.lives--;
        Destroy(gameObject);
    }
}
