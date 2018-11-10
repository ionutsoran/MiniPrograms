using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class GeneratePentagons : MonoBehaviour {

    public Mesh mesh;
    public List<Vector3> vertices;
    public List<Vector3> normals;
    public int[] triangles;
    public Material mainMat;

	void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = new List<Vector3>();
        CalculateHexagon();
        
    }

    void CalculateHexagon()
    {
        float height = (Mathf.Sin((Mathf.PI / 180) * 60)) * 10;
        //Debug.Log((Mathf.Sin((Mathf.PI / 180) * 60)) * 10);
        vertices.Add(new Vector3(0, 2, 0));  //0
        vertices.Add(new Vector3(5, 2, height)); //1    
        vertices.Add(new Vector3(-5, 2, height));//2
        vertices.Add(new Vector3(-10, 2, 0));//3
        vertices.Add(new Vector3(-5, 2, -height));//4
        vertices.Add(new Vector3(5, 2, -height));//5
        vertices.Add(new Vector3(10, 2, 0));//6

      //  vertices.Add(new Vector3(0, 0, 0));
        vertices.Add(new Vector3(5, 0, height));//7
        vertices.Add(new Vector3(-5, 0, height));//8
        vertices.Add(new Vector3(-10, 0, 0));//9
        vertices.Add(new Vector3(-5, 0, -height));//10
        vertices.Add(new Vector3(5, 0, -height));//11
        vertices.Add(new Vector3(10, 0, 0));//12


        triangles = new int[] { 0,2,1, 0,3,2, 4,3,0, 5,4,0, 6,5,0, 1,6,0,
                                1,2,7,7,2,8, 2,3,8,8,3,9, 3,4,9,9,4,10, 4,5,10,10,5,11, 5,6,11,11,6,12, 6,1,12,12,1,7};

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles;
        GetComponent<MeshRenderer>().material = mainMat;
        //mesh.normals = normals.ToArray();
        mesh.RecalculateNormals();

      //  for (int i = 0; i < mesh.normals.Length; i++)
         //   Debug.Log(mesh.normals[i].ToString());
    }

   
}
