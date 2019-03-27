using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class donut : MonoBehaviour {

    public int shape;
    public int flavor;

    SpriteRenderer renderer;
    bool active;
    bool isSettled;
    int potentialMatch;
    bool matched;
    float vel = 0f;
    float yVel = 0f;
    float distance = 0f;
    float yDistance = 0f;
    float dest;
    float yDest;
    float finalYDest;

    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        renderer.enabled = false;
        potentialMatch = 0;
        flavor = shape = 9;
        isSettled = true;
        active = false;
    }

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		if( vel != 0 )
        {
            float displacement = vel * 6 * Time.deltaTime;
            transform.Translate(displacement, 0f, 0f);
            distance += displacement;
            if( Math.Abs(distance) >= 1 )
            {
                Debug.Log("Distance: " + distance);
                Debug.Log("Should correct by: " + (distance - 1f));
                Vector3 newPos = new Vector3(dest, transform.position.y, 0f);
                transform.position = newPos;
                vel = 0f;
                distance = 0f;
                globals.isSwapping = false;
                cursorControl.swapLock = false;
            }
        }
        if (yVel != 0)
        {
            float displacement = yVel * 5f * Time.deltaTime;
            transform.Translate(0f, displacement, 0f);
            yDistance += displacement;
            //if (Math.Abs(yDistance) >= 1)
            if( transform.position.y <= yDest )
            {
                //Debug.Log("Distance: " + distance);
                //Debug.Log("Should correct by: " + (distance - 1f));
                Vector3 newPos = new Vector3(transform.position.x, yDest, 0f);
                transform.position = newPos;
                if (transform.position.y == finalYDest)
                {
                    yVel = 0f;
                    yDistance = 0f;
                    globals.isSwapping = false;
                }
                else
                {
                    yDest -= 1f;
                }
            }
        }
    }

    public void setSwap( int dir )
    {
        vel = dir;
        dest = transform.position.x + dir;
        globals.isSwapping = true;
        cursorControl.swapLock = true;
    }

    public void setFall()
    {
        if( yVel == 0 )
        {
            yDest = transform.position.y - 1f;
            finalYDest = yDest;
        }
        else
        {
            finalYDest -= 1f;
        }
        yVel = -1f;
        globals.isSwapping = true;
    }

    public void select()
    {
        anim.SetBool("isSelected", true);
    }

    public void deselect()
    {
        anim.SetBool("isSelected", false);
    }

    public void setDonutType( donut rhsDonut )
    {
        this.flavor = rhsDonut.flavor;
        this.shape = rhsDonut.shape;
        anim.SetBool("isMatch", false);
        renderer.enabled = true;
        //Debug.Log("Attempting to load donut: " + flavor.ToString() + " " + shape.ToString());
        renderer.sprite = globals.sprites[flavor, shape];
        active = true;
        matched = false;
    }

    public int getFlavor()
    {
        if (!isSettled)
            return 9;
        return flavor;
    }

    public int getShape()
    {
        if (!isSettled)
            return 9;
        return shape;
    }

    public bool canMove()
    {
        if (flavor > 4 || shape > 4)
            return false;
        return true;
    }

    public bool isMatch( donut rhsDonut )
    {
        if (!isSettled || matched)
            return false;
        if ( (rhsDonut.potentialMatch == 0 || rhsDonut.potentialMatch == 1) && (this.flavor == rhsDonut.flavor))
        {
            //Debug.Log("Flavor: " + flavor);
            rhsDonut.potentialMatch = 1;
            return true;
        }
        if ((rhsDonut.potentialMatch == 0 || rhsDonut.potentialMatch == 2) && (this.shape == rhsDonut.shape))
        {
            //Debug.Log("Shape: " + shape);
            rhsDonut.potentialMatch = 2;
            return true;
        }
        rhsDonut.potentialMatch = 0;
        return false;
    }

    public bool matchColor( donut rhsDonut )
    {
        if (flavor > 4)
            return false;
        if (!active || matched)
            return false;
        if (this.flavor == rhsDonut.getFlavor())
        {
            //Debug.Log("Flavor: " + flavor);
            return true;
        }
        return false;
    }

    public bool matchShape(donut rhsDonut)
    {
        if (shape > 4)
            return false;
        if (!active || matched)
            return false;
        if (this.shape == rhsDonut.getShape())
        {
            //Debug.Log("Shape: " + shape);
            return true;
        }
        return false;
    }

    public bool anyMatch( donut rhsDonut )
    {
        if (this.shape == rhsDonut.shape || this.flavor == rhsDonut.flavor)
            return true;
        return false;
    }

    public bool exactMatch( donut rhsDonut )
    {
        if (this.shape == rhsDonut.shape && this.flavor == rhsDonut.flavor)
            return true;
        return false;
    }

    public void setMatch()
    {
        globals.isPaused = true;
        //deselect();
        anim.SetBool("isMatch", true);
        matched = true;
    }

    public void finishMatch()
    {
        deactivate();
        globals.isPaused = false;
        //Destroy(gameObject);
    }

    public void drop()
    {
        isSettled = false;
    }

    public void settle()
    {
        isSettled = true;
    }

    public bool isFalling()
    {
        if( isSettled )
            return false;
        return true;
    }

    public bool isMatched()
    {
        if (matched)
            return true;
        return false;
    }

    public bool getActive()
    {
        //if (flavor > 4 || shape > 4)
            //return false;
        return active;
    }

    public void deactivate()
    {
        flavor = shape = 9;
        anim.SetBool("isMatch", false);
        active = false;
        renderer.enabled = false;
        //transform.GetChild(1).GetComponent<TextMesh>().text = "";
        //transform.GetChild(2).GetComponent<TextMesh>().text = "";
        //transform.GetChild(3).GetComponent<TextMesh>().text = "";
        //transform.GetChild(4).GetComponent<TextMesh>().text = "";
        //transform.GetChild(5).GetComponent<TextMesh>().text = "";
    }
}
