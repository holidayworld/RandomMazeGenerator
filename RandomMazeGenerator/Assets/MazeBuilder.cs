using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Use for shopping assistant
public class MazeBuilder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // endPoint(5, 5, startPoint(5, 5));
        // deadEnds(99, 100, 3);
        /* 
        int x = 99;
        int y = 99;
        Point start = startPoint(x, y);
        Point end = endPoint(x, y, start);
        initializeMaze(x, y, start, end, deadEndPoints(x, y, deadEnds(x, y, 3), start, end));
        */
        /* 
        int[,] test = new int[4, 4] { {0, 0, 4, 0}, {4, 0, 4, 0}, {0, 0, 4, 0}, {0, 0, 0, 4} };
        Point begin;
        begin.x = 3;
        begin.y = 0;
        Point ending;
        ending.x = 0;
        ending.y = 3;
        Debug.Log(canReach(test, begin, ending));
        */
        
        int[,] testMaze = designMaze(50, 50, 1);
        /* 
        for (int i = 0; i < testMaze.GetLength(0); ++i)
        {
            for (int j = 0; j < testMaze.GetLength(1); ++j)
            {
                // Debug.Log("(" + i + ", " + j + "): " + testMaze[i, j]);
                // Debug.Log(testMaze[i, j]);
            }
        }
        */
        createMaze(testMaze);   
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

    // Size of the block component that makes up the maze
    const float blockX = 5f;
    const float blockY = 5f;
    const float blockZ = 5f;

    // Initial position in maze
    const float positX = 2.5f;
    const float positY = 2.5f;
    const float positZ = 2.5f;

    
    // Main Functions

    // Designs the maze behind the scenes
    // Returns the designed maze
    // x is maze width
    // y is maze length
    // d is maze difficulty
    int[,] designMaze(int x, int y, int d)
    {
        // Initialization of the maze
        Point start = startPoint(x, y);
        Point end = endPoint(x, y, start);
        int numDeadEnds = deadEnds(x, y, d);
        Point[] deadEndsLocations = deadEndPoints(x, y, numDeadEnds, start, end);
        int[,] maze = initializeMaze(x, y, start, end, deadEndsLocations);

        // Populate the maze with walls
        // 1st method is placing walls in order; 2nd method (in future) will be placing walls randomly
        Point[,] shuffledMaze = shuffle(maze);
        for (int i = 0; i < x; ++i)
        {
            for (int j = 0; j < y; ++j)
            {
                if (maze[shuffledMaze[i, j].x, shuffledMaze[i, j].y] == 0) // Point that hasn't been set yet
                {
                    maze[shuffledMaze[i, j].x, shuffledMaze[i, j].y] = 4; // Try to make it a wall
                    // Check to see if end and dead ends can still be reached from start
                    if (!canReach(maze, start, end))
                    {
                        maze[shuffledMaze[i, j].x, shuffledMaze[i, j].y] = 0;
                    }
                    else
                    {
                        bool failedOnce = false; // Used to short circuit following if statement to increase efficiency
                        for (int h = 0; h < deadEndsLocations.Length; ++h)
                        {
                            if (!failedOnce && !canReach(maze, start, deadEndsLocations[h]))
                            {
                                maze[shuffledMaze[i, j].x, shuffledMaze[i, j].y] = 0;
                                failedOnce = true;
                            }
                        }
                    }
                }
            }
        }
        return maze;
    }
    
    // Builds the maze visually in Unity
    // maze is the designed maze
    void createMaze(int[,] maze)
    {
        /* 
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        floor.transform.position = new Vector3(15f, 2.5f, 11f);
        floor.transform.localScale = new Vector3(5, 5, 5);
        */

        for (int i = 0; i < maze.GetLength(0); ++i)
        {
            for (int j = 0; j < maze.GetLength(1); ++j)
            {
                if (maze[i, j] == 4) // Point is a wall
                {
                    float xPosition = positX + (j * blockX);
                    float zPosition = positZ + (i * blockZ);
                    GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    wall.transform.localScale = new Vector3(blockX, blockY, blockZ);
                    wall.transform.position = new Vector3(xPosition, positY, zPosition);
                }
                if (maze[i, j] == 1) // Point is the start
                {
                    float xPosition = positX + (j * blockX);
                    float zPosition = positZ + (i * blockZ);
                    GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    wall.transform.localScale = new Vector3(1, 1, 1);
                    wall.transform.position = new Vector3(xPosition, positY, zPosition);
                    wall.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                    GameObject playableCharacter = GameObject.Find("FPSController");
                    playableCharacter.transform.position = new Vector3(xPosition, positY, zPosition);
                }
                if (maze[i, j] == 2) // Point is the end
                {
                    float xPosition = positX + (j * blockX);
                    float zPosition = positZ + (i * blockZ);
                    GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    wall.transform.localScale = new Vector3(1, 1, 1);
                    wall.transform.position = new Vector3(xPosition, positY, zPosition);
                    wall.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
                }
            }
        }
    }
    
    // Helper Functions

    // Returns an initialized maze with start, end, and dead end points
    // x & y are the maze's size
    int[,] initializeMaze(int x, int y, Point start, Point end, Point[] deadEnds)
    {
        int[,] maze = new int[x, y];
        maze[start.x, start.y] = 1; // 1 represents startPoint
        maze[end.x, end.y] = 2; // 2 represents endPoint
        for (int i = 0; i < deadEnds.Length; ++i)
        {
            maze[deadEnds[i].x, deadEnds[i].y] = 3; // 3 represents deadEnds
        }
        /* 
        for (int j = 0; j < x; ++j)
        {
            for (int h = 0; h < y; ++h)
            {
                Debug.Log(maze[j, h]);
            }
        }
        */
        return maze;
    }
    
    // Returns the number of dead ends in the maze
    // x & y are the size of the maze; d is the difficulty
    int deadEnds(int x, int y, int d)
    {
        // Need to come up with complex function later through testing
        // May need to consider edge case of super small maze (1x1, 1x2, etc)
        int deadEnds = 0;
        if (x < 10 || y < 10)
        {
            deadEnds = 1;
        }
        else if (x < 100 || y < 100)
        {
            deadEnds = 1 + 20;
        }
        else
        {
            deadEnds = 2 + d;
        }
        // Debug.Log(deadEnds);
        return deadEnds;
    }
    
    // Returns the randomized dead end locations
    // x and y are the size of the maze; n is number of dead ends
    // start and end points to avoid duplicate spots being chosen
    Point[] deadEndPoints(int x, int y, int n, Point startPoint, Point endPoint)
    {
        Point[] deadEndsLocation = new Point[n];
        for (int i = 0; i < n; ++i)
        {
            deadEndsLocation[i].x = rand.Next(0, x);
            deadEndsLocation[i].y = rand.Next(0, y);
            while ((startPoint.x == deadEndsLocation[i].x && startPoint.y == deadEndsLocation[i].y) 
            || (endPoint.x == deadEndsLocation[i].x && endPoint.y == deadEndsLocation[i].y))
            {
                deadEndsLocation[i].x = rand.Next(0, x);
                deadEndsLocation[i].y = rand.Next(0, y);
            }
        }
        /* 
        Debug.Log(deadEndsLocation[0].x);
        Debug.Log(deadEndsLocation[0].y);
        Debug.Log(deadEndsLocation[1].x);
        Debug.Log(deadEndsLocation[1].y);
        Debug.Log(deadEndsLocation[2].x);
        Debug.Log(deadEndsLocation[2].y);
        Debug.Log(deadEndsLocation[3].x);
        Debug.Log(deadEndsLocation[3].y);
        */
        return deadEndsLocation;
    }
    
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
    
    // Returns true (& the path when I optimize later) if you can reach end from start
    // maze is the maze initialized maze
    // start and end are the test points for a possible path 
    // implementation initially as DFS (will optimize later)
    bool canReach(int[,] maze, Point start, Point end)
    {
        // Iterative DFS
        int[,] visited = new int[maze.GetLength(0), maze.GetLength(1)];
        Stack xStack = new Stack();
        Stack yStack = new Stack();
        xStack.Push(start.x);
        yStack.Push(start.y);
        while (xStack.Count != 0)
        {
            // Work on point from top of stack
            int xTemp = (int)xStack.Pop();
            int yTemp = (int)yStack.Pop();
            if (visited[xTemp, yTemp] == 0)
            {
                visited[xTemp, yTemp] = 1;
            }
            // Push adjacent points onto stack
            if (isValidPoint(xTemp + 1, yTemp, maze)) // moving right
            {
                Point pathPart;
                pathPart.x = xTemp + 1;
                pathPart.y = yTemp;
                if (visited[pathPart.x, pathPart.y] == 0)
                {
                    xStack.Push(pathPart.x);
                    yStack.Push(pathPart.y);
                }
            }
            if (isValidPoint(xTemp - 1, yTemp, maze)) // moving left
            {
                Point pathPart;
                pathPart.x = xTemp - 1;
                pathPart.y = yTemp;
                if (visited[pathPart.x, pathPart.y] == 0)
                {
                    xStack.Push(pathPart.x);
                    yStack.Push(pathPart.y);
                }
            }
            if (isValidPoint(xTemp, yTemp + 1, maze)) // moving up
            {
                Point pathPart;
                pathPart.x = xTemp;
                pathPart.y = yTemp + 1;
                if (visited[pathPart.x, pathPart.y] == 0)
                {
                    xStack.Push(pathPart.x);
                    yStack.Push(pathPart.y);
                }
            }
            if (isValidPoint(xTemp, yTemp - 1, maze)) // moving down
            {
                Point pathPart;
                pathPart.x = xTemp;
                pathPart.y = yTemp - 1;
                if (visited[pathPart.x, pathPart.y] == 0)
                {
                    xStack.Push(pathPart.x);
                    yStack.Push(pathPart.y);
                }
            }
        }
        // If end point was visited, then a path from start to end exists
        if (visited[end.x, end.y] == 1) 
        {
            return true;
        }
        return false;

        /* Recursive DFS 
        int[,] visited = new int[maze.GetLength(0), maze.GetLength(1)];
        canReachHelper(start, ref visited, maze);
        // If end point was visited, then a path from start to end exists
        if (visited[end.x, end.y] == 1) 
        {
            return true;
        }
        return false;
        */
    }  

    /* Commented out for now because recursive DFS is giving a call stack overflow error past a certain maze size
    // Helper function for DFS search in canReach
    // start is the point the function is currently on
    // visited is the 2D array determining if a point has been visited
    void canReachHelper(Point start, ref int[,] visited, int[,] maze)
    {
        // Mark point as visited
        visited[start.x, start.y] = 1;
        // Debug.Log(start.x);
        // Debug.Log(start.y);

        // Recursively call for adjacent points
        if (isValidPoint(start.x + 1, start.y, maze)) // moving right
        {
            Point pathPart;
            pathPart.x = start.x + 1;
            pathPart.y = start.y;
            if (visited[pathPart.x, pathPart.y] == 0)
            {
                canReachHelper(pathPart, ref visited, maze);
            }
        }
        if (isValidPoint(start.x - 1, start.y, maze)) // moving left
        {
            Point pathPart;
            pathPart.x = start.x - 1;
            pathPart.y = start.y;
            if (visited[pathPart.x, pathPart.y] == 0)
            {
                canReachHelper(pathPart, ref visited, maze);
            }
        }
        if (isValidPoint(start.x, start.y + 1, maze)) // moving up
        {
            Point pathPart;
            pathPart.x = start.x;
            pathPart.y = start.y + 1;
            if (visited[pathPart.x, pathPart.y] == 0)
            {
                canReachHelper(pathPart, ref visited, maze);
            }
        }
        if (isValidPoint(start.x, start.y - 1, maze)) // moving down
        {
            Point pathPart;
            pathPart.x = start.x;
            pathPart.y = start.y - 1;
            if (visited[pathPart.x, pathPart.y] == 0)
            {
                canReachHelper(pathPart, ref visited, maze);
            }
        }
    }
    */

    // Determines if a point is in the maze and not a wall
    // x & y represent the point's coordinates
    // maze is the current maze
    bool isValidPoint(int x, int y, int[,] maze)
    {
        if (x < maze.GetLength(0) && y < maze.GetLength(1) && x > -1 && y > -1 && maze[x, y] != 4) // 4 represents walls
        {
            return true;
        }
        return false;
    }

    // Returns a maze with shuffled points to assist in randomization when placing walls
    Point[,] shuffle(int[,] maze)
    {
        // Fill shuffled
        Point[,] shuffled = new Point[maze.GetLength(0), maze.GetLength(1)];
        for (int i = 0; i < shuffled.GetLength(0); ++i)
        {
            for (int j = 0; j < shuffled.GetLength(1); ++j)
            {
                Point temp;
                temp.x = i;
                temp.y = j;
                shuffled[i, j] = temp;
            }
        }
        // Shuffle Shuffled
        for (int i = 0; i < shuffled.GetLength(0); ++i)
        {
            for (int j = 0; j < shuffled.GetLength(1); ++j)
            {
                Point temp = shuffled[i, j];
                int randX = rand.Next(0, shuffled.GetLength(0));
                int randY = rand.Next(0, shuffled.GetLength(1));
                shuffled[i, j] = shuffled[randX, randY];
                shuffled[randX, randY] = temp;
            }
        }
        return shuffled;
    }
}
