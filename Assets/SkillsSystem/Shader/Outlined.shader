// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/OutLine1"
{
	Properties
	{
		_MainTex("main tex",2D) = "black"{}
	_RimColor("rim color",Color) = (1,1,1,1)//边缘颜色
		_RimPower("rim power",range(1,10)) = 2//边缘强度
	}

		SubShader
	{
		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include"UnityCG.cginc"

		struct v2f
	{
		float4 vertex:POSITION;
		float4 uv:TEXCOORD0;
		float4 NdotV:COLOR;
	};

	sampler2D _MainTex;
	float4 _RimColor;
	float _RimPower;

	v2f vert(appdata_base v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv = v.texcoord;
		float3 V = WorldSpaceViewDir(v.vertex);
		V = mul(unity_WorldToObject,V);//视方向从世界到模型坐标系的转换
		o.NdotV.x = saturate(dot(v.normal,normalize(V)));//必须在同一坐标系才能正确做点乘运算
		return o;
	}

	half4 frag(v2f IN) :COLOR
	{
		half4 c = tex2D(_MainTex,IN.uv);
		//用视方向和法线方向做点乘，越边缘的地方，法线和视方向越接近90度，点乘越接近0.
		//用（1- 上面点乘的结果）*颜色，来反映边缘颜色情况
		c.rgb += pow((1 - IN.NdotV.x) ,_RimPower)* _RimColor.rgb;
		return c;
	}
		ENDCG
	}
	}
		FallBack "Diffuse"
}