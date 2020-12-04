using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LoaderBase", menuName = "CreateLoader")]
public class LoaderBase : ScriptableObject
{
    public string gameTypeName;
    public Color backColor;
    public Sprite gameLoaderSprite;
    public string gameLoaderTipText;
}
