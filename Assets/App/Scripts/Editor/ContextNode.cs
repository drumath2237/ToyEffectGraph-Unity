using System;
using System.Collections.Generic;
using System.Linq;
using Unity.GraphToolkit.Editor;

namespace ToyEffectGraph.Editor
{
    [Serializable]
    public class Spawn : ContextNode, IEvaluatableExpression
    {
        public struct SpawnContextOutput
        {
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context
                .AddOutputPort<SpawnContextOutput>("")
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }

        public string EvaluateExpression()
        {
            throw new NotImplementedException();
        }
    }

    [Serializable]
    public class Initialize : ContextNode, IEvaluatableExpression
    {
        public struct InitializeContextOutput
        {
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context
                .AddInputPort<Spawn.SpawnContextOutput>("")
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
            context
                .AddOutputPort<InitializeContextOutput>("")
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }

        public string EvaluateExpression()
        {
            var nodes = BlockNodes as IEnumerable<IEvaluatableExpression>;
            var body = string.Join("\n", nodes.Select(n => n.EvaluateExpression()));
            var eval = $@"[numthreads(64, 1, 1)]
void initialize(uint id : SV_DispatchThreadID)
{{
    Particle p;

    {body}

    particles[id] = p;
}}";

            return eval;
        }
    }

    [Serializable]
    public class Update : ContextNode, IEvaluatableExpression
    {
        public struct UpdateContextOutput
        {
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context
                .AddInputPort<Initialize.InitializeContextOutput>("")
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
            context
                .AddOutputPort<UpdateContextOutput>("")
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }

        public string EvaluateExpression()
        {
            var nodes = BlockNodes as IEnumerable<IEvaluatableExpression>;
            var body = string.Join("\n", nodes.Select(n => n.EvaluateExpression()));
            var eval = $@"[numthreads(64, 1, 1)]
void update(uint id : SV_DispatchThreadID)
{{
    Particle p = particles[id];

    {body}

    particles[id] = p;
}}";

            return eval;
        }
    }

    [Serializable]
    public class Output : ContextNode
    {
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context
                .AddInputPort<Update.UpdateContextOutput>("")
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }
    }
}