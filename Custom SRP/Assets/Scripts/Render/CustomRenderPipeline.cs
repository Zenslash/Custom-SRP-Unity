using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CustomRenderPipeline : RenderPipeline
{
    private Color _clearColor;
    private CameraRenderer _cameraRenderer = new CameraRenderer();
    public CustomRenderPipeline(Color clearColor)
    {
        _clearColor = clearColor;
    }
    
    protected override void Render(ScriptableRenderContext context, Camera[] cameras)
    {
        foreach (var cam in cameras)
        {
            _cameraRenderer.Render(context, cam);
        }
    }
}
