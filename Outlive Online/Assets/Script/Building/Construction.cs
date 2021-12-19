using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Construction : MonoBehaviour
{

    private int querConstruir;
    private List<GameObject> construindo;
    private Vector2Int[] posicoesPossiveis;
    public Vector2Int[] posicoesDisponiveis;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddQuerConstruir()
    {
        querConstruir ++;
    }
    public void RemoveQuerConstruir()
    {
        querConstruir --;
        if(querConstruir == 0)
            Destroy(gameObject);
    }

    public int QuerConstruir {
        get
        {
            return querConstruir;
        }
    }

    // public void 
}
