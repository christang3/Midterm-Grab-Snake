using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGame : MonoBehaviour
{
    GameObject Snake;
    bool GameStart;
    bool Vert;
    bool Direction;

    float speed = 5;
    float playerVelocity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Horizontal"))
        {
            GameStart = true;
            Debug.Log(Input.mousePosition);
            Vert = false;
            Direction = Input.GetButtonDown("Horizontal");
        } else if (Input.GetButtonDown("Vertical"))
        {
            GameStart = true;
            Vert = true;
            Direction = Input.GetButtonDown("Vertical");
        }

        if (GameStart)
        {
            //playerVelocity += gravityValue * Time.deltaTime;
            //controller.Move(playerVelocity * Time.deltaTime);
        }
    }
}
