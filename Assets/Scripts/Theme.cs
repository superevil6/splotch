using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "theme", menuName = "ScriptableObjects/Themes", order = 1)]
public class Theme : ScriptableObject
{
    public string ThemeName;
    public Sprite Background;
    public Sprite BallSprite;
    public Sprite CursorSprite;
}
