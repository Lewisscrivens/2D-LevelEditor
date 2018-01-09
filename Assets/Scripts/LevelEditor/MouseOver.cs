using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseOver : MonoBehaviour {

    private bool stillOver = false;

    [SerializeField]
    private bool levelObject;

    void OnMouseOver()
    {
        if (stillOver == false)
        {
            stillOver = true;

            // If this.gameobject is a level object then set over object in the Level Editor to true.
            if (levelObject)
            {
                LevelEditor.instance.setOverObject(true);
            }

            // Pass currently selected grid tile to the Level Editor.
            LevelEditor.instance.setSelectedGridTile(this.gameObject);
        }
    }

    void OnMouseExit()
    {
        if (stillOver)
        {
            stillOver = false;

            //On exit automatically set over object back to false and unselect the tile in Level Editor.
            LevelEditor.instance.setOverObject(false);
            LevelEditor.instance.setUnselectedTile();
        }
    }
}
