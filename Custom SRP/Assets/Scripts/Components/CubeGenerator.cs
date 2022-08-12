using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


//GPU Instancing Sample
public class CubeGenerator : MonoBehaviour
{
    private static int _baseColorId = Shader.PropertyToID("_BaseColor");
    private static int _metallicId = Shader.PropertyToID("_Metallic");
    private static int _smoothnessId = Shader.PropertyToID("_Smoothness");
    
    [SerializeField] private Mesh _mesh = default;
    [SerializeField] private Material _material = default;

    private Matrix4x4[] _matrices = new Matrix4x4[1023];
    private Vector4[] _colors = new Vector4[1023];
    private float[] _metallic = new float[1023];
    private float[] _smoothness = new float[1023];

    private MaterialPropertyBlock _materialPropertyBlock;

    private void Awake()
    {
        for (int i = 0; i < _matrices.Length; i++)
        {
            _matrices[i] = Matrix4x4.TRS(
                Random.insideUnitSphere * 10f, Quaternion.Euler(
                    Random.value * 360f, Random.value * 360f, Random.value * 360f
                ), Vector3.one * Random.Range(0.5f, 1.5f));
            _colors[i] = new Vector4(
                Random.value, Random.value, Random.value, Random.Range(0.5f, 1f));
            _metallic[i] = Random.Range(0.0f, 1f);
            _smoothness[i] = Random.Range(0.0f, 1.0f);
        }
    }

    private void Update()
    {
        if (_materialPropertyBlock == null)
        {
            _materialPropertyBlock = new MaterialPropertyBlock();
            _materialPropertyBlock.SetVectorArray(_baseColorId, _colors);
            _materialPropertyBlock.SetFloatArray(_metallicId, _metallic);
            _materialPropertyBlock.SetFloatArray(_smoothnessId, _smoothness);
        }
        Graphics.DrawMeshInstanced(_mesh, 0, _material, _matrices, 1023, _materialPropertyBlock);
    }
}
