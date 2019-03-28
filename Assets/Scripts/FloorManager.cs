using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    public GameBoard GameBoard;
    public RectTransform RectTransform;
    // Start is called before the first frame update
    void Start()
    {
        GameBoard = GetComponentInParent<GameBoard>();
        RectTransform = GetComponent<RectTransform>();
        RectTransform.sizeDelta = new Vector2(GameBoard.GameboardWidth, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
