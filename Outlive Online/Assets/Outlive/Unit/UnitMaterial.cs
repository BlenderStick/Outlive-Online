using System.Collections;
using System.Collections.Generic;
using Outlive.Manager.Generic;
using UnityEngine;

namespace Outlive.Unit
{
    [AddComponentMenu("Outlive/Unit/Material")]
    public class UnitMaterial : MonoBehaviour
    {

        [SerializeField] private Material _teamMaterial;
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void SetupMaterial(UnitStarter.StarterEvent evt)
        {
            if (_teamMaterial == null)
                return;
            Color cor = evt.currentPlayer == null? Color.white: evt.currentPlayer.color;

            _teamMaterial.SetColor("_Color", cor);
        }
    }
}

