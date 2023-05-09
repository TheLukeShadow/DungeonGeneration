using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public class Cell
    {
        public bool visited = false;
        public bool[] status = new bool[4];
    }

    [System.Serializable]
    public class Rule
    {
        public GameObject room;
        public Vector2Int minPosition;
        public Vector2Int maxPosition;

        public bool obligatory;

        public int ProbabilityofSpawning(int x, int y)
        {
            //0 -cant spawn, 1 -can spawn, 2-has to spawn

            if (x>=minPosition.x && x<=maxPosition.x && y >= minPosition.y && y <= maxPosition.y)
            {
                if (obligatory) return 2; 
                else return 1;
            }

            return 0;
        }
    }

    [SerializeField] Vector2Int size;
    [SerializeField] int startPos = 0;
    [SerializeField] Rule[] rooms;
    public Vector2 roomOffset;

    List<Cell> board;
   

    // Start is called before the first frame update
    void Start()
    {
        MazeGenerator();
    }


    private void GenerateDungeon()
    {

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                Cell currentCell = board[(i+ j *size.x)];
                if (currentCell.visited)
                {
                    int randomRoom = -1;
                    List<int> availableRooms = new List<int>();

                    for(int k = 0; k < rooms.Length; k++)
                    {
                        int p = rooms[k].ProbabilityofSpawning(i, j);

                        if(p == 2)
                        {
                            randomRoom = k;
                            break;
                        }
                        else if (p == 1)
                        {
                            availableRooms.Add(k);
                        }
                    }

                    if(randomRoom == -1)
                    {
                        if (availableRooms.Count > 0)
                        {
                            randomRoom = availableRooms[Random.Range(0, availableRooms.Count)];
                        }
                        else
                        {
                            randomRoom = 0;
                        }
                    }

                    //int randomRoom = Random.Range(0, rooms.Length);
                 var newRoom = Instantiate(rooms[randomRoom].room, new Vector3(i * roomOffset.x, 0, -j * roomOffset.y), Quaternion.identity, transform).GetComponent<RoomBehaviour>();
                 newRoom.UpdateRoom(currentCell.status);

                  newRoom.name += " " + i + "-" + j;
                }
            }
        }
    }



    void MazeGenerator()
    {
        board = new List<Cell>();

        for (int i = 0; i < size.x; i++)
            {
            for (int j = 0; j < size.y; j++)
            {
                board.Add(new Cell());  
            }
        }
        int currentCell = startPos;

        Stack<int> path = new Stack<int>();

        int k = 0;

        while (k < 1000)
        {
            k++;

            board[currentCell].visited = true;

            if(currentCell== board.Count -1)
            {
                break;
            }

            List<int> neighbbours = CheckNeighbors(currentCell);

            if(neighbbours.Count == 0 )
            {
                if(path.Count == 0)
                {
                    break;
                }
                else
                {
                    currentCell= path.Pop();
                }
            }
            else
            {
                path.Push(currentCell);

                int newCell = neighbbours[Random.Range(0, neighbbours.Count)];

                if(newCell>currentCell)
                {
                    //right then down
                    if(newCell-1 == currentCell)
                    {
                        board[currentCell].status[2] = true;
                        currentCell= newCell;
                        board[currentCell].status[3] = true;
                    }
                    else
                    {
                        board[currentCell].status[1] = true;
                        currentCell = newCell;
                        board[currentCell].status[0] = true;
                    }
                }
                else
                {
                    //left then up
                    if (newCell + 1 == currentCell)
                    {
                        board[currentCell].status[3] = true;
                        currentCell = newCell;
                        board[currentCell].status[2] = true;
                    }
                    else
                    {
                        board[currentCell].status[0] = true;
                        currentCell = newCell;
                        board[currentCell].status[1] = true;
                    }
                }
            }

        }
        GenerateDungeon();
    }

    List<int> CheckNeighbors(int cell)
    {
        List<int> neighbours = new List<int>();
        //check up neighbour, then down, right, left
        if(cell - size.x >= 0 && !board[(cell-size.x)].visited)
        {
            neighbours.Add(Mathf.FloorToInt(cell - size.x));
        }
        if (cell + size.x < board.Count && !board[(cell + size.x)].visited)
        {
            neighbours.Add(Mathf.FloorToInt(cell + size.x));
        }
        if ((cell+1) % size.x != 0 && !board[(cell +1)].visited)
        {
            neighbours.Add(Mathf.FloorToInt(cell + 1));
        }
        if (cell % size.x != 0 && !board[(cell - 1)].visited)
        {
            neighbours.Add(Mathf.FloorToInt(cell - 1));
        }



        return neighbours;
    }

}
