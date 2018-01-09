using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    public static Grid instance = null;

    [SerializeField]
    private Vector2 gridSize;

    //Used decimal as some values used float but my method needed to check for equality and floats are only approximations I needed exact decimal values.
    private decimal tileSize;

    private float gridTotalHeight;
    private float gridTotalWidth;

    private bool borderEnabled;
    private bool boundsEnabled;

    [SerializeField]
    private GameObject border;
    private GameObject borderGroup;

    private GameObject boundsGroup;
    private EdgeCollider2D borderCollision;
    private Vector2[] borderCollisionPoints;

    [SerializeField]
    private GameObject tile;
    private GameObject tileGroup;

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

    // Use this for initialization
    void Start ()
    {
        tileSize = (decimal)tile.GetComponent<SpriteRenderer>().sprite.bounds.size.x;

        borderEnabled = true;// Needs to be instantiated before createBorderGroup method is ran so it doesnt return null.
        CreateTileGroup();
        CreateBorderGroup();

        boundsGroup = new GameObject();
        boundsGroup.name = "Level Bounds";
        boundsGroup.transform.parent = LevelEditor.instance.getLevelFolder().transform;

        boundsGroup.AddComponent<EdgeCollider2D>();
        borderCollision = boundsGroup.GetComponent<EdgeCollider2D>();
        borderCollision.transform.localPosition = new Vector3(-(float)(tileSize / 2), -(float)(tileSize / 2), 0);// Position of the collider was off by half the tile size, for now its a quick fix.
        borderCollisionPoints = new Vector2[5];// Going to be 5 points in the edge collider.

        gridTotalHeight = 0;
        gridTotalWidth = 0;

        setGridSize(20, 10);

        borderEnabled = true;
        boundsEnabled = true;
    }
	
	// Updates the grid using the current grid height and width as it is called after the gridSize has been set.
	void UpdateGrid()
    {
        float gridVertical = 0;
        float gridHorizontal = 0;

        // Get the total height and width of the current tiles for spacing.
        gridTotalHeight = (float)((decimal)gridSize.y * tileSize);
        gridTotalWidth = (float)((decimal)gridSize.x * tileSize);

        // Loop through each row of each collumn and instantiate a grid tile or border depending on position.
        for (int x = -1; x <= gridSize.x; x += 1)
        {
            for (int y = -1; y <= gridSize.y; y += 1)
            {
                gridHorizontal = (float)(x * tileSize);
                gridVertical = (float)(y * tileSize);


                // Instantiates borders around grid otherwise instantiates a grid tile.
                if (x == -1 || x == gridSize.x || y == -1 || y == gridSize.y)
                {
                    GameObject newBorder = Instantiate(border, new Vector3(gridHorizontal, gridVertical, 0), Quaternion.identity);// Converting to float after calculation to keep accuracy.
                    newBorder.transform.parent = borderGroup.transform;
                }
                else
                {
                    GameObject newTile = Instantiate(tile, new Vector3(gridHorizontal, gridVertical, 5), Quaternion.identity);// Converting to float after calculation to keep accuracy.
                    newTile.transform.parent = tileGroup.transform;
                }
            }
        }

        // Call method to build the borders collision.
        BuildBorderCollision();

        // Delete any stray placed tiles that are now outside of the new grid size.
        LevelEditor.instance.deleteStrayObjects();
    }

    // Destroys the grid and border groups and instalises them so they are not null.
    void WipeGrid()
    {
        Destroy(tileGroup);
        Destroy(borderGroup);

        CreateTileGroup();
        CreateBorderGroup();
    }

    void CreateTileGroup()
    {
        //Creating game object for the tiles to be grouped into.
        tileGroup = new GameObject();
        tileGroup.name = "Grid Tiles";
        tileGroup.transform.parent = this.gameObject.transform;
    }

    void CreateBorderGroup()
    {
        //Create the border group for the borders around the level.
        borderGroup = new GameObject();
        borderGroup.name = "Border Tiles";
        borderGroup.transform.parent = LevelEditor.instance.getLevelFolder().transform;

        borderGroup.SetActive(borderEnabled);
    }

    // Places an edge collider along the edge of the grid so the player cannot fall out of the map.
    void BuildBorderCollision()
    {
        borderCollisionPoints[0] = new Vector2(0, 0);// Bottom left corner
        borderCollisionPoints[1] = new Vector2(0, gridTotalHeight);// Top left corner
        borderCollisionPoints[2] = new Vector2(gridTotalWidth, gridTotalHeight);// Top right corner
        borderCollisionPoints[3] = new Vector2(gridTotalWidth, 0);// Bottom right corner
        borderCollisionPoints[4] = new Vector2(0, 0);// Close loop at bottom right corner.

        //Applies the new points in the edge collider to it.
        borderCollision.points = borderCollisionPoints;
    }

    // Changes the gridSize variable and clears and re-instantiates the grid with new size variable.
    public void setGridSize(int width, int height)
    {
        gridSize = new Vector2(width, height);
        WipeGrid();
        UpdateGrid();
    }

    // Toggles the border group between active and in-active.
    public void toggleBorder(bool input)
    {
        borderGroup.SetActive(input);
        borderEnabled = input;
    }

    // Toggles the bounds group between active and in-active.
    public void toggleBounds(bool input)
    {
        boundsGroup.SetActive(input);
        boundsEnabled = input;
    }


    // Get methods for returning variables from this class when called.
    public decimal getTileSize()
    {
        return tileSize;
    }

    public bool getBorderEnabled()
    {
        return borderEnabled;
    }

    public bool getBoundsEnabled()
    {
        return boundsEnabled;
    }
}

