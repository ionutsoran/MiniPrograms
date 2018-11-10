using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMooore : MonoBehaviour {

    public GameObject obj;
    public float width;
    public float height;
    public GameObject[,] objs;
    public float[,] offsets;
    public float[,] toSubtract;
    public int n;
   // public bool changeSign;


	void Start () {

        width = 15;
        height = -8.66f;
        n = 10;
        toSubtract = new float[n, n]; 
        objs = new GameObject[n, n];
        offsets = new float[n, n];
        
     
        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
            {
                if (j % 2 == 0)
                {
                    objs[i, j] = Instantiate(obj, new Vector3(j * width, 0, i * (height * 2)), Quaternion.identity);
                    objs[i, j].AddComponent<MeshCollider>();
                    objs[i, j].GetComponent<MeshCollider>().sharedMesh = objs[i, j].GetComponent<MeshFilter>().mesh;
                    objs[i, j].GetComponent<MeshCollider>().convex = true;
                }    
                else
                {
                    objs[i, j] = Instantiate(obj, new Vector3(j * width, 0, i * (height * 2) + height), Quaternion.identity);
                    objs[i, j].AddComponent<MeshCollider>();
                    objs[i, j].GetComponent<MeshCollider>().sharedMesh = objs[i, j].GetComponent<MeshFilter>().mesh;
                    objs[i, j].GetComponent<MeshCollider>().convex = true;
                }
                   
            }

        IntializeDiagOffSets(0.25f);

        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
                objs[i, j].transform.localScale = new Vector3(1, offsets[i, j]*Time.deltaTime, 1);

        StartCoroutine(myCoRoutine());
    }

    void InitializeRandomSubtracts()
    {
        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
                toSubtract[i, j] = 0.10f + 0.05f * (int)Random.Range(2, 10);
    }

    void InitializeRandomOffSets()
    {
        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
                offsets[i, j] = (int)Random.Range(1, 12);
    }

    void IntializeDiagOffSets(float tsb)
    {
        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
            {
                toSubtract[i, j] = tsb;
                offsets[i, j] = Mathf.Sin(i/3f)*8;
            }
               
    }

    IEnumerator myCoRoutine()
    {
        yield return new WaitForSeconds(0.008f);

        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
            {
                if (offsets[i, j] < 2 && toSubtract[i, j] < 0)
                    toSubtract[i, j] = -1f * toSubtract[i, j];

                if (offsets[i, j] > 12 && toSubtract[i, j] > 0)
                    toSubtract[i, j] = -1f * toSubtract[i, j];

                offsets[i, j] += toSubtract[i, j];
                objs[i, j].transform.localScale = new Vector3(1, offsets[i, j], 1);
            }

        StartCoroutine(myCoRoutine());
    }

    void Update()
    {
        

    }
}
