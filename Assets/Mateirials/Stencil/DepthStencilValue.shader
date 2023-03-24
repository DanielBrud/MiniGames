Shader "MyStencil/DepthStencilValue"
{
    Properties
    {
        //_MainTex ("Texture", 2D) = "black" {}
        //_Color("AlphaTest",Color) = (0,0,0,0)
        [IntRange] _StencilID("Stencil ID",Range(0,255)) = 0
    }
    SubShader
    {
        Tags 
        { 
            "RenderPipeline" = "UniversalPipeline"
            "RenderType"="Transparent"
            "Queue" = "Transparent"
        
        }
        Blend SrcAlpha OneMinusSrcAlpha

        ZWrite On
        
             
        
        Pass
        {
        //Cull Front
        //ColorMask 0
        
        //ZTest LEqual
        //ZWrite On
        HLSLPROGRAM
            // This line defines the name of the vertex shader.
            #pragma vertex vert
            // This line defines the name of the fragment shader.
            #pragma fragment frag

            // The Core.hlsl file contains definitions of frequently used HLSL
            // macros and functions, and also contains #include references to other
            // HLSL files (for example, Common.hlsl, SpaceTransforms.hlsl, etc.).
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // The structure definition defines which variables it contains.
            // This example uses the Attributes structure as an input structure in
            // the vertex shader.
            struct Attributes
            {
                // The positionOS variable contains the vertex positions in object
                // space.
                float4 positionOS   : POSITION;
            };

            struct Varyings
            {
                // The positions in this struct must have the SV_POSITION semantic.
                float4 positionHCS  : SV_POSITION;
            };

            // The vertex shader definition with properties defined in the Varyings
            // structure. The type of the vert function must match the type (struct)
            // that it returns.
            Varyings vert(Attributes IN)
            {
                // Declaring the output object (OUT) with the Varyings struct.
                Varyings OUT;
                // The TransformObjectToHClip function transforms vertex positions
                // from object space to homogenous clip space.
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                // Returning the output.
                return OUT;
            }

            // The fragment shader definition.
            half4 frag() : SV_Target
            {
                // Defining the color variable and returning it.
                half4 customColor = half4(1, 0, 0, 0.3);
                return customColor;
            }
             
            ENDHLSL
                Stencil
            {
                    Ref [_StencilID]
                    Comp Always
                    Pass Keep
                    Fail Keep
                    ZFail Replace
            }

        }
    }
}
