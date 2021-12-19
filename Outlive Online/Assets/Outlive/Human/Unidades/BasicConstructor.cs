using System.Collections;
using System.Collections.Generic;
using Outlive.Human.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicConstructor : MonoBehaviour, IConstructorHandler
{

    [SerializeField] private NavMeshAgent navigation;
    private IConstructableHandler construction;
    private Vector3 positionToConstructField;
    public void ConnectConstructable(IConstructableHandler constructable)
    {
        construction = constructable;
    }

    public void DisconectConstructable(IConstructableHandler constructable)
    {
        if (construction == constructable)
            construction = null;
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

    public bool IsBuilding(IConstructableHandler constructable)
    {
        if (constructable == construction)
        {
            return constructable.VerifyConstructor(this);
        }
        return false;
    }

    public void SetPositionToConstruct(Vector3 position, IConstructableHandler constructable)
    {
        if (constructable == construction)
        {
            if (position != positionToConstructField)
            {
                positionToConstructField = position;
                navigation.destination = position;
            }
        }
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
                        for (int i = 0; i < pathVect.Length - 1; i+=2)
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Propriedades
        public Vector3 positionToConstruct
        {
            get
            {
                return positionToConstructField;
            }
        }
    #endregion
}
