using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canvas_stuff : MonoBehaviour {

    public Vector2 gridSize;
    public Texture2D gridMap;
   

    
    public Vector2 cellSize;
    public Texture2D cellMap;


    public int n;
    public int m;

    public float cell_spacing;
    public float maring_spacing;
    public float cellCalculatedsize;


    void Awake()
    {
        gridSize = new Vector2(GetComponent<Rect>().width, GetComponent<Rect>().height);

        cellCalculatedsize=(gridSize.x-(6*gridSize.x)/100)/5;
        cell_spacing = gridSize.x / 100;
        maring_spacing = 2 * cell_spacing;



        gridMap.Resize((int)gridSize.x, (int)gridSize.y);

    }


}
