using System.IO;
using UnityEngine;

public class LevelFileReader : MonoBehaviour
{
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject boxPrefab;
    [SerializeField] private GameObject floorPrefab;
    [SerializeField] private GameObject goalPrefab;
    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private float tileSize = 1.0f;
    [SerializeField] private string fileName = "level.txt";

    private GameObject[,] grid;

    void Start()
    {
        LoadLevelFromFile();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            InstantiatePrefabOnHover(floorPrefab);
        }
        else if (Input.GetKey(KeyCode.P))
        {
            InstantiatePrefabOnHover(playerPrefab);
        }
        else if (Input.GetKey(KeyCode.B))
        {
            InstantiatePrefabOnHover(boxPrefab);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            InstantiatePrefabOnHover(wallPrefab);
        }
        else if (Input.GetKey(KeyCode.G))
        {
            InstantiatePrefabOnHover(goalPrefab);
        }
    }

    void InstantiatePrefabOnHover(GameObject prefab)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            int x = Mathf.FloorToInt(hit.point.x / tileSize);
            int y = Mathf.FloorToInt(hit.point.y / tileSize);

            if (x >= 0 && x < grid.GetLength(1) && y >= 0 && y < grid.GetLength(0))
            {
                if (grid[y, x] != null)
                {
                    Destroy(grid[y, x]);
                }

                Vector3 position = new Vector3(x * tileSize, y * tileSize, 0);
                grid[y, x] = Instantiate(prefab, position, Quaternion.identity);
            }
        }
    }

    void LoadLevelFromFile()
    {
        /*var lines = File.ReadLines($"/Users/{Application.streamingAssetsPath}/50").ToArray();

        for (int j = 0; j < lines[i].Length; j++)
        {
            if (lines[i][j] == "@")
            {
                Instantiate(wallPrefab);
            }

            if (lines[i][j] == " ")
            {
                Instantiate(floorPrefab);
            }

            if (lines[i][j] == "x")
            {
                Instantiate(goalPrefab);
            }

            if (lines[i][j] == "o")
            {
                Instantiate(boxPrefab);
            }

            if (lines[i][j] == "<")
            {
                Instantiate(playerPrefab);
            }
        } */

        var filePath = File.ReadLines($"/Users/{Application.streamingAssetsPath}, {fileName}");

        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            int numberOfRows = lines.Length;
            int numberOfColumns = lines[0].Length;
            grid = new GameObject[numberOfRows, numberOfColumns];

            for (int i = 0; i < numberOfRows; i++)
            {
                for (int j = 0; j < numberOfColumns; j++)
                {
                    char tile = lines[i][j];
                    Vector3 position = new Vector2(j * tileSize, i * tileSize);

                    switch (tile)
                    {
                        case '@':
                            grid[i, j] = Instantiate(wallPrefab, position, Quaternion.identity);
                            break;
                        case ' ':
                            grid[i, j] = Instantiate(floorPrefab, position, Quaternion.identity);
                            break;
                        case 'x':
                            grid[i, j] = Instantiate(goalPrefab, position, Quaternion.identity);
                            break;
                        case 'o':
                            grid[i, j] = Instantiate(boxPrefab, position, Quaternion.identity);
                            break;
                        case '<':
                        case '>':
                        case '^':
                        case 'v':
                            grid[i, j] = Instantiate(playerPrefab, position, Quaternion.identity);
                            break;
                    }
                }
            }
        }
    }

    public void SaveLevelToFile()
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        string[] levelData = new string[rows];

        for (int i = 0; i < rows; i++)
        {
            string row = "";
            for (int j = 0; j < cols; j++)
            {
                if (grid[i, j] == null)
                {
                    row += " ";
                }
                else if (grid[i, j].CompareTag("Wall"))
                {
                    row += "@";
                }
                else if (grid[i, j].CompareTag("Floor"))
                {
                    row += " ";
                }
                else if (grid[i, j].CompareTag("Goal"))
                {
                    row += "x";
                }
                else if (grid[i, j].CompareTag("Box"))
                {
                    row += "o";
                }
                else if (grid[i, j].CompareTag("Player"))
                {
                    row += "<";
                }
            }
            levelData[i] = row;
        }

        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        File.WriteAllLines(filePath, levelData);
    }
}