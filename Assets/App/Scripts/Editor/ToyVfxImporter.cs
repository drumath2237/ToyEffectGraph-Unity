using System.IO;
using System.Linq;
using App.Scripts.Runtime;
using Unity.GraphToolkit.Editor;
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
            var asset = ScriptableObject.CreateInstance<ToyEffectGraphAsset>();
            asset.hlslCode = GenerateHlslCodeFromGraph(graph);
            ctx.AddObjectToAsset("ToyEffectGraphAsset", asset);
            ctx.SetMainObject(asset);

            var hlslDirName = Path.GetDirectoryName(ctx.assetPath);
            var hlslFileName = Path.GetFileNameWithoutExtension(ctx.assetPath);
            var hlslPath = $"{Path.Combine(hlslDirName, hlslFileName)}.toyvfx.compute";
            File.WriteAllText(hlslPath, asset.hlslCode);
        }

        private string GenerateHlslCodeFromGraph(ToyEffectGraph graph)
        {
            var initializeFunctionCode = graph.GetNodes().OfType<Initialize>().First().EvaluateExpression();
            var updateFunctionCode = graph.GetNodes().OfType<Update>().First().EvaluateExpression();

            var hlslCode = $@"// ReSharper disable CppInconsistentNaming

#pragma kernel Initialize
#pragma kernel Update


float GetRandomNumber(float2 texCoord, int Seed)
{{
    return frac(sin(dot(texCoord.xy, float2(12.9898, 78.233)) + Seed) * 43758.5453);
}}

float rand()
{{
    return GetRandomNumber(float2(0, 0), 0);
}}

struct Particle
{{
    float3 position;
    // float3 velocity;
    // float lifetime;
    // float age;
}};

RWStructuredBuffer<Particle> _Particles;

// float _DeltaTime;
float _Time;

{initializeFunctionCode}

{updateFunctionCode}
";


            return hlslCode;
        }
    }
}