using System;
using Unity.GraphToolkit.Editor;
using UnityEditor;

namespace ToyEffectGraph.Editor
{
    [Serializable]
    [Graph(AssetExtension)]
    public class ToyEffectGraph : Graph
    {
        public const string AssetExtension = "toyvfx";

        [MenuItem("Assets/Create/Toy Effect Graph/Create Graph", false)]
        private static void CreateAssetFile()
        {
            GraphDatabase.PromptInProjectBrowserToCreateNewAsset<ToyEffectGraph>();
        }
    }
}