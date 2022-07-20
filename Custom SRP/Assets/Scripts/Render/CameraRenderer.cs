using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraRenderer
{
    private ScriptableRenderContext _context;
    private Camera _camera;

    public void Render(ScriptableRenderContext context, Camera camera)
    {
        _context = context;
        _camera = camera;

        Setup();
        DrawSkybox();
        Sumbit();
    }

    private void Setup()
    {
        _context.SetupCameraProperties(_camera);
    }
    private void DrawSkybox()
    {
        _context.DrawSkybox(_camera);
    }

    private void Sumbit()
    {
        _context.Submit();
    }

}
