using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseSize3 : MonoBehaviour
{
    public void LoadScene(int level)
   {
       GameObject dataCube = GameObject.Find("MazeSize");
       DontDestroy.size = 150;
       Application.LoadLevel(level);
   }
}
