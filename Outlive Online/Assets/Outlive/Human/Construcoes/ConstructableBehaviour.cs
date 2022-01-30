using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Outlive.Unit;
using Outlive.Human;
using Outlive.Human.Generic;
using Outlive.Human.Construcoes;

public class ConstructableBehaviour : MonoBehaviour, IUnitConstructable
{

    [SerializeField]
    private HumanConstructable construct;

    public IConstructableHandler constructable 
    {
        get
        {
            return construct;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        HumanConstructable hConst = new HumanConstructable(gameObject);
        hConst.gridManager = FindObjectOfType<GridManager>();
        hConst.locaisParaConstruir = new Vector2[]{
            new Vector2(-1, 1),
            new Vector2(-1, -1),
            new Vector2(1, 1),
            new Vector2(1, -1)
        };
        construct = hConst;
    }

    // Update is called once per frame
    void Update()
    {
        construct.VerifyConstructors();
        // Debug.Log(construct.countConstructorsBuilding);
    }
}
