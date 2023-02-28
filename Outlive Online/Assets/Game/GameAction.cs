using System.Collections;
using System.Collections.Generic;
using Outlive.Grid;
using UnityEngine;
using Photon.Pun;
using Outlive;

public class GameAction : MonoBehaviourPunCallbacks
{
    [SerializeField] private GridMap _map;
    [SerializeField] private LayerMask _projectLayer;
    [SerializeField] private string[] _layers;
    [SerializeField] private float _high;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public bool CreatePrefab(string name, Vector2Int position, out GameObject instance)
    {
        instance = null;
        PrefabInfo info = PrefabInfoLoader.GetPrefab(name);
        //check if hashset intersect info.GetPoints(position)
        if (_map.GetLayers(_layers).Overlaps(info.GetPoints(position)))
        {
            Vector3 project = OutliveUtilites.Project(_high, _projectLayer, position);
            instance = Instantiate(info.Prefab, project, Quaternion.identity);
            this.photonView.RPC("RPC_CreatePrefab", RpcTarget.All, name, project);
            return true;
        }
        return false;
    }

    [PunRPC] private void RPC_CreatePrefab(string name, Vector3 worldPosition)
    {
        PrefabInfo info = PrefabInfoLoader.GetPrefab(name);
        Instantiate(info.Prefab, worldPosition, Quaternion.identity);
    }

    
}
