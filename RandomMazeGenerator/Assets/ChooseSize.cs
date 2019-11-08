using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseSize : MonoBehaviour
{
   public void LoadScene(int level)
   {
        GameObject dataCube = GameObject.Find("MazeSize");
        DontDestroy.size = 50;
        Application.LoadLevel(level);
   }
}
