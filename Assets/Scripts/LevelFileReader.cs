using System.IO;
using System.Linq;
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
                            grid[i,j] = Instantiate(wallPrefab, position, Quaternion.identity);
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
}
