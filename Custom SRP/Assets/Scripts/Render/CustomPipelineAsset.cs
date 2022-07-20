using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "SRP/Create Custom SRP")]
public class CustomPipelineAsset : RenderPipelineAsset
{
    [SerializeField] private Color _clearColor;
    
    protected override RenderPipeline CreatePipeline()
    {
        return new CustomRenderPipeline(_clearColor);
    }
}
