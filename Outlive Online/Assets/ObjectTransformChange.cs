using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ObjectTransformChange : MonoBehaviour
{

    public UnityEvent TransformChange;
    private Vector3 lastPosition;
    // Start is called before the first frame update
    void Start()
    {
            TransformChange.Invoke();
        // lastPosition = transform.position;
    }

    

    // Update is called once per frame
    void Update()
    {
        if(!transform.position.Equals(lastPosition))
        {
            lastPosition = transform.position;
            TransformChange.Invoke();
        }
    }
}
