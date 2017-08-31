using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UITween;
using System.Collections;

[CustomEditor(typeof(ReferencedFrom))] 
public class ReferencedProxy : Editor
{
	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector ();
		EditorGUILayout.HelpBox ("This object is an UI Object", MessageType.Info);
	}
}

[CustomEditor(typeof(EasyTween))]
public class EditorUITween : Editor
{
	private bool positionEnabled;
	private bool scaleEnabled;
	private bool rotationEnabled;
	EasyTween tweenScript;

	public void OnEnable ()
	{
		tweenScript = ((EasyTween)target);
	}

	public void OnSceneGUI ()
	{
		if (tweenScript != null) {
			if (tweenScript.rectTransform) {
				Handles.color = new Color (1f, 0f, 0f, 0.5f);
				Handles.DrawSolidDisc (tweenScript.rectTransform.position, Vector3.forward, 10f);
				Handles.color = Color.cyan;
			}
		}
	}

	public override void OnInspectorGUI ()
	{
        EditorGUILayout.BeginVertical();

		DrawDefaultInspector ();

		if (tweenScript != null) {
			if (tweenScript.rectTransform) {
				if (!Application.isPlaying) {
					tweenScript.animationParts.SetAniamtioDuration (EditorGUILayout.Slider ("Animation Duration (Sec)", tweenScript.animationParts.GetAnimationDuration (), 0.01f, 10f));

					EditorFade ();
					EditorPos ();
					EditorRot ();
					EditorScale ();
					GetButtonPos ();
					
					if (!tweenScript.rectTransform.gameObject.GetComponent<ReferencedFrom> ()) {
						tweenScript.rectTransform.gameObject.AddComponent<ReferencedFrom> ();
					}
					
					if (GUI.changed) {
						EditorUtility.SetDirty (tweenScript);
					}
				} else {
					if (GUILayout.Button ("Animate")) {
						tweenScript.OpenCloseObjectAnimation ();
					}
					EditorGUILayout.HelpBox ("Editor Not Available in Play Mode", MessageType.Info);
				}
			} else {
				EditorGUILayout.HelpBox ("Please set \"Rect Trasnform Variable\" that contains \"RectTransform\" component. UI Components", MessageType.Info);
			}
		}

        EditorGUILayout.EndVertical();
	}

	void GetAniamButtons ()
	{
		if (positionEnabled || rotationEnabled || scaleEnabled || tweenScript.animationParts.FadePropetiesAnim.IsFadeEnabled ()) {
			if (GUILayout.Button ("Animate")) {
				tweenScript.OpenCloseObjectAnimation ();
			}
		}
	}

	void GetButtonPos ()
	{
		EditorGUILayout.BeginHorizontal ();
		if (positionEnabled || rotationEnabled || scaleEnabled) {
			if (GUILayout.Button ("Get Start Values")) {
				GetStartValues ();
			}
			
			if (GUILayout.Button ("Get End Values")) {
				GetEndValues ();
			}
		}
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.Space ();
		
		GetAniamButtons ();
		
		EditorGUILayout.Space ();
		EditorGUILayout.BeginHorizontal ();
		if (positionEnabled || rotationEnabled || scaleEnabled || tweenScript.animationParts.FadePropetiesAnim.IsFadeEnabled ()) {
			if (GUILayout.Button ("Set To Start Values")) {
				SetStartValues ();
			}
			
			if (GUILayout.Button ("Set To End Values")) {
				SetEndValues ();
			}
		}
		EditorGUILayout.EndHorizontal ();
	}

	void GetStartValues ()
	{
		RectTransform selectedTransform = tweenScript.rectTransform;
		
		tweenScript.animationParts.PositionPropetiesAnim.SetPosStart ((Vector3)selectedTransform.anchoredPosition, selectedTransform);
		tweenScript.animationParts.ScalePropetiesAnim.StartScale = selectedTransform.localScale;
		tweenScript.animationParts.RotationPropetiesAnim.StartRot = selectedTransform.localEulerAngles;
	}

	void GetEndValues ()
	{
		RectTransform selectedTransform = tweenScript.rectTransform;
		
		tweenScript.animationParts.PositionPropetiesAnim.SetPosEnd ((Vector3)selectedTransform.anchoredPosition, selectedTransform.transform);
		tweenScript.animationParts.ScalePropetiesAnim.EndScale = selectedTransform.localScale;
		tweenScript.animationParts.RotationPropetiesAnim.EndRot = selectedTransform.localEulerAngles;
	}

	void SetStartValues ()
	{
		RectTransform selectedTransform = tweenScript.rectTransform;
		
		if (tweenScript.animationParts.PositionPropetiesAnim.IsPositionEnabled ())
			selectedTransform.anchoredPosition = (Vector2)tweenScript.animationParts.PositionPropetiesAnim.StartPos;
		
		if (tweenScript.animationParts.ScalePropetiesAnim.IsScaleEnabled ())
			selectedTransform.localScale = tweenScript.animationParts.ScalePropetiesAnim.StartScale; 
		
		if (tweenScript.animationParts.RotationPropetiesAnim.IsRotationEnabled ())
			selectedTransform.localEulerAngles = tweenScript.animationParts.RotationPropetiesAnim.StartRot;
		
		if (tweenScript.animationParts.FadePropetiesAnim.IsFadeEnabled ()) {
			if (tweenScript.IsObjectOpened ())
				SetAlphaValue (selectedTransform.transform, tweenScript.animationParts.FadePropetiesAnim.GetEndFadeValue ());
			else
				SetAlphaValue (selectedTransform.transform, tweenScript.animationParts.FadePropetiesAnim.GetStartFadeValue ());
		}
	}

