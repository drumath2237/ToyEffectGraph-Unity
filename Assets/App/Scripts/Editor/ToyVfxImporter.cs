using System.IO;
using System.Linq;
using App.Scripts.Runtime;
using Unity.GraphToolkit.Editor;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace ToyEffectGraph.Editor
{
    [ScriptedImporter(1, ToyEffectGraph.AssetExtension)]
    public class ToyVfxImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var graph = GraphDatabase.LoadGraphForImporter<ToyEffectGraph>(ctx.assetPath);


            var initializeEvalStr = graph.GetNodes().OfType<Initialize>().First().EvaluateExpression();
            var updateEvalStr = graph.GetNodes().OfType<Update>().First().EvaluateExpression();

            var asset = ScriptableObject.CreateInstance<ToyEffectGraphAsset>();
            asset.hlslCode = $"{initializeEvalStr}\n{updateEvalStr}";
            ctx.AddObjectToAsset("ToyEffectGraphAsset", asset);
            ctx.SetMainObject(asset);

            var hlslDirName = Path.GetDirectoryName(ctx.assetPath);
            var hlslFileName = Path.GetFileNameWithoutExtension(ctx.assetPath);
            var hlslPath = $"{Path.Combine(hlslDirName, hlslFileName)}.toyvfx.compute";
            File.WriteAllText(hlslPath, asset.hlslCode);
        }
    }
}