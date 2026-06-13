using UnityEngine;

namespace App.Scripts.Runtime
{
    public class ToyEffectGraphAsset : ScriptableObject
    {
        public string hlslCode;
        public ComputeShader computeShader;
    }
}