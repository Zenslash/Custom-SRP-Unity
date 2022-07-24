using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public partial class CameraRenderer
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

    public void Render(ScriptableRenderContext context, Camera camera,
        bool useDynamicBatching, bool useGPUInstancing)
    {
        _context = context;
        _camera = camera;

        PrepareBuffer();
        PrepareForSceneWindow();
        if (!Cull())
        {
            return;
        }

        Setup();
        DrawVisibleGeometry(useDynamicBatching, useGPUInstancing);
        DrawLegacyShaders();
        DrawGizmos();
        Submit();
    }

    private void Setup()
    {
        _context.SetupCameraProperties(_camera);
        _commandBuffer.BeginSample(SampleName);
        CameraClearFlags clearFlags = _camera.clearFlags;
        _commandBuffer.ClearRenderTarget(
            clearFlags <= CameraClearFlags.Depth, clearFlags == CameraClearFlags.Color,
            clearFlags == CameraClearFlags.Color ? _camera.backgroundColor.linear : Color.clear);
        ExecuteBuffer();
    }

    private partial void PrepareBuffer();
    private partial void PrepareForSceneWindow();
    private partial void DrawLegacyShaders();
    private partial void DrawGizmos();
    private void DrawVisibleGeometry(bool useDynamicBatching, bool useGPUInstancing)
    {
        //Draw opaque geometry
        var sortingSettings = new SortingSettings(_camera)
        {
            criteria = SortingCriteria.CommonOpaque
        };
        var drawingSettings = new DrawingSettings(_unlitShaderTag, sortingSettings)
        {
            enableDynamicBatching = useDynamicBatching,
            enableInstancing = useGPUInstancing
        };
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
        _commandBuffer.EndSample(SampleName);
        ExecuteBuffer();
        _context.Submit();
    }
    
    private void ExecuteBuffer()
    {
        _context.ExecuteCommandBuffer(_commandBuffer);
        _commandBuffer.Clear();
    }

}
