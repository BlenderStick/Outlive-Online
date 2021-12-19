using System.Collections;
using System.Collections.Generic;
using Outlive.Human.Generic;
using Outlive.Unit.Command;
using Outlive.Unit.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Construtor : MonoBehaviour, IConstructorHandler, IGUIUnit, ISelectableUnit, ICommandableUnit
{

    private IConstructableHandler construct;
    [SerializeField] private NavMeshAgent navigation;
    [SerializeField] private Light selectLight;
    
    public string guiName 
    {
        get
        {
            return "construtor";
        }
    }

    public Player player
    {
        get
        {
            return null;
        }
    }

    public void ConnectConstructable(IConstructableHandler constructable)
    {
        if(construct != null)
            DisconectConstructable(construct);
        construct = constructable;
        constructable.ConstructorTryToBuild(this);
    }

    public void DisconectConstructable(IConstructableHandler constructable)
    {
        if(construct == constructable)
        {
            if (constructable.VerifyConstructor(this))
                constructable.ConstructorNotTryToBuild(this);
        }
        construct = null;
    }

    public float DistancePathTo(Vector3 coord)
    {
        if(navigation != null)
        {
            NavMeshPath path = new NavMeshPath();
            if (navigation.CalculatePath(coord, path))
            {
                if (path.status == NavMeshPathStatus.PathComplete)
                {
                    float distance = 0f;
                    Vector3[] pathVect = path.corners;
                    if(pathVect.Length > 0)
                    {
                        for (int i = 0; i < pathVect.Length - 1; i++)
                        {
                            distance += Vector3.Distance(pathVect[i], pathVect[i + 1]);
                        }
                    }
                    return distance;
                }
            }

        }
        return -1f;
    }
    public float SqrDistancePathTo(Vector3 coord)
    {
        if(navigation != null)
        {
            NavMeshPath path = new NavMeshPath();
            if (navigation.CalculatePath(coord, path))
            {
                if (path.status == NavMeshPathStatus.PathComplete)
                {
                    float distance = 0f;
                    Vector3[] pathVect = path.corners;
                    if(pathVect.Length > 0)
                    {
                        for (int i = 0; i < pathVect.Length; i++)
                        {
                            distance += (pathVect[i] - pathVect[i + 1]).sqrMagnitude;
                        }
                    }
                    return distance;
                }
            }
        }
        return -1f;
    }

    public void GUILostFocus()
    {}

    public void GUIReceivedFocus()
    {}

    public bool IsBuilding(IConstructableHandler constructable)
    {
        return (constructable != null && construct == constructable && constructable.VerifyConstructor(this));
    }

    public void SetPositionToConstruct(Vector3 position, IConstructableHandler constructable)
    {
        if (navigation != null && construct == constructable)
        {
            navigation.destination = position;
        }
    }

    public void UnidDeselect()
    {
        if (selectLight != null)
            selectLight.enabled = true;
    }

    public void UnitSelect()
    {
        if (selectLight != null)
            selectLight.enabled = true;
    }

    public void PutCommand(ICommand command, bool enfilerate)
    {
        throw new System.NotImplementedException();
    }

    public void PutInteract(GameObject target, bool enfilerate)
    {
        throw new System.NotImplementedException();
    }

    public void UpdateCommand()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UnitHover()
    {
        throw new System.NotImplementedException();
    }

    public void UnitNotHover()
    {
        throw new System.NotImplementedException();
    }
}