	void SetEndValues ()
	{
		RectTransform selectedTransform = tweenScript.rectTransform;
		
		if (tweenScript.animationParts.PositionPropetiesAnim.IsPositionEnabled ())
			selectedTransform.anchoredPosition = (Vector2)tweenScript.animationParts.PositionPropetiesAnim.EndPos;
		
		if (tweenScript.animationParts.ScalePropetiesAnim.IsScaleEnabled ())
			selectedTransform.localScale = tweenScript.animationParts.ScalePropetiesAnim.EndScale; 
		
		if (tweenScript.animationParts.RotationPropetiesAnim.IsRotationEnabled ())
			selectedTransform.localEulerAngles = tweenScript.animationParts.RotationPropetiesAnim.EndRot;
		
		if (tweenScript.animationParts.FadePropetiesAnim.IsFadeEnabled ()) {
			if (tweenScript.IsObjectOpened ())
				SetAlphaValue (selectedTransform.transform, tweenScript.animationParts.FadePropetiesAnim.GetStartFadeValue ());
			else
				SetAlphaValue (selectedTransform.transform, tweenScript.animationParts.FadePropetiesAnim.GetEndFadeValue ());
		}
	}

	void SetAlphaValue (Transform _objectToSetAlpha, float alphaValue)
	{
		if (_objectToSetAlpha.GetComponent<MaskableGraphic> ()) {
			MaskableGraphic GraphicElement = _objectToSetAlpha.GetComponent<MaskableGraphic> ();
			
			Color objectColor = GraphicElement.color;
			
			objectColor.a = alphaValue;
			GraphicElement.color = objectColor;
		}
		
		if (_objectToSetAlpha.childCount > 0) {
			for (int i = 0; i < _objectToSetAlpha.childCount; i++) {
				if (!_objectToSetAlpha.GetChild (i).GetComponent<ReferencedFrom> () || tweenScript.animationParts.FadePropetiesAnim.IsFadeOverrideEnabled ()) {
					SetAlphaValue (_objectToSetAlpha.GetChild (i), alphaValue);
				}
			}
		}
	}

	void EditorFade ()
	{
		tweenScript.animationParts.FadePropetiesAnim.SetFadeEnable (EditorGUILayout.BeginToggleGroup ("Fade In & Out",
                tweenScript.animationParts.FadePropetiesAnim.IsFadeEnabled ()));
	

		if (tweenScript.animationParts.FadePropetiesAnim.IsFadeEnabled ()) {
			EditorGUILayout.LabelField ("Fade Start and End Values");

			EditorGUILayout.BeginHorizontal ();

			float fadeValueStart = EditorGUILayout.FloatField ("Start Value", tweenScript.animationParts.FadePropetiesAnim.GetStartFadeValue ());
			float fadeValueEnd = EditorGUILayout.FloatField ("End Value", tweenScript.animationParts.FadePropetiesAnim.GetEndFadeValue ());

			tweenScript.animationParts.FadePropetiesAnim.SetFadeValues (fadeValueStart, fadeValueEnd);

			EditorGUILayout.EndHorizontal ();
            
			tweenScript.animationParts.FadePropetiesAnim.SetFadeOverride (EditorGUILayout.BeginToggleGroup ("Fade Override", 
                    tweenScript.animationParts.FadePropetiesAnim.IsFadeOverrideEnabled ()));
                     
            EditorGUILayout.EndToggleGroup ();
		}

		EditorGUILayout.EndToggleGroup ();
	}

	void EditorPos ()
	{
		tweenScript.animationParts.PositionPropetiesAnim.SetPositionEnable (EditorGUILayout.BeginToggleGroup ("Position Animation", 
                tweenScript.animationParts.PositionPropetiesAnim.IsPositionEnabled ()));
		positionEnabled = tweenScript.animationParts.PositionPropetiesAnim.IsPositionEnabled ();
		
		if (positionEnabled) {
			tweenScript.animationParts.PositionPropetiesAnim.SetPosStart (EditorGUILayout.Vector3Field ("Start Move", tweenScript.animationParts.PositionPropetiesAnim.StartPos), tweenScript.rectTransform);
			tweenScript.animationParts.PositionPropetiesAnim.SetPosEnd (EditorGUILayout.Vector3Field ("End Move", tweenScript.animationParts.PositionPropetiesAnim.EndPos), tweenScript.rectTransform.transform);
			
			EditorGUILayout.BeginHorizontal ();

			if (tweenScript.animationParts.PositionPropetiesAnim.TweenCurveEnterPos == null) {
				tweenScript.animationParts.PositionPropetiesAnim.TweenCurveEnterPos = new AnimationCurve ();
			}
				
			if (tweenScript.animationParts.PositionPropetiesAnim.TweenCurveExitPos == null) {
				tweenScript.animationParts.PositionPropetiesAnim.TweenCurveExitPos = new AnimationCurve ();
			}
				
			tweenScript.animationParts.PositionPropetiesAnim.TweenCurveEnterPos = EditorGUILayout.CurveField ("Start Tween Move", 
                    tweenScript.animationParts.PositionPropetiesAnim.TweenCurveEnterPos);
			EditorGUILayout.Space ();
			tweenScript.animationParts.PositionPropetiesAnim.TweenCurveExitPos = EditorGUILayout.CurveField ("Exit Tween Move", 
                    tweenScript.animationParts.PositionPropetiesAnim.TweenCurveExitPos);

			EditorGUILayout.EndHorizontal ();
			
			EditorGUILayout.Space ();
		}
		
		EditorGUILayout.EndToggleGroup ();
	}

