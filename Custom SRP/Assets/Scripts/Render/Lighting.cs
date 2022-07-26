using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Lighting
{
    private const string _bufferName = "Lighting";
    private CommandBuffer _buffer = new CommandBuffer()
    {
        name = _bufferName
    };

    private static int _dirLightColorId = Shader.PropertyToID("_DirectionalLightColor"),
                        _dirLightDirId = Shader.PropertyToID("_DirectionalLightDir"); 

    public void Setup(ScriptableRenderContext context)
    {
        _buffer.BeginSample(_bufferName);
        SetupDirectionalLight();
        _buffer.EndSample(_bufferName);
        context.ExecuteCommandBuffer(_buffer);
        _buffer.Clear();
    }

    private void SetupDirectionalLight()
    {
        Light light = RenderSettings.sun;
        _buffer.SetGlobalVector(_dirLightColorId, light.color.linear * light.intensity);
        _buffer.SetGlobalVector(_dirLightDirId, -light.transform.forward);
    }
}
