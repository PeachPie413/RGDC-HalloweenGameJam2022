using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tile Type")]
public class TileType : ScriptableObject
{
    public Sprite sprite;
    public bool isPassable;
}
