Shader "Custom/MyShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_Normal ("Normal", 2D) = "blue" {}

        // Slicing effect
        _SliceColor ("SliceColor", Color) = (0, 1, 0, .15)
        _PlanePos ("PlanePos", Vector) = (0, 0, 0, 0)
        _PlanePulseRate ("PlanePulseRate", Float) = 4
        _PlaneNormal ("PlaneNormal", Vector) = (1, 0, 0, 1)
        _PlaneOpacity ("PlaneOpacity", Float) = 1
        _PlaneFalloff ("PlaneFalloff", Float) = 2
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows
		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

        // Bind properties
		sampler2D _MainTex;
		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

        float4 _SliceColor;
        float4 _PlanePos;
        float _PlanePulseRate;
        float4 _PlaneNormal;
        float _PlaneOpacity;
        float _PlaneFalloff;

        struct Input {
            float2 uv_MainTex;
            float3 worldPos;
        };

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)


        // TODO: Move to lib
        float DistanceToPlane(float3 pt, float3 planePos, float3 planeNormal)
        {
            // Given a point and a plane defined by (pos, normal), returns distance from the pt to the plane
            // per https://mathinsight.org/distance_point_plane
            float dee = -(planeNormal.x * planePos.x + planeNormal.y * planePos.y + planeNormal.z * planePos.z);

            float distToPlane = planeNormal.x * pt.x + planeNormal.y * pt.y + planeNormal.z * pt.z + dee;
            distToPlane /= sqrt(planeNormal.x * planeNormal.x + planeNormal.y * planeNormal.y + planeNormal.z * planeNormal.z);

            return distToPlane;
        }

        float3 GetPlaneEmissive(float3 pt, float3 planePos, float3 planeNormal)
        {
            // Pretty plane
            // Pull plane back origin a bit
            //planePos -= float3(sign(planePos.x),sign(planePos.y),sign(planePos.z)) * 0;
            planePos -= normalize(planeNormal) * .5;

            float3 planeEmiss = _SliceColor.xyz * smoothstep(0, 1, DistanceToPlane(pt, planePos, planeNormal) * _PlaneFalloff) * _SliceColor.w;
            // Pulse glow 
            float modulation = lerp(.5, 1, sin(_Time.y * _PlanePulseRate) * .5 + .5);
            return planeEmiss * modulation;
        }

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
            o.Emission += GetPlaneEmissive(IN.worldPos, _PlanePos, _PlaneNormal) * _PlaneOpacity;

			o.Normal = float3(0,0,1);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
