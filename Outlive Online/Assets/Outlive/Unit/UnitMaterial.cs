using System;
using System.Collections;
using System.Collections.Generic;
using Outlive.Manager.Generic;
using UnityEngine;

namespace Outlive.Unit
{
    [AddComponentMenu("Outlive/Unit/Material")]
    public class UnitMaterial : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer _skin;
        [SerializeField] private int _materialIndex;
        [SerializeField] private Color _undefinedColor = Color.white;

        public void OnColorChange(Color color)
        {
            if (_materialIndex < 0)
                return;

            MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
            _skin.GetPropertyBlock(propertyBlock, _materialIndex);
            propertyBlock.SetColor("_Color", color);

            _skin.SetPropertyBlock(propertyBlock, _materialIndex);
        }

        public void OnPlayerLoad(IPlayer player) => OnColorChange(player == null? _undefinedColor : player.color);
    }
}

