using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DungeonCreator : MonoBehaviour
{
    [SerializeField] int dungeonWidth;
    [SerializeField] int dungeonLength;
    [SerializeField] int roomWidthMin;
    [SerializeField] int roomLengthMin;
    [SerializeField] int maxIterations;
    [SerializeField] int corridorWidth;
    [SerializeField] Material material;
    [Range(0.0f,0.3f)]
    [SerializeField] float roomBottomModifier;
    [Range(0.7f, 1f)]
    [SerializeField] float roomTopModifier;
    [Range(0.0f, 2f)]
    [SerializeField] int roomOffset;

   [SerializeField] GameObject wallVerticcal;
    [SerializeField] GameObject wallHorizontal;
    List<Vector3Int> possibleDoorVertical;
    List<Vector3Int> possibleDoorHorizontal;
    List<Vector3Int> possibleWallHorizontal;
    List<Vector3Int> possibleWallVertical;


    // Start is called before the first frame update
    void Start()
    {
        CreateDungeon();
    }

    public void CreateDungeon()
    {
        DestroyAllChildren();
        DungeonsGenerator generator = new DungeonsGenerator(dungeonWidth, dungeonLength);
        var listOfRooms = generator.CalculateDungeon(maxIterations, roomWidthMin, roomLengthMin, roomBottomModifier, roomTopModifier, roomOffset, corridorWidth);
        GameObject wallParent = new GameObject("WallParent");
        wallParent.transform.parent = transform;
        possibleDoorVertical = new List<Vector3Int>();
        possibleDoorHorizontal = new List<Vector3Int>();
        possibleWallHorizontal = new List<Vector3Int>();
        possibleWallVertical = new List<Vector3Int>();
        for (int i = 0; i < listOfRooms.Count; i++)
        {
            CreateMesh(listOfRooms[i].BottomLeftCorner, listOfRooms[i].TopRightCorner);
        }
        CreateWalls(wallParent);
    }

    private void CreateWalls(GameObject wallParent)
    {
        foreach(var wallPosition in possibleWallHorizontal)
        {
            CreateWall(wallParent, wallPosition, wallHorizontal);
        }
        foreach(var wallPosition in possibleWallVertical)
        {
            CreateWall(wallParent, wallPosition, wallVerticcal);
        }
    }

    private void CreateWall(GameObject wallParent, Vector3Int wallPosition, GameObject wallPrefab)
    {
        Instantiate(wallPrefab, wallPosition, Quaternion.identity, wallParent.transform);
    }

    private void CreateMesh(Vector2 bottomLeftCorner, Vector2 topRightCorner)
    {
        Vector3 bottomLeftV = new Vector3(bottomLeftCorner.x, 0, bottomLeftCorner.y);
        Vector3 bottomRightV = new Vector3(topRightCorner.x, 0, bottomLeftCorner.y);
        Vector3 topLeftV = new Vector3(bottomLeftCorner.x, 0, topRightCorner.y);
        Vector3 topRightV = new Vector3(topRightCorner.x, 0, topRightCorner.y);

        Vector3[] vertices = new Vector3[]
        {
            topLeftV, topRightV, bottomLeftV, bottomRightV,
        };

        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }

        int[] triangles = new int[]
        {
            0, 1, 2, 2, 1, 3
        };
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        GameObject dungeonFloor = new GameObject("Mesh" + bottomLeftCorner, typeof(MeshFilter), typeof(MeshRenderer));

        dungeonFloor.transform.position = Vector3.zero;
        dungeonFloor.transform.localScale = Vector3.one;
        dungeonFloor.GetComponent<MeshFilter>().mesh = mesh;

        dungeonFloor.GetComponent<MeshRenderer>().material = material;
        dungeonFloor.transform.parent = transform;

        for (int row = (int)bottomLeftV.x; row < (int)bottomRightV.x; row++)
        { 
            var wallPosition = new Vector3(row,0,bottomLeftV.z);
            AddWallPositionToList(wallPosition, possibleWallHorizontal, possibleDoorHorizontal);
        }
        for(int row = (int)topLeftV.x; row < (int)topRightCorner.x; row++)
        {
            var wallPosition = new Vector3(row, 0, topRightV.z);
            AddWallPositionToList(wallPosition, possibleWallHorizontal, possibleDoorHorizontal);
        }
        for (int col = (int)bottomLeftV.z; col < (int)topLeftV.z; col++)
        {
            var wallPosition = new Vector3(bottomLeftV.x, 0, col);
            AddWallPositionToList(wallPosition, possibleWallVertical, possibleDoorVertical);
        }
        for (int col = (int)bottomRightV.z; col < (int)topRightV.z; col++)
        {
            var wallPosition = new Vector3(bottomRightV.x, 0, col);
            AddWallPositionToList(wallPosition, possibleWallVertical, possibleDoorVertical);
        }
    }

    private void AddWallPositionToList(Vector3 wallPosition, List<Vector3Int> wallList, List<Vector3Int> doorList)
    {
        Vector3Int point = Vector3Int.CeilToInt(wallPosition);
        if (wallList.Contains(point))
        {
            doorList.Add(point);
            wallList.Remove(point);
        }
        else
        {
            wallList.Add(point);
        }
    }

    private void DestroyAllChildren()
    {
        while(transform.childCount != 0) 
            {
            foreach(Transform item in transform)
            {
                DestroyImmediate(item.gameObject);
            }
            } 
    }
}
