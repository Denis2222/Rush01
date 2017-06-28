using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour {

    private Vector2 mouse;
    private int w = 32;
    private int h = 32;
    public Texture2D cursor;
     
    void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        mouse = new Vector2(Input.mousePosition.x+10, Screen.height - Input.mousePosition.y +10);
    }

    void OnGUI()
    {
        GUI.DrawTexture(new Rect(mouse.x - (w / 2), mouse.y - (h / 2), w, h), cursor);
    }
}
