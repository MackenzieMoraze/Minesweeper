using TMPro;
using Unity.Mathematics;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Composites;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public static GameController Instance
    {
        get
        { 
            if(instance == null)
            {
                instance = FindFirstObjectByType<GameController>();

                if(instance != null )
                {
                    GameObject gameController = new GameObject(typeof(GameController).Name);
                    instance = gameController.AddComponent<GameController>();
                }
            }
            return instance;
        }
    }


    public GameObject BoxPrefab;
    [SerializeField] Color unflaggedColor = Color.white;
    [SerializeField] Color flaggedColor = Color.orange;


    public int bombsToSpawn = 10;
    private Tile[,] grid;

    private int uncoveredBombs;

    [SerializeField] TMP_InputField height;
    [SerializeField] TMP_InputField width;
    [SerializeField] TMP_InputField  bombs;

    [SerializeField] TMP_Text timeText;
    [SerializeField] TMP_Text bombsLeftText;
    [SerializeField] TMP_Text winLoseText;



    [SerializeField] Canvas canvas;
    [SerializeField] Vector2 gridOffset;
    [SerializeField]public int gridWidth = 10;
    [SerializeField] public int gridHeight = 10;
    [SerializeField] float tileGapSize = 0.5f;

    private StopWatch stopWatch;



    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        } 
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stopWatch = GetComponent<StopWatch>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    

    public void StartGame()
    {
        stopWatch.StartStopwatch();
        GenerateGrid();
        


    }

    private void GenerateGrid()
    {
        gridHeight = int.Parse(height.text);
        gridWidth = int.Parse(width.text);
        bombsToSpawn = int.Parse(bombs.text);
        uncoveredBombs = bombsToSpawn;
        bombsLeftText.text = "Bombs: " + uncoveredBombs.ToString();


        grid = new Tile[gridWidth, gridHeight];
        Debug.Log("Grid Generating");
        int index = 0;

        for (int j = 0; j < gridHeight; j++)
        {
            for (int i = 0; i < gridWidth; i++)
            {
                int RowPos = (int)BoxPrefab.GetComponent<RectTransform>().rect.height * j;
                int ColPos = (int)BoxPrefab.GetComponent<RectTransform>().rect.width * i;

                GameObject tempTile = Instantiate(BoxPrefab, new Vector2(ColPos + tileGapSize * i, RowPos + tileGapSize * j) + gridOffset, quaternion.identity);
                tempTile.transform.SetParent(canvas.transform, false);

                grid[i, j] = tempTile.GetComponent<Tile>();

                grid[i, j].tileIndex = index;
                index++;

            }
        }


        while (bombsToSpawn > 0)
        {
            int tempRandX = UnityEngine.Random.Range(0, gridWidth);
            int tempRandY = UnityEngine.Random.Range(0, gridHeight);

            if (grid[tempRandX, tempRandY].isBomb != true)
            {
                grid[tempRandX, tempRandY].isBomb = true;
                //.Log(grid[tempRandX, tempRandY].tileIndex);

                bombsToSpawn--;
            }

        }
    }

    public void CheckIfGameOver()
    {
        foreach (Tile tile in grid)
        {
            if(tile.GetComponent<Button>().IsInteractable()&&!tile.isBomb)
            {
                //there are still interactable tiles, the game continues
                return; 
            }
        }
        // we know there are no more interactable tiles that arent bombs

        //TODO: end game as a win because a bomb didnt explode to lose game

        winLoseText.text = "You Win, no more bombs";
        winLoseText.gameObject.SetActive(true);
        stopWatch.StopStopwatch();

        Debug.Log("You Win, no more bombs");
    }

    public void LoseGame()
    {
     
        // turn on winlose text
        winLoseText.gameObject.SetActive(true);

        // show all bombs and set unselected ones to noninteractable
        foreach (Tile tile in grid)
        {
            if (tile.GetComponent<Button>().IsInteractable())
            {
                //there are still interactable tiles, the game continues
                tile.GetComponent<Button>().interactable = false;

                if(tile.isBomb)
                {
                    tile.GetComponent<Image>().color = Color.red;
                }
                
            }
        }
        stopWatch.StopStopwatch();
        winLoseText.text = "You Lose, hit a bomb";
    }

    public void RightClick(Button targetButton)
    {
        //todo flag placing functionality 

        ToggleFlag(targetButton);

       
    }
    
    private void ToggleFlag(Button button)
    {
        Tile clickedTile = button.GetComponent<Tile>();
        if (clickedTile == null) return;

         Color clickedTileColor = clickedTile.GetComponent<Image>().color;
        //check if thee tile is unflagged
        if (clickedTileColor == unflaggedColor)
        {
            clickedTile.GetComponent<Image>().color = flaggedColor;
            uncoveredBombs--;
            bombsLeftText.text = "Bombs: " + uncoveredBombs.ToString();
        }
        else if(clickedTileColor == flaggedColor)
        {
            clickedTile.GetComponent<Image>().color = unflaggedColor;
            uncoveredBombs++;
            bombsLeftText.text = "Bombs: " + uncoveredBombs.ToString();
        }

        
    }


    public void RestartGame()
    {
        foreach(Tile tile in grid)
        {
            GameObject.Destroy(tile.gameObject);
        }
        grid = null;
        stopWatch.ResetStopwatch();
        GenerateGrid();
    }


// Helper Functions
    public Tile GetTile(Vector2Int coords)
    {
        return grid[coords.x, coords.y];
    }


    public int GetIndexFromCoords(int x, int y)
    {
        return x + y * gridWidth;
    }

    public Vector2Int GetCoordsFromIndex(int index)
    {

        return new Vector2Int(index % gridWidth, index / gridWidth);
    }



}
