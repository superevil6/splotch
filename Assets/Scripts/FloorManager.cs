using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    public GameBoard GameBoard;
    public RectTransform RectTransform;
    public BoxCollider2D BoxCollider2D;
    // Start is called before the first frame update
    void Start()
    {
        GameBoard = GetComponentInParent<GameBoard>();
        RectTransform = GetComponent<RectTransform>();
        BoxCollider2D = GetComponent<BoxCollider2D>();
        print(GameBoard.GameboardWidth);
        Vector2 size = new Vector2(GameBoard.GameboardWidth, 1);
        RectTransform.sizeDelta = size;
        BoxCollider2D.size = size;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
