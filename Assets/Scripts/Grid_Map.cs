using GameJolt.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Grid_Map : MonoBehaviour {

    const int width = 7;
    const int height = 9;
    const float fallDelay = 0.3f;
    const float fillDelay = 0.4f;
    const float shiftDelay = 0.5f;
    const int anyMatchBonus = 100;
    const int exactMatchBonus = 300;

    float touchDirection;
    Vector2 touchStart, touchEnd;
    Touch thisTouch;

    public AudioClip matchSound, levelUpSfx, level1Music, level2Music, level3Music, level4Music;
    public Text timerText, scoreText, gameOver, customers, winner, roundTimerText, levelText, levelNumber, streakText, happyText, sadText, submitting, desire;
    public float maxTimer, maxRoundTimer;
    float maxFallTimer = 0.20f;
    float fallTimer;
    float timer, timerAdjust, roundTimer;
    float timeBuffer = 1f;

    public Image donutImage;
    public int table_id;
    public InputField userInputField;
    public Button restartButton;
    public Transform sky;
    public bool isMobile;
    public bool allowWilds = false;
    public bool allowStrict = false;
    public bool hasObstacles = false;

    private List<AudioClip> countingClips;

    public GameObject mobileCursor;

    /*GameObject meshObject1;
    GameObject meshObject2;
    GameObject meshObject3;
    GameObject meshObject4;
    GameObject meshObject5;

    TextMesh mesh;
    TextMesh mesh1;
    TextMesh mesh2;
    TextMesh mesh3;
    TextMesh mesh4;*/
    
    int numMoves;
    int[] nextLevel;
    int currentLevel;

    int currentCombo, maxCombo;
    int currentStreak, maxStreak;

    AudioSource sfxSrc;

    score_text score_t;

    Vector2 mouseTarget;
    bool onTarget;

    int score;
    int customersRemaining = 10;
    bool paused;
    bool justSwapped = false;

    bool satisfied = true;

    public struct gridObject
    {
        public int x;
        public int y;
        public GameObject donutObject;
    }

    //STRUCTURE TO HOLD GRID COORDINATES DURING CHECKMATCH
    struct coord
    {
        public coord( int x, int y )
        {
            this.x = x;
            this.y = y;
        }
        int x;
        int y;

        public int getX()
        {
            return x;
        }

        public int getY()
        {
            return y;
        }
    }

    public GameObject donutPrefab, donutPrefab2, donutPrefab3, donut4, donut5, donut6, donut7, donut8, donut9, donut10, donut11, donut12, donut13, donut14, donut15, donut16,
        donut17, donut18, donut19, donut20, donut21, donut22, donut23, donut24, donut25, staleDonut;

    public GameObject submitButton;
    public GameObject victory_screen;

    public gridObject[,] grid = new gridObject[width, height];

    public GameObject previewScreen;

    public GameObject mission_object;
    mission thisMission;

    public GameObject puzzle_object;
    public GameObject obstacle_object;
    public GameObject dialogue_list;

    int current_dialogue = 0;
    int puzzle_donuts_remaining = 0;
    int puzzles_remaining = 0;
    int current_puzzle = 0;
    int happyNumber = 0;
    int sadNumber = 0;
    float preview_scale;

    public GameObject dog1, dog2, dog3, dog4;
    GameObject[] dogQueue = new GameObject[4];

    gridObject[] previewRow = new gridObject[width];
    GameObject[] donuts;
    List<gridObject> fallingDonuts;
    List<gridObject> checkingDonuts;
    List<GameObject> donutList;
    List<GameObject> scoreList;

    GameObject desired;
    //Desiretype indicates whether they strictly want that donut, want a donut of that type, or don't actually care
    //0: Standard desire by color or shape, 1: Weak desire, 2: Strong desire; donut must match exactly
    int desireType;

    private void Awake()
    {
        thisMission = mission_object.GetComponent<mission>();

        scoreList = new List<GameObject>();
        GameObject scoreObject1 = new GameObject();
        scoreObject1.AddComponent<score_text>();
        scoreList.Add(scoreObject1);
        GameObject scoreObject2 = new GameObject();
        scoreObject2.AddComponent<score_text>();
        scoreList.Add(scoreObject2);
        GameObject scoreObject3 = new GameObject();
        scoreObject3.AddComponent<score_text>();
        scoreList.Add(scoreObject3);
        GameObject scoreObject4 = new GameObject();
        scoreObject4.AddComponent<score_text>();
        scoreList.Add(scoreObject4);

        countingClips = new List<AudioClip>();
        countingClips.Add(Resources.Load("AudioClips/Counting_01") as AudioClip);
        countingClips.Add(Resources.Load("AudioClips/Counting_02") as AudioClip);
        countingClips.Add(Resources.Load("AudioClips/Counting_03") as AudioClip);
        countingClips.Add(Resources.Load("AudioClips/Counting_04") as AudioClip);
        countingClips.Add(Resources.Load("AudioClips/Counting_05") as AudioClip);
        countingClips.Add(Resources.Load("AudioClips/Counting_06") as AudioClip);
        countingClips.Add(Resources.Load("AudioClips/Counting_07") as AudioClip);
        countingClips.Add(Resources.Load("AudioClips/Counting_08") as AudioClip);
        countingClips.Add(Resources.Load("AudioClips/Counting_09") as AudioClip);
        countingClips.Add(Resources.Load("AudioClips/Counting_10") as AudioClip);
        //LOAD GLOBAL SPRITE RESOURCES
        globals.loadSprites();

        globals.isPaused = false;

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

        //CREATE LIST OF DONUT OBJECTS TO POOL FROM, INACTIVE DONUTS ARE BLANK SPACES
        donuts = new GameObject[width * height];
        for (int i = 0; i < width * height; i++)
        {
            donuts[i] = (GameObject)Instantiate(donutPrefab.gameObject, new Vector3(1f, 0f, 0f), Quaternion.identity);

            //donuts[i] = new GameObject();
            //donuts[i].AddComponent<SpriteRenderer>();
            //donuts[i].AddComponent<donut>();
        }


        //FILL GRID WITH EMPTY OBJECTS (INACTIVE DONUTS)
        int k = 0;
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                grid[j, i] = new gridObject();
                grid[j, i].donutObject = donuts[k];
                //grid[j, i].donutObject.transform.GetChild(1).GetComponent<MeshRenderer>().sortingOrder = 8;
                //grid[j, i].donutObject.transform.GetChild(2).GetComponent<MeshRenderer>().sortingOrder = 8;
                //grid[j, i].donutObject.transform.GetChild(3).GetComponent<MeshRenderer>().sortingOrder = 8;
                //grid[j, i].donutObject.transform.GetChild(4).GetComponent<MeshRenderer>().sortingOrder = 8;
                //grid[j, i].donutObject.transform.GetChild(5).GetComponent<MeshRenderer>().sortingOrder = 8;
                //Debug.Log("Filling grid with donut #" + k.ToString());
                //grid[j, i].donutObject.GetComponent<donut>().deactivate();
                k++;
            }
        }

        //INITIALIZE PREVIEW DONUTS
        for (int i = 0; i < width; i++)
        {
            previewRow[i] = new gridObject();
            previewRow[i].donutObject = (GameObject)Instantiate(donutList[Random.Range(0, donutList.Count)].gameObject, new Vector2(i, 9f), Quaternion.identity);
            //previewRow[i].donutObject.transform.position = new Vector3(i, 9f, 0f);
        }

        //INITIALIZE FALLING DONUT COLUMNS
        fallingDonuts = new List<gridObject>();
        checkingDonuts = new List<gridObject>();

        //USE FOR MOBILE
        if( isMobile )
            desired = (GameObject)Instantiate(donutList[Random.Range(0, donutList.Count)].gameObject, new Vector2(3f, -1.75f), Quaternion.identity);
        else
            desired = (GameObject)Instantiate(donutList[Random.Range(0, donutList.Count)].gameObject, new Vector2(8.25f, 4.25f), Quaternion.identity);
        if (mission_object.GetComponent<mission>().condition == 3 || mission_object.GetComponent<mission>().condition == 4)
            desired.GetComponent<SpriteRenderer>().enabled = false;
            //USE FOR DESKTOP/CONSOLE
            

            //POPULATE DOG QUEUE
            dogQueue[0] = dog1;
        dogQueue[1] = dog2;
        dogQueue[2] = dog3;
        dogQueue[3] = dog4;

        preview_scale = previewScreen.transform.localScale.y;
        
    }

    // Use this for initialization
    void Start () {

        if (thisMission.condition == 1 || thisMission.condition == 2)
        {
            submitting.enabled = false;
            submitButton.SetActive(false);
        }
        onTarget = false;
        touchDirection = 0f;

        //userInputField.gameObject.SetActive(false);
        if (thisMission.condition == 3)
        {
            //restartButton.onClick.AddListener(restart);
            //restartButton.gameObject.SetActive(false);
        }
        sky.GetComponent<Renderer>().sortingOrder = 0;

        //ADD DESIRED DONUT
        if (mission_object.GetComponent<mission>().condition == 3 || mission_object.GetComponent<mission>().condition == 4)
        {
            desired.GetComponent<SpriteRenderer>().enabled = false;
            donutImage.enabled = false;
        }
        else
        {
            desired.GetComponent<donut>().setDonutType(donutList[Random.Range(0, donutList.Count)].GetComponent<donut>());
            donutImage.sprite = desired.GetComponent<SpriteRenderer>().sprite;
        }
        
        //GENERATE FIRST PREVIEW ROW
        for (int i = 0; i < width; i++)
        {
            previewRow[i].donutObject.GetComponent<donut>().setDonutType(donutList[Random.Range(0, donutList.Count)].GetComponent<donut>());
            if( i > 0 )
            {
                //DO NOT LET DONUT MATCH PREVIOUS DONUT
                while(previewRow[i - 1].donutObject.GetComponent<donut>().anyMatch(previewRow[i].donutObject.GetComponent<donut>()))
                {
                    previewRow[i].donutObject.GetComponent<donut>().setDonutType(donutList[Random.Range(0, donutList.Count)].GetComponent<donut>());
                }
            }
            //previewRow[i].donutObject.transform.position = new Vector3(i, 9f, 0f);
        }

        //INITIALIZE SCORE
        if (thisMission.condition == 8)
        {
            score = System.Convert.ToInt32(scoreText.text);
        }
        else
        {
            score = 0;
            scoreText.text = score.ToString();
        }

        numMoves = 0;

        customersRemaining = mission_object.GetComponent<mission>().target_customers;
        customers.text = customersRemaining.ToString();

        gameOver.enabled = false;
        winner.enabled = false;
        currentLevel = 0;
        levelNumber.text = currentLevel.ToString();

        currentCombo = 0;
        currentStreak = 0;

        streakText.text = currentStreak.ToString();

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
        
        roundTimer = mission_object.GetComponent<mission>().time_limit;

        sfxSrc = GetComponent<AudioSource>();

        fallTimer = maxFallTimer;

        paused = false;

        if (mission_object.GetComponent<mission>().condition == 3 || mission_object.GetComponent<mission>().condition == 13 || thisMission.condition == 4)
        {
            puzzles_remaining = puzzle_object.transform.childCount - 1;
            customers.text = (puzzles_remaining + 1).ToString();
            current_puzzle = 0;
            if (hasObstacles)
            {
                PlaceObstaclesOnGrid(0);
            }
            PlaceDonutsOnGrid(0);
            

            if( mission_object.GetComponent<mission>().condition == 13 )
            {
                dialogue_list.transform.GetChild(current_dialogue).gameObject.SetActive(true);
                dialogue_list.transform.GetChild(current_dialogue).gameObject.GetComponent<MeshRenderer>().sortingOrder = 9;
            }
        }

        

        victory_screen.SetActive(false);
        if (thisMission.condition != 3 && thisMission.condition != 13 && thisMission.condition != 4 && thisMission.condition != 5 && thisMission.condition != 6)
        {
            AddRowSmart(2);
        }

        if (thisMission.condition == 5)
        {
            PlaceDonutsOnGrid(0);
            thisMission.condition = 1;
        }
        else if (thisMission.condition == 6)
        {
            PlaceDonutsOnGrid(0);
            thisMission.condition = 2;
        }
    }

    void PlaceDonutsOnGrid(int i)
    {
        foreach (Transform child in puzzle_object.transform.GetChild(i).transform)
        {
            if( child != puzzle_object.transform)
            {
                grid[(int)child.position.x, (int)child.position.y].donutObject.GetComponent<donut>().setDonutType(child.GetComponent<donut>());
                grid[(int)child.position.x, (int)child.position.y].donutObject.transform.position = new Vector2((int)child.position.x, (int)child.position.y);
                grid[(int)child.position.x, (int)child.position.y].x = (int)child.position.x;
                grid[(int)child.position.x, (int)child.position.y].y = (int)child.position.y;

                puzzle_donuts_remaining++;
            }
            
        }
        Debug.Log("Puzzle tiles: " + puzzle_donuts_remaining);
    }

    void PlaceObstaclesOnGrid(int i)
    {
        foreach (Transform child in obstacle_object.transform.GetChild(i).transform)
        {
            if (child != puzzle_object.transform)
            {
                grid[(int)child.position.x, (int)child.position.y].donutObject.GetComponent<donut>().setDonutType(child.GetComponent<donut>());
                grid[(int)child.position.x, (int)child.position.y].donutObject.transform.position = new Vector2((int)child.position.x, (int)child.position.y);
                grid[(int)child.position.x, (int)child.position.y].x = (int)child.position.x;
                grid[(int)child.position.x, (int)child.position.y].y = (int)child.position.y;

                //puzzle_donuts_remaining++;
            }

        }
        Debug.Log("Obstacle tiles: " + puzzle_donuts_remaining);
    }

    // Update is called once per frame
    void Update () {

        if( paused )
        {
            if( Input.GetKeyDown(KeyCode.Return) )
            {
                
                //restart();
            }
        }

        if( !paused && !globals.isPaused && timer > 0 )
        {
            if ((fallingDonuts.Count == 0 && mission_object.GetComponent<mission>().condition != 3 && mission_object.GetComponent<mission>().condition != 13) || thisMission.condition == 2)
            {
                float time = Time.deltaTime;
                timer -= time;
                previewScreen.transform.localScale -= new Vector3(0f, (time / maxTimer)*preview_scale, 0f);
            }
            //timerText.text = timer.ToString();

            //if ((roundTimer >= 0 & fallingDonuts.Count == 0) || thisMission.condition == 2)
            if ((roundTimer >= 0) || thisMission.condition == 2)
            {
                float timePassed = Time.deltaTime;
                timeBuffer -= timePassed;
                roundTimer -= timePassed;
                if( roundTimer <= 30 && timeBuffer <= 0 )
                {
                    roundTimerText.GetComponent<Animator>().SetBool("timeLow", true);
                    timeBuffer = 1f;
                    //roundTimerText.GetComponent<Animator>().SetBool("timeLow", false);
                }
                if (roundTimer < 0)
                    roundTimer = 0;
                roundTimerText.text = ((int)roundTimer).ToString();
            }
            if( roundTimer <= 0 )
            {
                if(mission_object.GetComponent<mission>().condition == 2 )
                {
                    //score -= 500 * sadNumber;
                    if (score < 0)
                        score = 0;
                    //scoreText.text = score.ToString();
                    globals.isPaused = true;
                    int rank = 0;
                    if (happyNumber > sadNumber)
                    {
                        rank = 1;
                        if (score > thisMission.target_score)
                            rank = 2;
                    }
                    int tip = 0;
                    if (rank == 2)
                        tip = thisMission.tip;
                    //score -= 500 * sadNumber;
                    globals.setStatus(thisMission.mission_id, score - (500 * sadNumber), (maxRoundTimer - roundTimer), rank, tip);
                    victory_screen.SetActive(true);
                    victory_screen.GetComponent<victory_screen>().SetStatsSurvival(score, sadNumber, (score - (sadNumber * 500)), table_id,isMobile, mission_object.GetComponent<mission>().tip);
                    
                    
                    //transform.GetChild(63).GetComponent<SpriteRenderer>().enabled = true;
                    //transform.GetChild(63).GetComponent<BoxCollider2D>().enabled = true;
                    //userInputField.gameObject.SetActive(true);
                    //submitButton.SetActive(true);
                    //userInputField.onEndEdit.AddListener(submitAndShowScore);
                }
                else
                    endgame();
                //gameOver.enabled = true;
                //paused = true;
                return;
            }

            if( timer <= 0 )
            {
                if( thisMission.condition == 8 && customersRemaining == 0 )
                {
                    endgame();
                }
                addRow();
                timer = maxTimer;
                previewScreen.transform.localScale = new Vector3(previewScreen.transform.localScale.x, preview_scale, 1f);
            }

            //CHECK PUZZLE COMPLETION
            if( puzzle_donuts_remaining <= 0 )
            {
                if (mission_object.GetComponent<mission>().condition == 3 || mission_object.GetComponent<mission>().condition == 4)
                {
                    if (puzzles_remaining > 0)
                    {
                        puzzles_remaining--;
                        customers.text = (puzzles_remaining + 1).ToString();
                        current_puzzle++;
                        PlaceDonutsOnGrid(current_puzzle);
                        PlaceObstaclesOnGrid(0);
                    }
                    else
                    {
                        puzzles_remaining--;
                        customers.text = (puzzles_remaining + 1).ToString();
                        transform.parent.GetComponent<AudioSource>().Stop();
                        sfxSrc.PlayOneShot(levelUpSfx);
                        globals.isPaused = true;
                        int rank = 1;
                        //if (numMoves < thisMission.target_moves)
                            //rank = 2;
                        globals.setStatusPuzzle(thisMission.mission_id, score, (maxRoundTimer - roundTimer), rank, 0);
                        victory_screen.SetActive(true);
                        victory_screen.GetComponent<victory_screen>().SetStatsPuzzle();
                        if( isMobile )
                        {
                            globals.SaveLocalData();
                        }
                        else
                            victory_screen.GetComponent<victory_screen>().SaveData();
                        
                        //transform.GetChild(63).GetComponent<SpriteRenderer>().enabled = true;
                        //transform.GetChild(63).GetComponent<BoxCollider2D>().enabled = true;
                    }
                }
                //CHECK IF MODE IS TUTORIAL
                else if (mission_object.GetComponent<mission>().condition == 13)
                {
                    if (puzzles_remaining > 0)
                    {
                        puzzles_remaining--;
                        customers.text = (puzzles_remaining + 1).ToString();
                        current_puzzle++;
                        PlaceDonutsOnGrid(current_puzzle);
                        dialogue_list.transform.GetChild(current_dialogue).gameObject.SetActive(false);
                        dialogue_list.transform.GetChild(current_dialogue++).gameObject.SetActive(true);
                        dialogue_list.transform.GetChild(current_dialogue).gameObject.GetComponent<MeshRenderer>().sortingOrder = 9;
                    }
                    else
                    {
                        puzzles_remaining--;
                        customers.text = (puzzles_remaining + 1).ToString();
                        transform.parent.GetComponent<AudioSource>().Stop();
                        sfxSrc.PlayOneShot(levelUpSfx);
                        globals.isPaused = true;
                        int rank = 1;
                        //if (numMoves < thisMission.target_moves)
                        //rank = 2;
                        //globals.setStatusPuzzle(thisMission.mission_id, score, (maxRoundTimer - roundTimer), rank, 0);
                        victory_screen.SetActive(true);
                        victory_screen.GetComponent<victory_screen>().SaveData();

                        //transform.GetChild(63).GetComponent<SpriteRenderer>().enabled = true;
                        //transform.GetChild(63).GetComponent<BoxCollider2D>().enabled = true;
                    }
                }
            }

            if (sadNumber >= thisMission.sad_limit)
            {
                Debug.Log("Should be game over");
                globals.isPaused = true;
                transform.GetChild(64).GetComponent<SpriteRenderer>().enabled = true;
                transform.GetChild(64).GetComponent<BoxCollider2D>().enabled = true;
            }

            //CHECK LEVEL UP CONDITION
            if (thisMission.condition == 8)
            {
                if (score <= 0)
                {
                    score = 0;
                    globals.isPaused = true;
                    victory_screen.SetActive(true);
                    int statusRank = 1;
                    score = (customersRemaining * 500) + ((int)roundTimer * 100);
                    if (thisMission.time_limit - roundTimer < thisMission.target_time)
                        statusRank = 2;
                    globals.setStatus(thisMission.mission_id, score, (maxRoundTimer - roundTimer), statusRank, thisMission.tip);
                    transform.parent.GetComponent<AudioSource>().Stop();
                    victory_screen.GetComponent<victory_screen>().SetStatsBoss(customersRemaining, roundTimer, table_id, isMobile, mission_object.GetComponent<mission>().tip);
                }
            }

            if (customersRemaining <= 0 && thisMission.condition != 8)
            {
                if (thisMission.condition != 2)
                {
                    //PLAY LEVEL UP SOUND
                    sfxSrc.PlayOneShot(levelUpSfx);
                    if (thisMission.condition == 4)
                    {
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
                    }

                    if (mission_object.GetComponent<mission>().condition == 1)
                    {
                        if( isMobile )
                        {
                            mobileCursor.SetActive(false);
                        }
                        globals.isPaused = true;
                        //score += (int)((int)roundTimer * 100);
                        //scoreText.text = score.ToString();
                        int statusRank = 1;
                        if (thisMission.time_limit - roundTimer < thisMission.target_time)
                            statusRank = 2;
                        int tip = 0;
                        if (statusRank == 2)
                            tip = thisMission.tip;
                        globals.setStatus(thisMission.mission_id, score + ((int)roundTimer * 100), (maxRoundTimer - roundTimer), statusRank, tip);
                        transform.parent.GetComponent<AudioSource>().Stop();
                        victory_screen.SetActive(true);
                        victory_screen.GetComponent<victory_screen>().SetStats(score, roundTimer, (score + ((int)roundTimer*100)), table_id, isMobile, mission_object.GetComponent<mission>().tip);
                        
                        //transform.GetChild(63).GetComponent<SpriteRenderer>().enabled = true;
                        //transform.GetChild(63).GetComponent<BoxCollider2D>().enabled = true;
                        //submitButton.SetActive(true);
                        //userInputField.gameObject.SetActive(true);
                        //userInputField.onEndEdit.AddListener(submitAndShowScore);

                    }

                    //RESET ROUND TIMER AND ADD EXTRA MINUTE
                    roundTimer += 60;// * currentLevel;

                    //REDUCE TIME FROM ROW TIMER
                    maxTimer -= 1f;

                    customersRemaining = nextLevel[currentLevel];
                    customers.text = customersRemaining.ToString();
                    //paused = true;
                }
            }
        }
        if( !paused && !globals.isPaused)
        {

            if (fallingDonuts.Count == 0)
                fallTimer = maxFallTimer;
            //RESOLVE FALLING DONUTS
            if( fallTimer > 0 && fallingDonuts.Count > 0 )
            {
                fallTimer -= Time.deltaTime;
                if( fallTimer <= 0 )
                {
                    //Debug.Log("This should be called rapidly");
                    //ITERATE BACKWARDS THROUGH LIST TO REMOVE INACTIVE DONUT ENTRIES
                    for (int i = fallingDonuts.Count - 1; i >= 0; i--)
                    {
                        if (!fallingDonuts[i].donutObject.GetComponent<donut>().getActive() || (fallingDonuts[i].donutObject.GetComponent<donut>().getActive() && !fallingDonuts[i].donutObject.GetComponent<donut>().canMove()))
                        {
                            fallingDonuts.Remove(fallingDonuts[i]);
                        }
                            
                    }

                    for (int i = fallingDonuts.Count - 1; i >= 0; i--)
                    {
                        // Debug.Log("Donut: " + fallingDonuts[i].x + ",0 " + " is " + grid[fallingDonuts[i].x, 0].donutObject.GetComponent<donut>().getActive());
                        if (fallingDonuts[i].y == 0)
                        {
                            //Debug.Log("Check match after fall at " + grid[fallingDonuts[i].x, fallingDonuts[i].y].x + "," + grid[fallingDonuts[i].x, fallingDonuts[i].y].y);
                            checkingDonuts.Add(grid[fallingDonuts[i].x, fallingDonuts[i].y]);
                            fallingDonuts.Remove(fallingDonuts[i]);
                        }
                        else if (grid[fallingDonuts[i].x, fallingDonuts[i].y - 1].donutObject.GetComponent<donut>().getActive())
                        {
                            //Debug.Log("Donut: " + fallingDonuts[i].x + "," + (fallingDonuts[i].y - 1) + " is " + grid[fallingDonuts[i].x, fallingDonuts[i].y - 1].donutObject.GetComponent<donut>().getActive());
                            checkingDonuts.Add(grid[fallingDonuts[i].x, fallingDonuts[i].y]);
                            fallingDonuts.Remove(fallingDonuts[i]);
                        }
                    }

                    for ( int i = 0; i < fallingDonuts.Count; i++ )
                    {
                        //Debug.Log("Donut: " + fallingDonuts[i].x + "," + fallingDonuts[i].y + " should fall");
                        //Debug.Log("Donut: " + fallingDonuts[i].x + "," + (fallingDonuts[i].y) + " is " + grid[fallingDonuts[i].x, fallingDonuts[i].y].donutObject.GetComponent<donut>().getActive());
                        //Debug.Log("Donut: " + fallingDonuts[i].x + "," + (fallingDonuts[i].y - 1) + " is " + grid[fallingDonuts[i].x, fallingDonuts[i].y - 1].donutObject.GetComponent<donut>().getActive());
                        gridObject gridTmp = grid[fallingDonuts[i].x, fallingDonuts[i].y - 1];
                        grid[fallingDonuts[i].x, fallingDonuts[i].y - 1] = grid[fallingDonuts[i].x, fallingDonuts[i].y];
                        //grid[fallingDonuts[i].x, fallingDonuts[i].y - 1].donutObject.transform.Translate(0f, -1f, 0f);
                        grid[fallingDonuts[i].x, fallingDonuts[i].y - 1].donutObject.GetComponent<donut>().setFall();
                        grid[fallingDonuts[i].x, fallingDonuts[i].y - 1].y--;
                        //fallingDonuts[i].y--;
                        grid[fallingDonuts[i].x, fallingDonuts[i].y] = gridTmp;
                        grid[fallingDonuts[i].x, fallingDonuts[i].y].y++;
                        
                        //ADD TOP ADJACENT DONUT ENTRY TO FALLING AFTER
                        if( fallingDonuts[i].y < height - 1 )
                        {
                            if( grid[fallingDonuts[i].x, fallingDonuts[i].y + 1].donutObject.GetComponent<donut>().getActive() && grid[fallingDonuts[i].x, fallingDonuts[i].y + 1].donutObject.GetComponent<donut>().canMove())
                                fallingDonuts.Add(grid[fallingDonuts[i].x, fallingDonuts[i].y + 1]);
                        }
                        fallingDonuts[i] = grid[fallingDonuts[i].x, fallingDonuts[i].y - 1];
                        
                    }
                    //CHECK FOR STOP CONDITION BEFORE AND AFTER FALLING
                    for (int i = fallingDonuts.Count - 1; i >= 0; i--)
                    {
                        // Debug.Log("Donut: " + fallingDonuts[i].x + ",0 " + " is " + grid[fallingDonuts[i].x, 0].donutObject.GetComponent<donut>().getActive());
                        if (fallingDonuts[i].y == 0)
                        {
                            Debug.Log("Check match after fall at " + grid[fallingDonuts[i].x, fallingDonuts[i].y].x + "," + grid[fallingDonuts[i].x, fallingDonuts[i].y].y);
                            checkingDonuts.Add(grid[fallingDonuts[i].x, fallingDonuts[i].y]);
                            fallingDonuts.Remove(fallingDonuts[i]);
                        }
                        else if (grid[fallingDonuts[i].x, fallingDonuts[i].y - 1].donutObject.GetComponent<donut>().getActive())
                        {
                            //Debug.Log("Donut: " + fallingDonuts[i].x + "," + (fallingDonuts[i].y - 1) + " is " + grid[fallingDonuts[i].x, fallingDonuts[i].y - 1].donutObject.GetComponent<donut>().getActive());
                            checkingDonuts.Add(grid[fallingDonuts[i].x, fallingDonuts[i].y]);
                            fallingDonuts.Remove(fallingDonuts[i]);
                        }
                    }

                    fallTimer = maxFallTimer;
                }
            }
        }

        if( fallingDonuts.Count == 0 && checkingDonuts.Count > 0 )
        {
            bool matchFound = false;
            for( int i = checkingDonuts.Count - 1; i >= 0; i-- )
            {
                if( checkMatch(grid[checkingDonuts[i].x, checkingDonuts[i].y].x, grid[checkingDonuts[i].x, checkingDonuts[i].y].y) )
                {
                    matchFound = true;
                }
                checkingDonuts.Remove(checkingDonuts[i]);
            }
            if( justSwapped && !matchFound )
            {
                currentStreak = 0;
                streakText.text = currentStreak.ToString();
            }
            justSwapped = false;
        }
        for( int i = 0; i < width; i++ )
        {
            //Debug.Log("Donut: " + i + ",0 " + " is " + grid[i, 0].donutObject.GetComponent<donut>().getActive());
        }

        //POLL ANDROID BACK BUTTON
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("month_01_android");
        }

        //POLL TOUCH OR MOUSE INPUT
        if ( Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began )
        {
            Debug.Log("Touched screen at ");
        }
        if( Input.GetMouseButtonUp(0) )
        {
            onTarget = false;
        }
        //if( Input.GetMouseButtonDown(0) )
        if (Input.touchCount > 0)
        {
            thisTouch = Input.GetTouch(0);
            if( thisTouch.phase == TouchPhase.Began )
            {
                
                mouseTarget = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                mouseTarget.x += 0.5f;
                mouseTarget.y += 0.5f;
                Debug.Log("Touched screen at " + mouseTarget.x + "," + mouseTarget.y);
                if( mouseTarget.y > 0 && mouseTarget.y < height && mouseTarget.x > 0 && mouseTarget.x < width )
                {
                    //mobileCursor.transform.position = new Vector3((int)(mouseTarget.x), (int)(mouseTarget.y), 0f);
                    onTarget = true;
                    touchStart = Input.GetTouch(0).position;
                }
                else if ( mouseTarget.y > height )
                {
                    addRow();
                }
            }
            if (onTarget && thisTouch.phase == TouchPhase.Moved && thisTouch.deltaPosition.magnitude > 4f)
            {
                //mobileCursor.transform.position = new Vector3(-100f, -100f, 0f);
                if (thisTouch.position.x - touchStart.x > 1f)
                {
                    //touchDirection = Input.GetTouch(0).deltaPosition.x;
                    swap((int)mouseTarget.x, (int)mouseTarget.y);
                }
                else if (thisTouch.position.x - touchStart.x < -1f)
                {
                    swap((int)mouseTarget.x - 1, (int)mouseTarget.y);
                }
                onTarget = false;
            }
        }
        
        if( onTarget )
        {
            Debug.Log("Should be checking for mouse movement");
        }
        
    }

    GameObject getNextDonut()
    {
        for( int i = 0; i < width * height; i++ )
        {
            if (!donuts[i].GetComponent<donut>().getActive())
            {
                Debug.Log("Returning donut: " + i.ToString());
                return donuts[i];
            }
        }
        //NULL CODE SHOULD NEVER BE RETURNED
        Debug.Log("Could not find inactive donut");
        return null;
    }

    public void swap( int x, int y )
    {

        if ( (grid[x, y].donutObject.GetComponent<donut>().getActive() && !grid[x, y].donutObject.GetComponent<donut>().canMove()) || 
            (grid[x + 1, y].donutObject.GetComponent<donut>().getActive() && !grid[x + 1, y].donutObject.GetComponent<donut>().canMove()))
            return;
        
        numMoves++;

        if (x < 0 || x > width - 1)
            return;
        currentCombo = 0;

        if (fallingDonuts.Count > 0)
            return;
        gridObject gridTmp = grid[x, y];
        grid[x, y] = grid[ x + 1, y];
        grid[x, y].x--;
        grid[x, y].donutObject.GetComponent<donut>().setSwap(-1);
        //grid[x, y].donutObject.transform.Translate(-1f, 0f, 0f);
        grid[x + 1, y] = gridTmp;
        grid[x + 1, y].x++;
        grid[x + 1, y].donutObject.GetComponent<donut>().setSwap(1);
        //grid[x + 1, y].donutObject.transform.Translate(1f, 0f, 0f);

        

        //ADD BOTH SWAPPED ITEMS TO CHECKLIST
        if (grid[x, y].donutObject.GetComponent<donut>().getActive())
        {
            if (y > 0 && !grid[x, y - 1].donutObject.GetComponent<donut>().getActive())
            {
                fallingDonuts.Add(grid[x, y]);
            }
            else
            {
                checkingDonuts.Add(grid[x, y]);
            }
        }
        else if( y < height - 1 && grid[x, y + 1].donutObject.GetComponent<donut>().getActive())
        {
            fallingDonuts.Add(grid[x, y + 1]);
        }
        if (grid[x + 1, y].donutObject.GetComponent<donut>().getActive())
        {
            if (y > 0 && !grid[x + 1, y - 1].donutObject.GetComponent<donut>().getActive())
            {
                fallingDonuts.Add(grid[x + 1, y]);
            }
            else
            {
                checkingDonuts.Add(grid[x + 1, y]);
            }
        }
        else if (y < height - 1 && grid[x + 1, y + 1].donutObject.GetComponent<donut>().getActive())
        {
            fallingDonuts.Add(grid[x + 1, y + 1]);
        }

        justSwapped = true;

        //IEnumerator shiftDownDelay( int x, int y, float delay )
        //{
        /*
        Debug.Log("Grid object: " + x.ToString() + "," + y.ToString());
        if (grid[x, y] == null)
        {
            grid[x, y] = new GameObject();
            yield break;
        }
        if( grid[x, y].GetComponent<donut>() != null )
        {
            while (globals.isPaused)
            { yield return new WaitForSecondsRealtime(0.01f); }

            if (grid[x, y].GetComponent<donut>() != null)
            {
                if (grid[x, y].GetComponent<donut>().isMatched())
                    yield break;
                grid[x, y].GetComponent<donut>().drop();
            }
            float delayAdjust = 0.01f;
            //WAIT UNTIL MATCHED DONUTS ARE RESOLVED
            
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
                //grid[x, y].transform.Translate(0f, 1f, 0f);
                    
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

            */
    }
    //}

    void shiftDown( int x, int y )
    {
        /*
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
            
        }*/
    }

    IEnumerator fillEmptySpace(int x, int y, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        //grid[x, y] = new GameObject();
    }

    void checkNextMatchLeftColor(int x, int y, List<coord> list)
    {
        if( x > 0 && grid[x - 1, y].donutObject.GetComponent<donut>() != null && grid[x - 1, y].donutObject.GetComponent<donut>().matchColor(grid[x,y].donutObject.GetComponent<donut>()) )
        {
            list.Add(new coord(x - 1, y));
            checkNextMatchLeftColor(x - 1, y, list);
        }
    }

    void checkNextMatchRightColor(int x, int y, List<coord> list)
    {
        if (x < width - 1 && grid[x + 1, y].donutObject.GetComponent<donut>() != null && grid[x + 1, y].donutObject.GetComponent<donut>().matchColor(grid[x, y].donutObject.GetComponent<donut>()))
        {
            list.Add(new coord(x + 1, y));
            checkNextMatchRightColor(x + 1, y, list);
        }
    }

    void checkNextMatchUpColor(int x, int y, List<coord> list)
    {
        if (y < height - 1 && grid[x, y + 1].donutObject.GetComponent<donut>() != null && grid[x, y + 1].donutObject.GetComponent<donut>().matchColor(grid[x, y].donutObject.GetComponent<donut>()))
        {
            list.Add(new coord(x, y + 1));
            checkNextMatchUpColor(x, y + 1, list);
        }
    }

    void checkNextMatchDownColor(int x, int y, List<coord> list)
    {
        if (y > 0 && grid[x, y - 1].donutObject.GetComponent<donut>() != null && grid[x, y - 1].donutObject.GetComponent<donut>().matchColor(grid[x, y].donutObject.GetComponent<donut>()))
        {
            list.Add(new coord(x, y - 1));
            checkNextMatchDownColor(x, y - 1, list);
        }
    }

    void checkNextMatchLeftShape(int x, int y, List<coord> list)
    {
        if (x > 0 && grid[x - 1, y].donutObject.GetComponent<donut>() != null && grid[x - 1, y].donutObject.GetComponent<donut>().matchShape(grid[x, y].donutObject.GetComponent<donut>()))
        {
            list.Add(new coord(x - 1, y));
            checkNextMatchLeftShape(x - 1, y, list);
        }
    }

    void checkNextMatchRightShape(int x, int y, List<coord> list)
    {
        if (x < width - 1 && grid[x + 1, y].donutObject.GetComponent<donut>() != null && grid[x + 1, y].donutObject.GetComponent<donut>().matchShape(grid[x, y].donutObject.GetComponent<donut>()))
        {
            list.Add(new coord(x + 1, y));
            checkNextMatchRightShape(x + 1, y, list);
        }
    }

    void checkNextMatchUpShape(int x, int y, List<coord> list)
    {
        if (y < height - 1 && grid[x, y + 1].donutObject.GetComponent<donut>() != null && grid[x, y + 1].donutObject.GetComponent<donut>().matchShape(grid[x, y].donutObject.GetComponent<donut>()))
        {
            list.Add(new coord(x, y + 1));
            checkNextMatchUpShape(x, y + 1, list);
        }
    }

    void checkNextMatchDownShape(int x, int y, List<coord> list)
    {
        if (y > 0 && grid[x, y - 1].donutObject.GetComponent<donut>() != null && grid[x, y - 1].donutObject.GetComponent<donut>().matchShape(grid[x, y].donutObject.GetComponent<donut>()))
        {
            list.Add(new coord(x, y - 1));
            checkNextMatchDownShape(x, y - 1, list);
        }
    }

    void resolveList( List<coord> list)
    {
        
        //SET FLAG TO CHECK DESIRED DONUT
        bool desireHit = false;
        bool exact = false;
        //ADD 100 POINTS FOR INITIAL MATCH
        int thisScore = 100 + 100 * (list.Count);
        //score += 100;
        for ( int i = 0; i < list.Count; i++ )
        {
            donut d = grid[list[i].getX(), list[i].getY()].donutObject.GetComponent<donut>();
            //StartCoroutine(fillEmptySpace(list[i].getX(), list[i].getY(), fillDelay));
            //StartCoroutine(shiftDownDelay(list[i].getX(), list[i].getY() + 1, shiftDelay));
            if ( d.exactMatch(desired.GetComponent<donut>()) )
            {
                thisScore += 200;
                desireHit = true;
                exact = true;
            }
            else if( d.anyMatch(desired.GetComponent<donut>()) )
            {
                thisScore += 100;
                if( desireType < 2 )
                    desireHit = true;
            }
            else if( desireType == 1 )
            {
                desireHit = true;
            }
            d.setMatch();
            if (thisMission.condition != 4)
            {
                puzzle_donuts_remaining--;
            }
            else
            {
                if( d.flavor == 5 )
                {
                    puzzle_donuts_remaining--;
                }
            }
            Debug.Log("Puzzle tiles remaining: " + puzzle_donuts_remaining);
            fallingDonuts.Add(grid[list[i].getX(), list[i].getY() + 1]);
        }
        thisScore *= (currentStreak + 1);
        thisScore *= (currentCombo + 1);
        
        if (!desireHit && currentCombo == 0)
            thisScore = 0;
        //TextMeshPro tmp;
        if (thisMission.condition == 8)
        {
            score -= thisScore;
        }
        else
        {
            score += thisScore;
        }
        scoreText.text = score.ToString();
        if (thisMission.condition != 3 && thisMission.condition != 4)
        {
            string s = "";
            if ((currentStreak > 0 && desireHit) || currentCombo > 0)
            {
                s += "x" + (currentStreak + 1) + "  ";
            }
            s += thisScore.ToString();
            if (exact)
            {
                s += "\r\nExact Match!";
            }
            for ( int i = 0; i < scoreList.Count; i++ )
            {
                if( !scoreList[i].GetComponent<score_text>().isInUse() )
                {
                    scoreList[i].GetComponent<score_text>().setScore(s, new Vector2(list[0].getX(), list[0].getY()));
                    break;
                }
            }
            
                /*Transform meshObject1 = grid[list[0].getX(), list[0].getY()].donutObject.transform.GetChild(1);
                Transform meshObject2 = grid[list[0].getX(), list[0].getY()].donutObject.transform.GetChild(2);
                Transform meshObject3 = grid[list[0].getX(), list[0].getY()].donutObject.transform.GetChild(3);
                Transform meshObject4 = grid[list[0].getX(), list[0].getY()].donutObject.transform.GetChild(4);
                Transform meshObject5 = grid[list[0].getX(), list[0].getY()].donutObject.transform.GetChild(5);

                TextMesh mesh = grid[list[0].getX(), list[0].getY()].donutObject.transform.GetChild(1).GetComponent<TextMesh>();
                TextMesh mesh1 = grid[list[0].getX(), list[0].getY()].donutObject.transform.GetChild(2).GetComponent<TextMesh>();
                TextMesh mesh2 = grid[list[0].getX(), list[0].getY()].donutObject.transform.GetChild(3).GetComponent<TextMesh>();
                TextMesh mesh3 = grid[list[0].getX(), list[0].getY()].donutObject.transform.GetChild(4).GetComponent<TextMesh>();
                TextMesh mesh4 = grid[list[0].getX(), list[0].getY()].donutObject.transform.GetChild(5).GetComponent<TextMesh>();
                mesh.color = Color.white;
                //mesh.outlineColor = Color.black;

                Vector3 tempPos = new Vector3(0f, 1f, -1f);

                mesh.fontSize = 32;
                meshObject1.localScale = new Vector2(0.25f, 0.25f);
                meshObject1.GetComponent<RectTransform>().localPosition = tempPos;
                mesh.GetComponent<Renderer>().sortingOrder = 9;
                mesh1.fontSize = 32;
                meshObject2.localScale = new Vector2(0.25f, 0.25f);
                //tempPos = new Vector3(0f, -0.04f, 0f);
                tempPos = new Vector3(0f, 0.96f, 0f);
                meshObject2.GetComponent<RectTransform>().localPosition = tempPos;
                mesh1.GetComponent<Renderer>().sortingOrder = 8;
                mesh2.fontSize = 32;

                meshObject3.localScale = new Vector2(0.25f, 0.25f);
                tempPos = new Vector3(0f, 1.04f, 0f);
                meshObject3.GetComponent<RectTransform>().localPosition = tempPos;
                mesh2.GetComponent<Renderer>().sortingOrder = 8;
                mesh3.fontSize = 32;
                meshObject4.localScale = new Vector2(0.25f, 0.25f);
                tempPos = new Vector3(0.04f, 1f, 0f);
                meshObject4.GetComponent<RectTransform>().localPosition = tempPos;
                mesh3.GetComponent<Renderer>().sortingOrder = 8;
                mesh4.fontSize = 32;
                meshObject5.localScale = new Vector2(0.25f, 0.25f);
                tempPos = new Vector3(-0.04f, 1f, 0f);
                meshObject5.GetComponent<RectTransform>().localPosition = tempPos;
                mesh.GetComponent<Renderer>().sortingOrder = 8;

                mesh.text = "";
                mesh1.text = "";
                mesh2.text = "";
                mesh3.text = "";
                mesh4.text = "";

                if ( (currentStreak > 0 && desireHit ) || currentCombo > 0 )
                {
                    mesh.text += "x" + (currentStreak + 1) + "  ";
                    mesh1.text += "x" + (currentStreak + 1) + "  "; 
                    mesh2.text += "x" + (currentStreak + 1) + "  "; 
                    mesh3.text += "x" + (currentStreak + 1) + "  "; 
                    mesh4.text += "x" + (currentStreak + 1) + "  "; 
                }
                mesh.text += thisScore.ToString();
                mesh1.text += thisScore.ToString();
                mesh2.text += thisScore.ToString();
                mesh3.text += thisScore.ToString();
                mesh4.text += thisScore.ToString();
                scoreText.text = score.ToString();

                if( exact )
                {
                    mesh.text += "\r\nExact Match!";
                    mesh1.text += "\r\nExact Match!";
                    mesh2.text += "\r\nExact Match!";
                    mesh3.text += "\r\nExact Match!";
                    mesh4.text += "\r\nExact Match!";
                }*/
            }
        sfxSrc.PlayOneShot(matchSound);
        if (desireHit)
        {
            if (currentStreak > 0)
            {
                //GIVE PLAYER MORE TIME TO ADD TO STREAK
                timer = maxTimer;
                previewScreen.transform.localScale = new Vector3(previewScreen.transform.localScale.x, preview_scale, 1f);
            }
            //REWARD PLAYER WITH EXTRA TIME FOR STREAK
            if( thisMission.condition == 4 )
                roundTimer += currentStreak;

            currentStreak++;
            streakText.GetComponent<Animator>().SetBool("timeLow", true);
            if( currentStreak > 1 || currentCombo > 0 )
            {
                sfxSrc.PlayOneShot(countingClips[currentStreak - 1]);
            }

            //Destroy(desired.gameObject);
            int oldFlavor = desired.GetComponent<donut>().getFlavor();
            int oldShape = desired.GetComponent<donut>().getShape();
            desired.GetComponent<donut>().setDonutType(donutList[Random.Range(0, donutList.Count)].GetComponent<donut>());
            while (desired.GetComponent<donut>().getFlavor() == oldFlavor && desired.GetComponent<donut>().getShape() == oldShape)
            {
                desired.GetComponent<donut>().setDonutType(donutList[Random.Range(0, donutList.Count)].GetComponent<donut>());
            }
            //desired.GetComponent<donut>().setDonutType(donutList[Random.Range(0, donutList.Count)].GetComponent<donut>());
            donutImage.sprite = desired.GetComponent<SpriteRenderer>().sprite;
            desireType = Random.Range(0, 3);
            if (desireType > 0)
            {
                if (!allowWilds)
                    desireType = 2;
                if (!allowStrict)
                    desireType = 1;
                if (!allowWilds && !allowStrict)
                    desireType = 0;
            }
            Debug.Log("Desire: " + desireType);
            switch (desireType)
            {
                case 0:
                    desire.text = "";
                    break;
                case 1:
                    desire.text = "?";
                    break;
                case 2:
                    desire.text = "!";
                    break;
                default:
                    desire.text = "";
                    break;

            }
            if (mission_object.GetComponent<mission>().condition == 3 || mission_object.GetComponent<mission>().condition == 4)
                desired.GetComponent<SpriteRenderer>().enabled = false;
            if( thisMission.condition != 8 )
                customersRemaining--;
            if( thisMission.condition != 3 && thisMission.condition != 4)
                customers.text = customersRemaining.ToString();
            //CYCLE THROUGH DOG QUEUE
            if (mission_object.GetComponent<mission>().condition != 3 && thisMission.condition != 8 && thisMission.condition != 4)
            {
                Vector3 tmpDogPosition = dogQueue[3].transform.position;
                GameObject tmpDog = dogQueue[3];
                dogQueue[3].transform.position = new Vector3(dogQueue[2].transform.position.x, dogQueue[3].transform.position.y, 0f);
                dogQueue[2].transform.position = new Vector3(dogQueue[1].transform.position.x, dogQueue[2].transform.position.y, 0f);
                dogQueue[1].transform.position = new Vector3(dogQueue[0].transform.position.x, dogQueue[1].transform.position.y, 0f);
                dogQueue[0].transform.position = new Vector3(tmpDogPosition.x, dogQueue[0].transform.position.y, 0f);
                //dogQueue[0] = dogQueue[1];
                // dogQueue[0].transform.Translate(tmpDog.transform.position.x, 0f, 0f);
                //dogQueue[1] = dogQueue[2];
                //dogQueue[1].transform.Translate(dogQueue[0].transform.position.x, 0f, 0f);
                //dogQueue[2] = dogQueue[3];
                //dogQueue[2].transform.Translate(-2.3f, 0f, 0f);
                //dogQueue[3] = tmpDog;
                // dogQueue[3].transform.Translate(7.8f, 0f, 0f);
            }
            happyNumber++;
            happyText.text = happyNumber.ToString();
        }
        else 
        {
            if (currentCombo == 0)
            {
                currentStreak = 0;
            }
            if (thisMission.condition == 2 && currentCombo == 0)
            {
                int oldFlavor = desired.GetComponent<donut>().getFlavor();
                int oldShape = desired.GetComponent<donut>().getShape();
                desired.GetComponent<donut>().setDonutType(donutList[Random.Range(0, donutList.Count)].GetComponent<donut>());
                while (desired.GetComponent<donut>().getFlavor() == oldFlavor && desired.GetComponent<donut>().getShape() == oldShape)
                {
                    desired.GetComponent<donut>().setDonutType(donutList[Random.Range(0, donutList.Count)].GetComponent<donut>());
                }
                donutImage.sprite = desired.GetComponent<SpriteRenderer>().sprite;
                desireType = Random.Range(0, 3);
                
                if( desireType > 0 )
                {
                    if (!allowWilds)
                        desireType = 2;
                    if (!allowStrict)
                        desireType = 1;
                    if (!allowWilds && !allowStrict)
                        desireType = 0;
                }
                Debug.Log("Desire: " + desireType);
                switch (desireType)
                {
                    case 0:
                        desire.text = "";
                        break;
                    case 1:
                        desire.text = "?";
                        break;
                    case 2:
                        desire.text = "!";
                        break;
                    default:
                        desire.text = "";
                        break;

                }
                Vector3 tmpDogPosition = dogQueue[3].transform.position;
                GameObject tmpDog = dogQueue[3];
                dogQueue[3].transform.position = new Vector3(dogQueue[2].transform.position.x, dogQueue[2].transform.position.y, 0f);
                dogQueue[2].transform.position = new Vector3(dogQueue[1].transform.position.x, dogQueue[1].transform.position.y, 0f);
                dogQueue[1].transform.position = new Vector3(dogQueue[0].transform.position.x, dogQueue[0].transform.position.y, 0f);
                dogQueue[0].transform.position = new Vector3(tmpDogPosition.x, tmpDogPosition.y, 0f);

                satisfied = false;
                sadNumber++;
                sadText.text = sadNumber.ToString();
            }
        }
        if( currentStreak > 0 )
            currentCombo++;
        streakText.text = currentStreak.ToString();
    }

    bool checkMatch( int x, int y )
    {
        List<coord> tmpList = new List<coord>();

        //CHECK LEFT SIDE FOR MATCH
        //if (grid[x, y].donutObject.GetComponent<donut>() == null)
        //return;

        Debug.Log("Checking match for donut at " + x + ", " + y);
        if (x - 1 > 0)
        {
            //Debug.Log("Should check left donuts");
            //Debug.Log("Left match!");
            if (grid[x, y].donutObject.GetComponent<donut>().matchColor(grid[x - 1, y].donutObject.GetComponent<donut>()) && grid[x, y].donutObject.GetComponent<donut>().matchColor(grid[x - 2, y].donutObject.GetComponent<donut>()))
            {
                Debug.Log("Matched three left!");
                //ADD MATCHING OBJECTS TO BE ADDED TO LIST TO BE RESOLVED LATER
                tmpList.Add(new coord(x, y));
                tmpList.Add(new coord(x - 1, y));
                tmpList.Add(new coord(x - 2, y));
                //CHECK FOR FURTHER MATCHES TO LEFT & RIGHT
                checkNextMatchLeftColor(x - 2, y, tmpList);
                checkNextMatchRightColor(x, y, tmpList);
                resolveList(tmpList);
                //grid[x - 2, y].GetComponent<donut>().setMatch();
                //grid[x - 1, y].GetComponent<donut>().setMatch();
                //grid[x, y].GetComponent<donut>().setMatch();



                /*score += 300;
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
                StartCoroutine(shiftDownDelay(x, y + 1, shiftDelay));*/
                return true;
            }
                //IF THE LAST DONUT DOESN'T MATCH THE SAME ATTRIBUTE, CHECK AGAINST THE SECONDARY ATTRIBUTE
                else if (grid[x, y].donutObject.GetComponent<donut>().matchShape(grid[x - 1, y].donutObject.GetComponent<donut>()) && grid[x, y].donutObject.GetComponent<donut>().matchShape(grid[x - 2, y].donutObject.GetComponent<donut>()) )
                {
                    //Debug.Log("Matched three!");
                    //grid[x - 2, y].GetComponent<donut>().setMatch();
                    //grid[x - 1, y].GetComponent<donut>().setMatch();
                    //grid[x, y].GetComponent<donut>().setMatch();

                    tmpList.Add(new coord(x, y));
                    tmpList.Add(new coord(x - 1, y));
                    tmpList.Add(new coord(x - 2, y));
                    //CHECK FOR FURTHER MATCHES TO LEFT & RIGHT
                    checkNextMatchLeftShape(x - 2, y, tmpList);
                    checkNextMatchRightShape(x, y, tmpList);
                    resolveList(tmpList);

                    /*score += 300;

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
                    */
                    return true;
                }
            }
        
        //CHECK RIGHT SIDE FOR MATCH
        if (x + 1 < width - 1)
        {
            //Debug.Log("Right match!");
            if (grid[x, y].donutObject.GetComponent<donut>().matchColor(grid[x + 1, y].donutObject.GetComponent<donut>()) && grid[x, y].donutObject.GetComponent<donut>().matchColor(grid[x + 2, y].donutObject.GetComponent<donut>()))
            {
                tmpList.Add(new coord(x, y));
                tmpList.Add(new coord(x + 1, y));
                tmpList.Add(new coord(x + 2, y));
                //CHECK FOR FURTHER MATCHES TO LEFT & RIGHT
                checkNextMatchLeftColor(x, y, tmpList);
                checkNextMatchRightColor(x + 2, y, tmpList);
                resolveList(tmpList);

                /*Debug.Log("Matched three!");
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
                */
                return true;
            }
            else if (grid[x, y].donutObject.GetComponent<donut>().matchShape(grid[x + 1, y].donutObject.GetComponent<donut>()) && grid[x, y].donutObject.GetComponent<donut>().matchShape(grid[x + 2, y].donutObject.GetComponent<donut>()))
            {

                tmpList.Add(new coord(x, y));
                tmpList.Add(new coord(x + 1, y));
                tmpList.Add(new coord(x + 2, y));
                //CHECK FOR FURTHER MATCHES TO LEFT & RIGHT
                checkNextMatchLeftShape(x, y, tmpList);
                checkNextMatchRightShape(x + 2, y, tmpList);
                resolveList(tmpList);

                /*Debug.Log("Matched three!");
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
                */
                return true;
            }
        }

        //CHECK MIDDLE FOR MATCH
        if (x > 0 && x < width - 1 ) 
        {
            //Debug.Log("Right match!");
            if (grid[x, y].donutObject.GetComponent<donut>().matchColor(grid[x - 1, y].donutObject.GetComponent<donut>()) && grid[x, y].donutObject.GetComponent<donut>().matchColor(grid[x + 1, y].donutObject.GetComponent<donut>()))
            {

                tmpList.Add(new coord(x, y));
                tmpList.Add(new coord(x + 1, y));
                tmpList.Add(new coord(x - 1, y));
                //CHECK FOR FURTHER MATCHES TO LEFT & RIGHT
                checkNextMatchLeftColor(x - 1, y, tmpList);
                checkNextMatchRightColor(x + 1, y, tmpList);
                resolveList(tmpList);

                /*Debug.Log("Matched three!");
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
                */
                return true;
            }
            else if (grid[x, y].donutObject.GetComponent<donut>().matchShape(grid[x + 1, y].donutObject.GetComponent<donut>()) && grid[x, y].donutObject.GetComponent<donut>().matchShape(grid[x - 1, y].donutObject.GetComponent<donut>()))
            {

                tmpList.Add(new coord(x, y));
                tmpList.Add(new coord(x + 1, y));
                tmpList.Add(new coord(x - 1, y));
                //CHECK FOR FURTHER MATCHES TO LEFT & RIGHT
                checkNextMatchLeftShape(x - 1, y, tmpList);
                checkNextMatchRightShape(x + 1, y, tmpList);
                resolveList(tmpList);

                /*Debug.Log("Matched three!");
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
                StartCoroutine(shiftDownDelay(x, y + 1, shiftDelay));*/
                
                return true;
            }
        }
        //CHECK TOP MATCH
        if (y + 1 < height - 1)
        {
            //Debug.Log("Top match!");
            if (grid[x, y].donutObject.GetComponent<donut>().matchColor(grid[x, y + 1].donutObject.GetComponent<donut>()) && grid[x, y].donutObject.GetComponent<donut>().matchColor(grid[x, y + 2].donutObject.GetComponent<donut>()))
            {

                tmpList.Add(new coord(x, y));
                tmpList.Add(new coord(x, y + 1));
                tmpList.Add(new coord(x, y + 2));
                //CHECK FOR FURTHER MATCHES TO LEFT & RIGHT
                checkNextMatchUpColor(x, y + 2, tmpList);
                checkNextMatchDownColor(x, y, tmpList);
                resolveList(tmpList);

                /*Debug.Log("Matched three!");
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
                */
                return true;
            }
            else if (grid[x, y].donutObject.GetComponent<donut>().matchShape(grid[x, y + 1].donutObject.GetComponent<donut>()) && grid[x, y].donutObject.GetComponent<donut>().matchShape(grid[x, y + 2].donutObject.GetComponent<donut>()))
            {

                tmpList.Add(new coord(x, y));
                tmpList.Add(new coord(x, y + 1));
                tmpList.Add(new coord(x, y + 2));
                //CHECK FOR FURTHER MATCHES TO LEFT & RIGHT
                checkNextMatchUpShape(x, y + 2, tmpList);
                checkNextMatchDownShape(x, y, tmpList);
                resolveList(tmpList);

                /*Debug.Log("Matched three!");
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
                */
                return true;
            }
        }

        //CHECK MIDDLE MATCH
        if (y < height - 1 && y > 0)
        {
            //Debug.Log("Top match!");
            if (grid[x, y].donutObject.GetComponent<donut>().matchColor(grid[x, y + 1].donutObject.GetComponent<donut>()) && grid[x, y].donutObject.GetComponent<donut>().matchColor(grid[x, y - 1].donutObject.GetComponent<donut>()))
            {

                tmpList.Add(new coord(x, y));
                tmpList.Add(new coord(x, y + 1));
                tmpList.Add(new coord(x, y - 1));
                //CHECK FOR FURTHER MATCHES TO LEFT & RIGHT
                checkNextMatchUpColor(x, y + 1, tmpList);
                checkNextMatchDownColor(x, y - 1, tmpList);
                resolveList(tmpList);

                /*Debug.Log("Matched three middle!");
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
                */
                return true;
            }
            else if (grid[x, y].donutObject.GetComponent<donut>().matchShape(grid[x, y + 1].donutObject.GetComponent<donut>()) && grid[x, y].donutObject.GetComponent<donut>().matchShape(grid[x, y - 1].donutObject.GetComponent<donut>()))
            {

                tmpList.Add(new coord(x, y));
                tmpList.Add(new coord(x, y + 1));
                tmpList.Add(new coord(x, y - 1));
                //CHECK FOR FURTHER MATCHES TO LEFT & RIGHT
                checkNextMatchUpShape(x, y + 1, tmpList);
                checkNextMatchDownShape(x, y - 1, tmpList);
                resolveList(tmpList);

                /*Debug.Log("Matched three middle!");
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
                */
                return true;
            }
        }

        //CHECK BOTTOM MATCH
        if (y - 1 > 0)
        {
            //Debug.Log("Bottom match!");
            if (grid[x, y].donutObject.GetComponent<donut>().matchColor(grid[x, y - 1].donutObject.GetComponent<donut>()) && grid[x, y].donutObject.GetComponent<donut>().matchColor(grid[x, y - 2].donutObject.GetComponent<donut>()))
            {

                tmpList.Add(new coord(x, y));
                tmpList.Add(new coord(x, y - 1));
                tmpList.Add(new coord(x, y - 2));
                //CHECK FOR FURTHER MATCHES TO LEFT & RIGHT
                checkNextMatchUpColor(x, y, tmpList);
                checkNextMatchDownColor(x, y - 2, tmpList);
                resolveList(tmpList);

                /*Debug.Log("Matched three!");
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
                */
                return true;
            }
            else if (grid[x, y].donutObject.GetComponent<donut>().matchShape(grid[x, y - 1].donutObject.GetComponent<donut>()) && grid[x, y].donutObject.GetComponent<donut>().matchShape(grid[x, y - 2].donutObject.GetComponent<donut>()))
            {

                tmpList.Add(new coord(x, y));
                tmpList.Add(new coord(x, y - 1));
                tmpList.Add(new coord(x, y - 2));
                //CHECK FOR FURTHER MATCHES TO LEFT & RIGHT
                checkNextMatchUpShape(x, y, tmpList);
                checkNextMatchDownShape(x, y - 2, tmpList);
                resolveList(tmpList);

                /*Debug.Log("Matched three!");
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
                */
                return true;
            }
        }
        return false;
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
        if (thisMission.condition == 3)
            return;
        if (fallingDonuts.Count > 0)
            return;
        for( int i = 0; i < width; i++ )
        {
            if( grid[i, 7].donutObject.GetComponent<donut>().getActive() )
            {
                endgame();
                return;
            }
            //Destroy(grid[i, 8].gameObject);
            //grid[i, 8].donutObject = getNextDonut();
            grid[i, 8].donutObject.GetComponent<donut>().setDonutType(previewRow[i].donutObject.GetComponent<donut>());
            grid[i, 8].donutObject.transform.position = new Vector2(i, 8f);
            grid[i, 8].x = i;
            grid[i, 8].y = 8;
            fallingDonuts.Add(grid[i, 8]);
            //RESET PREVIEW ROW
            previewRow[i].donutObject.GetComponent<donut>().setDonutType(donutList[Random.Range(0, donutList.Count)].GetComponent<donut>());
            if (i > 0)
            {
                if (thisMission.condition != 8 || customersRemaining > 0)
                {
                    //DO NOT LET DONUT MATCH PREVIOUS DONUT
                    while (previewRow[i - 1].donutObject.GetComponent<donut>().anyMatch(previewRow[i].donutObject.GetComponent<donut>()))
                    {
                        previewRow[i].donutObject.GetComponent<donut>().setDonutType(donutList[Random.Range(0, donutList.Count)].GetComponent<donut>());
                    }
                }
                
            }
            
            //grid[i, 8] = (GameObject)Instantiate(donutList[Random.Range(0, donutList.Count)].gameObject, new Vector3(i, 8f, 0f), Quaternion.identity);
            //Debug.Log("Display something damnit");
            //StartCoroutine(shiftDownDelay(i, 8, 0.3f));
        }
        if (thisMission.condition == 8)
        {
            customersRemaining--;
            customers.text = customersRemaining.ToString();
        }
        timer = maxTimer;
        previewScreen.transform.localScale = new Vector3(previewScreen.transform.localScale.x, preview_scale, 1f);
        currentCombo = 0;
        currentStreak = 0;
        streakText.text = currentStreak.ToString();
    }

    public void promptScore()
    {
        Debug.Log("Attempting to prompt score");
        userInputField.gameObject.SetActive(true);
        //userInputField.onEndEdit.AddListener(submitAndShowScore);
    }

    public void submitAndShowScore()
    {
        victory_screen.GetComponent<victory_screen>().submitting();
        userInputField.gameObject.SetActive(false);
        submitButton.SetActive(false);
        submitting.enabled = true;
        //GameJolt.API.Scores.Add(score, score.ToString(), guestName, table_id, "", (bool success) => {
            //Debug.Log(string.Format("Score Add {0}.", success ? "Successful" : "Failed"));
        //});
        //GameJoltUI.Instance.ShowLeaderboards();
        StartCoroutine(showLeaderboard(3.00f));
        //restartButton.gameObject.SetActive(true);
    }

    IEnumerator showLeaderboard(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        submitting.enabled = false;
        //GameJolt.API.Scores.Get(victory_screen.GetComponent<victory_screen>().showScores(Score), )
        
        //GameJoltUI.Instance.ShowLeaderboards(null, table_id);
        //int [] visible_tables = { table_id };
        //GameJolt.UI.Controllers.LeaderboardsWindow.Show(null, table_id, visible_tables);
        //GameJolt.API.Scores.Get(null, table_id, 10, false);
        GameJolt.API.Scores.Get(scores => { victory_screen.GetComponent<victory_screen>().showScores(scores); }, table_id, 10, false);
    }

    public void endgame()
    {
        //restartButton.enabled = true;
        transform.GetChild(64).GetComponent<SpriteRenderer>().enabled = true;
        transform.GetChild(64).GetComponent<BoxCollider2D>().enabled = true;
        //gameOver.enabled = true;
        
        //GameJoltUI.Instance.ShowLeaderboards();
        //sky.GetComponent<Renderer>().sortingOrder = 10;
        //userInputField.gameObject.SetActive(true);
        //userInputField.onEndEdit.AddListener(submitAndShowScore);
        paused = true;
    }

    public void restart()
    {
        if( thisMission.condition != 3 )
        {
            transform.GetChild(64).GetComponent<SpriteRenderer>().enabled = false;
            transform.GetChild(64).GetComponent<BoxCollider2D>().enabled = false;
            happyNumber = 0;
            sadNumber = 0;
            happyText.text = "0";
            sadText.text = "0";
            satisfied = true;
            globals.isPaused = false;
        }
        gameOver.enabled = false;
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                grid[j, i].donutObject.GetComponent<donut>().deactivate();
                
            }
        }
        //Destroy(desired.gameObject);
        /*for (int i = 0; i < height; i++)
            for (int j = 0; j < width; j++)
            {
                Destroy(grid[j, i].gameObject);
            }*/
        if(thisMission.condition != 3)
            Start();
        else if( thisMission.condition == 3 && roundTimer == 0 )
        {
            Start();
        }
        //Camera.main.GetComponent<AudioSource>().Stop();
        //Camera.main.GetComponent<AudioSource>().clip = level1Music;
        //Camera.main.GetComponent<AudioSource>().Play();

        if (hasObstacles)
        {
            PlaceObstaclesOnGrid(0);
        }

        if (mission_object.GetComponent<mission>().condition == 3 || mission_object.GetComponent<mission>().condition == 4)
        {
            puzzle_donuts_remaining = 0;
            PlaceDonutsOnGrid(current_puzzle);
        }
    }

    public void AddRowSmart(int numRows)
    {
        for( int i = 0; i < numRows; i++ )
        {
            for (int j = 0; j < width; j++)
            {
                grid[j, i].donutObject.GetComponent<donut>().setDonutType(previewRow[j].donutObject.GetComponent<donut>());
                grid[j, i].donutObject.transform.position = new Vector2(j, i);
                grid[j, i].x = j;
                grid[j, i].y = i;
                fallingDonuts.Add(grid[j, i]);
                //RESET PREVIEW ROW
                previewRow[j].donutObject.GetComponent<donut>().setDonutType(donutList[Random.Range(0, donutList.Count)].GetComponent<donut>());
                
                //ALSO DO NOT LET EVERY THIRD ROW MATCH SECOND
                if( i > 1 )
                {
                    Debug.Log("Checking third row");
                    while (previewRow[j].donutObject.GetComponent<donut>().anyMatch(grid[j, i - 1].donutObject.GetComponent<donut>()))
                    {
                        Debug.Log("Rechecking donut at " + j + ":" + i);
                        previewRow[j].donutObject.GetComponent<donut>().setDonutType(donutList[Random.Range(0, donutList.Count)].GetComponent<donut>());
                        if (j > 0)
                        {
                            while (previewRow[j - 1].donutObject.GetComponent<donut>().anyMatch(previewRow[j].donutObject.GetComponent<donut>()) 
                                || previewRow[j].donutObject.GetComponent<donut>().anyMatch(grid[j, i - 1].donutObject.GetComponent<donut>()))
                            {
                                Debug.Log("Doublechecking donut at " + j + ":" + i);
                                previewRow[j].donutObject.GetComponent<donut>().setDonutType(donutList[Random.Range(0, donutList.Count)].GetComponent<donut>());
                            }
                        }
                    }
                }
                else if (j > 0)
                {  
                    {
                        while (previewRow[j - 1].donutObject.GetComponent<donut>().anyMatch(previewRow[j].donutObject.GetComponent<donut>()))
                        {
                            previewRow[j].donutObject.GetComponent<donut>().setDonutType(donutList[Random.Range(0, donutList.Count)].GetComponent<donut>());
                        }
                    }
                }
                
            }
        }
    }

    public void nextCustomer()
    {

    }
}
