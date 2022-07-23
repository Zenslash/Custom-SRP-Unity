using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


//GPU Instancing Sample
public class CubeGenerator : MonoBehaviour
{
    private static int _baseColorId = Shader.PropertyToID("_BaseColor");
    
    [SerializeField] private Mesh _mesh = default;
    [SerializeField] private Material _material = default;

    private Matrix4x4[] _matrices = new Matrix4x4[1023];
    private Vector4[] _colors = new Vector4[1023];

    private MaterialPropertyBlock _materialPropertyBlock;

    private void Awake()
    {
        for (int i = 0; i < _matrices.Length; i++)
        {
            _matrices[i] = Matrix4x4.TRS(
                Random.insideUnitSphere * 10f, Quaternion.identity, Vector3.one);
            _colors[i] = new Vector4(
                Random.value, Random.value, Random.value, 1f);
        }
    }

    private void Update()
    {
        if (_materialPropertyBlock == null)
        {
            _materialPropertyBlock = new MaterialPropertyBlock();
            _materialPropertyBlock.SetVectorArray(_baseColorId, _colors);
        }
        Graphics.DrawMeshInstanced(_mesh, 0, _material, _matrices, 1023, _materialPropertyBlock);
    }
}