	void EditorScale ()
	{
		tweenScript.animationParts.ScalePropetiesAnim.SetScaleEnable (EditorGUILayout.BeginToggleGroup ("Scale Animation", 
                tweenScript.animationParts.ScalePropetiesAnim.IsScaleEnabled ()));
		scaleEnabled = tweenScript.animationParts.ScalePropetiesAnim.IsScaleEnabled ();
		
		if (scaleEnabled) {
			tweenScript.animationParts.ScalePropetiesAnim.StartScale = EditorGUILayout.Vector3Field ("Start Scale", tweenScript.animationParts.ScalePropetiesAnim.StartScale);
			tweenScript.animationParts.ScalePropetiesAnim.EndScale = EditorGUILayout.Vector3Field ("End Scale", tweenScript.animationParts.ScalePropetiesAnim.EndScale);
			
			EditorGUILayout.BeginHorizontal ();

			if (tweenScript.animationParts.ScalePropetiesAnim.TweenCurveEnterScale == null) {
				tweenScript.animationParts.ScalePropetiesAnim.TweenCurveEnterScale = new AnimationCurve ();
			}
				
			if (tweenScript.animationParts.ScalePropetiesAnim.TweenCurveExitScale == null) {
				tweenScript.animationParts.ScalePropetiesAnim.TweenCurveExitScale = new AnimationCurve ();
			}
				
			tweenScript.animationParts.ScalePropetiesAnim.TweenCurveEnterScale = EditorGUILayout.CurveField ("Start Tween Scale", 
                    tweenScript.animationParts.ScalePropetiesAnim.TweenCurveEnterScale);
			EditorGUILayout.Space ();
			tweenScript.animationParts.ScalePropetiesAnim.TweenCurveExitScale = EditorGUILayout.CurveField ("Exit Tween Scale", 
                    tweenScript.animationParts.ScalePropetiesAnim.TweenCurveExitScale);

			EditorGUILayout.EndHorizontal ();
			
			EditorGUILayout.Space ();
		}
		
		EditorGUILayout.EndToggleGroup ();
	}

	void EditorRot ()
	{
		tweenScript.animationParts.RotationPropetiesAnim.SetRotationEnable (EditorGUILayout.BeginToggleGroup ("Rotation Animation", 
                tweenScript.animationParts.RotationPropetiesAnim.IsRotationEnabled ()));
		rotationEnabled = tweenScript.animationParts.RotationPropetiesAnim.IsRotationEnabled ();
		
		if (rotationEnabled) {
			tweenScript.animationParts.RotationPropetiesAnim.StartRot = EditorGUILayout.Vector3Field ("Start Rotation", tweenScript.animationParts.RotationPropetiesAnim.StartRot);
			tweenScript.animationParts.RotationPropetiesAnim.EndRot = EditorGUILayout.Vector3Field ("End Rotation", tweenScript.animationParts.RotationPropetiesAnim.EndRot);
			
			EditorGUILayout.BeginHorizontal ();

			if (tweenScript.animationParts.RotationPropetiesAnim.TweenCurveEnterRot == null) {
				tweenScript.animationParts.RotationPropetiesAnim.TweenCurveEnterRot = new AnimationCurve ();
			}
				
			if (tweenScript.animationParts.RotationPropetiesAnim.TweenCurveExitRot == null) {
				tweenScript.animationParts.RotationPropetiesAnim.TweenCurveExitRot = new AnimationCurve ();
			}
				
			tweenScript.animationParts.RotationPropetiesAnim.TweenCurveEnterRot = EditorGUILayout.CurveField ("Start Tween Rotation", 
                    tweenScript.animationParts.RotationPropetiesAnim.TweenCurveEnterRot);
			EditorGUILayout.Space ();
			tweenScript.animationParts.RotationPropetiesAnim.TweenCurveExitRot = EditorGUILayout.CurveField ("Exit Tween Rotation", 
                    tweenScript.animationParts.RotationPropetiesAnim.TweenCurveExitRot);

			EditorGUILayout.EndHorizontal ();
			
			EditorGUILayout.Space ();
		}
		
		EditorGUILayout.EndToggleGroup ();
	}
}