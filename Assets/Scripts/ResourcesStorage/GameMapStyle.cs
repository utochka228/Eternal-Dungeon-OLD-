using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "NewMapStyle", menuName = "CreateMapStyle/NewMapStyle")]
public class GameMapStyle : ScriptableObject
{
    [SerializeField] SpriteAtlas atlas;
    public GameObject groundPrefab;
    [SerializeField] string[] ground;
    public GameObject wallPrefab;
    [SerializeField] string[] walls;
    public GameObject blockHolder;
    public BlockBase[] blocks;
    public GameObject propHolder;
    [SerializeField] Prop[] props;

    public VolumeProfile levelPostProccess;

    public Sprite GetGroundSprite(int index){
        return atlas.GetSprite(ground[index]);
    }
    public Sprite GetWallSprite(int index){
        return atlas.GetSprite(walls[index]);
    }
}
