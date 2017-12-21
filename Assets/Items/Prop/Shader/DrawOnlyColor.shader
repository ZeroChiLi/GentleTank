
Shader "Custom/DrawOnlyColor" {
	Properties {
		_Color ("Color", Color) = (0, 0, 0, 1)
	}
    SubShader {
		Tags { "RenderType"="Opaque" "Queue"="Geometry"}
		
		Pass {
			
			ZWrite OFF							// 关闭深度写入
			Blend SrcAlpha OneMinusSrcAlpha		// 透明混合颜色
			
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			
			fixed4 _Color;
             
            float4 vert (float4 vertex : POSITION) : SV_POSITION
            {
                return UnityObjectToClipPos(vertex);
            }

			half4 frag() :COLOR {
				return _Color;
			}
			ENDCG
		}
		
	}
	FallBack OFF
}
