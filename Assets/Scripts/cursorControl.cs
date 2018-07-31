using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cursorControl : MonoBehaviour {

    const int unitSize = 1;

    int posX, posY;
    bool paused;

    public Grid_Map grid;

	// Use this for initialization
	void Start () {
        posX = posY = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if( Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (posX > 0)
            {
                transform.Translate(-unitSize, 0f, 0f);
                posX--;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (posX < 5)
            {
                transform.Translate(unitSize, 0f, 0f);
                posX++;
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (posY < 8)
            {
                transform.Translate(0f, unitSize, 0f);
                posY++;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (posY > 0)
            {
                transform.Translate(0f, -unitSize, 0f);
                posY--;
            }
            
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            grid.swap(posX, posY);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            grid.addRow();
        }

    }
}
