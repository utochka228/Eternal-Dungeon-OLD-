using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "NewMapStyle", menuName = "CreateMapStyle/NewMapStyle")]
public class GameMapStyle : ScriptableObject
{
    [Header("Sprite atlas for this level style")]
    [SerializeField] SpriteAtlas atlas;
    [Header("Level sprite name of ground in atlas")]
    [Header("Sprite must be named like: \"BlockName0\" or \"BlockName_0\"")]
    [SerializeField] GroundLayer[] ground;
    public GameObject groundPrefab;
    [Header("Level sprite name of walls in atlas")]
    [SerializeField] string[] walls;
    public GameObject wallPrefab;
    [Header("Possible blocks on this level")]
    public BlockBase[] blocks;
    public GameObject blockHolder;
    [Header("Possible props on this level")]
    [SerializeField] public Prop[] props;
    //Musics..
    [Header("Game-Ready prefabs (Buldings..)")]
    public GameObject[] gmReadyPrefabs;
    [Header("Possible bosses on this level")]
    public GameObject[] bossGates;
    [Header("Possible level mobs")] public GameObject[] mobs;
    public VolumeProfile levelPostProccess;
    public int GetGroundSize(){
        return ground.Length;
    }
    public Sprite GetGroundSprite(int groundLayer, int index){
        Sprite sprite;
        string layerName = ground[groundLayer].layerName;
        layerName += index;
        sprite = atlas.GetSprite(layerName);
        if(sprite == null)
            layerName = ground[groundLayer].layerName + "_" + index;
        else
            return sprite;
        sprite = atlas.GetSprite(layerName);
        return sprite;
    }
    public Sprite GetWallSprite(int index){
        return atlas.GetSprite(walls[index]);
    }
}
[System.Serializable]
struct GroundLayer{
    public string layerName;
}
