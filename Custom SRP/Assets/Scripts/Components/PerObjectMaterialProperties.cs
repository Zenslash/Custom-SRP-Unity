using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PerObjectMaterialProperties : MonoBehaviour
{
    private static int BaseColorId = Shader.PropertyToID("_BaseColor");
    private static int CutoffId = Shader.PropertyToID("_Cutoff");
    private static int MetallicId = Shader.PropertyToID("_Metallic");
    private static int SmoothnessId = Shader.PropertyToID("_Smoothness");
    private static MaterialPropertyBlock _block;
    
    [SerializeField] private Color _baseColor = Color.white;
    [SerializeField, Range(0f, 1f)] private float _cutoff = 0.5f;
    [SerializeField, Range(0f, 1f)] private float _metallic = 0.5f;
    [SerializeField, Range(0f, 1f)] private float _smoothness = 0.5f;

    private void Awake()
    {
        OnValidate();
    }

    private void OnValidate()
    {
        if (_block == null)
        {
            _block = new MaterialPropertyBlock();
        }
        
        _block.SetColor(BaseColorId, _baseColor);
        _block.SetFloat(CutoffId, _cutoff);
        _block.SetFloat(MetallicId, _metallic);
        _block.SetFloat(SmoothnessId, _smoothness);
        GetComponent<Renderer>().SetPropertyBlock(_block);
    }
}
