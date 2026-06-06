using System;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace ToyEffectGraph.Editor
{
    internal interface IEvaluatableExpressionNode
    {
        string EvaluateExpression();
    }

    [Serializable]
    public abstract class BinaryOpsNode<T> : Node
    {
        public const string InputPortAName = "InputPortA";
        public const string InputPortBName = "InputPortB";
        public const string OutputPortName = "OutputPort";

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort<T>(InputPortAName).WithDisplayName("x").Build();
            context.AddInputPort<T>(InputPortBName).WithDisplayName("y").Build();
            context.AddOutputPort<T>(OutputPortName).WithDisplayName("").Build();
        }
    }

    [Serializable]
    public class AddFloat : BinaryOpsNode<float>
    {
    }

    [Serializable]
    public class MultiplyFloat : BinaryOpsNode<float>
    {
    }

    [Serializable]
    public abstract class SingleArgFunctionsNode<I, O> : Node
    {
        public const string InputPortName = "InputPort";
        public const string OutputPortName = "OutputPort";

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort<I>(InputPortName).WithDisplayName("x").Build();
            context.AddOutputPort<O>(OutputPortName).WithDisplayName("").Build();
        }
    }

    [Serializable]
    public class Sin : SingleArgFunctionsNode<float, float>
    {
    }

    [Serializable]
    public class RandomFloat : SingleArgFunctionsNode<Vector2, float>
    {
    }

    [Serializable]
    public class Length : SingleArgFunctionsNode<Vector3, float>
    {
    }

    [Serializable]
    public abstract class GetSinglePropertyNode<T> : Node
    {
        public const string OutputPortName = "OutputPort";

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddOutputPort<T>(OutputPortName).WithDisplayName("value").Build();
        }
    }

    [Serializable]
    public class GetTime : GetSinglePropertyNode<float>
    {
    }

    [Serializable]
    public class GetPosition : GetSinglePropertyNode<Vector3>
    {
    }

    [Serializable]
    public abstract class ConstNode<T> : Node
    {
        public const string OutputPortName = "OutputPort";
        public const string ConstPropertyName = "ConstProperty";

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddOutputPort<T>(OutputPortName).WithDisplayName("value").Build();
        }

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption<T>(ConstPropertyName).WithDisplayName("").Build();
        }
    }

    [Serializable]
    public class ConstFloat : ConstNode<float>
    {
    }

    [Serializable]
    public class ConstVector3 : ConstNode<Vector3>
    {
    }

    [Serializable]
    public abstract class VectorConverterNode : Node
    {
    }

    [Serializable]
    public class ComposeVector : VectorConverterNode
    {
        public const string InputPortAName = "InputPortA";
        public const string InputPortBName = "InputPortB";
        public const string InputPortCName = "InputPortC";
        public const string OutputPortName = "OutputPort";

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort<float>(InputPortAName).WithDisplayName("x").Build();
            context.AddInputPort<float>(InputPortBName).WithDisplayName("y").Build();
            context.AddInputPort<float>(InputPortCName).WithDisplayName("z").Build();
            context.AddOutputPort<Vector3>(OutputPortName).WithDisplayName("").Build();
        }
    }

    [Serializable]
    public class DecomposeVector : VectorConverterNode
    {
        public const string InputPortName = "InputPort";
        public const string OutputPortAName = "OutputPortA";
        public const string OutputPortBName = "OutputPortB";
        public const string OutputPortCName = "OutputPortC";

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort<Vector3>(InputPortName).WithDisplayName("vector").Build();
            context.AddOutputPort<Vector3>(OutputPortAName).WithDisplayName("x").Build();
            context.AddOutputPort<Vector3>(OutputPortBName).WithDisplayName("y").Build();
            context.AddOutputPort<Vector3>(OutputPortCName).WithDisplayName("z").Build();
        }
    }
}