using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseSize2 : MonoBehaviour
{
    public void LoadScene(int level)
   {
       GameObject dataCube = GameObject.Find("MazeSize");
       DontDestroy.size = 100;
       Application.LoadLevel(level);
   }
}
