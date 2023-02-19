Shader "Custom/ParallaxTest" {
	Properties {
		_Color1 ("Color1", Color) = (1,1,1,1)
		_Color2 ("Color2", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_NormalMap ("Normal", 2D) = "blue" {}

        _Parallax("Parallax", 2D) = "white" {}
        _ParallaxDepth("Parallax Depth", Range(0,1)) = .1
	}
	SubShader {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf_main Standard fullforwardshadows vertex:vs_main
        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        // Bind properties
        sampler2D _MainTex;
        sampler2D _NormalMap;
        sampler2D _Parallax;

        half _Glossiness;
        half _Metallic;
        fixed4 _Color1;
        fixed4 _Color2;
        half _ParallaxDepth;

        struct Input {
            float4 _uv1;
            float3 worldPos;
            float3 tangentViewDir;
        };

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)


        //Get the height from a uv position
        float getHeight(float2 uvs, sampler2D depthTex)
        {
            // Don't mip depth texture
            float depthColor = tex2Dlod(depthTex, float4(uvs, 0, 0)).r;
            //Calculate the height at this uv coordinate
            //(1-color.r) because black should be low
            //-1 because the ray is going down so the ray's y-coordinate will be negative
            return (1 - depthColor) * -1 * _ParallaxDepth;
        }
        //Get the texture position by interpolation between the position where we hit terrain and the position before
        float2 getWeightedUVs(float3 rayPos, float3 rayDir, float stepDistance,
                              sampler2D depthTex)
        {
            //Move one step back to the position before we hit terrain
            float3 oldPos = rayPos - stepDistance * rayDir;
            float oldHeight = getHeight(oldPos.xz, depthTex);
            //Always positive
            float oldDistToSurface = abs(oldHeight - oldPos.y);

            float currentHeight = getHeight(rayPos.xz, depthTex);
            //Always negative
            float currentDistToSurface = rayPos.y - currentHeight;

            float weight = currentDistToSurface / (currentDistToSurface - oldDistToSurface);

            //Calculate a weighted texture coordinate
            //If height is -2 and oldHeight is 2, then weightedTex is 0.5, which is good because we should use 
            //the exact middle between the coordinates
            float2 weightedUVs = oldPos.xz * weight + rayPos.xz * (1 - weight);
            return weightedUVs;
        }

        // Get parallax UVs and height
        void parallax(in float2 uvs, in float3 rayDir, in sampler2D depthTex,
                      out float2 offsetUVs, out float rayLength)
        {
            offsetUVs = uvs;
            rayLength = 0.0;

            // Raymarch the depth texture. Initial pos/dir
            float3 rayPos = float3(uvs.x, 0, uvs.y);
            float3 origRayPos = rayPos;
            rayDir = normalize(rayDir);

            //Find where the ray is intersecting with the terrain with a raymarch algorithm
            int STEPS = 300;
            float stepDistance = 0.001;

            for (int i = 0; i < STEPS; i++)
            {
                //Get the current height at this uv coordinate
                float height = getHeight(rayPos.xz, depthTex);

                //If the ray is below the surface
                if (rayPos.y < height)
                {
                    //Get the texture position by interpolation between the position where we hit terrain and the position before
                    offsetUVs = getWeightedUVs(rayPos, rayDir, stepDistance,
                                                  depthTex);
                    height = getHeight(offsetUVs, depthTex);
                    rayLength = length(origRayPos - rayPos);
                    break;
                }
                //Move along the ray
                rayPos += stepDistance * rayDir;
            }
        }

        // ----------------------------------------------------------

        void vs_main(inout appdata_full vs_in, out Input ps_in)
        {
            UNITY_INITIALIZE_OUTPUT(Input, ps_in);

            //Transform the view direction from world space to tangent space			
            float3 worldVertexPos = mul(unity_ObjectToWorld, vs_in.vertex).xyz;
            float3 worldViewDir = worldVertexPos - _WorldSpaceCameraPos;

            //To convert from world space to tangent space we need the following
            //https://docs.unity3d.com/Manual/SL-VertexFragmentShaderExamples.html
            float3 worldNormal = UnityObjectToWorldNormal(vs_in.normal);
            float3 worldTangent = UnityObjectToWorldDir(vs_in.tangent.xyz);
            float3 worldBitangent = cross(worldNormal, worldTangent) * vs_in.tangent.w * unity_WorldTransformParams.w;

            ps_in._uv1 = vs_in.texcoord;

            //Use dot products instead of building the matrix
            ps_in.tangentViewDir = float3(
                dot(worldViewDir, worldTangent),
                dot(worldViewDir, worldNormal),
                dot(worldViewDir, worldBitangent)
                );
        }

		void surf_main(Input ps_in, inout SurfaceOutputStandard outSurf) {
            float2 baseUVs = ps_in._uv1.xy;

            // TODO: Get parallax UV and depth
            float2 parallaxUV;
            float parallaxRayLength;
            parallax(baseUVs, ps_in.tangentViewDir, _Parallax,
                     parallaxUV, parallaxRayLength);

            float4 coolColor = lerp(_Color1, _Color2, saturate(parallaxRayLength));

			// Albedo comes from a texture tinted by color
			fixed4 albedo = tex2D(_MainTex, parallaxUV) * coolColor;
			//fixed4 albedo = tex2D(_Parallax, ps_in._uv1.xy);
            outSurf.Albedo = albedo.rgb;

			// Metallic and smoothness come from slider variables
            outSurf.Metallic = _Metallic;
            outSurf.Smoothness = _Glossiness;
            //outSurf.Normal = float3(0, 0, 1);
            outSurf.Normal = tex2D(_NormalMap, parallaxUV);
            outSurf.Alpha = 1.0;

            // Debug Testing. Remove!
            //outSurf.Albedo = float3(.5, .5, .5);
            outSurf.Normal.xy = (outSurf.Normal.xy * 2) - 1;
            //outSurf.Metallic = 0;
            //outSurf.Smoothness = 0.0;
            //outSurf.Albedo = float3(0, 0, 0);
            //outSurf.Emission = tex2D(_Parallax, parallaxUV);
            //outSurf.Emission = float3(parallaxRayLength, 0, 0);
		}

		ENDCG
	}
	FallBack "Diffuse"
}
