//http://answers.unity3d.com/questions/882134/shader-based-on-vertex-height.html

Shader "Custom/HeightDependentTint"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
	_HeightMin("Height Min", Float) = -1
		_HeightMax("Height Max", Float) = 1
		_Height("Height", Float) = 0
		_ColorMin("Tint Color At Min", Color) = (0,0,0,1)
		_ColorMax("Tint Color At Max", Color) = (1,1,1,1)
		_MaxDiff("Max diff", Float) = 1
	}

		SubShader
	{
		//Tags{"RenderType" = "Cutout"}
		Lighting On
		CGPROGRAM

#pragma surface surf Lambert alpha vertex:vert

		sampler2D _MainTex;
	fixed4 _ColorMin;
	fixed4 _ColorMax;
	float _HeightMin;
	float _HeightMax;
	float _Height;
	float _MaxDiff;

	struct Input
	{
		float2 uv_MainTex;
		float3 worldPos;
		float3 localPos;
	};

	void vert(inout appdata_full v, out Input o) {
		UNITY_INITIALIZE_OUTPUT(Input, o);
		o.localPos = v.vertex.xyz;
	}

	void surf(Input IN, inout SurfaceOutput o)
	{
		half4 c = tex2D(_MainTex, IN.uv_MainTex);
		float y = IN.localPos.z -1;
		float diff = _Height - y;

		float a = 1;
		
		if (diff < -_MaxDiff) {
			if (diff < 0)
				diff *= -1;
			a = (1 / diff) * _MaxDiff;
		}

		//float h = ((_HeightMax+_HeightMin) - IN.worldPos.y) / (_HeightMax + _HeightMin);
		float h = (_HeightMax+_HeightMin - diff) / (_HeightMax + _HeightMin);
		fixed4 tintColor = lerp(_ColorMax.rgba, _ColorMin.rgba, h);
		
		//fixed4 tintColor = _ColorMax.rgba;
		tintColor.a = a;
		o.Albedo = c.rgb * tintColor.rgb;
		o.Alpha = c.a * tintColor.a;
	}

	ENDCG
	}
		Fallback "Diffuse"
}