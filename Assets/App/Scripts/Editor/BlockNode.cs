using System;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace ToyEffectGraph.Editor
{
    [UseWithContext(typeof(Initialize), typeof(Update), typeof(Output))]
    [Serializable]
    public class SetPosition : BlockNode
    {
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort<Vector3>("position").WithDefaultValue(Vector3.zero).Build();
        }
    }
}