using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBehvaior : MonoBehaviour
{

    public bool isTitle;
    public bool isGame;
    // Start is called before the first frame update
    void Start()
    {
        if (isTitle)
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
        if (isGame)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
