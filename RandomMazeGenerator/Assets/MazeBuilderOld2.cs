using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeBuilderOld2 : MonoBehaviour
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
        int[,] test = new int[4, 4] { {0, 0, 0, 0}, {4, 0, 4, 0}, {1, 1, 4, 0}, {1, 0, 0, 4} };
        Point begin;
        begin.x = 3;
        begin.y = 0;
        Point ending;
        ending.x = 0;
        ending.y = 3;
        // Debug.Log(isCube(test, begin, ending));
         
        Path test2 = canReach(test, begin, ending);
        // Debug.Log(test2.reachable);
        Debug.Log(test2.path.Length);
         
        for (int i = 0; i < test2.path.Length; ++i)
        {
            Debug.Log(test2.path[i].x + " " + test2.path[i].y);
        }
        */
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
        int[,] testMaze = designMaze(100, 100, 1);
        createMaze(testMaze);   
    }

    // Struct to make coordinate data easier to manage
    public struct Point
    {
        public int x; // x-axis
        public int y; // y-axis
    }

    // Struct to make canReach() method return data more informative
    /* 
    public struct Path
    {
        public bool reachable; // can the 2 points reach each other
        public Point[] path; // the path from point a to b (if one exists)
    }
    */
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
        List<Point> startToEnd = pathMaker(maze, start, end);
        List<Point>[] startToDeadEnds = new List<Point>[numDeadEnds];
        for (int i = 0; i < numDeadEnds; ++i)
        {
            startToDeadEnds[i] = pathMaker(maze, start, deadEndsLocations[i]);
        }
        int[,] definedPaths = new int[maze.GetLength(0), maze.GetLength(1)];
        for (int i = 0; i < startToEnd.Count; ++i)
        {
            definedPaths[startToEnd[i].x, startToEnd[i].y] = 1;
        }
        for (int i = 0; i < startToDeadEnds.Length; ++i)
        {
            for (int j = 0; j < startToDeadEnds[i].Count; ++j)
            {
                definedPaths[startToDeadEnds[i][j].x, startToDeadEnds[i][j].y] = 1;
            }
        }
        for (int i = 0; i < x; ++i)
        {
            for (int j = 0; j < y; ++j)
            {
                if (definedPaths[i, j] == 0) // Point that hasn't been set yet
                {
                    maze[i, j] = 4; // Try to make it a wall
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
                    // Sets red cube as start point
                    float xPosition = positX + (j * blockX);
                    float zPosition = positZ + (i * blockZ);
                    GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    wall.transform.localScale = new Vector3(1, 1, 1);
                    wall.transform.position = new Vector3(xPosition, positY, zPosition);
                    wall.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                    // GameObject playableCharacter = GameObject.Find("FirstPersonCharacter");
                    // playableCharacter.transform.position = new Vector3(xPosition, positY, zPosition);
                }
                if (maze[i, j] == 2) // Point is the end
                {
                    // Sets blue cube as end point
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
            deadEnds = 10; // 5, 10 for 50x50      5 run speed, 10 jump speed
        }
        else
        {
            deadEnds = 30;
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
    List<Point> pathMaker(int[,] maze, Point start, Point end)
    {   
        int[,] visited = new int[maze.GetLength(0), maze.GetLength(1)];
        const int dist = 5; // Strictly vertical or horizontal distance between 2 points 
        Point curr;
        curr.x = start.x;
        curr.y = start.y;
        visited[curr.x, curr.y] = 1; 
        List<Point> builtPath = new List<Point>();
        builtPath.Add(curr);
        while (visited[end.x, end.y] != 1)
        {
            if (curr.x > end.x && curr.y < end.y) // end point is right and up from start
            {
                int choice = rand.Next(0, 2);
                if (choice == 0 && isValidPoint(curr.x, curr.y + 1, maze) && visited[curr.x, curr.y + 1] != 1) // go right
                {
                    curr.y = curr.y + 1;
                    visited[curr.x, curr.y] = 1;
                    builtPath.Add(curr);
                }
                else if (choice == 1 && isValidPoint(curr.x - 1, curr.y, maze) && visited[curr.x - 1, curr.y] != 1) // go up
                {
                    curr.x = curr.x - 1;
                    visited[curr.x, curr.y] = 1;
                    builtPath.Add(curr);
                }                    
            }
            else if (curr.x < end.x && curr.y < end.y) // end point is right and down from start
            {
                int choice = rand.Next(0, 2);
                if (choice == 0 && isValidPoint(curr.x, curr.y + 1, maze) && visited[curr.x, curr.y + 1] != 1) // go right
                {
                    curr.y = curr.y + 1;
                    visited[curr.x, curr.y] = 1;
                    builtPath.Add(curr);
                }
                else if (choice == 1 && isValidPoint(curr.x + 1, curr.y, maze) && visited[curr.x + 1, curr.y] != 1) // go down
                {
                    curr.x = curr.x + 1;
                    visited[curr.x, curr.y] = 1;
                    builtPath.Add(curr);
                }                 
            }
            else if (curr.x > end.x && curr.y > end.y) // end point is left and up from start
            {
                int choice = rand.Next(0, 2);
                if (choice == 0 && isValidPoint(curr.x, curr.y - 1, maze) && visited[curr.x, curr.y - 1] != 1) // go left
                {
                    curr.y = curr.y - 1;
                    visited[curr.x, curr.y] = 1;
                    builtPath.Add(curr);
                }
                else if (choice == 1 && isValidPoint(curr.x - 1, curr.y, maze) && visited[curr.x - 1, curr.y] != 1) // go up
                {
                    curr.x = curr.x - 1;
                    visited[curr.x, curr.y] = 1;
                    builtPath.Add(curr);
                }              
            }
            else if (curr.x < end.x && curr.y > end.y) // end point is left and down from start
            {
                int choice = rand.Next(0, 2);
                if (choice == 0 && isValidPoint(curr.x, curr.y - 1, maze) && visited[curr.x, curr.y - 1] != 1) // go left
                {
                    curr.y = curr.y - 1;
                    visited[curr.x, curr.y] = 1;
                    builtPath.Add(curr);
                }
                else if (choice == 1 && isValidPoint(curr.x + 1, curr.y, maze) && visited[curr.x + 1, curr.y] != 1) // go down
                {
                    curr.x = curr.x + 1;
                    visited[curr.x, curr.y] = 1;
                    builtPath.Add(curr);
                }              
            }
            else if (curr.y < end.y) // end is right from start
            {
                if (end.y - curr.y < dist) // just go right
                {
                    if (isValidPoint(curr.x, curr.y + 1, maze) && visited[curr.x, curr.y + 1] != 1)
                    {
                        curr.y = curr.y + 1;
                        visited[curr.x, curr.y] = 1;
                        builtPath.Add(curr);
                    }
                }
                else
                {
                    int choice = rand.Next(0, 3);
                    if (choice == 0 && isValidPoint(curr.x, curr.y + 1, maze) && visited[curr.x, curr.y + 1] != 1) // go right
                    {
                        curr.y = curr.y + 1;
                        visited[curr.x, curr.y] = 1;
                        builtPath.Add(curr);
                    }
                    else if (choice == 1 && isValidPoint(curr.x - 1, curr.y, maze) && visited[curr.x - 1, curr.y] != 1) // go up
                    {
                        curr.x = curr.x - 1;
                        visited[curr.x, curr.y] = 1;
                        builtPath.Add(curr);
                    }        
                    else if (choice == 2 && isValidPoint(curr.x + 1, curr.y, maze) && visited[curr.x + 1, curr.y] != 1) // go down
                    {
                        curr.x = curr.x + 1;
                        visited[curr.x, curr.y] = 1;
                        builtPath.Add(curr);
                    }           
                }
            }
            else if (curr.y > end.y) // end is left from start
            {
                if (curr.y - end.y < dist) // just go left
                {
                    if (isValidPoint(curr.x, curr.y - 1, maze) && visited[curr.x, curr.y - 1] != 1)
                    {
                        curr.y = curr.y - 1;
                        visited[curr.x, curr.y] = 1;
                        builtPath.Add(curr);
                    }
                }
                else
                {
                    int choice = rand.Next(0, 3);
                    if (choice == 0 && isValidPoint(curr.x, curr.y - 1, maze) && visited[curr.x, curr.y - 1] != 1) // go left
                    {
                        curr.y = curr.y - 1;
                        visited[curr.x, curr.y] = 1;
                        builtPath.Add(curr);
                    }
                    else if (choice == 1 && isValidPoint(curr.x - 1, curr.y, maze) && visited[curr.x - 1, curr.y] != 1) // go up
                    {
                        curr.x = curr.x - 1;
                        visited[curr.x, curr.y] = 1;
                        builtPath.Add(curr);
                    }        
                    else if (choice == 2 && isValidPoint(curr.x + 1, curr.y, maze) && visited[curr.x + 1, curr.y] != 1) // go down
                    {
                        curr.x = curr.x + 1;
                        visited[curr.x, curr.y] = 1;
                        builtPath.Add(curr);
                    }           
                }
            }
            else if (curr.x > end.x) // end is up from start
            {
                if (curr.x - end.x < dist) // just go up
                {
                    if (isValidPoint(curr.x - 1, curr.y, maze) && visited[curr.x - 1, curr.y] != 1)
                    {
                        curr.x = curr.x - 1;
                        visited[curr.x, curr.y] = 1;
                        builtPath.Add(curr);
                    }
                }
                else
                {
                    int choice = rand.Next(0, 3);
                    if (choice == 0 && isValidPoint(curr.x, curr.y - 1, maze) && visited[curr.x, curr.y - 1] != 1) // go left
                    {
                        curr.y = curr.y - 1;
                        visited[curr.x, curr.y] = 1;
                        builtPath.Add(curr);
                    }
                    else if (choice == 1 && isValidPoint(curr.x - 1, curr.y, maze) && visited[curr.x - 1, curr.y] != 1) // go up
                    {
                        curr.x = curr.x - 1;
                        visited[curr.x, curr.y] = 1;
                        builtPath.Add(curr);
                    }        
                    if (choice == 2 && isValidPoint(curr.x, curr.y + 1, maze) && visited[curr.x, curr.y + 1] != 1) // go right
                    {
                        curr.y = curr.y + 1;
                        visited[curr.x, curr.y] = 1;
                        builtPath.Add(curr);
                    }      
                }
            }
            else if (curr.x < end.x) // end is down from start
            {
                if (end.x - curr.x < dist) // just go down
                {
                    if (isValidPoint(curr.x + 1, curr.y, maze) && visited[curr.x + 1, curr.y] != 1)
                    {
                        curr.x = curr.x + 1;
                        visited[curr.x, curr.y] = 1;
                        builtPath.Add(curr);
                    }
                }
                else
                {
                    int choice = rand.Next(0, 3);
                    if (choice == 0 && isValidPoint(curr.x, curr.y - 1, maze) && visited[curr.x, curr.y - 1] != 1) // go left
                    {
                        curr.y = curr.y - 1;
                        visited[curr.x, curr.y] = 1;
                        builtPath.Add(curr);
                    }
                    else if (choice == 1 && isValidPoint(curr.x + 1, curr.y, maze) && visited[curr.x + 1, curr.y] != 1) // go up
                    {
                        curr.x = curr.x + 1;
                        visited[curr.x, curr.y] = 1;
                        builtPath.Add(curr);
                    }        
                    if (choice == 2 && isValidPoint(curr.x, curr.y + 1, maze) && visited[curr.x, curr.y + 1] != 1) // go right
                    {
                        curr.y = curr.y + 1;
                        visited[curr.x, curr.y] = 1;
                        builtPath.Add(curr);
                    }      
                }
            }
        }
        return builtPath;

        /* 
        // Iterative DFS
        Stack xStack = new Stack();
        Stack yStack = new Stack();
        // Stacks representing the points in the path from start to end
        Stack myPathX = new Stack();
        Stack myPathY = new Stack();
        xStack.Push(start.x);
        yStack.Push(start.y);
        myPathX.Push(start.x);
        myPathY.Push(start.y);
        try {
            while (visited[end.x, end.y] != 1)
            {
                // Work on point from top of stack
                int popPath = 0; // If this is 0 after the 4 isValidPoint calls, then pop from myPath stacks
                int xTemp = (int)xStack.Pop();
                int yTemp = (int)yStack.Pop();
                if (visited[xTemp, yTemp] == 0)
                {
                    visited[xTemp, yTemp] = 1;
                }
                Point curr, endU, endD, endR, endL; // end up, end down, etc...
                    curr.x = xTemp;
                    curr.y = yTemp;
                    endU.x = xTemp - 1;
                    endU.y = yTemp;
                    endD.x = xTemp + 1;
                    endD.y = yTemp;
                    endR.x = xTemp;
                    endR.y = yTemp + 1;
                    endL.x = xTemp;
                    endL.y = yTemp - 1;
                int randPath = rand.Next(0, 4); // Helps to randomize the path from start to end
                // Push adjacent points onto stack
                if (randPath == 0)
                {
                    if (isValidPoint(xTemp - 1, yTemp, maze) && visited[xTemp - 1, yTemp] != 1 && !isCube(visited, curr, endU)) // moving up
                    {
                        Point pathPart;
                        pathPart.x = xTemp - 1;
                        pathPart.y = yTemp;
                        if (visited[pathPart.x, pathPart.y] == 0 && visited[end.x, end.y] != 1) 
                        {
                            visited[pathPart.x, pathPart.y] = 1;
                            popPath++;
                            xStack.Push(pathPart.x);
                            yStack.Push(pathPart.y);
                            myPathX.Push(pathPart.x);
                            myPathY.Push(pathPart.y);
                        }
                    }
                    else if (isValidPoint(xTemp, yTemp + 1, maze) && visited[xTemp, yTemp + 1] != 1 && !isCube(visited, curr, endR)) // moving right
                    {
                        Point pathPart;
                        pathPart.x = xTemp;
                        pathPart.y = yTemp + 1;
                        if (visited[pathPart.x, pathPart.y] == 0 && visited[end.x, end.y] != 1)
                        {
                            visited[pathPart.x, pathPart.y] = 1;
                            popPath++;
                            xStack.Push(pathPart.x);
                            yStack.Push(pathPart.y);
                            myPathX.Push(pathPart.x);
                            myPathY.Push(pathPart.y);
                        }
                    }
                    else if (isValidPoint(xTemp, yTemp - 1, maze) && visited[xTemp, yTemp - 1] != 1 && !isCube(visited, curr, endL)) // moving left
                    {
                        Point pathPart;
                        pathPart.x = xTemp;
                        pathPart.y = yTemp - 1;
                        if (visited[pathPart.x, pathPart.y] == 0 && visited[end.x, end.y] != 1)
                        {
                            visited[pathPart.x, pathPart.y] = 1;
                            popPath++;
                            xStack.Push(pathPart.x);
                            yStack.Push(pathPart.y);
                            myPathX.Push(pathPart.x);
                            myPathY.Push(pathPart.y);
                            
                        }
                    }
                    else if (isValidPoint(xTemp + 1, yTemp, maze) && visited[xTemp + 1, yTemp] != 1 && !isCube(visited, curr, endD)) // moving down
                    {
                        Point pathPart;
                        pathPart.x = xTemp + 1;
                        pathPart.y = yTemp;
                        if (visited[pathPart.x, pathPart.y] == 0 && visited[end.x, end.y] != 1)
                        {
                            visited[pathPart.x, pathPart.y] = 1;
                            popPath++;
                            xStack.Push(pathPart.x);
                            yStack.Push(pathPart.y);
                            myPathX.Push(pathPart.x);
                            myPathY.Push(pathPart.y);
                        }
                    }
                    // Debug.Log(xTemp + " " + yTemp + " " + popPath);
                    if (popPath == 0)
                    {
                        // visited[(int)myPathX.Pop(), (int)myPathY.Pop()] = 0;
                        myPathX.Pop();
                        myPathY.Pop();
                    }
                }
                else if (randPath == 1)
                {
                    if (isValidPoint(xTemp, yTemp + 1, maze) && visited[xTemp, yTemp + 1] != 1 && !isCube(visited, curr, endR)) // moving right
                    {
                        Point pathPart;
                        pathPart.x = xTemp;
                        pathPart.y = yTemp + 1;
                        if (visited[pathPart.x, pathPart.y] == 0 && visited[end.x, end.y] != 1)
                        {
                            visited[pathPart.x, pathPart.y] = 1;
                            popPath++;
                            xStack.Push(pathPart.x);
                            yStack.Push(pathPart.y);
                            myPathX.Push(pathPart.x);
                            myPathY.Push(pathPart.y);
                        }
                    }
                    else if (isValidPoint(xTemp, yTemp - 1, maze) && visited[xTemp, yTemp - 1] != 1 && !isCube(visited, curr, endL)) // moving left
                    {
                        Point pathPart;
                        pathPart.x = xTemp;
                        pathPart.y = yTemp - 1;
                        if (visited[pathPart.x, pathPart.y] == 0 && visited[end.x, end.y] != 1)
                        {
                            visited[pathPart.x, pathPart.y] = 1;
                            popPath++;
                            xStack.Push(pathPart.x);
                            yStack.Push(pathPart.y);
                            myPathX.Push(pathPart.x);
                            myPathY.Push(pathPart.y);
                            
                        }
                    }
                    else if (isValidPoint(xTemp + 1, yTemp, maze) && visited[xTemp + 1, yTemp] != 1 && !isCube(visited, curr, endD)) // moving down
                    {
                        Point pathPart;
                        pathPart.x = xTemp + 1;
                        pathPart.y = yTemp;
                        if (visited[pathPart.x, pathPart.y] == 0 && visited[end.x, end.y] != 1)
                        {
                            visited[pathPart.x, pathPart.y] = 1;
                            popPath++;
                            xStack.Push(pathPart.x);
                            yStack.Push(pathPart.y);
                            myPathX.Push(pathPart.x);
                            myPathY.Push(pathPart.y);
                        }
                    }
                    else if (isValidPoint(xTemp - 1, yTemp, maze) && visited[xTemp - 1, yTemp] != 1 && !isCube(visited, curr, endU)) // moving up
                    {
                        Point pathPart;
                        pathPart.x = xTemp - 1;
                        pathPart.y = yTemp;
                        if (visited[pathPart.x, pathPart.y] == 0 && visited[end.x, end.y] != 1)
                        {
                            visited[pathPart.x, pathPart.y] = 1;
                            popPath++;
                            xStack.Push(pathPart.x);
                            yStack.Push(pathPart.y);
                            myPathX.Push(pathPart.x);
                            myPathY.Push(pathPart.y);
                        }
                    }
                    // Debug.Log(xTemp + " " + yTemp + " " + popPath);
                    if (popPath == 0)
                    {
                        // visited[(int)myPathX.Pop(), (int)myPathY.Pop()] = 0;
                        myPathX.Pop();
                        myPathY.Pop();
                    }
                }
                else if (randPath == 2)
                {
                    if (isValidPoint(xTemp, yTemp - 1, maze) && visited[xTemp, yTemp - 1] != 1 && !isCube(visited, curr, endL)) // moving left
                    {
                        Point pathPart;
                        pathPart.x = xTemp;
                        pathPart.y = yTemp - 1;
                        if (visited[pathPart.x, pathPart.y] == 0 && visited[end.x, end.y] != 1)
                        {
                            visited[pathPart.x, pathPart.y] = 1;
                            popPath++;
                            xStack.Push(pathPart.x);
                            yStack.Push(pathPart.y);
                            myPathX.Push(pathPart.x);
                            myPathY.Push(pathPart.y);
                            
                        }
                    }
                    else if (isValidPoint(xTemp + 1, yTemp, maze) && visited[xTemp + 1, yTemp] != 1 && !isCube(visited, curr, endD)) // moving down
                    {
                        Point pathPart;
                        pathPart.x = xTemp + 1;
                        pathPart.y = yTemp;
                        if (visited[pathPart.x, pathPart.y] == 0 && visited[end.x, end.y] != 1)
                        {
                            visited[pathPart.x, pathPart.y] = 1;
                            popPath++;
                            xStack.Push(pathPart.x);
                            yStack.Push(pathPart.y);
                            myPathX.Push(pathPart.x);
                            myPathY.Push(pathPart.y);
                        }
                    }
                    else if (isValidPoint(xTemp - 1, yTemp, maze) && visited[xTemp - 1, yTemp] != 1 && !isCube(visited, curr, endU)) // moving up
                    {
                        Point pathPart;
                        pathPart.x = xTemp - 1;
                        pathPart.y = yTemp;
                        if (visited[pathPart.x, pathPart.y] == 0 && visited[end.x, end.y] != 1)
                        {
                            visited[pathPart.x, pathPart.y] = 1;
                            popPath++;
                            xStack.Push(pathPart.x);
                            yStack.Push(pathPart.y);
                            myPathX.Push(pathPart.x);
                            myPathY.Push(pathPart.y);
                        }
                    }
                    else if (isValidPoint(xTemp, yTemp + 1, maze) && visited[xTemp, yTemp + 1] != 1 && !isCube(visited, curr, endR)) // moving right
                    {
                        Point pathPart;
                        pathPart.x = xTemp;
                        pathPart.y = yTemp + 1;
                        if (visited[pathPart.x, pathPart.y] == 0 && visited[end.x, end.y] != 1)
                        {
                            visited[pathPart.x, pathPart.y] = 1;
                            popPath++;
                            xStack.Push(pathPart.x);
                            yStack.Push(pathPart.y);
                            myPathX.Push(pathPart.x);
                            myPathY.Push(pathPart.y);
                        }
                    }    
                    // Debug.Log(xTemp + " " + yTemp + " " + popPath);
                    if (popPath == 0)
                    {
                        // visited[(int)myPathX.Pop(), (int)myPathY.Pop()] = 0;
                        myPathX.Pop();
                        myPathY.Pop();
                    }
                }
                else if (randPath == 3)
                {
                    if (isValidPoint(xTemp + 1, yTemp, maze) && visited[xTemp + 1, yTemp] != 1 && !isCube(visited, curr, endD)) // moving down
                    {
                        Point pathPart;
                        pathPart.x = xTemp + 1;
                        pathPart.y = yTemp;
                        if (visited[pathPart.x, pathPart.y] == 0 && visited[end.x, end.y] != 1)
                        {
                            visited[pathPart.x, pathPart.y] = 1;
                            popPath++;
                            xStack.Push(pathPart.x);
                            yStack.Push(pathPart.y);
                            myPathX.Push(pathPart.x);
                            myPathY.Push(pathPart.y);
                        }
                    }
                    else if (isValidPoint(xTemp - 1, yTemp, maze) && visited[xTemp - 1, yTemp] != 1 && !isCube(visited, curr, endU)) // moving up
                    {
                        Point pathPart;
                        pathPart.x = xTemp - 1;
                        pathPart.y = yTemp;
                        if (visited[pathPart.x, pathPart.y] == 0 && visited[end.x, end.y] != 1)
                        {
                            visited[pathPart.x, pathPart.y] = 1;
                            popPath++;
                            xStack.Push(pathPart.x);
                            yStack.Push(pathPart.y);
                            myPathX.Push(pathPart.x);
                            myPathY.Push(pathPart.y);
                        }
                    }
                    else if (isValidPoint(xTemp, yTemp + 1, maze) && visited[xTemp, yTemp + 1] != 1 && !isCube(visited, curr, endR)) // moving right
                    {
                        Point pathPart;
                        pathPart.x = xTemp;
                        pathPart.y = yTemp + 1;
                        if (visited[pathPart.x, pathPart.y] == 0 && visited[end.x, end.y] != 1)
                        {
                            visited[pathPart.x, pathPart.y] = 1;
                            popPath++;
                            xStack.Push(pathPart.x);
                            yStack.Push(pathPart.y);
                            myPathX.Push(pathPart.x);
                            myPathY.Push(pathPart.y);
                        }
                    }
                    else if (isValidPoint(xTemp, yTemp - 1, maze) && visited[xTemp, yTemp - 1] != 1 && !isCube(visited, curr, endL)) // moving left
                    {
                        Point pathPart;
                        pathPart.x = xTemp;
                        pathPart.y = yTemp - 1;
                        if (visited[pathPart.x, pathPart.y] == 0 && visited[end.x, end.y] != 1)
                        {
                            visited[pathPart.x, pathPart.y] = 1;
                            popPath++;
                            xStack.Push(pathPart.x);
                            yStack.Push(pathPart.y);
                            myPathX.Push(pathPart.x);
                            myPathY.Push(pathPart.y);
                            
                        }
                    }
                    // Debug.Log(xTemp + " " + yTemp + " " + popPath);
                    if (popPath == 0)
                    {
                        // visited[(int)myPathX.Pop(), (int)myPathY.Pop()] = 0;
                        myPathX.Pop();
                        myPathY.Pop();
                    }
                }
            }
        }
        catch (InvalidOperationException e) 
        {
            // Debug.Log("Fails");
            // canReach(maze, start, end);
        }
        // Building and returning path from start to end
        Path pathExists;
        pathExists.reachable = true;
        pathExists.path = new Point[myPathX.Count];
        for (int i = 0; i < pathExists.path.Length; ++i)
        {
            pathExists.path[i].x = (int)myPathX.Pop();
            pathExists.path[i].y = (int)myPathY.Pop();
        }
        return pathExists;
        */

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

    // Determines if a point is in the maze
    // x & y represent the point's coordinates
    // maze is the current maze
    bool isValidPoint(int x, int y, int[,] maze)
    {
        if (x < maze.GetLength(0) && y < maze.GetLength(1) && x > -1 && y > -1)
        {
            return true;
        }
        return false;
    }

    /* 
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
    */

    /* 
    // Returns true if adding the end point to the path will result in a cube (4 points all touching)
    // A maze is a visited binary maze used to determine if cubes exist
    bool isCube(int[,] maze, Point start, Point end)
    {
        
        if ((isValidPoint(end.x - 1, end.y, maze) && maze[end.x - 1, end.y] == 1) &&
        (isValidPoint(end.x, end.y - 1, maze) && maze[end.x, end.y - 1] == 1) &&
        (isValidPoint(end.x - 1, end.y - 1, maze) && maze[end.x - 1, end.y - 1] == 1))
        {
            return true;
        }
        if ((isValidPoint(end.x - 1, end.y, maze) && maze[end.x - 1, end.y] == 1) &&
        (isValidPoint(end.x, end.y + 1, maze) && maze[end.x, end.y + 1] == 1) &&
        (isValidPoint(end.x - 1, end.y + 1, maze) && maze[end.x - 1, end.y + 1] == 1))
        {
            return true;
        }
        if ((isValidPoint(end.x, end.y - 1, maze) && maze[end.x, end.y - 1] == 1) &&
        (isValidPoint(end.x + 1, end.y - 1, maze) && maze[end.x + 1, end.y - 1] == 1))
        {
            return true;
        }
        if ((isValidPoint(end.x, end.y + 1, maze) && maze[end.x, end.y + 1] == 1) &&
        (isValidPoint(end.x + 1, end.y + 1, maze) && maze[end.x + 1, end.y + 1] == 1))
        {
            return true;
        }
        return false;
    }
    */
}