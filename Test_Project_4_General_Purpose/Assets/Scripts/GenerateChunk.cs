using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateChunk : MonoBehaviour {

    public Mesh meshutz;
    public List<int> triangle;
    public List<Vector3> verticies;
    public List<Color32> vertColors;

    public int atVertex = 0;
    public float treshold=2f;
    public GameObject obj;
    public Material mat;

    void Start()
    {
        treshold = 0.25f;
        meshutz = new Mesh();
        verticies = new List<Vector3>();
        vertColors = new List<Color32>();
        triangle = new List<int>();
        //mat = new Material(Shader.Find("Particles/Anim Alpha Blended"));
        CalculateSeprinskyTriangle(new Vector3(5, 7.05f, 5f), new Vector3(10, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 10), new Vector3(10, 0, 10));
        meshutz.vertices = verticies.ToArray();
        obj = new GameObject("Serpinsky");
        obj.AddComponent<MeshFilter>();
        obj.AddComponent<MeshRenderer>();
        obj.GetComponent<MeshRenderer>().material = mat;
        obj.GetComponent<MeshFilter>().mesh = meshutz;
        obj.GetComponent<MeshFilter>().mesh.triangles = triangle.ToArray();
       // obj.GetComponent<MeshFilter>().mesh.colors32 = vertColors.ToArray();
        obj.GetComponent<MeshFilter>().mesh.RecalculateNormals();
        // CalculateSeprinskyTriangle(new Vector3(5, 8.66f, 0), new Vector3(10, 0, 0), new Vector3(0, 0, 0));

    }

    void GenerateATriangle(Vector3 v1,Vector3 v2,Vector3 v3,Vector3 v4, Vector3 v5)
    {
        /*  meshutz = new Mesh();

          verticies = new List<Vector3>();
          vertColors = new List<Color32>();
          triangle = new List<int>();
          mat = new Material(Shader.Find("Particles/Anim Alpha Blended"));
          //   Color32 c = new Color32((byte)Random.Range(0, 256), (byte)Random.Range(0, 256), (byte)Random.Range(0, 256), 255);
          //for (int i = 0; i < 3; i++)
          //   vertColors.Add(c);*/

        //  vertColors.Add(new Color32(255, 0, 0, 255));
        //  vertColors.Add(new Color32(0, 255, 0, 255));
        //  vertColors.Add(new Color32(0, 0, 255, 255));

        Color32 c = new Color32((byte)Random.Range(0, 256), (byte)Random.Range(0, 256), (byte)Random.Range(0, 256), 255);
        for (int i = 0; i < 3; i++)
            vertColors.Add(c);

        verticies.Add(v1);
        verticies.Add(v2);
        verticies.Add(v3);
        triangle.Add(atVertex);
        triangle.Add(atVertex + 1);
        triangle.Add(atVertex + 2);

        //  vertColors.Add(new Color32(255, 0, 0, 255));
        //  vertColors.Add(new Color32(0, 255, 0, 255));
        //  vertColors.Add(new Color32(0, 0, 255, 255));

        c = new Color32((byte)Random.Range(0, 256), (byte)Random.Range(0, 256), (byte)Random.Range(0, 256), 255);
        for (int i = 0; i < 3; i++)
            vertColors.Add(c);

        verticies.Add(v1);
        verticies.Add(v3);
        verticies.Add(v4);
        triangle.Add(atVertex+3);
        triangle.Add(atVertex+4);
        triangle.Add(atVertex+5);

        // vertColors.Add(new Color32(255, 0, 0, 255));
        // vertColors.Add(new Color32(0, 255, 0, 255));
        // vertColors.Add(new Color32(0, 0, 255, 255));

        c = new Color32((byte)Random.Range(0, 256), (byte)Random.Range(0, 256), (byte)Random.Range(0, 256), 255);
        for (int i = 0; i < 3; i++)
            vertColors.Add(c);

        verticies.Add(v1);
        verticies.Add(v4);
        verticies.Add(v5);
        triangle.Add(atVertex + 6);
        triangle.Add(atVertex + 7);
        triangle.Add(atVertex + 8);

        //vertColors.Add(new Color32(255, 0, 0, 255));
        //vertColors.Add(new Color32(0, 255, 0, 255));
        //vertColors.Add(new Color32(0, 0, 255, 255));

        c = new Color32((byte)Random.Range(0, 256), (byte)Random.Range(0, 256), (byte)Random.Range(0, 256), 255);
        for (int i = 0; i < 3; i++)
            vertColors.Add(c);

        verticies.Add(v1);
        verticies.Add(v5);
        verticies.Add(v2);
        triangle.Add(atVertex + 9);
        triangle.Add(atVertex + 10);
        triangle.Add(atVertex + 11);
        atVertex += 12;
        /*
        meshutz.vertices = verticies.ToArray();
        obj = new GameObject("Serpinsky"+atVertex);
        obj.AddComponent<MeshFilter>();
        obj.AddComponent<MeshRenderer>();
        obj.GetComponent<MeshRenderer>().material = mat;
        obj.GetComponent<MeshFilter>().mesh = meshutz;
        obj.GetComponent<MeshFilter>().mesh.triangles = triangle.ToArray();
        obj.GetComponent<MeshFilter>().mesh.colors32 = vertColors.ToArray();*/

      
    }

    void CalculateSeprinskyTriangle(Vector3 v1, Vector3 v2, Vector3 v3,Vector3 v4,Vector3 v5)
    {
        if ((Mathf.Abs(v1.x - v2.x)) <= treshold && (Mathf.Abs(v1.y - v2.y))<= treshold && (Mathf.Abs(v1.z - v2.z)) <= treshold)
            GenerateATriangle(v1, v2, v3,v4,v5);
        else
        {
            Vector3 b1 = new Vector3((v1.x + v2.x) / 2, (v1.y + v2.y) / 2, (v1.z + v2.z) / 2);
            Vector3 b2 = new Vector3((v2.x + v3.x) / 2, (v2.y + v3.y) / 2, (v2.z + v3.z) / 2);
            Vector3 b3 = new Vector3((v1.x + v3.x) / 2, (v1.y + v3.y) / 2, (v1.z + v3.z) / 2);
            Vector3 b4 = new Vector3((v3.x + v4.x) / 2, (v3.y + v4.y) / 2, (v3.z + v4.z) / 2);
            Vector3 b5 = new Vector3((v1.x + v4.x) / 2, (v1.y + v4.y) / 2, (v1.z + v4.z) / 2);
            Vector3 b6 = new Vector3((v4.x + v5.x) / 2, (v4.y + v5.y) / 2, (v4.z + v5.z) / 2);
            Vector3 b7 = new Vector3((v1.x + v5.x) / 2, (v1.y + v5.y) / 2, (v1.z + v5.z) / 2);
            Vector3 b8 = new Vector3((v5.x + v2.x) / 2, (v5.y + v2.y) / 2, (v5.z + v2.z) / 2);
            Vector3 b_mid = new Vector3(v1.x, v3.y, v1.z);

            CalculateSeprinskyTriangle(v1, b1, b3, b5, b7);
            CalculateSeprinskyTriangle(b1, v2, b2,b_mid,b8);
            CalculateSeprinskyTriangle(b3, b2, v3,b4,b_mid);
            CalculateSeprinskyTriangle(b5, b4, v4, b6, b_mid);
            CalculateSeprinskyTriangle(b7, b6, v5, b8, b_mid);

        }
    }
}
