using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    public static int size = 0; // Size of the maze
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    void Awake () {
        // Keeps the maze size data between scenes
        DontDestroyOnLoad(gameObject);
    }
}
