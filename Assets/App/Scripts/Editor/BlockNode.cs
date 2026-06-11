using System;
using JetBrains.Annotations;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace ToyEffectGraph.Editor
{
    [UseWithContext(typeof(Initialize), typeof(Update), typeof(Output))]
    [Serializable]
    public class SetPosition : BlockNode, IEvaluatableExpression
    {
        [NotNull]
        public readonly string PositionPortName = "positionPort";

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort<Vector3>(PositionPortName)
                .WithDisplayName("position")
                .WithDefaultValue(Vector3.zero)
                .Build();
        }

        public string EvaluateExpression()
        {
            var connectedNode =
                GetInputPortByName(PositionPortName).FirstConnectedPort.GetNode() as IEvaluatableExpression;
            return $"_Position[i] = ( {connectedNode.EvaluateExpression()} )";
        }
    }
}