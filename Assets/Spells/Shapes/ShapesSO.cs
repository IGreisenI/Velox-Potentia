using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MagicShape
{
    public string shape;
    public Sprite shapeSprite;
}

[CreateAssetMenu(fileName = "Shapes", menuName = "ScriptableObjects/Shapes/Shapes")]
public class ShapesSO : ScriptableObject
{
    public List<MagicShape> shapes;
}
