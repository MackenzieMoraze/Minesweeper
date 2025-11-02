using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Unity.UI;
using static UnityEngine.RuleTile.TilingRuleOutput;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    private GameController gameController;
    private List<Tile> neighbours;
    private int closeBombs;

    public bool isBomb = false;
    public bool isChecked = false;
    public int tileIndex;
    [SerializeField] TMP_Text text;

    Vector2Int[] cardinalOffsets = {
        new Vector2Int(0, 1), // North
        new Vector2Int(0, -1), // South
        new Vector2Int(1, 0), // East
        new Vector2Int(-1, 0) // West
    };

    Vector2Int[] diagonalOffsets = {
        new Vector2Int(1, 1), // Northeast
        new Vector2Int(-1, 1), // Northwest
        new Vector2Int(1, -1), // Southeast
        new Vector2Int(-1, -1) // Southwest
    };



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameController = FindAnyObjectByType<GameController>();

        neighbours = new List<Tile>();
        //if (isBomb) SetText("B");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetText(string text)
    {
        this.text.text = text;
    }

    public void Clicked()
    {
        //clickedIndex = tileIndex;
        if (isChecked == true) return;
        isChecked = true;
        if (isBomb)
        {
            SetText("B");
            GameController.instance.LoseGame();
            return;
        }

        FindNeighbours();
        FindBombsInNeighbours();

        if (closeBombs != 0)
        {
            SetText(closeBombs.ToString());
        }
        else
        {
            SetText("");
            foreach (Tile tile in neighbours)
            {
                tile.Clicked();
            }
        }
        GetComponent<Button>().interactable = false;

        GameController.Instance.CheckIfGameOver();
        
    }

    

    private void FindBombsInNeighbours()
    {
        for (int i = 0; i < neighbours.Count; i++)
        {
            Tile nTile = neighbours[i];
            if (nTile.isBomb)
            {
                closeBombs++;
            }
        }
    }

    public void FindNeighbours()
    {
        Vector2Int myCoords = gameController.GetCoordsFromIndex(tileIndex);
        int myX = myCoords.x;
        int myY = myCoords.y;

        foreach (Vector2Int offset in cardinalOffsets)
        {
            Vector2Int neighborPos = myCoords + offset;
            if (IsValidGridPosition(neighborPos, gameController.gridWidth, gameController.gridHeight))
            {
                neighbours.Add(gameController.GetTile(neighborPos));
            }
        }

        foreach (Vector2Int offset in diagonalOffsets)
        {
            Vector2Int neighborPos = myCoords + offset;
            if (IsValidGridPosition(neighborPos, gameController.gridWidth, gameController.gridHeight))
            {
                neighbours.Add(gameController.GetTile(neighborPos));
            }
        }
    }

    private bool IsValidGridPosition(Vector2Int position, int gridWidth, int gridHeight)
    {
        return position.x >= 0 && position.x < gridWidth &&
               position.y >= 0 && position.y < gridHeight;
    }
}
