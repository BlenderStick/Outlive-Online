using System.Collections;
using System.Collections.Generic;
using Outlive.Grid;
using UnityEngine;

public class GridBlock : MonoBehaviour
{


    [SerializeField] private GridMap _gridManager;
    [SerializeField] private int _radius;
    [SerializeField] private string _gridLayer;

    private HashSet<Vector2Int> _blocks;


    public GridMap GridManager { get => _gridManager; set => _gridManager = value; }
    public HashSet<Vector2Int> Blocks => _blocks;

    private void Awake() 
    {
        _blocks = Outlive.OutliveUtilites.CalculePointsAroundGrid(Vector2Int.FloorToInt(Outlive.OutliveUtilites.From3DTo2DCoordinates(transform.position)), _radius);
    }

    // Collider objCollider;
    // public GameObject reference;
    // Start is called before the first frame update
    void Start()
    {
        if (_gridManager == null)
            _gridManager = Object.FindObjectOfType<GridMap>();

        foreach (var item in _blocks)
        {
            _gridManager.Add(item, _gridLayer);
        }
        // objCollider = gameObject.GetComponent<Collider>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy() 
    {
        foreach (var item in _blocks)
        {
            _gridManager.Remove(item, _gridLayer);
        }
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
