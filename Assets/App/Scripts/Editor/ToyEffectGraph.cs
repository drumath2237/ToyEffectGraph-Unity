using System;
using Unity.GraphToolkit.Editor;

namespace ToyEffectGraph.Editor
{
    [Serializable]
    [Graph(AssetExtension)]
    public class ToyEffectGraph : Graph
    {
        public const string AssetExtension = "toyvfx";
    }
}