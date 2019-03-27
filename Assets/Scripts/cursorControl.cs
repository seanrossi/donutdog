using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cursorControl : MonoBehaviour {

    const int unitSize = 1;

    int posX, posY;
    bool paused;

    public Grid_Map grid;
    public bool isMobile;

    float vel = 0f;
    float yVel = 0f;
    float distance = 0f;
    float yDistance = 0f;
    float dest;
    float yDest;
    public static bool swapLock = false;
    bool swapQueue = false;

    // Use this for initialization
    void Start () {
        posX = posY = 0;
	}
	
	// Update is called once per frame
	void Update () {

        if (vel != 0)
        {
            float displacement = vel * 6 * Time.deltaTime;
            transform.Translate(displacement, 0f, 0f);
            distance += displacement;
            if (Math.Abs(distance) >= 1)
            {
                Debug.Log("Distance: " + distance);
                Debug.Log("Should correct by: " + (distance - 1f));
                Vector3 newPos = new Vector3(dest, transform.position.y, 0f);
                transform.position = newPos;
                posX += (int)vel;
                vel = 0f;
                distance = 0f;
                if( swapQueue )
                {
                    grid.swap(posX, posY);
                    swapQueue = false;
                }
            }
        }
        if (yVel != 0)
        {
            float displacement = yVel * 5f * Time.deltaTime;
            transform.Translate(0f, displacement, 0f);
            yDistance += displacement;
            if (Math.Abs(yDistance) >= 1)
            {
                //Debug.Log("Distance: " + distance);
                //Debug.Log("Should correct by: " + (distance - 1f));
                Vector3 newPos = new Vector3(transform.position.x, yDest, 0f);
                transform.position = newPos;
                posY += (int)yVel;
                yVel = 0f;
                yDistance = 0f;
                if (swapQueue)
                {
                    grid.swap(posX, posY);
                    swapQueue = false;
                }
            }
        }

        if ( Input.GetKey(KeyCode.LeftArrow))
        {
            if (posX > 0)
            {
                //transform.Translate(-unitSize, 0f, 0f);
                if (vel == 0)
                {
                    vel = -1;
                    dest = transform.position.x + vel;
                }
                //posX--;
            }
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (posX < 5)
            {
                if (vel == 0)
                {
                    //transform.Translate(unitSize, 0f, 0f);
                    vel = 1;
                    dest = transform.position.x + vel;
                }
                //posX++;
            }
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (posY < 8)
            {
                if (yVel == 0)
                {
                    yVel = 1;
                    yDest = transform.position.y + yVel;
                }
                //transform.Translate(0f, unitSize, 0f);
                //posY++;
            }
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (posY > 0)
            {
                if (yVel == 0)
                {
                    yVel = -1;
                    yDest = transform.position.y + yVel;
                }
                //transform.Translate(0f, -unitSize, 0f);
                //posY--;
            }
            
        }
        if (globals.isPaused)
            return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!swapLock)
            {
                if (vel == 0 && yVel == 0)
                    grid.swap(posX, posY);
                else
                    swapQueue = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            grid.addRow();
        }
        if( Input.GetKeyDown(KeyCode.Backspace))
        {
            SceneManager.LoadScene("month_01_menu");
        }
    }
}
