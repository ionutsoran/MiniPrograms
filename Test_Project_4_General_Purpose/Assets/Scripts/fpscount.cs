using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class fpscount : MonoBehaviour {

    // Use this for initialization
    public float deltaTime;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;

        if (fps > 120)
            GetComponent<Text>().color = Color.magenta;
        else
        if (fps > 60)
            GetComponent<Text>().color = Color.green;
        else
            if (fps > 30)
            GetComponent<Text>().color = Color.yellow;
        else
            GetComponent<Text>().color = Color.red;

        GetComponent<Text>().text = "FPS:" + fps;
    }
}
