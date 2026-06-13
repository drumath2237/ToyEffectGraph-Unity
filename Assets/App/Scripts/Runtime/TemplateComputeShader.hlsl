#pragma kernel initialize
#pragma kernel update

struct Particle
{
    float3 position;
    // float3 velocity;
    // float lifetime;
    // float age;
};

RWStructuredBuffer<Particle> particles;

// float delta_time;
float time;

[numthreads(64, 1, 1)]
void initialize(uint id : SV_DispatchThreadID)
{
    Particle p;

    particles[id] = p;
}

[numthreads(64, 1, 1)]
void update(uint id : SV_DispatchThreadID)
{
    Particle p = particles[id];

    particles[id] = p;
}
