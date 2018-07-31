using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class donut : MonoBehaviour {

    public int shape;
    public int flavor;

    bool isSettled;
    int potentialMatch;
    bool matched;

    Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        potentialMatch = 0;
        isSettled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
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

    public bool isMatch( donut rhsDonut )
    {
        if (!isSettled || matched)
            return false;
        if ( (rhsDonut.potentialMatch == 0 || rhsDonut.potentialMatch == 1) && (this.flavor == rhsDonut.flavor))
        {
            Debug.Log("Flavor: " + flavor);
            rhsDonut.potentialMatch = 1;
            return true;
        }
        if ((rhsDonut.potentialMatch == 0 || rhsDonut.potentialMatch == 2) && (this.shape == rhsDonut.shape))
        {
            Debug.Log("Shape: " + shape);
            rhsDonut.potentialMatch = 2;
            return true;
        }
        rhsDonut.potentialMatch = 0;
        return false;
    }

    public bool matchColor( donut rhsDonut )
    {
        if (!isSettled || matched)
            return false;
        if (this.flavor == rhsDonut.getFlavor())
        {
            Debug.Log("Flavor: " + flavor);
            return true;
        }
        return false;
    }

    public bool matchShape(donut rhsDonut)
    {
        if (!isSettled || matched)
            return false;
        if (this.shape == rhsDonut.getShape())
        {
            Debug.Log("Shape: " + shape);
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
        anim.SetBool("isMatch", true);
        matched = true;
    }

    public void finishMatch()
    {
        globals.isPaused = false;
        Destroy(gameObject);
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
}
