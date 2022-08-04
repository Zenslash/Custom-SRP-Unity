#ifndef SURFACE_INCLUDED
#define SURFACE_INCLUDED

struct Surface
{
    float3 color;
    float3 normal;
    float3 viewDirection;
    float alpha;
    float metallic;
    float smoothness;
};

#endif
