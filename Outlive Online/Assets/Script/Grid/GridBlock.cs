using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBlock : MonoBehaviour
{

    public Vector3Int[] blocks;
    // Collider objCollider;
    // public GameObject reference;
    // Start is called before the first frame update
    void Start()
    {
        // objCollider = gameObject.GetComponent<Collider>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public Vector3Int[] getBlocks(){
        Vector3Int[] b = new Vector3Int[blocks.Length];
        Vector3 position = new Vector3(
            x: transform.position.x + 0.5f,
            y: transform.position.y,
            z: transform.position.z + 0.5f);
            
        Vector3Int pos = Vector3Int.FloorToInt(position);
        int count = 0;
        foreach (Vector3Int v in blocks)
        {
            b[count] = v + pos;
            count++;
        }
        return b;
    }

    #region Metodos de verificação de pontos
    public  bool IsInCollider(Collider other, Vector3 point) 
    {
        Vector3 from = (Vector3.up * 5000f);
        Vector3 dir = (point - from).normalized;
        float dist = Vector3.Distance(from, point);
        //fwd      
        int hit_count = Cast_Till(from, point, other);
        //back
        dir = (from - point).normalized;
        hit_count += Cast_Till(point, point + (dir * dist), other);

        if (hit_count % 2 == 1) 
        {
            return (true);
        }
        return (false);
    }

    int Cast_Till(Vector3 from, Vector3 to, Collider other) 
    {
        int counter = 0;
        Vector3 dir = (to - from).normalized;
        float dist = Vector3.Distance(from, to);
        bool Break = false;
        while (!Break) 
        {
            Break = true;
            RaycastHit[] hit = Physics.RaycastAll(from, dir, dist);
            for (int tt = 0; tt < hit.Length; tt++) 
            {
                if (hit[tt].collider == other)
                {
                    counter++;
                    from = hit[tt].point+dir.normalized*.001f;
                    dist = Vector3.Distance(from, to);
                    Break = false;
                    break;
                }
            }
        }
        return (counter);
    }
    #endregion
    
}
