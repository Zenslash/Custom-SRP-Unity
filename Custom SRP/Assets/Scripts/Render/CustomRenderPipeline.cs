using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CustomRenderPipeline : RenderPipeline
{
    private CameraRenderer _cameraRenderer = new CameraRenderer();
    public CustomRenderPipeline()
    {
        //Enable SRP Batching
        GraphicsSettings.useScriptableRenderPipelineBatching = true;
    }
    
    protected override void Render(ScriptableRenderContext context, Camera[] cameras)
    {
        foreach (var cam in cameras)
        {
            _cameraRenderer.Render(context, cam);
        }
    }
}
