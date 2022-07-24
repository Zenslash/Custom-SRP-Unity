using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "SRP/Create Custom SRP")]
public class CustomPipelineAsset : RenderPipelineAsset
{
    [SerializeField] private bool _useDynamicBatching, _useGPUInstancing, _useSRPBatcher;
    
    protected override RenderPipeline CreatePipeline()
    {
        return new CustomRenderPipeline(_useDynamicBatching, _useGPUInstancing, _useSRPBatcher);
    }
}
