using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class LevelEditorUIController : MonoBehaviour {

    public static LevelEditorUIController instance = null;

    // UI Objects
    [SerializeField]
    private GameObject areYouSureExitWindow;

    [SerializeField]
    private GameObject tileWindow;

    [SerializeField]
    private GameObject selectedTileUI;

    [SerializeField]
    private GameObject settingsWindow;

    [SerializeField]
    private InputField gridWidth;

    [SerializeField]
    private InputField gridHeight;

    [SerializeField]
    private Toggle gridBorderToggle;

    [SerializeField]
    private Toggle gridBoundsToggle;

    [SerializeField]
    private InputField levelName;

    [SerializeField]
    private GameObject helpWindow;

    // Class Variables
    private string selectedTile;

    // Creates UIController instance
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

    // Use this for initialization
    void Start ()
    {
        selectedTile = "Empty";
	}

    // Set method for the selectedTile method. Called from mouseOver.
    public void setSelectedTile(GameObject input)
    {
        selectedTile = input.tag;
        selectedTileUI.transform.localPosition = input.transform.localPosition;

        LevelEditor.instance.setSelectedLevelTile(selectedTile);
    }

    // Get method for selectedTile. Returns selectedTile string.
    public string getSelectedTile()
    {
        return selectedTile;
    }

    
    public void onExitPressed()
    {
        areYouSureExitWindow.SetActive(true);// Shows the are you sure you want to exit menu.
    }

    public void onExitWindowYes()
    {
        SceneManager.LoadScene("MainMenu");// Use scene manager to load the MainMenu scene.
        areYouSureExitWindow.SetActive(false);// Close exit window.
    }

    public void onExitWindowNo()
    {
        areYouSureExitWindow.SetActive(false);// Close exit window.
    }

    public void onTileMenuPressed()
    {
        tileWindow.SetActive(true);// Open tile window.
    }

    public void onTileMenuExit()
    {
        tileWindow.SetActive(false);// Close tile window.
    }

    public void settingsWindowOpen()
    {
        settingsWindow.SetActive(true);// Open settings window.
    }

    public void settingsWindowExit()
    {
        settingsWindow.SetActive(false);// Close settings window.
    }

    public void helpWindowOpen()
    {
        helpWindow.SetActive(true);// Open help window.
    }

    public void helpWindowExit()
    {
        helpWindow.SetActive(false);// Close help window.
    }

    // Clears the placed items from the level editor.
    public void clearLevel()
    {
        LevelEditor.instance.wipeLevel();
    }

    // Reset LevelEditor to default and clears the level to give the illusion that a new level has been opened.
    public void newLevel()
    {
        levelName.text = "";

        Grid.instance.setGridSize(20, 10);// Default value.
        Grid.instance.toggleBorder(true);// Default value.
        Grid.instance.toggleBounds(true);// Default value.

        clearLevel();

        Debug.Log("Created new level");
    }

    // Run the play level method in LevelEditor if there is text in the levelName input box.
    public void playLevel()
    {
        LevelEditor.instance.PlayLevel();
    }
    
    // Method for changing grid size using the UI in the settings window.
    public void setGridSize()
    {
        string width = gridWidth.text;
        string height = gridHeight.text;

        int x = 0;
        int y = 0;

        // Try to parse width and height strings into x and y.
        int.TryParse(width, out x);
        int.TryParse(height, out y);

        // Set the grid size in level editor.
        LevelEditor.instance.setGridSize(x, y);
    }

    public void saveLevel()
    {
        string levelNm = levelName.text;
        
        if (levelNm != "")
        {
            LevelEditor.instance.SaveLevel(levelNm);
        }
        else
        {
            Debug.Log("Enter a level name!");
        }
    }

    public void loadLevel()
    {
        string levelNm = levelName.text;

        if (levelNm != "")
        {
            LevelEditor.instance.LoadLevel(levelNm);
        }
        else
        {
            Debug.Log("Enter a level name!");
        }
    }

    public void deleteLevel()
    {
        string levelNm = levelName.text;

        if (levelNm != "")
        {
            LevelEditor.instance.DeleteLevel(levelNm);
        }
        else
        {
            Debug.Log("Enter a level name!");
        }
    }
    
    // Added methods to call to other public methods to keep the script consistent. 
    public void toggleBorder()// Toggles border enabled value in Grid.
    {
        Grid.instance.toggleBorder(!Grid.instance.getBorderEnabled());
    }

    public void toggleBounds()// Toggles bounds enabled value in Grid.
    {
        Grid.instance.toggleBounds(!Grid.instance.getBoundsEnabled());
    }

    public void updateSettingsPanel(int gridX, int gridY, bool border, bool bounds)// Updates values in settings panel after update.
    {
        gridWidth.text = gridX.ToString();
        gridHeight.text = gridY.ToString();

        gridBorderToggle.isOn = border;
        gridBoundsToggle.isOn = bounds;
    }
}
