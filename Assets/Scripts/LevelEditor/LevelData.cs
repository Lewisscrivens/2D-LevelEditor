using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Creating serializable class to store the information I want to save.
[System.Serializable]
public class LevelData
{
    public int numberOfTiles;
    public int gridSizeX;
    public int gridSizeY;
    public bool gridBorder;
    public bool gridBounds;
    public string[] tileName;
    public float[] xPos;
    public float[] yPos;


    public void setNumberOfTiles(int input)
    {
        numberOfTiles = input;
        instaliseArrays(numberOfTiles);
    }

    public void instaliseArrays(int size)
    {
        tileName = new string[size];
        xPos = new float[size];
        yPos = new float[size];
    }

    public void setGridSizeX(int input)
    {
        gridSizeX = input;
    }

    public void setGridSizeY(int input)
    {
        gridSizeY = input;
    }

    public void setGridBorder(bool input)
    {
        gridBorder = input;
    }

    public void setGridBounds(bool input)
    {
        gridBounds = input;
    }

    public void setTileName(int arrayPos, string input)
    {
        tileName[arrayPos] = input;
    }

    public void setXPos(int arrayPos, float input)
    {
        xPos[arrayPos] = input;
    }

    public void setYPos(int arrayPos, float input)
    {
        yPos[arrayPos] = input;
    }

    public int getNumberOfTiles()
    {
        return numberOfTiles;
    }

    public int getGridSizeX()
    {
        return gridSizeX;
    }

    public int getGridSizeY()
    {
        return gridSizeY;
    }

    public bool getGridBorder()
    {
        return gridBorder;
    }

    public bool getGridBounds()
    {
        return gridBounds;
    }

    public string[] getTileName()
    {
        return tileName;
    }

    public float[] getXPos()
    {
        return xPos;
    }

    public float[] getYPos()
    {
        return yPos;
    }

}
