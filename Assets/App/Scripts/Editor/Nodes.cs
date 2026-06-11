using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace ToyEffectGraph.Editor
{
    [Serializable]
    public abstract class BinaryOpsNode<T> : Node, IEvaluatableExpression
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

        public abstract string EvaluateExpression();
    }

    [Serializable]
    public class AddFloat : BinaryOpsNode<float>
    {
        public override string EvaluateExpression()
        {
            var connectedNodeA =
                GetInputPortByName(InputPortAName).FirstConnectedPort.GetNode() as IEvaluatableExpression;
            var connectedNodeB =
                GetInputPortByName(InputPortBName).FirstConnectedPort.GetNode() as IEvaluatableExpression;

            return $"{connectedNodeA.EvaluateExpression()} + {connectedNodeB.EvaluateExpression()}";
        }
    }

    [Serializable]
    public class MultiplyFloat : BinaryOpsNode<float>
    {
        public override string EvaluateExpression()
        {
            var connectedNodeA =
                GetInputPortByName(InputPortAName).FirstConnectedPort.GetNode() as IEvaluatableExpression;
            var connectedNodeB =
                GetInputPortByName(InputPortBName).FirstConnectedPort.GetNode() as IEvaluatableExpression;

            return $"{connectedNodeA.EvaluateExpression()} * {connectedNodeB.EvaluateExpression()}";
        }
    }

    [Serializable]
    public abstract class SingleArgFunctionsNode<I, O> : Node, IEvaluatableExpression
    {
        public const string InputPortName = "InputPort";
        public const string OutputPortName = "OutputPort";

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort<I>(InputPortName).WithDisplayName("x").Build();
            context.AddOutputPort<O>(OutputPortName).WithDisplayName("").Build();
        }

        public abstract string EvaluateExpression();
    }

    [Serializable]
    public class Sin : SingleArgFunctionsNode<float, float>
    {
        public override string EvaluateExpression()
        {
            var connectedNode =
                GetInputPortByName(InputPortName).FirstConnectedPort.GetNode() as IEvaluatableExpression;
            return $"sin( ({connectedNode.EvaluateExpression()}) )";
        }
    }


    [Serializable]
    public class Length : SingleArgFunctionsNode<Vector3, float>
    {
        public override string EvaluateExpression()
        {
            var connectedNode =
                GetInputPortByName(InputPortName).FirstConnectedPort.GetNode() as IEvaluatableExpression;
            return $"length( ({connectedNode.EvaluateExpression()}) )";
        }
    }

    [Serializable]
    public class DecomposeVectorX : SingleArgFunctionsNode<Vector3, float>
    {
        public override string EvaluateExpression()
        {
            var connectedNode =
                GetInputPortByName(InputPortName).FirstConnectedPort.GetNode() as IEvaluatableExpression;
            return $"({connectedNode.EvaluateExpression()}).x";
        }
    }

    public class DecomposeVectorZ : SingleArgFunctionsNode<Vector3, float>
    {
        public override string EvaluateExpression()
        {
            var connectedNode =
                GetInputPortByName(InputPortName).FirstConnectedPort.GetNode() as IEvaluatableExpression;
            return $"({connectedNode.EvaluateExpression()}).z";
        }
    }

    [Serializable]
    public abstract class GetSinglePropertyNode<T> : Node, IEvaluatableExpression
    {
        public const string OutputPortName = "OutputPort";

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddOutputPort<T>(OutputPortName).WithDisplayName("value").Build();
        }

        public abstract string EvaluateExpression();
    }

    [Serializable]
    public class GetTime : GetSinglePropertyNode<float>
    {
        public override string EvaluateExpression()
        {
            return "_time";
        }
    }

    [Serializable]
    public class RandomFloat : GetSinglePropertyNode<float>
    {
        public override string EvaluateExpression()
        {
            return "rand()";
        }
    }


    [Serializable]
    public class GetPosition : GetSinglePropertyNode<Vector3>
    {
        public override string EvaluateExpression()
        {
            return "_Position[i]";
        }
    }

    [Serializable]
    public abstract class ConstNode<T> : Node, IEvaluatableExpression
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

        public abstract string EvaluateExpression();
    }

    [Serializable]
    public class ConstFloat : ConstNode<float>
    {
        public override string EvaluateExpression()
        {
            if (!GetNodeOptionByName(ConstPropertyName).TryGetValue(out float value))
            {
                throw new Exception("ConstFloat property doesn't exist");
            }

            return value.ToString(CultureInfo.InvariantCulture);
        }
    }

    [Serializable]
    public class ConstVector3 : ConstNode<Vector3>
    {
        public override string EvaluateExpression()
        {
            if (!GetNodeOptionByName(ConstPropertyName).TryGetValue(out Vector3 value))
            {
                throw new Exception("ConstVector3 property doesn't exist");
            }

            return $"float3( {value.x}, {value.y}, {value.z} )";
        }
    }

    [Serializable]
    public abstract class VectorConverterNode : Node, IEvaluatableExpression
    {
        public abstract string EvaluateExpression();
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

        public override string EvaluateExpression()
        {
            var connectedNodeA =
                GetInputPortByName(InputPortAName).FirstConnectedPort.GetNode() as IEvaluatableExpression;
            var connectedNodeB =
                GetInputPortByName(InputPortBName).FirstConnectedPort.GetNode() as IEvaluatableExpression;
            var connectedNodeC =
                GetInputPortByName(InputPortCName).FirstConnectedPort.GetNode() as IEvaluatableExpression;

            return
                $"float( {connectedNodeA.EvaluateExpression()}, {connectedNodeB.EvaluateExpression()}, {connectedNodeC.EvaluateExpression()} )";
        }
    }

    // [Serializable]
    // public class DecomposeVector : VectorConverterNode
    // {
    //     public const string InputPortName = "InputPort";
    //     public const string OutputPortAName = "OutputPortA";
    //     public const string OutputPortBName = "OutputPortB";
    //     public const string OutputPortCName = "OutputPortC";
    //
    //     protected override void OnDefinePorts(IPortDefinitionContext context)
    //     {
    //         context.AddInputPort<Vector3>(InputPortName).WithDisplayName("vector").Build();
    //         context.AddOutputPort<Vector3>(OutputPortAName).WithDisplayName("x").Build();
    //         context.AddOutputPort<Vector3>(OutputPortBName).WithDisplayName("y").Build();
    //         context.AddOutputPort<Vector3>(OutputPortCName).WithDisplayName("z").Build();
    //     }
    //
    //     public override string EvaluateExpression()
    //     {
    //         throw new NotImplementedException();
    //     }
    // }
}