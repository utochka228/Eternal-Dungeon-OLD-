using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EscapeState", menuName = "EnemyStatements/Escape")]
public class Escape : State
{
    
    int safeDistance;

    private Vector2 escapePoint;
    
    public override void Init()
    {
        //enemy.movementScript.OnPathNotSuccessful += Escaping;
        //safeDistance = Mathf.RoundToInt(enemy.distanceToDetecting * 2f);
        Escaping();
    }
    public override void Run()
    {
        
    }

    void Escaping()
    {
        Debug.Log("Escaping!");

        //Vector2[] directions = {
        //    new Vector2(0, 1),
        //    new Vector2(1, 1),
        //    new Vector2(1, 0),
        //    new Vector2(1, -1),
        //    new Vector2(0, -1),
        //    new Vector2(-1, -1),
        //    new Vector2(-1, 0),
        //    new Vector2(-1, 1),
        //};
        //for (int i = 0; i < 8; i++)
        //{
        //    directions[i] *= safeDistance;
        //    directions[i] += enemy.currentPosition;
        //    directions[i].x = Mathf.Clamp(directions[i].x, 0, GameMap.GM.yMapSize - 1);
        //    directions[i].y = Mathf.Clamp(directions[i].y, 0, GameMap.GM.yMapSize - 1);
        //    Debug.Log("Dir[" + i + "]:" + directions[i].x + "|" + directions[i].y);
        //}

        //escapePoint = directions[0];
        //for (int i = 0; i < 7; i++)
        //{
        //    Debug.Log($"Escape point before: {escapePoint.x} {escapePoint.y}");
        //    Debug.Log($"{directions[i].magnitude} < {directions[i + 1].magnitude}");
        //    if (directions[i].magnitude < directions[i + 1].magnitude)
        //        escapePoint = directions[i + 1];
        //    Debug.Log($"Escape point after: {escapePoint.x} {escapePoint.y}");
        //}
        escapePoint = Vector2.zero;

    }
}
