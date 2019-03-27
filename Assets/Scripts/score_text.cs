using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class score_text : MonoBehaviour {

    float MAX_BUFFER = 1.5f;
    float buffer = 0f;
    string score;
    bool inUse = false;

    GameObject meshObject1;
    GameObject meshObject2;
    GameObject meshObject3;
    GameObject meshObject4;
    GameObject meshObject5;

    TextMesh mesh;
    TextMesh mesh1;
    TextMesh mesh2;
    TextMesh mesh3;
    TextMesh mesh4;

    // Use this for initialization
    void Awake () {
        meshObject1 = new GameObject();
        meshObject1.AddComponent<TextMesh>();
        meshObject1.AddComponent<RectTransform>();
        mesh = meshObject1.GetComponent<TextMesh>();
        meshObject2 = new GameObject();
        meshObject2.AddComponent<RectTransform>();
        meshObject2.AddComponent<TextMesh>();
        mesh1 = meshObject2.GetComponent<TextMesh>();
        meshObject3 = new GameObject();
        meshObject3.AddComponent<TextMesh>();
        meshObject3.AddComponent<RectTransform>();
        mesh2 = meshObject3.GetComponent<TextMesh>();
        meshObject4 = new GameObject();
        meshObject4.AddComponent<TextMesh>();
        meshObject4.AddComponent<RectTransform>();
        mesh3 = meshObject4.GetComponent<TextMesh>();
        meshObject5 = new GameObject();
        meshObject5.AddComponent<TextMesh>();
        meshObject5.AddComponent<RectTransform>();
        mesh4 = meshObject5.GetComponent<TextMesh>();

        mesh.color = Color.white;
        mesh1.color = Color.black;
        mesh2.color = Color.black;
        mesh3.color = Color.black;
        mesh4.color = Color.black;
    }
	
	// Update is called once per frame
	void Update () {
		if( buffer > 0 )
        {
            buffer -= Time.deltaTime;
            if( buffer <= 0 )
            {
                meshObject1.SetActive(false);
                meshObject2.SetActive(false);
                meshObject3.SetActive(false);
                meshObject4.SetActive(false);
                meshObject5.SetActive(false);
                inUse = false;
            }
        }
	}

    public void setScore( string s, Vector2 pos )
    {
        inUse = true;

        meshObject1.SetActive(true);
        meshObject2.SetActive(true);
        meshObject3.SetActive(true);
        meshObject4.SetActive(true);
        meshObject5.SetActive(true);

        
        Vector3 tempPos = new Vector3(pos.x + 0f, pos.y + 1f, -1f);

        mesh.fontSize = 32;
        meshObject1.transform.localScale = new Vector2(0.25f, 0.25f);
        meshObject1.GetComponent<RectTransform>().localPosition = tempPos;
        mesh.GetComponent<Renderer>().sortingOrder = 15;
        mesh1.fontSize = 32;
        meshObject2.transform.localScale = new Vector2(0.25f, 0.25f);
        //tempPos = new Vector3(0f, -0.04f, 0f);
        tempPos = new Vector3(pos.x + 0f, pos.y + 0.96f, 0f);
        meshObject2.GetComponent<RectTransform>().localPosition = tempPos;
        mesh1.GetComponent<Renderer>().sortingOrder = 14;
        mesh2.fontSize = 32;

        meshObject3.transform.localScale = new Vector2(0.25f, 0.25f);
        tempPos = new Vector3(pos.x + 0f, pos.y + 1.04f, 0f);
        meshObject3.GetComponent<RectTransform>().localPosition = tempPos;
        mesh2.GetComponent<Renderer>().sortingOrder = 14;
        mesh3.fontSize = 32;
        meshObject4.transform.localScale = new Vector2(0.25f, 0.25f);
        tempPos = new Vector3(pos.x + 0.04f, pos.y + 1f, 0f);
        meshObject4.GetComponent<RectTransform>().localPosition = tempPos;
        mesh3.GetComponent<Renderer>().sortingOrder = 14;
        mesh4.fontSize = 32;
        meshObject5.transform.localScale = new Vector2(0.25f, 0.25f);
        tempPos = new Vector3(pos.x - 0.04f, pos.y + 1f, 0f);
        meshObject5.GetComponent<RectTransform>().localPosition = tempPos;
        mesh4.GetComponent<Renderer>().sortingOrder = 14;

        mesh.text = s;
        mesh1.text = s;
        mesh2.text = s;
        mesh3.text = s;
        mesh4.text = s;

        buffer = MAX_BUFFER;
    }

    public bool isInUse()
    {
        return inUse;
    }
}
