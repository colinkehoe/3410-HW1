/*
 * PlayerController
 ? Controls player movement and game logic.
 * Authored by Colin Kehoe 
 * Last Edited 9/21
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;
    public float boostSpeed;
    public Text timerText;
    public Text winText;

    public bool gameOver;
    public bool gameWin;
    private float timer;
    private bool boostCooldown;
    private float boostMeter;

    public Button restartButton;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        //set up timers and text before game start
        timer = 60.0f;
        boostCooldown = true;
        boostMeter = 2.0f;
        timerText.text = "Time Remaining: " + timer.ToString(); 
        winText.text = "";

        //hide restart button at start of game
        restartButton.gameObject.SetActive(false);
    }

    void Update()
    {
        //do not update timers or text if game is lost
        if (gameOver) return;

        //count down timer
        timer -= Time.deltaTime;
        int seconds = (int)timer % 60;

        //if player has used boost, recharge
        if (!boostCooldown)
        {
            boostMeter += Time.deltaTime / 2;
        }
        //if boost reaches full charge, allow them to use it again.
        if (boostMeter >= 2.0f)
        {
            boostCooldown = true;
        }

        //update timer
        timerText.text = "Time Remaining: " + ((int)timer % 60).ToString();

        //win condition
        if (timer <= 0)
        {
            gameWin = true;
        }
        if (gameWin)
        {
            winText.color = Color.green;
            winText.text = "You win!";
            winText.gameObject.SetActive(true);
            restartButton.gameObject.SetActive(true);
        }
    }

    // FixedUpdate is in sync with the physics engine
    void FixedUpdate()
    {
        //grab player input
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        bool boost = Input.GetKey(KeyCode.Space);

        //update movement vector
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        rb.velocity = movement * speed;

        //allow use of boost if it's off cooldown
        if (boost && boostCooldown)
        {
            transform.position = new Vector3(transform.position.x + (moveHorizontal * boostSpeed), transform.position.y + (moveVertical * boostSpeed));
            boostMeter -= Time.deltaTime;

            if (boostMeter <= 0.0f)
            {
                boostCooldown = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //lose game if player touches a rock
        if(other.gameObject.CompareTag("PickUp"))
        {
            gameOver = true;
        }

        if (gameOver)
        {
            winText.color = Color.red;
            winText.text = "Game Over! \nTime Survived: " + (60 - ((int)timer % 60)).ToString();
            restartButton.gameObject.SetActive(true);
            winText.gameObject.SetActive(true);
        }
    }

    public void OnRestartButtonPress()
    {
        SceneManager.LoadScene("SampleScene"); //restart the game
    }
}
