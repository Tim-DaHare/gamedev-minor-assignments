using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellState
{
    Empty,
    Room,
    NextToRoom,
}



public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] private int _gridSizeX = 100;
    [SerializeField] private int _gridSizeY = 100;
    [SerializeField] private int _roomCount = 20;
    [SerializeField] private int _roomMinWidth = 2;
    [SerializeField] private int _roomMaxWidth = 10;
    [SerializeField] private int _roomMinHeight = 2;
    [SerializeField] private int _roomMaxHeight = 10;
    [SerializeField] private int _attemptsPerRoom = 50;
    
    public class Room
    {
        public Room(int width, int length)
        {
            Width = width;
            Length = length;
        }
        public int Width { get; }
        public int Length { get; }

        public bool DoesOverlapWith(Room room)
        {
        
        }
    }

    private CellState[,] _gridData;
    private Room[] _rooms;
    
    private void Awake()
    {
        InitializeGrid(_gridSizeX, _gridSizeY);
        GenerateRooms(_roomCount);
        PopulateGrid();
    }
    
    private void InitializeGrid(int xSize, int ySize)
    {
        _gridData = new CellState[xSize, ySize];
    }

    private void GenerateRooms(int roomCount)
    {
        _rooms = new Room[roomCount];
        for (var i = 0; i < roomCount; i++)
        {
            var room = new Room(Random.Range(_roomMinWidth, _roomMaxWidth), Random.Range(_roomMinHeight, _roomMaxHeight));
            _rooms[i] = room;
        }
    }
    
    private void PopulateGrid()
    {
        foreach (var room in _rooms)
        {
            AttemptRoomPlacement(room);
        }
    }
    
    private void AttemptRoomPlacement(Room room)
    {
        for (var i = 0; i < _attemptsPerRoom; i++)
        {
            var randomPos = new Vector2(Random.Range(0, _gridSizeX), Random.Range(0, _gridSizeY));
            
            // if room doesn't fit on the grid skip this attempt and go to the next attempt
            if (randomPos.x + room.Width > _gridSizeX || randomPos.y + room.Length > _gridSizeY)
                continue;
            
            // if room overlaps with other rooms skip this attempt and go to the next attempt
            var isOverlapping = false;
            foreach (var r in _rooms)
            {
                if (r == room)
                    continue;
                
                
            }
            
            if (isOverlapping)
                continue;

            break;
        }
    }
}
