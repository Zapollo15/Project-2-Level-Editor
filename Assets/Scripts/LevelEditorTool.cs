using System.IO;
using System.Linq;
using UnityEngine;

public class LevelEditorTool : MonoBehaviour
{
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject boxPrefab;
    [SerializeField] private GameObject floorPrefab;
    [SerializeField] private GameObject goalPrefab;
    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private float tileSize = 1.0f;

    [SerializeField] public int fileIndex;

    int height;
    int width;

    private GameObject[,] grid;

    void Awake()
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

    public void LoadLevelFromFile()
    {

        var filePath = File.ReadLines($"{Application.streamingAssetsPath}/{fileIndex}.txt").ToArray();

        height = filePath[1].Length;
        width = filePath[0].Length;

        grid = new GameObject[height, width];

        for (int i = 0; i < filePath.Length; i++)
        {
            string line = filePath[i];
            for (int j = 0; j < line.Length; j++)
            {
                char tile = line[j];
                Vector2 position = new Vector2(i * tileSize, j * tileSize);

                if (tile == '@')
                {
                    grid[i, j] = Instantiate(wallPrefab, position, Quaternion.identity);
                }

                if (tile == 'x')
                {
                    grid[i, j] = Instantiate(goalPrefab, position, Quaternion.identity);
                }

                if (tile == ' ')
                {
                    grid[i, j] = Instantiate(floorPrefab, position, Quaternion.identity);
                }

                if (tile == 'o')
                {
                    grid[i, j] = Instantiate(boxPrefab, position, Quaternion.identity);
                }

                if (tile == '<' || tile == '>' || tile == 'v' || tile == '^')
                {
                    grid[i, j] = Instantiate(playerPrefab, position, Quaternion.identity);
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

        var filePath = Path.Combine($"{Application.streamingAssetsPath}, {fileIndex}.txt");
        File.WriteAllLines(filePath, levelData);
    }
}