using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject BoxPrefab;


    public int bombNumber = 10;
    private Tile[,] grid;

    [SerializeField] TMP_InputField height;
    [SerializeField] TMP_InputField width;
    [SerializeField] TMP_InputField  bombs;

    [SerializeField] TMP_Text timeText;
    [SerializeField] TMP_Text bombsLeftText;



    [SerializeField] Canvas canvas;
    [SerializeField] Vector2 gridOffset;
    [SerializeField]public int gridWidth = 10;
    [SerializeField] public int gridHeight = 10;
    [SerializeField] float tileGapSize = 0.5f;

   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        GenerateGrid();
        

    }

    private void GenerateGrid()
    {
        gridHeight = int.Parse(height.text);
        gridWidth = int.Parse(width.text);
        bombNumber = int.Parse(bombs.text);


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


        while (bombNumber > 0)
        {
            int tempRandX = UnityEngine.Random.Range(0, gridWidth);
            int tempRandY = UnityEngine.Random.Range(0, gridHeight);

            if (grid[tempRandX, tempRandY].isBomb != true)
            {
                grid[tempRandX, tempRandY].isBomb = true;
                Debug.Log(grid[tempRandX, tempRandY].tileIndex);

                bombNumber--;
            }

        }
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
