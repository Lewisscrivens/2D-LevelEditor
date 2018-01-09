using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// TEST VERSION OF THE GRID I WAS GOING TO USE BUT IT IS USED FOR DEBUGGING AND RENDERS OVER ALL WORLD OBJECTS.


public class GridDebug : MonoBehaviour {

    public static GridDebug instance = null;


    [SerializeField]
    private int gridLength;//Variable for the grid size along x.

    [SerializeField]
    private int gridHeight;//Variable for the grid size alone y.

    [SerializeField]
    private int gridDepth;//Variable for the grid size along z (As the game is 2D this is only useful for adding layers into the level editor).

    [SerializeField]
    private Material gridLineMat;

    private float tileSize;
    private float gridOffsetX;
    private float gridOffsetY;
    private Vector3 startPos;

    //Creates Grid instance
    void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        tileSize = 1;//Sets default tiles to have size 1x1. This changes the anound of tiles per length and height. So gridHeight of 10 would be 5 if the tile size is 2...
        startPos = new Vector3(0,0,0);//Starting positio for the grid (Default is the origin).
    }

    void OnPostRender()
    {
        gridLineMat.SetPass(0);

        //LineRenderer gLine;


        GL.Begin(GL.LINES);//Creates the lines for the grid.
        GL.Color(new Color(1f, 1f, 1f, 1f));//Sets the grid lines to black.

        //Calculate the new grid offsets.
        gridOffsetX = -0.5f;
        gridOffsetY = -0.5f;

        //The following for loops in this method I have found and tweaked from omline documentation that I will provide in my references document...

        //This will create the grid for layers if the gridDepth is ever set to anything higher than 0.
        for (float y = 0; y <= gridHeight; y += tileSize)
        {
            //Now do a for loop for the gridDepth on the current gridHeight. (Creates the X axis lines).
            for(float z = 0; z <= gridDepth; z += tileSize)
            {
                GL.Vertex3(startPos.x + gridOffsetX, y + gridOffsetY, startPos.z + z);
                GL.Vertex3(gridLength + gridOffsetX, y + gridOffsetY, startPos.z + z);
            }

            //Now do a loop for the gridLength on the current gridHeight. (Creates the Z axis lines).
            for(float x = 0; x <= gridLength; x += tileSize)
            {
                GL.Vertex3(startPos.x + x + gridOffsetX, y + gridOffsetY, startPos.z);
                GL.Vertex3(startPos.x + x + gridOffsetX, y + gridOffsetY, gridDepth);
            }
        }

        //This loop will create the Y axis lines for the grid.
        for(float z = 0; z <= gridDepth; z += tileSize)
        {
            for(float x = 0; x <= gridLength; x += tileSize)
            {
                GL.Vertex3(startPos.x + x + gridOffsetX, startPos.y + gridOffsetY, startPos.z + z);
                GL.Vertex3(startPos.x + x + gridOffsetX, gridHeight + gridOffsetY, startPos.z + z);
            }
        }

        GL.End();//Ends the grid line
    }

    public void setGridLength(int input)
    {
        gridLength = input;//If this method the gridLenth is set to the input given.
    }

    public void setGridHeight(int input)
    {
        gridHeight = input;//If this method the gridHeight is set to the input given.
    }

    public void setGridDepth(int input)
    {
        gridDepth = input;//If this method the gridDepth is set to the input given.
    }
}
