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
    [SerializeField] string[] ground;
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
    public Sprite GetGroundSprite(int index){
        return atlas.GetSprite(ground[index]);
    }
    public Sprite GetWallSprite(int index){
        return atlas.GetSprite(walls[index]);
    }
}
