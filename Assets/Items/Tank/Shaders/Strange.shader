
Shader "Custom/Strange" {
	Properties {
		_Color ("Color Tint",Color) = (1.0,1.0,1.0,1.0)		// 声明一个Color类型的属性
	}

	SubShader {
		Pass {
			CGPROGRAM
			
			#pragma vertex vert								// 告诉Unity，vert函数包含顶点着色器代码
			#pragma fragment frag							// frag函数包含片元着色器代码

			fixed4 _Color;									// Cg中声明和属性名称类型相同的变量，这样才能在Cg中使用

			struct a2v {									// application To vertex shader 应用到着色器
				float4 vertex : POSITION;					// POSITION：把模型的顶点坐标填充到 vertex
				float3 normal : NORMAL;						// NORMAL：法线方向填充到 normal，范围[-1.0,1.0]
				float4 texcoord : TEXCOORD0;				// TEXCOORD0：纹理坐标填充到 texcoord
			};

			struct v2f {									// 顶点到片着色器
				float4 pos : SV_POSITION;					// 裁剪空间中的顶点坐标
				fixed3 color : COLOR0;						// 存储颜色信息
			};

			v2f vert(a2v v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);		// mul(UNITY_MATRIX_MVP,v),把顶点坐标从模型空间转到裁剪空间
				o.color = v.normal * 0.5 + fixed3(0.5,0.5,0.5);
				return o;	
			}

			float4 frag(v2f i) : SV_Target {	// SV_Target：把用户的输出颜色存到一个渲染目标中。这里输出到默认的帧缓存中
				fixed3 c = i.color;
				c *= _Color.rgb;				// 使用_Color属性来控制输出颜色
				return fixed4(c,1.0);			// 将插值后的i.color显示到屏幕
			}

			ENDCG
		}
	}
	FallBack "Diffuse"
}
