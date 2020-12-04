using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float timeToBoom = 3f;
    public GameObject itemEffect;
    List<IDamageble> targetsInRadius = new List<IDamageble>();

    Collider[] inSphere;

    public GameObject owner;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), transform.position.y, Mathf.RoundToInt(transform.position.z));
        inSphere = Physics.OverlapSphere(transform.position, 1.2f, 1 << 8);
        
        StartCoroutine(Explode());
    }

    void OnTriggerEnter(Collider other)
    {
        IDamageble pl = other.GetComponent<IDamageble>();
        if (pl != null)
        {
            if(!targetsInRadius.Contains(pl))
                targetsInRadius.Add(pl);
        }
    }
    void OnTriggerExit(Collider other)
    {
        IDamageble pl = other.GetComponent<IDamageble>();
        if (pl != null)
        {
            targetsInRadius.Remove(pl);
        }
    }

    IEnumerator Explode()
    {
        GetComponent<TraumaInducer>().Delay = timeToBoom;

        yield return new WaitForSeconds(timeToBoom);
        foreach (var target in targetsInRadius)
        {
            target.TakeDamage(owner, 3f);
            Debug.Log("TAKEN DAMAGE");
        }

        DestroyCellsInRadius();

        Destroy(gameObject);
    }

    void DestroyCellsInRadius()
    {
        var positions = inSphere.Select(x => x.transform.position).ToArray();
        Vector2[] positionsV2 = new Vector2[positions.Length];
        Cell[] cells = new Cell[positions.Length];
        for (int i = 0; i < positions.Length; i++)
        {
            positionsV2[i] = new Vector2(positions[i].x, positions[i].y);
            cells[i] = GameMap.GM.gameField[positionsV2[i]];
        }
        GameMap.GM.DestroyMapCells(cells);
    }
}
