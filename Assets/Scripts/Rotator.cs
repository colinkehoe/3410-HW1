/*
 * Rotator
 ? Controls rock movement and logic. 
 * Authored by Colin Kehoe 
 * Last Edited 9/21
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    private Rigidbody2D rb;

    private float rotationTimer;
    private Vector2 movement;
    public float speed;
    private float horizontal;
    private float vertical;
    private PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rotationTimer = 0.0f;

        //set initial movement
        horizontal = Random.Range(-10.0f, 10.0f);
        vertical = Random.Range(-10.0f, 10.0f);
        movement = new Vector2(horizontal, vertical);

        //grab the player controller to observe win/lose bools
        player = GameObject.Find("Player").GetComponent<PlayerController>();

        rb.AddForce(movement * speed);
    }

    // Update is called once per frame
    void Update()
    {
        //rotate the rock
        rotationTimer += Time.deltaTime;
        int seconds = (int)rotationTimer % 60;
        transform.Rotate(new Vector3(0, 0, 45) * Time.deltaTime);

        //if player touches rock, disappear
        if (player.gameOver || player.gameWin)
        {
            gameObject.SetActive(false);
        }
    }
}
