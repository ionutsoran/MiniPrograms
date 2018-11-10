using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rename : MonoBehaviour {

    // Use this for initialization
    public float Awidth;
    public float Aheight;
    public static bool hasBeenInstantiated = false;

	void Start () {
        GetActualSize();
    }
	
	// Update is called once per frame
	void Update () {
       
    }

    void GetActualSize()
    {
        Vector3[] verts = GetComponent<MeshFilter>().mesh.vertices;

        Awidth = Mathf.Abs(verts[0].x - verts[2].x);
        Aheight = Mathf.Abs(verts[0].y - verts[3].y);
    }

    void OnMouseDown()
    {
        Debug.Log("Something!");
        if(!hasBeenInstantiated)
        {
            hasBeenInstantiated = true;
            Instantiate(Resources.Load("somelk") as GameObject, new Vector3(0.25f,-1.45f,-5), Quaternion.identity);
        }
    }
}
