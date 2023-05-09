using System;
using System.Collections.Generic;
using UnityEngine;

public static class StructureHelper
{
    public static List<Node> GetLowest(Node parentNode)
    {
        Queue<Node> nodesToCheck = new Queue<Node>();
        List<Node> listToReturn= new List<Node>();
        if(parentNode.ChildrenNodeList.Count == 0 )
        {
            return new List<Node>() { parentNode };
        }
    foreach (var child in parentNode.ChildrenNodeList)
        {
            nodesToCheck.Enqueue(child);
        }
    while(nodesToCheck.Count > 0)
        {
            var currentNode = nodesToCheck.Dequeue();
            if(currentNode.ChildrenNodeList.Count == 0 )
            {
                listToReturn.Add(currentNode);
            }
            else
            {
                foreach (var child in currentNode.ChildrenNodeList)
                {
                    nodesToCheck.Enqueue(child);
                }
            }
        }
        return listToReturn;
    }

    public static Vector2Int GenerateBottomLeftCorner(Vector2Int boundryLeft, Vector2Int boundryRight, float pointModifier, int offset)
    {
        int minX = boundryLeft.x+offset;
        int maxX = boundryRight.x-offset;
        int minY = boundryLeft.y+offset;
        int maxY = boundryRight.y - offset;

        return new Vector2Int(UnityEngine.Random.Range(minX,(int)(minX+(maxX-minX)*pointModifier)), UnityEngine.Random.Range(minY,(int)(minY+(maxY-minY)*pointModifier))); 
    }

    public static Vector2Int GenerateTopRightCorner(Vector2Int boundryLeft, Vector2Int boundryRight, float pointModifier, int offset)
    {
        int minX = boundryLeft.x + offset;
        int maxX = boundryRight.x - offset;
        int minY = boundryLeft.y + offset;
        int maxY = boundryRight.y - offset;

        return new Vector2Int(UnityEngine.Random.Range((int)(minX+(maxX-minX)*pointModifier),maxX ), UnityEngine.Random.Range((int)(minY+(maxY-minY)*pointModifier), maxY)); 
    }

    public static Vector2Int CalculateMIddlePoint(Vector2Int v1, Vector2Int v2)
    {
        Vector2 sum = v1 + v2;
        Vector2 tempVector = sum / 2;
        return new Vector2Int((int)tempVector.x, (int)tempVector.y);
    }
}
    public enum RelativePosition
    {
        Up, Down, Right, Left
    }