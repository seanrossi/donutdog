using UnityEngine;

public class hint : MonoBehaviour {

    public GameObject game_frame;

	// Use this for initialization
	void Start () {
        transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = 9;
        transform.GetChild(1).GetComponent<MeshRenderer>().sortingOrder = 9;
        transform.GetChild(2).GetComponent<MeshRenderer>().sortingOrder = 9;
        transform.GetChild(3).GetComponent<MeshRenderer>().sortingOrder = 13;
        transform.GetChild(4).GetComponent<MeshRenderer>().sortingOrder = 14;

        game_frame.SetActive(false);
        //Application.targetFrameRate = 60;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseDown()
    {
        game_frame.SetActive(true);
        gameObject.SetActive(false);
    }
}
