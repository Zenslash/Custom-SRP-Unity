using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Lighting
{
    private const string _bufferName = "Lighting";
    private CommandBuffer _buffer = new CommandBuffer()
    {
        name = _bufferName
    };

    private CullingResults _cullingResults;
    private const int _maxDirLightCount = 4;

    private static int _dirLightColorsId = Shader.PropertyToID("_DirectionalLightColors"),
                        _dirLightDirsId = Shader.PropertyToID("_DirectionalLightDirs"),
                        _dirLightCountId = Shader.PropertyToID("_DirectionalLightCount");

    private static Vector4[] _dirLightColors = new Vector4[_maxDirLightCount],
        _dirLightDirs = new Vector4[_maxDirLightCount];

    public void Setup(ScriptableRenderContext context, CullingResults cullingResults)
    {
        _cullingResults = cullingResults;
        
        _buffer.BeginSample(_bufferName);
        SetupLights();
        _buffer.EndSample(_bufferName);
        context.ExecuteCommandBuffer(_buffer);
        _buffer.Clear();
    }

    private void SetupLights()
    {
        int dirLightCount = 0;
        NativeArray<VisibleLight> visibleLights = _cullingResults.visibleLights;
        for (int i = 0; i < visibleLights.Length; i++)
        {
            VisibleLight light = visibleLights[i];
            if (light.lightType == LightType.Directional)
            {
                SetupDirectionalLight(dirLightCount++, ref light);
                if (dirLightCount >= _maxDirLightCount)
                {
                    break;
                }
            }
        }
        
        _buffer.SetGlobalInt(_dirLightCountId, visibleLights.Length);
        _buffer.SetGlobalVectorArray(_dirLightColorsId, _dirLightColors);
        _buffer.SetGlobalVectorArray(_dirLightDirsId, _dirLightDirs);
    }

    private void SetupDirectionalLight(int index, ref VisibleLight visibleLight)
    {
        _dirLightColors[index] = visibleLight.finalColor;
        _dirLightDirs[index] = -visibleLight.localToWorldMatrix.GetColumn(2);
    }
}
