using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraRenderer
{
    private ScriptableRenderContext _context;
    private Camera _camera;

    private const string _bufferName = "Render Camera";
    private CommandBuffer _commandBuffer = new CommandBuffer
    {
        name = _bufferName
    };

    private CullingResults _cullingResults;

    private static ShaderTagId _unlitShaderTag = new ShaderTagId("SRPDefaultUnlit");
    private static ShaderTagId _deferredShaderTag = new ShaderTagId("Deferred");
    private static ShaderTagId _forwardBaseShaderTag = new ShaderTagId("ForwardBase");

    public void Render(ScriptableRenderContext context, Camera camera)
    {
        _context = context;
        _camera = camera;

        if (!Cull())
        {
            return;
        }

        Setup();
        DrawVisibleGeometry();
        Submit();
    }

    private void Setup()
    {
        _context.SetupCameraProperties(_camera);
        _commandBuffer.BeginSample(_bufferName);
        _commandBuffer.ClearRenderTarget(true, true, Color.clear);
        ExecuteBuffer();
    }

    private void DrawVisibleGeometry()
    {
        //Draw opaque geometry
        var sortingSettings = new SortingSettings(_camera)
        {
            criteria = SortingCriteria.CommonOpaque
        };
        var drawingSettings = new DrawingSettings(_unlitShaderTag, sortingSettings);
        var filteringSettings = new FilteringSettings(RenderQueueRange.opaque);
        
        _context.DrawRenderers(_cullingResults, ref drawingSettings, ref filteringSettings);
        
        //Draw skybox
        _context.DrawSkybox(_camera);
        
        //Draw transparent geometry
        sortingSettings.criteria = SortingCriteria.CommonTransparent;
        drawingSettings.sortingSettings = sortingSettings;
        filteringSettings.renderQueueRange = RenderQueueRange.transparent;
        
        _context.DrawRenderers(_cullingResults, ref drawingSettings, ref filteringSettings);
    }

    private bool Cull()
    {
        if (_camera.TryGetCullingParameters(out ScriptableCullingParameters parameters))
        {
            _cullingResults = _context.Cull(ref parameters);
            return true;
        }

        return false;
    }

    private void Submit()
    {
        _commandBuffer.EndSample(_bufferName);
        ExecuteBuffer();
        _context.Submit();
    }
    
    private void ExecuteBuffer()
    {
        _context.ExecuteCommandBuffer(_commandBuffer);
        _commandBuffer.Clear();
    }

}
