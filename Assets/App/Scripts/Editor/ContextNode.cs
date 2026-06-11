using System;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace ToyEffectGraph.Editor
{
    [Serializable]
    public class Spawn : ContextNode
    {
        public struct SpawnContextOutput
        {
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddOutputPort<SpawnContextOutput>("").WithConnectorUI(PortConnectorUI.Arrowhead).Build();
        }
    }

    [Serializable]
    public class Initialize : ContextNode
    {
        public struct InitializeContextOutput
        {
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort<Spawn.SpawnContextOutput>("").WithConnectorUI(PortConnectorUI.Arrowhead).Build();
            context.AddOutputPort<InitializeContextOutput>("").WithConnectorUI(PortConnectorUI.Arrowhead).Build();
        }
    }

    [Serializable]
    public class Update : ContextNode
    {
        public struct UpdateContextOutput
        {
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort<Initialize.InitializeContextOutput>("").WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
            context.AddOutputPort<UpdateContextOutput>("").WithConnectorUI(PortConnectorUI.Arrowhead).Build();
        }
    }

    [Serializable]
    public class Output : ContextNode
    {
        public struct OutputContextOutput
        {
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort<Update.UpdateContextOutput>("").WithConnectorUI(PortConnectorUI.Arrowhead).Build();
        }
    }


}