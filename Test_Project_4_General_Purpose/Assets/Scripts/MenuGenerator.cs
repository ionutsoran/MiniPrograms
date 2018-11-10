using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuGenerator : MonoBehaviour {

    public static GameObject[,] UIcharacters;
    public GameObject pref;
    public Vector3 startpoint;

    public int n;
    public int m;

    public float Coffset = 0.5f;
    public float ValueOfX = 0.2f;
    public float Spread = 2f;
    public float Ymargin = 0;
    public float Xmargin = 0;

    private float ToAdd = 0;
    private float Xoffset = 0;
   

	void Start ()
    {
        startpoint = new Vector3(0, 3.5f, 0);
        UIcharacters = new GameObject[n, m];
        GenerateGrid();
	}
	
	public void GenerateGrid()
    {
        for(int i=0;i<n;i++)
        {
            if (i <= n / 2)
                ToAdd += Coffset;
            else
                ToAdd -= Coffset;

            Xoffset = ValueOfX;
      
            for (int j=0;j<m;j++)
            {
                if (j < m / 2)
                {
                    Xoffset = j + ValueOfX;
                    UIcharacters[i, j] = Instantiate(pref, new Vector3(-Spread - Xoffset - (ToAdd) - j * Xmargin, startpoint.y - i - i * Ymargin, 0), Quaternion.identity);       
                }         
                else
                {
                    Xoffset = (j-m/2) + ValueOfX;
                    UIcharacters[i, j] = Instantiate(pref, new Vector3(+Spread + Xoffset + ToAdd + j * Xmargin, startpoint.y - i - i * Ymargin, 0), Quaternion.identity);
                }
  
            }
        }
    }
}
