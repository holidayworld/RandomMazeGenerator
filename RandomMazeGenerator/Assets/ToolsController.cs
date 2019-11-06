using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ToolsController : MonoBehaviour
{
    public GameObject masterCube;
    public GameObject cube;
    public GameObject mirror;
    public Camera mirrorCamera;
    int[,] map;
    MazeBuilder.Point position;

    float size;
    bool mapAquired;

    void Start()
    {
        mapAquired = false;
        size = cube.transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (!mapAquired)
        {
            map = masterCube.GetComponent<MazeBuilder>().testMaze;
            mapAquired = true;
        }
        updatePosition();           
    }

    void updatePosition()
    {
        position.x = Mathf.FloorToInt(this.transform.position.x / size);
        position.y = Mathf.FloorToInt(this.transform.position.z / size);
        MazeBuilder.Point turn = getNearestTurn(getDirection());
        mirror.transform.position = new Vector3((turn.x + .5f) * size, this.gameObject.transform.position.y, (turn.y + .5f) * size);
    }

    MazeBuilder.Point getNearestTurn(int direction)
    {
        MazeBuilder.Point scanner = new MazeBuilder.Point(position);
        int overload = 0;
        while (map[scanner.y, scanner.x] != 4)
        {
            
            overload++;
            switch (direction) {
                case 0:
                    scanner.goRight();
                    if (map[scanner.getUp().y, scanner.getUp().x] == 0) {
                        mirror.transform.rotation = Quaternion.Euler(0, 0, 0);
                        mirrorCamera.transform.rotation = Quaternion.Euler(0, -90, 0);
                        Debug.Log(mirrorCamera.transform.rotation);
                        return new MazeBuilder.Point(scanner);
                    }
                    if (map[scanner.getDown().y, scanner.getDown().x] == 0) {
                        mirror.transform.rotation = Quaternion.Euler(0, 0, 0);
                        mirrorCamera.transform.rotation = Quaternion.Euler(0, 90, 0);
                        return new MazeBuilder.Point(scanner);
                    }
                    break;
                case 1:
                    scanner.goDown();
                    if (map[scanner.getRight().y, scanner.getRight().x] == 0)
                    {
                        mirror.transform.rotation = Quaternion.Euler(0, 270, 0);
                        mirrorCamera.transform.rotation = Quaternion.Euler(0, -90, 0);
                        return new MazeBuilder.Point(scanner);
                    }
                    if (map[scanner.getLeft().y, scanner.getLeft().x] == 0)
                    {
                        mirror.transform.rotation = Quaternion.Euler(0, 270, 0);
                        mirrorCamera.transform.rotation = Quaternion.Euler(0, 90, 0); 
                        return new MazeBuilder.Point(scanner);
                    }
                    break;
                case 2:
                    scanner.goLeft();
                    if (map[scanner.getUp().y, scanner.getUp().x] == 0)
                    {
                        mirror.transform.rotation = Quaternion.Euler(0, 180, 0);
                        mirrorCamera.transform.rotation = Quaternion.Euler(0, 90, 0); 
                        return new MazeBuilder.Point(scanner);
                    }
                    if (map[scanner.getDown().y, scanner.getDown().x] == 0)
                    {
                        mirror.transform.rotation = Quaternion.Euler(0, 180, 0);
                        mirrorCamera.transform.rotation = Quaternion.Euler(0, -90, 0); 
                        return new MazeBuilder.Point(scanner);
                    }
                    break;
                case 3:
                    scanner.goUp();
                    if (map[scanner.getRight().y, scanner.getRight().x] == 0)
                    {
                        mirror.transform.rotation = Quaternion.Euler(0, 90, 0);
                        mirrorCamera.transform.rotation = Quaternion.Euler(0, 90, 0); 
                        return new MazeBuilder.Point(scanner);
                    }
                    if (map[scanner.getLeft().y, scanner.getLeft().x] == 0)
                    {
                        mirror.transform.rotation = Quaternion.Euler(0, 90, 0);
                        mirrorCamera.transform.rotation = Quaternion.Euler(0, -90, 0); 
                        return new MazeBuilder.Point(scanner);
                    }
                    break;
                default:
                    break;
            }
            if (overload > 10000)
                return new MazeBuilder.Point(-1, -1);
        }
        return new MazeBuilder.Point(-1, -1);
    }

    int getDirection()
    {
        float rotation = this.transform.eulerAngles.y;
        rotation %= 360;
        if (rotation < 0)
        {
            rotation = 360 - rotation;
        }
        if (rotation > 315 || rotation < 45)
        {
            return 0;
        }
        if (rotation >= 45 && rotation < 135)
        {
            return 1;
        }
        if (rotation >= 135 && rotation < 225)
        {
            return 2;
        }
        if (rotation >= 225 && rotation < 315)
        {
            return 3;
        }
        return -1;
    }

}
