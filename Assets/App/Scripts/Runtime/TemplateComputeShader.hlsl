// ReSharper disable CppInconsistentNaming

#pragma kernel Initialize
#pragma kernel Update


float GetRandomNumber(float2 texCoord, int Seed)
{
    return frac(sin(dot(texCoord.xy, float2(12.9898, 78.233)) + Seed) * 43758.5453);
}

float rand()
{
    return GetRandomNumber(float2(0, 0), 0);
}

struct Particle
{
    float3 position;
    // float3 velocity;
    // float lifetime;
    // float age;
};

RWStructuredBuffer<Particle> _Particles;

// float _DeltaTime;
float _Time;

[numthreads(64, 1, 1)]
void Initialize(uint id : SV_DispatchThreadID)
{
    Particle p = (Particle)0;

    _Particles[id] = p;
}

[numthreads(64, 1, 1)]
void Update(uint id : SV_DispatchThreadID)
{
    Particle p = _Particles[id];

    _Particles[id] = p;
}
