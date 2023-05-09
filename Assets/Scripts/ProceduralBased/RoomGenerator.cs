using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator
{
    private int maxIterations;
    private int roomLengthMin;
    private int roomWidthMin;

    public RoomGenerator(int maxIterations, int roomLengthMin, int roomWidthMin)
    {
        this.maxIterations = maxIterations;
        this.roomLengthMin = roomLengthMin;
        this.roomWidthMin = roomWidthMin;
    }

    public List<RoomNode> GenerateRooms(List<Node> roomSpaces, float roomBottomModifier, float roomTopModifier, int roomOffset)
    {
        List<RoomNode> listToReturn = new List<RoomNode>();
        foreach (var space in roomSpaces)
        {
            Vector2Int newBottomLeft = StructureHelper.GenerateBottomLeftCorner(space.BottomLeftCorner, space.TopRightCorner, roomBottomModifier, roomOffset);
            Vector2Int newTopRight = StructureHelper.GenerateTopRightCorner(space.BottomLeftCorner, space.TopRightCorner, roomTopModifier, roomOffset);

            space.BottomLeftCorner = newBottomLeft;
            space.TopRightCorner = newTopRight;
            space.BottomRightCorner = new Vector2Int(newTopRight.x, newBottomLeft.y);
            space.TopLeftCorner = new Vector2Int(newBottomLeft.x, newTopRight.y);
            listToReturn.Add((RoomNode)space);
        }
        return listToReturn;
    }
}