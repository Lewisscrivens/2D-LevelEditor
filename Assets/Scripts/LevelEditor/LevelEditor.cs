using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;


public class LevelEditor : MonoBehaviour {

    public static LevelEditor instance = null;

    [SerializeField]
    private Camera mainCam;//Main camera needs to be passed in as default for this script.

    // Level Editors stored value of gridSize.
    private Vector2 gridSize;

    // Variabes for Level Editor Camera
    private Vector3 camStartPos;
    private float cameraSize;

    [SerializeField]
    private GameObject[] tiles;
    private GameObject currentTilePrefab;
    private string currentTileName;

    private GameObject customLevel;
    private List<GameObject> objectsPlaced;

    [SerializeField]// Variables for the selected grid tile and its prefab.
    private GameObject selectedGridTilePrefab;
    private GameObject selectedGridTile;
    private GameObject selectedTileGameObject;// The selected grid tile stored variable.

    private Vector3 selectedTileStartPos;// start position for the selectedGridTile.
    private GameObject selectedTile;// The current selected tile if its a game object.

    // The level game object from the scene used to group together level objects.
    [SerializeField]
    private GameObject LevelFolder;

    // LevelData variable for storing the required information to save/load levels.
    private LevelData lvlData;

    [SerializeField]// Player stored prefab and the player object as there can be only one player.
    private GameObject playerPrefab;
    private GameObject player;

    [SerializeField]// Enemy stored prefab and the enemy list as there can be multiple enemies.
    private GameObject enemyPrefab;
    private List<GameObject> enemies;

    [SerializeField]// Playerspawn stored prefab and the playerspawn object as there can be only one spawn.
    private GameObject playerSpawnPrefab;
    private GameObject playerSpawn;

    // Placed exit game object as there can only be one.
    private GameObject placedExit;

    // Booleans used to control method outcomes.
    private bool playerSpawnPlaced;
    private bool tileSelected;
    private bool offGrid;
    private bool mouseOverObject;

    // Integers used to limit the grid sizing.
    private int maxGridY;
    private int maxGridX;
    private int minGridY;
    private int minGridX;

    //Creates LevelEditor instance
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
        createNewLevel();// Set level variables to default.

        gridSize = new Vector2(20, 10);// Default grid size.
        objectsPlaced = new List<GameObject>();

        enemies = new List<GameObject>();

        camStartPos = new Vector3(13.3f, 7.4f, -20);//The starting match 
        selectedTileStartPos = new Vector3(0, 0, -5);

        selectedGridTile = new GameObject();// Set to empty
        Destroy(selectedGridTile);

        selectedTileGameObject = new GameObject();// Set to empty
        Destroy(selectedTileGameObject);

        currentTilePrefab = new GameObject();// Set to empty
        Destroy(currentTilePrefab);

        // Creates the selected tile game object for selection of tiles on the grid.
        selectedTile = Instantiate(selectedGridTilePrefab, selectedTileStartPos, Quaternion.identity);
        selectedTile.transform.parent = Grid.instance.transform;
        selectedTile.SetActive(false);// Hides the selected tile until the mouse enters the grid.

        // Camera default variable values.
        mainCam.transform.position = camStartPos;
        cameraSize = 10;
        mainCam.orthographicSize = cameraSize;
        mouseOverObject = false;

        // Variables for controlling the max and min input into gridSize.
        maxGridY = 100;
        maxGridX = 100;
        minGridX = 10;
        minGridY = 10;

