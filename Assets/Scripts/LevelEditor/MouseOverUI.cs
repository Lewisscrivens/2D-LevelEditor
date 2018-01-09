using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


// Used seperate class to mouseOver as that method cannot detect if the mouse is over the UI.
public class MouseOverUI : MonoBehaviour , IPointerEnterHandler , IPointerExitHandler {

    public static MouseOverUI instance = null;

    private bool UIover = false;

    //Creates MouseOverUI instance
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

    // When the mouse pointer goes over any UI element set UIover to true.
    public void OnPointerEnter(PointerEventData eventData)
    {
        UIover = true;
    }

    // When the mouse pointer exits from any UI element set UIover to false.
    public void OnPointerExit(PointerEventData eventData)
    {
        UIover = false;
    }

    // Return method for returning the UIover boolean.
    public bool getUIOver()
    {
        return UIover;
    }
}
