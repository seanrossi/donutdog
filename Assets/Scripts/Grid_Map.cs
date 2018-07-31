using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grid_Map : MonoBehaviour {

    const int width = 7;
    const int height = 9;
    const float fallDelay = 0.3f;
    const float fillDelay = 0.4f;
    const float shiftDelay = 0.5f;
    const int anyMatchBonus = 100;
    const int exactMatchBonus = 300;

    public AudioClip matchSound, levelUpSfx, level2Music, level3Music, level4Music;
    public Text timerText, scoreText, gameOver, customers, winner, roundTimerText, levelText, levelNumber;
    public float maxTimer, maxRoundTimer;
    float timer, timerAdjust, roundTimer;

    int[] nextLevel;
    int currentLevel;

    AudioSource sfxSrc;

    int score;
    int customersRemaining = 10;
    bool paused;

    public GameObject donutPrefab, donutPrefab2, donutPrefab3, donut4, donut5, donut6, donut7, donut8, donut9, donut10, donut11, donut12, donut13, donut14, donut15, donut16,
        donut17, donut18, donut19, donut20, donut21, donut22, donut23, donut24, donut25;
    
    public GameObject[,] grid = new GameObject[width, height];

    List<GameObject> donutList;

    GameObject desired;

	// Use this for initialization
	void Start () {

        //POPULATE LEVEL ARRAY
        nextLevel = new int[10];
        nextLevel[0] = 10;
        nextLevel[1] = 10;
        nextLevel[2] = 20;
        nextLevel[3] = 20;
        nextLevel[4] = 30;
        nextLevel[5] = 40;
        nextLevel[6] = 50;
        nextLevel[7] = 50;
        nextLevel[8] = 60;
        nextLevel[9] = 60;

        //POPULATE DONUT LIST WITH AVAILABLE DONUTS
        donutList = new List<GameObject>();
        donutList.Add(donutPrefab);
        donutList.Add(donutPrefab2);
        donutList.Add(donutPrefab3);
        donutList.Add(donut4);
        donutList.Add(donut5);
        donutList.Add(donut6);
        donutList.Add(donut7);
        donutList.Add(donut8);
        donutList.Add(donut9);
        donutList.Add(donut10);
        donutList.Add(donut11);
        donutList.Add(donut12);
        donutList.Add(donut13);
        donutList.Add(donut14);
        donutList.Add(donut15);
        donutList.Add(donut16);
        donutList.Add(donut17);
        donutList.Add(donut18);
        donutList.Add(donut19);
        donutList.Add(donut20);
        donutList.Add(donut21);
        donutList.Add(donut22);
        donutList.Add(donut23);
        donutList.Add(donut24);
        donutList.Add(donut25);

        //FILL GRID WITH EMPTY OBJECTS
        for ( int i = 0; i < height; i++ )
            for( int j = 0; j < width; j++ )
            {
                grid[j, i] = new GameObject();
            }

        //ADD DESIRED DONUT
        desired = (GameObject)Instantiate(donutList[Random.Range(0, donutList.Count)].gameObject, new Vector2(8f, 5f), Quaternion.identity);

        //INITIALIZE SCORE
        score = 0;
        scoreText.text = score.ToString();

        customersRemaining = 10;
        customers.text = customersRemaining.ToString();

        gameOver.enabled = false;
        winner.enabled = false;
        currentLevel = 0;
        levelNumber.text = currentLevel.ToString();

        //grid[1, 0] = (GameObject)Instantiate(donutPrefab.gameObject, new Vector3(1f, 0f, 0f), Quaternion.identity);
        //grid[2, 0] = (GameObject)Instantiate(donutPrefab2.gameObject, new Vector3(2f, 0f, 0f), Quaternion.identity);
        //grid[4, 0] = (GameObject)Instantiate(donutPrefab3.gameObject, new Vector3(4f, 0f, 0f), Quaternion.identity);
        //grid[1, 1] = (GameObject)Instantiate(donut4.gameObject, new Vector3(1f, 1f, 0f), Quaternion.identity);
        //grid[1, 2] = (GameObject)Instantiate(donut5.gameObject, new Vector3(1f, 2f, 0f), Quaternion.identity);
        //grid[2, 1] = (GameObject)Instantiate(donut6.gameObject, new Vector3(2f, 1f, 0f), Quaternion.identity);
        //grid[4, 1] = (GameObject)Instantiate(donut7.gameObject, new Vector3(4f, 1f, 0f), Quaternion.identity);
        //grid[4, 2] = (GameObject)Instantiate(donutPrefab.gameObject, new Vector3(4f, 2f, 0f), Quaternion.identity);

        //START ADD_ROW TIMER
        timer = maxTimer;
        roundTimer = maxRoundTimer;

        sfxSrc = GetComponent<AudioSource>();

        paused = false;
    }
	
	// Update is called once per frame
	void Update () {

        if( paused )
        {
            if( Input.GetKeyDown(KeyCode.Return) )
            {
                restart();
            }
        }

        if( !paused && !globals.isPaused && timer > 0 )
        {
            timer -= Time.deltaTime;
            timerText.text = timer.ToString();

            if (roundTimer >= 0)
            {
                roundTimer -= Time.deltaTime;
                roundTimerText.text = roundTimer.ToString();
            }

            if( timer <= 0 )
            {
                addRow();
                timer = maxTimer;
            }

            //CHECK LEVEL UP CONDITION
            if( customersRemaining <= 0 )
            {
                //PLAY LEVEL UP SOUND
                sfxSrc.PlayOneShot(levelUpSfx);

                //winner.enabled = true;
                currentLevel++;
                levelNumber.text = currentLevel.ToString();

                //IF NEXT EVEN LEVEL, PLAY NEXT TRACK
                if (currentLevel == 2)
                {
                    Camera.main.GetComponent<AudioSource>().Stop();
                    Camera.main.GetComponent<AudioSource>().clip = level2Music;
                    Camera.main.GetComponent<AudioSource>().Play();
                }
                if (currentLevel == 4)
                {
                    Camera.main.GetComponent<AudioSource>().Stop();
                    Camera.main.GetComponent<AudioSource>().clip = level3Music;
                    Camera.main.GetComponent<AudioSource>().Play();
                }
                if (currentLevel == 6)
                {
                    Camera.main.GetComponent<AudioSource>().Stop();
                    Camera.main.GetComponent<AudioSource>().clip = level4Music;
                    Camera.main.GetComponent<AudioSource>().Play();
                }

                score += (int)(roundTimer * 1000);
                scoreText.text = score.ToString();

                //RESET ROUND TIMER AND ADD EXTRA MINUTE
                roundTimer = maxRoundTimer + 60 * currentLevel;

                //REDUCE TIME FROM ROW TIMER
                maxTimer -= 1f;

                customersRemaining = nextLevel[currentLevel];
                customers.text = customersRemaining.ToString();
                //paused = true;
            }
        }

	}

    public void swap( int x, int y )
    {
        if (grid[x, y] == null)
            grid[x, y] = new GameObject();
        if (grid[x + 1, y] == null)
            grid[x + 1, y] = new GameObject();

        if ( (grid[x,y].GetComponent<donut>() != null && grid[x,y].GetComponent<donut>().isMatched()) || (grid[x + 1, y].GetComponent<donut>() != null && grid[x + 1, y].GetComponent<donut>().isMatched()) )
        {
            return;
        }
        if ((grid[x, y].GetComponent<donut>() != null && grid[x, y].GetComponent<donut>().isFalling()) || ((grid[x + 1, y].GetComponent<donut>() != null && grid[x + 1, y].GetComponent<donut>().isFalling())))
            return;
        GameObject gridTmp = grid[x, y];
        grid[x, y] = grid[ x + 1, y];
        grid[x, y].transform.Translate(-1f, 0f, 0f);
        grid[x + 1, y] = gridTmp;
        grid[x + 1, y].transform.Translate(1f, 0f, 0f);

        //Check swapped objects for match
        if( grid[x,y].GetComponent<donut>() != null )
        {
            StartCoroutine(shiftDownDelay(x, y, fallDelay));
            //checkMatch(x, y);
            /*if( (x > 0) && (grid[x - 1,y].GetComponent<donut>() != null) && (grid[x - 1, y].GetComponent<donut>().isMatch(grid[x, y].GetComponent<donut>()))) 
            {
                Debug.Log("Left match!");
                if ( (x - 1 > 0) && grid[x - 2, y].GetComponent<donut>() != null && grid[x - 2, y].GetComponent<donut>().isMatch(grid[x - 1, y].GetComponent<donut>()))
                {
                    Debug.Log("Matched three!");
                    grid[x - 2, y].GetComponent<donut>().setMatch();
                    grid[x - 1, y].GetComponent<donut>().setMatch();
                    grid[x, y].GetComponent<donut>().setMatch();
                    grid[x - 2, y] = new GameObject();
                    grid[x - 1, y] = new GameObject();
                    grid[x, y] = new GameObject();
                    StartCoroutine(shiftDownDelay(x - 2, y + 1, 0.7f));
                    StartCoroutine(shiftDownDelay(x - 1, y + 1, 0.7f));
                    StartCoroutine(shiftDownDelay(x, y + 1, 0.7f));
                }
            }*/
        }
        else
        {
            Debug.Log("Should shift down");
            StartCoroutine(shiftDownDelay(x, y + 1, fallDelay));
        }
        //if( y > 0 )
            
        if (grid[x+1, y].GetComponent<donut>() != null)
        {
            StartCoroutine(shiftDownDelay(x + 1, y, fallDelay));
            //checkMatch(x + 1, y);
            /*if ((x+1 < 6) && (grid[x + 2, y].GetComponent<donut>() != null) && (grid[x + 2, y].GetComponent<donut>().isMatch(grid[x+1, y].GetComponent<donut>())))
            {
                Debug.Log("Right match!");
                if ((x + 2 > 0) && grid[x + 3, y].GetComponent<donut>() != null && grid[x + 3, y].GetComponent<donut>().isMatch(grid[x + 2, y].GetComponent<donut>()))
                {
                    Debug.Log("Matched three!");
                    grid[x + 3, y].GetComponent<donut>().setMatch();
                    grid[x + 2, y].GetComponent<donut>().setMatch();
                    grid[x + 1, y].GetComponent<donut>().setMatch();
                    grid[x + 3, y] = new GameObject();
                    grid[x + 2, y] = new GameObject();
                    grid[x + 1, y] = new GameObject();
                    StartCoroutine(shiftDownDelay(x + 3, y + 1, 0.7f));
                    StartCoroutine(shiftDownDelay(x + 2, y + 1, 0.7f));
                    StartCoroutine(shiftDownDelay(x + 1, y + 1, 0.7f));
                }
            }*/
        }
        else
        {
            Debug.Log("Should shift down");
            StartCoroutine(shiftDownDelay(x + 1, y + 1, fallDelay));
        }
        //if( y > 0 )
            
    }

    IEnumerator shiftDownDelay( int x, int y, float delay )
    {
        if (grid[x, y] == null)
            grid[x, y] = new GameObject();
        if( grid[x, y].GetComponent<donut>() != null )
        {
            grid[x, y].GetComponent<donut>().drop();
            float delayAdjust = 0.01f;
            //WAIT UNTIL MATCHED DONUTS ARE RESOLVED
            while (globals.isPaused)
            { yield return new WaitForSecondsRealtime(0.01f); }
            while (y > 0 && grid[x,y] != null && grid[x, y - 1].GetComponent<donut>() == null)
            {
                while (globals.isPaused)
                { yield return new WaitForSecondsRealtime(0.01f); }

                yield return new WaitForSecondsRealtime(delay - delayAdjust);

                Debug.Log("This should fall");
                GameObject gridTmp = grid[x, y - 1];
                grid[x, y - 1] = grid[x, y];
                grid[x, y - 1].transform.Translate(0f, -1f, 0f);
                grid[x, y] = gridTmp;
                grid[x, y].transform.Translate(0f, 1f, 0f);
                    
                if (y < height - 1)
                    shiftDown(x, y + 1);
                y--;
                delayAdjust += .01f;
                Debug.Log("delayAdjust: " + delayAdjust);
            }
            //CHECK FOR MATCH AFTER FALLING ALL THE WAY DOWN
            if (grid[x, y] != null)
            {
                if (grid[x, y].GetComponent<donut>() != null)
                {
                    grid[x, y].GetComponent<donut>().settle();
                    checkMatch(x, y);
                }
            }
            else
                grid[x, y] = new GameObject();

            
        }
    }

    void shiftDown( int x, int y )
    {
        if (grid[x, y] == null)
            grid[x, y] = new GameObject();
        if (grid[x, y].GetComponent<donut>() != null)
        {
            grid[x, y].GetComponent<donut>().drop();
            //WAIT UNTIL MATCHED DONUTS ARE RESOLVED
            while (y > 0 && grid[x,y] != null && grid[x, y - 1].GetComponent<donut>() == null)
            {
                Debug.Log("This should fall");
                GameObject gridTmp = grid[x, y - 1];
                grid[x, y - 1] = grid[x, y];
                grid[x, y - 1].transform.Translate(0f, -1f, 0f);
                grid[x, y] = gridTmp;
                grid[x, y].transform.Translate(0f, 1f, 0f);
                    
                if (y < height - 1)
                    shiftDown(x, y + 1);
                y--;
            }
            if (grid[x, y] != null)
            {
                if (grid[x, y].GetComponent<donut>() != null)
                {
                    grid[x, y].GetComponent<donut>().settle();
                    checkMatch(x, y);
                }
            }
            else
                grid[x, y] = new GameObject();
            
        }
    }

    IEnumerator fillEmptySpace(int x, int y, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        grid[x, y] = new GameObject();
    }

    void checkMatch( int x, int y )
    {
        if( grid[x,y] == null )
        {
            grid[x, y] = new GameObject();
            return;
        }
        //CHECK LEFT SIDE FOR MATCH
        if (grid[x, y].GetComponent<donut>() == null)
            return;

        if ((x - 1 > 0) && (grid[x - 1, y].GetComponent<donut>() != null) && grid[x - 2, y].GetComponent<donut>() != null )
        {
            Debug.Log("Left match!");
            if (grid[x - 1, y].GetComponent<donut>().matchColor(grid[x, y].GetComponent<donut>()) && grid[x - 2, y].GetComponent<donut>().matchColor(grid[x, y].GetComponent<donut>()))
            {
                Debug.Log("Matched three!");
                grid[x - 2, y].GetComponent<donut>().setMatch();
                grid[x - 1, y].GetComponent<donut>().setMatch();
                grid[x, y].GetComponent<donut>().setMatch();

                sfxSrc.PlayOneShot(matchSound);

                score += 300;
                if (grid[x, y].GetComponent<donut>().exactMatch(desired.GetComponent<donut>()) || grid[x - 1, y].GetComponent<donut>().exactMatch(desired.GetComponent<donut>()) || grid[x - 2, y].GetComponent<donut>().exactMatch(desired.GetComponent<donut>()))
                {
                    score += 200;
                    Destroy(desired.gameObject);
                    desired = (GameObject)Instantiate(donutList[Random.Range(0, donutList.Count)].gameObject, new Vector2(8f, 5f), Quaternion.identity);
                    customersRemaining--;
                    customers.text = customersRemaining.ToString();
                }
                else if (grid[x, y].GetComponent<donut>().anyMatch(desired.GetComponent<donut>()) || grid[x - 1, y].GetComponent<donut>().anyMatch(desired.GetComponent<donut>()) || grid[x - 2, y].GetComponent<donut>().anyMatch(desired.GetComponent<donut>()))
                {
                    score += 100;
                    Destroy(desired.gameObject);
                    desired = (GameObject)Instantiate(donutList[Random.Range(0, donutList.Count)].gameObject, new Vector2(8f, 5f), Quaternion.identity);
                    customersRemaining--;
                    customers.text = customersRemaining.ToString();
                }
                scoreText.text = score.ToString();

                StartCoroutine(fillEmptySpace(x - 2, y, fillDelay));
                StartCoroutine(fillEmptySpace(x - 1, y, fillDelay));
                StartCoroutine(fillEmptySpace(x, y, fillDelay));
                StartCoroutine(shiftDownDelay(x - 2, y + 1, shiftDelay));
                StartCoroutine(shiftDownDelay(x - 1, y + 1, shiftDelay));
                StartCoroutine(shiftDownDelay(x, y + 1, shiftDelay));
                return;
            }
            //IF THE LAST DONUT DOESN'T MATCH THE SAME ATTRIBUTE, CHECK AGAINST THE SECONDARY ATTRIBUTE
            else if (grid[x, y].GetComponent<donut>().matchShape(grid[x - 1, y].GetComponent<donut>()) && grid[x, y].GetComponent<donut>().matchShape(grid[x - 2, y].GetComponent<donut>()) )
            {
                Debug.Log("Matched three!");
                grid[x - 2, y].GetComponent<donut>().setMatch();
                grid[x - 1, y].GetComponent<donut>().setMatch();
                grid[x, y].GetComponent<donut>().setMatch();

                score += 300;

                sfxSrc.PlayOneShot(matchSound);

                if (grid[x, y].GetComponent<donut>().exactMatch(desired.GetComponent<donut>()) || grid[x - 1, y].GetComponent<donut>().exactMatch(desired.GetComponent<donut>()) || grid[x - 2, y].GetComponent<donut>().exactMatch(desired.GetComponent<donut>()))
                {
                    score += 200;
                    Destroy(desired.gameObject);
                    desired = (GameObject)Instantiate(donutList[Random.Range(0, donutList.Count)].gameObject, new Vector2(8f, 5f), Quaternion.identity);
                    customersRemaining--;
                    customers.text = customersRemaining.ToString();
                }
                else if (grid[x, y].GetComponent<donut>().anyMatch(desired.GetComponent<donut>()) || grid[x - 1, y].GetComponent<donut>().anyMatch(desired.GetComponent<donut>()) || grid[x - 2, y].GetComponent<donut>().anyMatch(desired.GetComponent<donut>()))
                {
                    score += 100;
                    Destroy(desired.gameObject);
                    desired = (GameObject)Instantiate(donutList[Random.Range(0, donutList.Count)].gameObject, new Vector2(8f, 5f), Quaternion.identity);
                    customersRemaining--;
                    customers.text = customersRemaining.ToString();
                }
                scoreText.text = score.ToString();

                StartCoroutine(fillEmptySpace(x - 2, y, fillDelay));
                StartCoroutine(fillEmptySpace(x - 1, y, fillDelay));
                StartCoroutine(fillEmptySpace(x, y, fillDelay));
                StartCoroutine(shiftDownDelay(x - 2, y + 1, shiftDelay));
                StartCoroutine(shiftDownDelay(x - 1, y + 1, shiftDelay));
                StartCoroutine(shiftDownDelay(x, y + 1, shiftDelay));
                return;
            }
        }

        //CHECK RIGHT SIDE FOR MATCH
        if ((x + 1 < width - 1) && (grid[x + 1, y].GetComponent<donut>() != null) && grid[x + 2, y].GetComponent<donut>() != null)
        {
            Debug.Log("Right match!");
            if (grid[x + 1, y].GetComponent<donut>().matchColor(grid[x, y].GetComponent<donut>()) && grid[x + 2, y].GetComponent<donut>().matchColor(grid[x, y].GetComponent<donut>()))
            {
                Debug.Log("Matched three!");
                grid[x + 2, y].GetComponent<donut>().setMatch();
                grid[x + 1, y].GetComponent<donut>().setMatch();
                grid[x, y].GetComponent<donut>().setMatch();

                score += 300;
                sfxSrc.PlayOneShot(matchSound);

                if (grid[x, y].GetComponent<donut>().exactMatch(desired.GetComponent<donut>()) || grid[x + 1, y].GetComponent<donut>().exactMatch(desired.GetComponent<donut>()) || grid[x + 2, y].GetComponent<donut>().exactMatch(desired.GetComponent<donut>()))
                {
                    score += 200;
                    Destroy(desired.gameObject);
                    desired = (GameObject)Instantiate(donutList[Random.Range(0, donutList.Count)].gameObject, new Vector2(8f, 5f), Quaternion.identity);
                    customersRemaining--;
                    customers.text = customersRemaining.ToString();
                }
                else if (grid[x, y].GetComponent<donut>().anyMatch(desired.GetComponent<donut>()) || grid[x + 1, y].GetComponent<donut>().anyMatch(desired.GetComponent<donut>()) || grid[x + 2, y].GetComponent<donut>().anyMatch(desired.GetComponent<donut>()))
                {
                    score += 100;
                    Destroy(desired.gameObject);
                    desired = (GameObject)Instantiate(donutList[Random.Range(0, donutList.Count)].gameObject, new Vector2(8f, 5f), Quaternion.identity);
                    customersRemaining--;
                    customers.text = customersRemaining.ToString();
                }
                scoreText.text = score.ToString();

                StartCoroutine(fillEmptySpace(x + 2, y, fillDelay));
                StartCoroutine(fillEmptySpace(x + 1, y, fillDelay));
                StartCoroutine(fillEmptySpace(x, y, fillDelay));
                StartCoroutine(shiftDownDelay(x + 2, y + 1, shiftDelay));
                StartCoroutine(shiftDownDelay(x + 1, y + 1, shiftDelay));
                StartCoroutine(shiftDownDelay(x, y + 1, shiftDelay));
                return;
            }
            else if (grid[x, y].GetComponent<donut>().matchShape(grid[x + 1, y].GetComponent<donut>()) && grid[x, y].GetComponent<donut>().matchShape(grid[x + 2, y].GetComponent<donut>()))
            {
                Debug.Log("Matched three!");
                grid[x + 2, y].GetComponent<donut>().setMatch();
                grid[x + 1, y].GetComponent<donut>().setMatch();
                grid[x, y].GetComponent<donut>().setMatch();

                score += 300;
                sfxSrc.PlayOneShot(matchSound);

                if (grid[x, y].GetComponent<donut>().exactMatch(desired.GetComponent<donut>()) || grid[x + 1, y].GetComponent<donut>().exactMatch(desired.GetComponent<donut>()) || grid[x + 2, y].GetComponent<donut>().exactMatch(desired.GetComponent<donut>()))
                {
                    score += 200;
                    Destroy(desired.gameObject);
                    desired = (GameObject)Instantiate(donutList[Random.Range(0, donutList.Count)].gameObject, new Vector2(8f, 5f), Quaternion.identity);
                    customersRemaining--;
                    customers.text = customersRemaining.ToString();
                }
                else if (grid[x, y].GetComponent<donut>().anyMatch(desired.GetComponent<donut>()) || grid[x + 1, y].GetComponent<donut>().anyMatch(desired.GetComponent<donut>()) || grid[x + 2, y].GetComponent<donut>().anyMatch(desired.GetComponent<donut>()))
                {
                    score += 100;
                    Destroy(desired.gameObject);
                    desired = (GameObject)Instantiate(donutList[Random.Range(0, donutList.Count)].gameObject, new Vector2(8f, 5f), Quaternion.identity);
                    customersRemaining--;
                    customers.text = customersRemaining.ToString();
                }
                scoreText.text = score.ToString();

                StartCoroutine(fillEmptySpace(x + 2, y, fillDelay));
                StartCoroutine(fillEmptySpace(x + 1, y, fillDelay));
                StartCoroutine(fillEmptySpace(x, y, fillDelay));
                StartCoroutine(shiftDownDelay(x + 2, y + 1, shiftDelay));
                StartCoroutine(shiftDownDelay(x + 1, y + 1, shiftDelay));
                StartCoroutine(shiftDownDelay(x, y + 1, shiftDelay));
                return;
            }
        }

        //CHECK MIDDLE FOR MATCH
        if ((x > 0) && (x < width - 1 ) && (grid[x - 1, y].GetComponent<donut>() != null) && grid[x + 1, y].GetComponent<donut>() != null )
        {
            Debug.Log("Right match!");
            if (grid[x - 1, y].GetComponent<donut>().matchColor(grid[x, y].GetComponent<donut>()) && grid[x + 1, y].GetComponent<donut>().matchColor(grid[x, y].GetComponent<donut>()))
            {
                Debug.Log("Matched three!");
                grid[x + 1, y].GetComponent<donut>().setMatch();
                grid[x - 1, y].GetComponent<donut>().setMatch();
                grid[x, y].GetComponent<donut>().setMatch();

                score += 300;
                sfxSrc.PlayOneShot(matchSound);

                if (grid[x, y].GetComponent<donut>().exactMatch(desired.GetComponent<donut>()) || grid[x + 1, y].GetComponent<donut>().exactMatch(desired.GetComponent<donut>()) || grid[x - 1, y].GetComponent<donut>().exactMatch(desired.GetComponent<donut>()))
                {
                    score += 200;
                    Destroy(desired.gameObject);
                    desired = (GameObject)Instantiate(donutList[Random.Range(0, donutList.Count)].gameObject, new Vector2(8f, 5f), Quaternion.identity);
                    customersRemaining--;
                    customers.text = customersRemaining.ToString();
                }
                else if (grid[x, y].GetComponent<donut>().anyMatch(desired.GetComponent<donut>()) || grid[x + 1, y].GetComponent<donut>().anyMatch(desired.GetComponent<donut>()) || grid[x - 1, y].GetComponent<donut>().anyMatch(desired.GetComponent<donut>()))
                {
                    score += 100;
                    Destroy(desired.gameObject);
                    desired = (GameObject)Instantiate(donutList[Random.Range(0, donutList.Count)].gameObject, new Vector2(8f, 5f), Quaternion.identity);
                    customersRemaining--;
                    customers.text = customersRemaining.ToString();
                }
                scoreText.text = score.ToString();

                StartCoroutine(fillEmptySpace(x + 1, y, fillDelay));
                StartCoroutine(fillEmptySpace(x - 1, y, fillDelay));
                StartCoroutine(fillEmptySpace(x, y, fillDelay));
                StartCoroutine(shiftDownDelay(x + 1, y + 1, shiftDelay));
                StartCoroutine(shiftDownDelay(x - 1, y + 1, shiftDelay));
                StartCoroutine(shiftDownDelay(x, y + 1, shiftDelay));
                return;
            }
            else if (grid[x, y].GetComponent<donut>().matchShape(grid[x + 1, y].GetComponent<donut>()) && grid[x, y].GetComponent<donut>().matchShape(grid[x - 1, y].GetComponent<donut>()))
            {
                Debug.Log("Matched three!");
                grid[x + 1, y].GetComponent<donut>().setMatch();
                grid[x - 1, y].GetComponent<donut>().setMatch();
                grid[x, y].GetComponent<donut>().setMatch();

                score += 300;
                sfxSrc.PlayOneShot(matchSound);

                if (grid[x, y].GetComponent<donut>().exactMatch(desired.GetComponent<donut>()) || grid[x + 1, y].GetComponent<donut>().exactMatch(desired.GetComponent<donut>()) || grid[x - 1, y].GetComponent<donut>().exactMatch(desired.GetComponent<donut>()))
                {
                    score += 200;
                    Destroy(desired.gameObject);
                    desired = (GameObject)Instantiate(donutList[Random.Range(0, donutList.Count)].gameObject, new Vector2(8f, 5f), Quaternion.identity);
                    customersRemaining--;
                    customers.text = customersRemaining.ToString();
                }
                else if (grid[x, y].GetComponent<donut>().anyMatch(desired.GetComponent<donut>()) || grid[x + 1, y].GetComponent<donut>().anyMatch(desired.GetComponent<donut>()) || grid[x - 1, y].GetComponent<donut>().anyMatch(desired.GetComponent<donut>()))
                {
                    score += 100;
                    Destroy(desired.gameObject);
                    desired = (GameObject)Instantiate(donutList[Random.Range(0, donutList.Count)].gameObject, new Vector2(8f, 5f), Quaternion.identity);
                    customersRemaining--;
                    customers.text = customersRemaining.ToString();
                }
                scoreText.text = score.ToString();

                StartCoroutine(fillEmptySpace(x + 1, y, fillDelay));
                StartCoroutine(fillEmptySpace(x - 1, y, fillDelay));
                StartCoroutine(fillEmptySpace(x, y, fillDelay));
                StartCoroutine(shiftDownDelay(x + 1, y + 1, shiftDelay));
                StartCoroutine(shiftDownDelay(x - 1, y + 1, shiftDelay));
                StartCoroutine(shiftDownDelay(x, y + 1, shiftDelay));
                return;
            }
        }
        //CHECK TOP MATCH
        if ((y + 1 < height - 1) && (grid[x, y + 1].GetComponent<donut>() != null) && grid[x, y + 2].GetComponent<donut>() != null)
        {
            Debug.Log("Top match!");
            if (grid[x, y + 1].GetComponent<donut>().matchColor(grid[x, y].GetComponent<donut>()) && grid[x, y + 2].GetComponent<donut>().matchColor(grid[x, y].GetComponent<donut>()))
            {
                Debug.Log("Matched three!");
                grid[x, y].GetComponent<donut>().setMatch();
                grid[x, y + 1].GetComponent<donut>().setMatch();
                grid[x, y + 2].GetComponent<donut>().setMatch();

                score += 300;
                sfxSrc.PlayOneShot(matchSound);

                if (grid[x, y].GetComponent<donut>().exactMatch(desired.GetComponent<donut>()) || grid[x, y + 1].GetComponent<donut>().exactMatch(desired.GetComponent<donut>()) || grid[x, y + 2].GetComponent<donut>().exactMatch(desired.GetComponent<donut>()))
                {
                    score += 200;
                    Destroy(desired.gameObject);
                    desired = (GameObject)Instantiate(donutList[Random.Range(0, donutList.Count)].gameObject, new Vector2(8f, 5f), Quaternion.identity);
                    customersRemaining--;
                    customers.text = customersRemaining.ToString();
                }
                else if (grid[x, y].GetComponent<donut>().anyMatch(desired.GetComponent<donut>()) || grid[x, y + 1].GetComponent<donut>().anyMatch(desired.GetComponent<donut>()) || grid[x, y + 2].GetComponent<donut>().anyMatch(desired.GetComponent<donut>()))
                {
                    score += 100;
                    sfxSrc.PlayOneShot(matchSound);

                    Destroy(desired.gameObject);
                    desired = (GameObject)Instantiate(donutList[Random.Range(0, donutList.Count)].gameObject, new Vector2(8f, 5f), Quaternion.identity);
                    customersRemaining--;
                    customers.text = customersRemaining.ToString();
                }
                scoreText.text = score.ToString();

                StartCoroutine(fillEmptySpace(x, y + 1, fillDelay));
                StartCoroutine(fillEmptySpace(x, y + 2, fillDelay));
                StartCoroutine(fillEmptySpace(x, y, fillDelay));
                StartCoroutine(shiftDownDelay(x, y + 3, shiftDelay));
                //StartCoroutine(shiftDownDelay(x + 2, y + 1, 0.7f));
                //StartCoroutine(shiftDownDelay(x + 1, y + 1, 0.7f));
                return;
            }
            else if (grid[x, y + 1].GetComponent<donut>().matchShape(grid[x, y].GetComponent<donut>()) && grid[x, y + 2].GetComponent<donut>().matchShape(grid[x, y].GetComponent<donut>()))
            {
                Debug.Log("Matched three!");
                grid[x, y].GetComponent<donut>().setMatch();
                grid[x, y + 1].GetComponent<donut>().setMatch();
                grid[x, y + 2].GetComponent<donut>().setMatch();

                score += 300;
                sfxSrc.PlayOneShot(matchSound);

                if (grid[x, y].GetComponent<donut>().exactMatch(desired.GetComponent<donut>()) || grid[x, y + 1].GetComponent<donut>().exactMatch(desired.GetComponent<donut>()) || grid[x, y + 2].GetComponent<donut>().exactMatch(desired.GetComponent<donut>()))
                {
                    score += 200;
                    Destroy(desired.gameObject);
                    desired = (GameObject)Instantiate(donutList[Random.Range(0, donutList.Count)].gameObject, new Vector2(8f, 5f), Quaternion.identity);
                    customersRemaining--;
                    customers.text = customersRemaining.ToString();
                }
                else if (grid[x, y].GetComponent<donut>().anyMatch(desired.GetComponent<donut>()) || grid[x, y + 1].GetComponent<donut>().anyMatch(desired.GetComponent<donut>()) || grid[x, y + 2].GetComponent<donut>().anyMatch(desired.GetComponent<donut>()))
                {
                    score += 100;
                    Destroy(desired.gameObject);
                    desired = (GameObject)Instantiate(donutList[Random.Range(0, donutList.Count)].gameObject, new Vector2(8f, 5f), Quaternion.identity);
                    customersRemaining--;
                    customers.text = customersRemaining.ToString();
                }
                scoreText.text = score.ToString();

                StartCoroutine(fillEmptySpace(x, y + 1, fillDelay));
                StartCoroutine(fillEmptySpace(x, y + 2, fillDelay));
                StartCoroutine(fillEmptySpace(x, y, fillDelay));
                StartCoroutine(shiftDownDelay(x, y + 3, shiftDelay));
                //StartCoroutine(shiftDownDelay(x + 2, y + 1, 0.7f));
                //StartCoroutine(shiftDownDelay(x + 1, y + 1, 0.7f));
                return;
            }
        }

        //CHECK MIDDLE MATCH
        if ((y < height - 1) && y > 0 && (grid[x, y + 1].GetComponent<donut>() != null) && grid[x, y - 1].GetComponent<donut>() != null)
        {
            Debug.Log("Top match!");
            if (grid[x, y + 1].GetComponent<donut>().matchColor(grid[x, y].GetComponent<donut>()) && grid[x, y - 1].GetComponent<donut>().matchColor(grid[x, y].GetComponent<donut>()))
            {
                Debug.Log("Matched three middle!");
                grid[x, y].GetComponent<donut>().setMatch();
                grid[x, y + 1].GetComponent<donut>().setMatch();
                grid[x, y - 1].GetComponent<donut>().setMatch();

                score += 300;
                sfxSrc.PlayOneShot(matchSound);

                if (grid[x, y].GetComponent<donut>().exactMatch(desired.GetComponent<donut>()) || grid[x, y - 1].GetComponent<donut>().exactMatch(desired.GetComponent<donut>()) || grid[x, y + 1].GetComponent<donut>().exactMatch(desired.GetComponent<donut>()))
                {
                    score += 200;
                    Destroy(desired.gameObject);
                    desired = (GameObject)Instantiate(donutList[Random.Range(0, donutList.Count)].gameObject, new Vector2(8f, 5f), Quaternion.identity);
                    customersRemaining--;
                    customers.text = customersRemaining.ToString();
                }
                else if (grid[x, y].GetComponent<donut>().anyMatch(desired.GetComponent<donut>()) || grid[x, y - 1].GetComponent<donut>().anyMatch(desired.GetComponent<donut>()) || grid[x, y + 1].GetComponent<donut>().anyMatch(desired.GetComponent<donut>()))
                {
                    score += 100;
                    Destroy(desired.gameObject);
                    desired = (GameObject)Instantiate(donutList[Random.Range(0, donutList.Count)].gameObject, new Vector2(8f, 5f), Quaternion.identity);
                    customersRemaining--;
                    customers.text = customersRemaining.ToString();
                }
                scoreText.text = score.ToString();

                StartCoroutine(fillEmptySpace(x, y + 1, fillDelay));
                StartCoroutine(fillEmptySpace(x, y - 1, fillDelay));
                StartCoroutine(fillEmptySpace(x, y, fillDelay));
                StartCoroutine(shiftDownDelay(x, y + 2, shiftDelay));
                //StartCoroutine(shiftDownDelay(x + 2, y + 1, 0.7f));
                //StartCoroutine(shiftDownDelay(x + 1, y + 1, 0.7f));
                return;
            }
            else if (grid[x,y + 1].GetComponent<donut>().matchShape(grid[x, y].GetComponent<donut>()) && grid[x, y - 1].GetComponent<donut>().matchShape(grid[x, y].GetComponent<donut>()))
            {
                Debug.Log("Matched three middle!");
                grid[x, y].GetComponent<donut>().setMatch();
                grid[x, y + 1].GetComponent<donut>().setMatch();
                grid[x, y - 1].GetComponent<donut>().setMatch();

                score += 300;
                sfxSrc.PlayOneShot(matchSound);

                if (grid[x, y].GetComponent<donut>().exactMatch(desired.GetComponent<donut>()) || grid[x, y - 1].GetComponent<donut>().exactMatch(desired.GetComponent<donut>()) || grid[x, y + 1].GetComponent<donut>().exactMatch(desired.GetComponent<donut>()))
                {
                    score += 200;
                    Destroy(desired.gameObject);
                    desired = (GameObject)Instantiate(donutList[Random.Range(0, donutList.Count)].gameObject, new Vector2(8f, 5f), Quaternion.identity);
                    customersRemaining--;
                    customers.text = customersRemaining.ToString();
                }
                else if (grid[x, y].GetComponent<donut>().anyMatch(desired.GetComponent<donut>()) || grid[x, y - 1].GetComponent<donut>().anyMatch(desired.GetComponent<donut>()) || grid[x, y + 1].GetComponent<donut>().anyMatch(desired.GetComponent<donut>()))
                {
                    score += 100;
                    Destroy(desired.gameObject);
                    desired = (GameObject)Instantiate(donutList[Random.Range(0, donutList.Count)].gameObject, new Vector2(8f, 5f), Quaternion.identity);
                    customersRemaining--;
                    customers.text = customersRemaining.ToString();
                }
                scoreText.text = score.ToString();

                StartCoroutine(fillEmptySpace(x, y + 1, fillDelay));
                StartCoroutine(fillEmptySpace(x, y - 1, fillDelay));
                StartCoroutine(fillEmptySpace(x, y, fillDelay));
                StartCoroutine(shiftDownDelay(x, y + 2, shiftDelay));
                //StartCoroutine(shiftDownDelay(x + 2, y + 1, 0.7f));
                //StartCoroutine(shiftDownDelay(x + 1, y + 1, 0.7f));
                return;
            }
        }

        //CHECK BOTTOM MATCH
        if ((y - 1 > 0) && (grid[x, y - 1].GetComponent<donut>() != null) && grid[x, y - 2].GetComponent<donut>() != null)
        {
            Debug.Log("Bottom match!");
            if (grid[x, y - 1].GetComponent<donut>().matchColor(grid[x, y].GetComponent<donut>()) && grid[x, y - 2].GetComponent<donut>().matchColor(grid[x, y].GetComponent<donut>()))
            {
                Debug.Log("Matched three!");
                grid[x, y].GetComponent<donut>().setMatch();
                grid[x, y - 1].GetComponent<donut>().setMatch();
                grid[x, y - 2].GetComponent<donut>().setMatch();

                score += 300;
                sfxSrc.PlayOneShot(matchSound);

                if (grid[x, y].GetComponent<donut>().exactMatch(desired.GetComponent<donut>()) || grid[x, y - 1].GetComponent<donut>().exactMatch(desired.GetComponent<donut>()) || grid[x, y - 2].GetComponent<donut>().exactMatch(desired.GetComponent<donut>()))
                {
                    score += 200;
                    Destroy(desired.gameObject);
                    desired = (GameObject)Instantiate(donutList[Random.Range(0, donutList.Count)].gameObject, new Vector2(8f, 5f), Quaternion.identity);
                    customersRemaining--;
                    customers.text = customersRemaining.ToString();
                }
                else if (grid[x, y].GetComponent<donut>().anyMatch(desired.GetComponent<donut>()) || grid[x, y - 1].GetComponent<donut>().anyMatch(desired.GetComponent<donut>()) || grid[x, y - 2].GetComponent<donut>().anyMatch(desired.GetComponent<donut>()))
                {
                    score += 100;
                    Destroy(desired.gameObject);
                    desired = (GameObject)Instantiate(donutList[Random.Range(0, donutList.Count)].gameObject, new Vector2(8f, 5f), Quaternion.identity);
                    customersRemaining--;
                    customers.text = customersRemaining.ToString();
                }
                scoreText.text = score.ToString();

                StartCoroutine(fillEmptySpace(x, y - 1, fillDelay));
                StartCoroutine(fillEmptySpace(x, y - 2, fillDelay));
                StartCoroutine(fillEmptySpace(x, y, fillDelay));
                StartCoroutine(shiftDownDelay(x, y + 1, shiftDelay));
                //StartCoroutine(shiftDownDelay(x + 2, y + 1, 0.7f));
                //StartCoroutine(shiftDownDelay(x + 1, y + 1, 0.7f));
                return;
            }
            else if (grid[x, y - 1].GetComponent<donut>().matchShape(grid[x, y].GetComponent<donut>()) && grid[x, y - 2].GetComponent<donut>().matchShape(grid[x, y].GetComponent<donut>()))
            {
                Debug.Log("Matched three!");
                grid[x, y].GetComponent<donut>().setMatch();
                grid[x, y - 1].GetComponent<donut>().setMatch();
                grid[x, y - 2].GetComponent<donut>().setMatch();

                score += 300;
                sfxSrc.PlayOneShot(matchSound);

                if (grid[x, y].GetComponent<donut>().exactMatch(desired.GetComponent<donut>()) || grid[x, y - 1].GetComponent<donut>().exactMatch(desired.GetComponent<donut>()) || grid[x, y - 2].GetComponent<donut>().exactMatch(desired.GetComponent<donut>()))
                {
                    score += 200;
                    Destroy(desired.gameObject);
                    desired = (GameObject)Instantiate(donutList[Random.Range(0, donutList.Count)].gameObject, new Vector2(8f, 5f), Quaternion.identity);
                    customersRemaining--;
                    customers.text = customersRemaining.ToString();
                }
                else if (grid[x, y].GetComponent<donut>().anyMatch(desired.GetComponent<donut>()) || grid[x, y - 1].GetComponent<donut>().anyMatch(desired.GetComponent<donut>()) || grid[x, y - 2].GetComponent<donut>().anyMatch(desired.GetComponent<donut>()))
                {
                    score += 100;
                    Destroy(desired.gameObject);
                    desired = (GameObject)Instantiate(donutList[Random.Range(0, donutList.Count)].gameObject, new Vector2(8f, 5f), Quaternion.identity);
                    customersRemaining--;
                    customers.text = customersRemaining.ToString();
                }
                scoreText.text = score.ToString();

                StartCoroutine(fillEmptySpace(x, y - 1, fillDelay));
                StartCoroutine(fillEmptySpace(x, y - 2, fillDelay));
                StartCoroutine(fillEmptySpace(x, y, fillDelay));
                StartCoroutine(shiftDownDelay(x, y + 1, shiftDelay));
                //StartCoroutine(shiftDownDelay(x + 2, y + 1, 0.7f));
                //StartCoroutine(shiftDownDelay(x + 1, y + 1, 0.7f));  
                return;
            }
        }
    }

    IEnumerator WaitOneSecond()
    {
        print(Time.time);
        yield return new WaitForSecondsRealtime(5);
        print(Time.time);
    }

    /**
     *Add new row of donuts to the top of the grid 
     **/
    public void addRow()
    {
        for( int i = 0; i < width; i++ )
        {
            if( grid[i, 8].GetComponent<donut>() != null )
            {
                gameOver.enabled = true;
                paused = true;
                return;
            }
            Destroy(grid[i, 8].gameObject);
            grid[i, 8] = (GameObject)Instantiate(donutList[Random.Range(0, donutList.Count)].gameObject, new Vector3(i, 8f, 0f), Quaternion.identity);
            Debug.Log("Display something damnit");
            timer = maxTimer;
            StartCoroutine(shiftDownDelay(i, 8, 0.3f));
        }
    }

    public void restart()
    {
        gameOver.enabled = false;
        Destroy(desired.gameObject);
        for (int i = 0; i < height; i++)
            for (int j = 0; j < width; j++)
            {
                Destroy(grid[j, i].gameObject);
            }
        Start();
    }

    public void nextCustomer()
    {

    }
}
