Shader "MyStencil/StencilValue"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "black" {}
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
        

        Pass
        {
        Cull Back
        ColorMask 0
        //Blend Zero One
            Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
            ZTest LEqual
            ZWrite Off

            Stencil
            {
                Ref [_StencilID]
                Comp Always
                Pass Replace
                Fail Replace
                ZFail Keep
            }

        }
    }
}
