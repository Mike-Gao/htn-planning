using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using UnityEngine;
using Random = UnityEngine.Random;

using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public GameObject rock;

    public GameObject crate;

    public Steering mouseprefab;

    public int obstacleCount;
    public int mouseCount;
    public static int tileScale = 3; //default tile size 3
    public List<Vector2Int> allpoints = new List<Vector2Int>();

    public List<GameObject> obstacles = new List<GameObject>();

    public List<Steering> mice = new List<Steering>();
    public Transform start;
    public Transform end;

    public Transform spawn_pos;

    public Self player;

    public Text displayWin;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Self>();
        GeneratePath();
        GenerateRandomObstacles();
        SpawnMouse();
        InvokeRepeating(nameof(UpdateRandom), 0, 2);
    }

    void UpdateRandom(){
        Global.ws.random = Random.Range(0f, 1f);
    }


    // Update is called once per frame
    void Update()
    {   
        // If go back to spawn and treasure pickedup, player wins
        if(Global.ws.treasurePickedUp && (player.transform.position - spawn_pos.position).sqrMagnitude < 20) {
            displayWin.text = "YOU WIN!";
            //Pause the simulation
            Time.timeScale = 0;
        }
        if (Global.ws.hit == 3) {
            displayWin.text = "YOU LOST!";
            Time.timeScale = 0;
        }

    }

    void GenerateRandomObstacles()
    {
        int spawnedCount = 0;
        while(spawnedCount < obstacleCount)
        {
            Vector3 pos = new Vector3(Random.Range(start.position.x, end.position.x), 0, Random.Range(start.position.z, end.position.z));
            if(!Physics.CheckBox(pos, Global.obstacleHalfExtents)){
                GameObject prefab = Random.value > 0.5 ? rock : crate;
                obstacles.Add(Instantiate(prefab, pos, Quaternion.Euler(0, Random.Range(0f, 90f), 0)));
                spawnedCount++;
            }
        }

    }

    void SpawnMouse()
    {
        int spawnedCount = 0;
        while (spawnedCount < mouseCount)
        {
            Vector3 pos = new Vector3(Random.Range(start.position.x, end.position.x), 0, Random.Range(start.position.z, end.position.z));
            if(!Physics.CheckBox(pos, Global.obstacleHalfExtents)){
                mice.Add(Instantiate(mouseprefab, pos, Quaternion.Euler(0, Random.Range(0f, 90f), 0)));
                spawnedCount++;
            }
        }

    }

    void SpawnFloor(int x, int y)
    {
        // Spawn a tile
        GameObject prefab = Random.value > 0.5 ? rock : crate;
        obstacles.Add(Instantiate(prefab, new Vector3(x, 0.1f, y), Quaternion.identity));
    }

    List<int> GetAvailableDirection(int x, int y, List<Vector2Int> visited)
    {
        // visited: track already visited tiles
        List<int> direction = new List<int>();
        for(int i = 0; i < 4; i++)
        {
            direction.Add(i);
        }
        if (y == 7 * tileScale || visited.Contains(new Vector2Int(x, y + 3 * tileScale)))
        {
            // Cannot go forward
            direction.Remove(0);
        }
        if (y == tileScale || (x == 7 * tileScale && y == 4 * tileScale) || visited.Contains(new Vector2Int(x, y - 3 * tileScale))) 
        {
            // Cannot go backward
            direction.Remove(1);
        }
        if (x == tileScale || (y == 7 * tileScale && x == 4 * tileScale) || visited.Contains(new Vector2Int(x - 3 * tileScale, y)))
        {
            // Cannot go left
            direction.Remove(2);
        }
        if (x == 7 * tileScale || visited.Contains(new Vector2Int(x + 3 * tileScale, y)))
        {
            // Cannot go right
            direction.Remove(3);
        }
        // Return a set of available direction
        return direction;
    }

    void GeneratePath()
    {
        List<Vector2Int> visited = new List<Vector2Int>();
        int x = tileScale;
        int y = tileScale;
        visited.Add(new Vector2Int(x, y));
        SpawnFloor(x, y);
        int i = Random.Range(0, GetAvailableDirection(x, y, visited).Count);
        // Generate path leading up to the maze entrance
        while (x != 7 * tileScale || y != 7 * tileScale)
        {

            switch (GetAvailableDirection(x, y, visited)[i])
            {
                case 0:
                    for (int j = 1; j < 4; ++j)
                    {
                        SpawnFloor(x, y + j * tileScale);
                        allpoints.Add(new Vector2Int(x, y + j * tileScale));
                    }
                    y = y + 3 * tileScale;
                    visited.Add(new Vector2Int(x, y));
                    break;
                case 1:
                    for (int j = 1; j < 4; ++j)
                    {
                        SpawnFloor(x, y - j * tileScale);
                        allpoints.Add(new Vector2Int(x, y - j * tileScale));
                    }
                    y = y - 3 * tileScale;
                    visited.Add(new Vector2Int(x, y));
                    break;
                case 2:
                    for (int j = 1; j < 4; ++j)
                    {
                        SpawnFloor(x - j * tileScale, y);
                        allpoints.Add(new Vector2Int(x - j * tileScale, y));
                    }
                    x = x - 3 * tileScale;
                    visited.Add(new Vector2Int(x, y));
                    break;
                case 3:
                    for (int j = 1; j < 4; ++j)
                    {
                        SpawnFloor(x + j * tileScale, y);
                        allpoints.Add(new Vector2Int(x + j * tileScale, y));
                    }
                    x = x + 3 * tileScale;
                    visited.Add(new Vector2Int(x, y));
                    break;
            }    
            i = Random.Range(0, GetAvailableDirection(x, y, visited).Count);
        }

    }
}
