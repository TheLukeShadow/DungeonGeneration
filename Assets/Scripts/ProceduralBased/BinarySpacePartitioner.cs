using System;
using System.Collections.Generic;
using UnityEngine;


public class BinarySpacePartitioner
{
    RoomNode rootNode;
    
    public RoomNode RootNode { get => rootNode; }

    public BinarySpacePartitioner(int dungeonWidth, int dungeonLength)
    {
        this.rootNode = new RoomNode(new Vector2Int(0, 0), new Vector2Int(dungeonWidth, dungeonLength), null, 0);
    }

    public List<RoomNode> PrepareNodesCollection(int maxIterations, int roomWidthMin, int roomLengthMin)
    {
        Queue<RoomNode> graph = new Queue<RoomNode>();
        List<RoomNode> listToReturn = new List<RoomNode>();
        graph.Enqueue(this.rootNode);
        listToReturn.Add(this.rootNode);
        int iterations = 0;
        while (iterations < maxIterations && graph.Count >0)
        {
            iterations++;
            RoomNode currentNode = graph.Dequeue();
            if(currentNode.Width >= roomWidthMin*2 || currentNode.Length >= roomLengthMin*2) 
            {
                SplitSpace(currentNode, listToReturn, roomLengthMin, roomWidthMin, graph); 
            }
        }
        return listToReturn;
    }

    private void SplitSpace(RoomNode currentNode, List<RoomNode> listToReturn, int roomLengthMin, int roomWidthMin, Queue<RoomNode> graph)
    {
        Line line = GetLineDivider(currentNode.BottomLeftCorner, currentNode.TopRightCorner, roomWidthMin, roomLengthMin);

        RoomNode node1;
        RoomNode node2;
        if(line.Orientation == Orientation.Horizontal)
        {
            node1 = new RoomNode(currentNode.BottomLeftCorner, new Vector2Int(currentNode.TopRightCorner.x, line.Coordinates.y), currentNode, currentNode.TreeLayerIndex + 1);
            node2 = new RoomNode(new Vector2Int(currentNode.BottomLeftCorner.x, line.Coordinates.y), currentNode.TopRightCorner, currentNode, currentNode.TreeLayerIndex + 1);
        }
        else
        {
            node1 = new RoomNode(currentNode.BottomLeftCorner, new Vector2Int(line.Coordinates.x, currentNode.TopRightCorner.y), currentNode, currentNode.TreeLayerIndex + 1);
            node2 = new RoomNode(new Vector2Int(line.Coordinates.x, currentNode.BottomLeftCorner.y), currentNode.TopRightCorner, currentNode, currentNode.TreeLayerIndex + 1);
        }
        AddNodeToCollection(listToReturn, graph, node1);
        AddNodeToCollection(listToReturn, graph, node2);
    }

    private void AddNodeToCollection(List<RoomNode> listToReturn, Queue<RoomNode> graph, RoomNode node)
    {
        listToReturn.Add(node);
        graph.Enqueue(node);
    }

    private Line GetLineDivider(Vector2Int bottomLeftCorner, Vector2Int topRightCorner, int roomWidthMin, int roomLengthMin)
    {
        Orientation orientation;
        bool lengthStatus = (topRightCorner.y - bottomLeftCorner.y) >= 2 * roomLengthMin;
        bool widthStatus = (topRightCorner.x - bottomLeftCorner.x) >= 2 * roomWidthMin;
        if (lengthStatus && widthStatus)
        {
            orientation = (Orientation)(UnityEngine.Random.Range(0, 2));
        }
        else if (widthStatus)
        {
            orientation = Orientation.Vertical;
        }
        else
        {
            orientation = Orientation.Horizontal;
        }
        return new Line(orientation, GetCoordinatesForOrienatation(orientation, bottomLeftCorner, topRightCorner, roomWidthMin, roomLengthMin));
    }

    private Vector2Int GetCoordinatesForOrienatation(Orientation orientation, Vector2Int bottomLeftCorner, Vector2Int topRightCorner, int roomWidthMin, int roomLengthMin)
    {
        Vector2Int coordinates = Vector2Int.zero;
        if(orientation == Orientation.Horizontal)
        {
            coordinates = new Vector2Int(0, UnityEngine.Random.Range((bottomLeftCorner.y + roomLengthMin), (topRightCorner.y - roomLengthMin)));
        }
        else
        {
            coordinates = new Vector2Int(UnityEngine.Random.Range((bottomLeftCorner.x + roomWidthMin), (topRightCorner.x - roomWidthMin)), 0);
        }
        return coordinates;

    }
}