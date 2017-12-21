
Shader "Custom/DrawOutlineOnce" {
	Properties {
		_Width ("Width", Range(0, 1)) = 0.1						// 轮廓线宽度
		_Color ("Outline Color", Color) = (0, 0, 0, 1)			// 轮廓线颜色
	}
    SubShader {
		Tags { "RenderType"="Opaque" "Queue"="Geometry"}
		
		Pass {
			NAME "OUTLINE"
			
			ZWrite OFF							// 关闭深度写入
			Cull Front							// 剔除正面
			Blend SrcAlpha OneMinusSrcAlpha		// 透明混合颜色
			
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			
			float _Width;
			fixed4 _Color;
			
			struct a2v {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			}; 
			
			struct v2f {
			    float4 pos : SV_POSITION;
			};
			
			v2f vert (a2v v) {
				v2f o;
				
				// 顶点和法线变换到视角空间下，让描边可以在观察空间达到最好的效果
				//float4 pos = mul(UNITY_MATRIX_MV, v.vertex); 
				float4 pos = float4(UnityObjectToViewPos(v.vertex),1); 
				float3 normal = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);  
				normal.z = -0.5;	// 让法线向视角方向外扩，避免物体有背面遮挡正面
				pos = pos + float4(normalize(normal), 0) * _Width;		//对外扩展，出现轮廓
				o.pos = mul(UNITY_MATRIX_P, pos);
				
				return o;
			}
			
			float4 frag(v2f i) : SV_Target { 
				return float4(_Color.rgb, 1);               
			}
			
			ENDCG
		}
		
	}
	FallBack OFF
}