        LoadLevel("example");// Loads the example level from the scenes folder.
    }

    // Update the currently selected tile from the UI.
    void selectLevelTile()
    {
        foreach (GameObject tile in tiles)// If any of the tiles in the tiles list match the selected tile in the UI then set that as the current tile prefab.
        {
            if (tile.tag == currentTileName)
            {
                currentTilePrefab = tile;
            }
        }
    }

    // Public so it can be called from the Input class. Checks if it is ok to place a tile before creating it.
    public void placeTile()
    {
        if (!mouseOverObject && !offGrid && tileSelected)// The mouse has to be over an empty slot, on the grid and have a tile selected to place a tile.
        {
            if (currentTilePrefab.tag == "PlayerSpawn" && playerSpawnPlaced)// If the tile is a player spawn and one has already been placed move it to the new selected position.
            {
                playerSpawn.transform.position = selectedTile.transform.position;
            }
            else if (currentTilePrefab.tag == "Exit" && placedExit != null)// If the tile is an Exit object and exit isnt null then move it to the new selected position.
            {
                placedExit.transform.position = selectedTile.transform.position;
            }
            else// Otherwise run the createtile method.
            {
                createTile(selectedTile.transform.position);
            }
        }   
    }

    // Creates a tile at the given pos (position).
    void createTile(Vector3 pos)
    {
        GameObject tile = Instantiate(currentTilePrefab, pos, Quaternion.identity);// Creates current tile prefab game object in scene.
        tile.transform.parent = customLevel.transform;// Makes it a child to the customlevel game object.
        objectsPlaced.Add(tile);// Adds the created tile to the list of placed objects.


        // If the tile that has just been instantiated is a player spawn object set playerSpawnPlaced to true and the player spawn object to the instantiated one.
        if (currentTilePrefab.tag == "PlayerSpawn")
        {
            playerSpawnPlaced = true;
            playerSpawn = tile;
        }
        else if (currentTilePrefab.tag == "Exit")// Otherwise if the tile is an exit object then set the placed exit object to the instantiatated object.
        {
            placedExit = tile;
        }
    }

    // Check if the mouse is over an object and that its not off the grid.
    public void destroyTile()
    {
        if (mouseOverObject && !offGrid)
        {
            deleteObjectFromList(selectedTileGameObject);// Delete the selectedTileGameObject from the objectsPlaced list.
        }
    }

    // Update the current selected tile in the UI.
    void updateSelectedTile()
    {
        currentTileName = LevelEditorUIController.instance.getSelectedTile();
        selectLevelTile();
    }

    // Deletes the gameobject input into the method from the objectsPlaced list.
    void deleteObjectFromList(GameObject input)
    {
        if (input != null)
        {
            // Needed to convert list to array to check for specific indexes.
            for (int i = 0; i < objectsPlaced.ToArray().Length; i++)
            {
                if (objectsPlaced.ToArray()[i] == input && input.name == objectsPlaced.ToArray()[i].name)
                {
                    Destroy(input);// If the object to be deleted is equal to the object in the array then destroy the game object.
                    objectsPlaced.Remove(objectsPlaced.ToArray()[i]);// Remove the object from the objectsPlaced list.
                }
                if (input.tag == "PlayerSpawn")// If the player spawn is destroyed then set playerSpawnPlaced to false.
                {
                    playerSpawnPlaced = false;
                }
            }
        }
    }

    // Used to change the current gridSize by updating the grid in the Grid instance.
    void setGridLayout()
    {
        Grid.instance.setGridSize((int)gridSize.x, (int)gridSize.y);// Sets the gridSize to the currently stored gridSize.  
        resetSelectedTile();// Reset selected tile to origin to prevent bugs.

    }

    // Method called to change the grid size which also validates anything input.
    public void setGridSize(int inputX, int inputY)
    {
        // Only change the grid size if the input for X and Y are within the limits set by minX/Y and maxX/Y.
        if (inputX <= maxGridX && inputX >= minGridX && inputY <= maxGridY && inputY >= minGridY)
        {
            gridSize = new Vector2(inputX, inputY);
            setGridLayout();
        }// Otherwise if the input X or Y was too big then log message to the console.
        else if (inputX > maxGridX || inputY > maxGridY)
        {
            Debug.Log("Grid size too big.");
        }
        else
        {
            Debug.Log("Grid size too small.");// Otherwise the input X or Y must be too small so output message to consoel.
        }
    }

    // Method decreases size of camera if its not too small.
    public void ZoomIn()
    {
        if (mainCam.orthographicSize > 2)
        {
            mainCam.orthographicSize = mainCam.orthographicSize - 0.5f;
        }
    }

    // Method increases size of camera if its not too big.
    public void ZoomOut()
    {
        if (mainCam.orthographicSize < 20)
        {
            mainCam.orthographicSize = mainCam.orthographicSize + 0.5f;
        }
    }

    // Resets the camera position back to the instalised start position.
    public void ResetCamPos()
    {
        mainCam.transform.position = camStartPos;
        mainCam.orthographicSize = cameraSize;
    }

    // Called from mouse over scripts which inputs the current selected grid tile as the selectedGridTile.
    public void setSelectedGridTile(GameObject input)
    {
        selectedGridTile = input;
        selectGridTile(selectedGridTile);
    }

    // Sets the mouse off grid to false and moves the selectedTile game objects position to the input position.
    public void selectGridTile(GameObject input)
    {
        offGrid = false;
        selectedTile.SetActive(true);

        selectedTile.transform.position = (input.transform.position + selectedTileStartPos);

        if (input.tag != "GridTile")// If the input isnt a grid tile then set the selected tile game object as the input. (UI button calling this method)
        {
            selectedTileGameObject = input;
        }
    }

    // Method is ran when the mouse exits the grid.
    public void setUnselectedTile()
    {
        offGrid = true;
        selectedTileGameObject = null;
        selectedTile.SetActive(false);// Hide the selected tile when mouse isnt in the grid.
    }

    // Method is ran when mouse is over a level object. See the mouse over script.
    public void setOverObject(bool input)
    {
        mouseOverObject = input;
    }

    // Set method for tetting the selected level tile.
    public void setSelectedLevelTile(string input)
    {
        if (input == "Empty")
        {
            tileSelected = false;
        }
        else 
        {
            tileSelected = true;
            currentTileName = input;
            selectLevelTile();
        }
    }

    // Destroy the custom level object containing the list of game object in the level. Then re-initialising a new level.
    public void wipeLevel()
    {
        Destroy(customLevel);
        createNewLevel();
    }

    // Reset all variables that make up the current level.
    void createNewLevel()
    {
        customLevel = new GameObject();
        customLevel.name = "Custom Level";
        customLevel.transform.parent = LevelFolder.transform;
        playerSpawnPlaced = false;
        objectsPlaced = new List<GameObject>();
    }

    // This code deletes all objects outside of the current gridSize.
    public void deleteStrayObjects()
    {
        decimal tileWidth = Grid.instance.getTileSize();// Variable for the size of the tiles.

        if (objectsPlaced.Count != 0)// Only bother running if the tile count is over 0.
        {
            foreach (Transform tile in customLevel.transform)// foreach tile in the level check that it is not outside of the grid bounds.
            {
                if (tile.transform.localPosition.x >= (float)(tileWidth * (decimal)gridSize.x) || tile.transform.localPosition.y >= (float)(tileWidth * (decimal)gridSize.y))
                {
                    deleteObjectFromList(tile.gameObject);// Delete current tile thats out of bounds from objectsPlaced list.
                }
            }
        }
    }

    //  Reset the selected tile to origin.
    public void resetSelectedTile()
    {
        selectedTile.transform.position = selectedTileStartPos;
    }

    // Returns the level folder serialised in this.gameObject.
    public GameObject getLevelFolder()
    {
        return LevelFolder;
    }

    // Method for entering play mode.
    void playMode()
    {
        // All level editor objects are set to inactive.
        LevelEditor.instance.transform.gameObject.SetActive(false);
        LevelEditorUIController.instance.transform.gameObject.SetActive(false);
        LevelEditorInput.instance.transform.gameObject.SetActive(false);
        playerSpawn.SetActive(false);

        // For each of the enemySpawn objects in the objectsPlaced list set them to inactive.
        foreach (GameObject obj in objectsPlaced)
        {
            if (obj.tag == "EnemySpawn")
            {
                obj.SetActive(false);
            }
        }
    }

    // Does the opposite of play mode and re-enables all of the deactivated scripts.
    public void editorMode()
    {
        LevelEditor.instance.transform.gameObject.SetActive(true);
        LevelEditorUIController.instance.transform.gameObject.SetActive(true);
        LevelEditorInput.instance.transform.gameObject.SetActive(true);

        Destroy(player);// Destroy the player object.

        foreach (GameObject enemy in enemies)// Destroy all enemy game objects.
        {
            Destroy(enemy);
        }

        playerSpawn.SetActive(true);

        foreach (GameObject obj in objectsPlaced)
        {
            if (obj.tag == "EnemySpawn")
            {
                obj.SetActive(true);
            }
        }
    }

    // Instantiates a player into the currently loaded word.
    public void PlayLevel()
    {
        if (playerSpawnPlaced)
        {
            spawnPlayer();
            spawnEnemies();
            playMode();
        }
        else
        {
            Debug.Log("Need a player spawn to play!");
        }
    }

    // If the player is null create the player from the playerPrefab at the placed spawn position.
    void spawnPlayer()
    {
        if (player != null)
        {
            Destroy(player);
        }

        player = Instantiate(playerPrefab, playerSpawn.transform.position, Quaternion.identity);
    }

    // Do the same process as spawnPlayer but foreach enemy from the enemies list.
    void spawnEnemies()
    {
        if (enemies.Count != 0)
        {
            foreach (GameObject enemy in enemies)
            {
                Destroy(enemy);
            }
        }

        foreach (GameObject obj in objectsPlaced)
        {
            if (obj.tag == "EnemySpawn")
            {
                GameObject enemy = Instantiate(enemyPrefab, obj.transform.position, Quaternion.identity);
                enemies.Add(enemy);
            }
        }

    }

    // Method saves the level under the input string levelName.
    public void SaveLevel(string levelName)
    {
        // Using a binary formater to save the level information.
        BinaryFormatter bf = new BinaryFormatter();
        FileStream saveLvl = new FileStream(Application.persistentDataPath + "/" + levelName + ".dat", FileMode.Create);


        // Making use of the lvlData set methods for setting the data before serializing it.
        lvlData = new LevelData();
        lvlData.setNumberOfTiles(objectsPlaced.Count);
        lvlData.setGridSizeX((int)gridSize.x);
        lvlData.setGridSizeY((int)gridSize.y);
        lvlData.setGridBorder(Grid.instance.getBorderEnabled());
        lvlData.setGridBounds(Grid.instance.getBoundsEnabled());

        // For each tile in the level save the tile type and position.
        foreach (GameObject tile in objectsPlaced)
        {
            int i = objectsPlaced.IndexOf(tile);

            lvlData.setTileName(i, tile.tag);
            lvlData.setXPos(i, tile.transform.position.x);
            lvlData.setYPos(i, tile.transform.position.y);
        }

        // Serialize lvlData into the saveLvl file.
        bf.Serialize(saveLvl, lvlData);
        saveLvl.Close();

        Debug.Log("Level Saved");
    }

    // Method loads the level under the input string levelName.
    public void LoadLevel(string levelName)
    {
        // If the file to be loaded exists then load it.
        if (File.Exists(Application.persistentDataPath + "/" + levelName + ".dat") || levelName == "example")
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream savedLvl;
            
            // If the level to be loaded is called example load the example level from the assets folder otherwise load from the save location.
            if (levelName != "example")
            {
                savedLvl = new FileStream(Application.persistentDataPath + "/" + levelName + ".dat", FileMode.Open);
            }
            else
            {
                savedLvl = new FileStream(Application.dataPath + "/Scenes/example.dat" , FileMode.Open);
            }

            // Deserialize the data from savedLvl back into the LevelData variable.
            lvlData = bf.Deserialize(savedLvl) as LevelData;

            savedLvl.Close();
            loadTiles();// Load the tiles back into the scene.

            Debug.Log("Level Loaded");
        }
        else
        {
            Debug.Log("Level doesnt exist");
        }
    }

    // Loads the tiles back into the level editor using the LevelEditor methods and variables.
    void loadTiles()
    {
        wipeLevel();// Clear the level so its ready to be loaded into.
        setGridSize(lvlData.getGridSizeX(), lvlData.getGridSizeY());// Get and set the grid size from save.
        LevelEditorUIController.instance.updateSettingsPanel(lvlData.getGridSizeX(), lvlData.getGridSizeY(), lvlData.getGridBorder(), lvlData.getGridBounds());//This will toggle borders to what they have to be automatically.

        int numberOfTiles = lvlData.getNumberOfTiles();

        for (int i = 0; i < numberOfTiles; i++)// For each tile saved selected that tile type and create the tile at the saved tile position.
        {
            currentTileName = lvlData.getTileName()[i];
            selectLevelTile();

            Vector2 tilePos = new Vector2(lvlData.getXPos()[i], lvlData.getYPos()[i]);
            createTile(tilePos);
        }

        updateSelectedTile();
    }

    // Method deletes the file with the input string levelName.
    public void DeleteLevel(string levelName)
    {
        // If the input level name exists then delete the file, otherwise output to the console.
        if (File.Exists(Application.persistentDataPath + "/" + levelName + ".dat"))
        {
            File.Delete(Application.persistentDataPath + "/" + levelName + ".dat");
            wipeLevel();
            Debug.Log("Level Deleted");
        }
        else
        {
            Debug.Log("Level doesnt exist");
        }
    }
}



