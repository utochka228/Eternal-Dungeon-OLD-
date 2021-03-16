using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [Range(0f, 1f)] public float spawnChance;
    public Vector2Int Size{get; private set;}
    public Vector2 Center{get; private set;}
    public Vector2[] entries;
    Collider2D buildCollider;

    void Awake() {
        buildCollider = GetComponent<Collider2D>();
        int xCenter = Mathf.RoundToInt(buildCollider.bounds.center.x);
        int yCenter = Mathf.RoundToInt(buildCollider.bounds.center.y);
        Center = new Vector2(xCenter, yCenter);
        int xSize = Mathf.RoundToInt(buildCollider.bounds.size.x);
        int ySize = Mathf.RoundToInt(buildCollider.bounds.size.y);
        Size = new Vector2Int(xSize, ySize);
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
