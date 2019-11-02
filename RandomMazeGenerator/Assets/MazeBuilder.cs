using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeBuilder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // endPoint(5, 5, startPoint(5, 5));
    }

    // Struct to make coordinate data easier to manage
    public struct Point
    {
        public int x; // x-axis
        public int y; // y-axis
    }

    // Struct to make canReach() method return data more informative
    public struct Path
    {
        public bool reachable; // can the 2 points reach each other
        public Point[] path; // the path from point a to b (if one exists)
    }

    // Random number variable to be used repeatedly
    System.Random rand = new System.Random();

    /*
    // Main Functions

    // Designs the maze behind the scenes
    // Returns the designed maze
    // x is maze width
    // y is maze length
    // d is maze difficulty
    int[,] designMaze(int x, int y, int d)
    {

    }

    // Builds the maze visually in Unity
    // maze is the designed maze
    void createMaze(int[,] maze)
    {

    }

    // Helper Functions

    // Returns an initialized maze with start, end, and dead end points
    int[,] initializeMaze(Point start, Point end, Point[] deadEnds)
    {

    }

    // Returns the number of dead ends in the maze
    // x & y are the size of the maze; d is the difficulty
    int deadEnds(int x, int y, int d)
    {

    }

    // Returns the randomized dead end locations
    // x and y are the size of the maze; n is number of dead ends
    // start and end points to avoid duplicate spots being chosen
    Point[] deadEndPoints(int x, int y, int n, Point startPoint, Point endPoint)
    {

    }
    */
    // Returns the randomized start position in the maze
    // x & y are the maze's size
    Point startPoint(int x, int y)
    {
        Point coord;
        coord.x = rand.Next(0, x);
        coord.y = rand.Next(0, y);
        // Debug.Log(coord.x);
        // Debug.Log(coord.y);
        return coord;
    }

    // Returns the randomized end position in the maze
    // x & y are maze's size
    // startPoint to avoid duplicate spots being chosen
    Point endPoint(int x, int y, Point startPoint)
    {
        Point coord;
        coord.x = rand.Next(0, x);
        coord.y = rand.Next(0, y);
        while (startPoint.x == coord.x && startPoint.y == coord.y)
        {
            coord.x = rand.Next(0, x);
            coord.y = rand.Next(0, y);
        }
        // Debug.Log(coord.x);
        // Debug.Log(coord.y);
        return coord;
    }
    /*
    // Returns true & the path if you can reach end from start
    // maze is the maze initialized maze
    // start and end are the test points for a possible path 
    // implementation initially as DFS (will optimize later)
    Path canReach(int[,] maze, Point start, Point end)
    {

    }  
    */   
}