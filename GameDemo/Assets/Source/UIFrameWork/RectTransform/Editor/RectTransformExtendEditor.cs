using UnityEngine;
using System.Collections;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(RectTransformExtend))]
public class RectTransformExtendEditor : Editor {

	private RectTransformExtend _rectTransformExtend;
	private RectTransform _rectTransform;

	private SerializedProperty anchorsType;

	private SerializedProperty enableUse;
	
	private SerializedProperty posX;
	private SerializedProperty posY;
	
	private SerializedProperty width;
	private SerializedProperty height;
	
	private SerializedProperty left;
	private SerializedProperty right;
	
	private SerializedProperty top;
	private SerializedProperty bottom;

	void OnEnable()
	{
		_rectTransformExtend = target as RectTransformExtend;
		_rectTransform = _rectTransformExtend.gameObject.GetComponent<RectTransform> ();

		anchorsType = serializedObject.FindProperty ("anchorsType");

		enableUse = serializedObject.FindProperty ("enableUse");

		posX = serializedObject.FindProperty ("posX");
		posY = serializedObject.FindProperty ("posY");

		width = serializedObject.FindProperty ("width");
		height = serializedObject.FindProperty ("height");

		left = serializedObject.FindProperty ("left");
		right = serializedObject.FindProperty ("right");

		top = serializedObject.FindProperty ("top");
		bottom = serializedObject.FindProperty ("bottom");
	}

	public override void OnInspectorGUI ()
	{
		serializedObject.Update ();

		anchorsType.enumValueIndex = GetAnchorsType ();

		EditorGUILayout.BeginVertical ("box");
        
		EditorGUILayout.PropertyField(anchorsType, new GUIContent("AnchorsType"));
		GUILayout.Space (5);

		EditorGUILayout.PropertyField (enableUse, new GUIContent("EnableUse"));

		GUILayout.Space (5);
		Draw ();

		EditorGUILayout.EndVertical ();

		if (GUI.changed)
		{
			EditorUtility.SetDirty(target);
		}

		serializedObject.ApplyModifiedProperties ();
	}

	private void Draw()
	{
		if (anchorsType.enumValueIndex >= (int)ANCHORSTYPE.TOP_LEFT && anchorsType.enumValueIndex <= (int)ANCHORSTYPE.BOTTOM_RIGHT) 
		{
			DrawTypeOne();
		}
		else if (anchorsType.enumValueIndex <= (int)ANCHORSTYPE.BOTTOM_STRETCH)
		{
			DrawTypeTwo();
		}
		else if (anchorsType.enumValueIndex <= (int)ANCHORSTYPE.STRETCH_RIGHT)
		{
			DrawTypeThree();
		}
		else if (anchorsType.enumValueIndex <= (int)ANCHORSTYPE.STRETCH_STRETCH)
		{
			DrawTypeFour();
		}
	}

	private void DrawTypeOne()
	{
		if (GUILayout.Button ("Get")) 
		{
			posX.floatValue = _rectTransform.anchoredPosition.x;
			posY.floatValue = _rectTransform.anchoredPosition.y;
			
			width.floatValue = _rectTransform.rect.width;
			height.floatValue = _rectTransform.rect.height;
		}

		GUILayout.Space (5);
		EditorGUILayout.PropertyField (posX, new GUIContent("PosX"));
		GUILayout.Space (5);

		EditorGUILayout.PropertyField (posY, new GUIContent("PosY"));
		GUILayout.Space (5);

		EditorGUILayout.PropertyField (width, new GUIContent("Width"));
		GUILayout.Space (5);

		EditorGUILayout.PropertyField (height, new GUIContent("Height"));
		GUILayout.Space (5);

		if (GUILayout.Button ("Use")) 
		{
			RectTransformSetRect.SetLeftMiddleRight( _rectTransform, posX.floatValue, posY.floatValue, width.floatValue, height.floatValue);
		}
	}

	private void DrawTypeTwo()
	{
		if (GUILayout.Button ("Get")) 
		{
			left.floatValue = _rectTransform.offsetMin.x;
			posY.floatValue = _rectTransform.anchoredPosition.y;

			right.floatValue = -_rectTransform.offsetMax.x;

			height.floatValue = _rectTransform.rect.height;
		}
		
		GUILayout.Space (5);
		EditorGUILayout.PropertyField (left, new GUIContent("Left"));
		GUILayout.Space (5);

		EditorGUILayout.PropertyField (posY, new GUIContent("PosY"));
		GUILayout.Space (5);

		EditorGUILayout.PropertyField (right, new GUIContent("Right"));
		GUILayout.Space (5);

		EditorGUILayout.PropertyField (height, new GUIContent("Height"));
		GUILayout.Space (5);
		
		if (GUILayout.Button ("Use"))
		{
			RectTransformSetRect.SetStretchBottomMiddleTop( _rectTransform, left.floatValue, right.floatValue, posY.floatValue, height.floatValue);
		}
	}

	public void DrawTypeThree()
	{
		if (GUILayout.Button ("Get")) 
		{
			posX.floatValue = _rectTransform.anchoredPosition.x;
			top.floatValue = _rectTransform.offsetMax.y;
			width.floatValue = _rectTransform.rect.width;
			bottom.floatValue = _rectTransform.offsetMin.y;
		}
		
		GUILayout.Space (5);
		EditorGUILayout.PropertyField (posX, new GUIContent("PosX"));
		GUILayout.Space (5);
		
		EditorGUILayout.PropertyField (top, new GUIContent("Top"));
		GUILayout.Space (5);
		
		EditorGUILayout.PropertyField (width, new GUIContent("Width"));
		GUILayout.Space (5);
		
		EditorGUILayout.PropertyField (bottom, new GUIContent("Bottom"));
		GUILayout.Space (5);
		
		if (GUILayout.Button ("Use"))
		{
			RectTransformSetRect.SetStretchLeftCenterRight( _rectTransform, posX.floatValue, top.floatValue, width.floatValue, bottom.floatValue);
		}
	}
	
	private void DrawTypeFour()
	{
		if (GUILayout.Button ("Get"))
		{
			left.floatValue = _rectTransform.offsetMin.x;
			top.floatValue = (-1) * _rectTransform.offsetMax.y;
			right.floatValue = -_rectTransform.offsetMax.x;
			bottom.floatValue = _rectTransform.offsetMin.y;
		}

		GUILayout.Space (5);
		EditorGUILayout.PropertyField (left, new GUIContent("Left"));
		GUILayout.Space (5);
		
		EditorGUILayout.PropertyField (top, new GUIContent("Top"));
		GUILayout.Space (5);
		
		EditorGUILayout.PropertyField (right, new GUIContent("right"));
		GUILayout.Space (5);
		
		EditorGUILayout.PropertyField (bottom, new GUIContent("Bottom"));
		GUILayout.Space (5);
		
		if (GUILayout.Button ("Use"))
		{
			RectTransformSetRect.SetStretchStretch( _rectTransform, left.floatValue, top.floatValue, right.floatValue, bottom.floatValue);
		}
	}
	
	private int GetAnchorsType()
	{
		Vector2 anchorsMin = _rectTransform.anchorMin;
		Vector2 anchorsMax = _rectTransform.anchorMax;

		if (anchorsMin.x == 0 && anchorsMin.y == 1 && anchorsMax.x == 0 && anchorsMax.y == 1) 
		{
			return (int)ANCHORSTYPE.TOP_LEFT;
		}
		else if (anchorsMin.x == 0.5f && anchorsMin.y == 1 && anchorsMax.x == 0.5f && anchorsMax.y == 1) 
		{
			return (int)ANCHORSTYPE.TOP_CENTER;
		}
		else if (anchorsMin.x == 1 && anchorsMin.y == 1 && anchorsMax.x == 1 && anchorsMax.y == 1) 
		{
			return (int)ANCHORSTYPE.TOP_RIGHT;
		}
	    else if (anchorsMin.x == 0 && anchorsMin.y == 1 && anchorsMax.x == 1 && anchorsMax.y == 1) 
		{
			return (int)ANCHORSTYPE.TOP_STRETCH;
		}
		else if (anchorsMin.x == 0 && anchorsMin.y == 0.5f && anchorsMax.x == 0 && anchorsMax.y == 0.5f) 
		{
			return (int)ANCHORSTYPE.MIDDLE_LEFT;
		}
		else if (anchorsMin.x == 0.5f && anchorsMin.y == 0.5f && anchorsMax.x == 0.5f && anchorsMax.y == 0.5f) 
		{
			return (int)ANCHORSTYPE.MIDDLE_CENTER;
		}
		else if (anchorsMin.x == 1 && anchorsMin.y == 0.5f && anchorsMax.x == 1 && anchorsMax.y == 0.5f) 
		{
			return (int)ANCHORSTYPE.MIDDLE_RIGHT;
		}
		else if (anchorsMin.x == 0 && anchorsMin.y == 0.5f && anchorsMax.x == 1 && anchorsMax.y == 0.5f) 
		{
			return (int)ANCHORSTYPE.MIDDLE_STRETCH;
		}
		else if (anchorsMin.x == 0 && anchorsMin.y == 0.0f && anchorsMax.x == 0 && anchorsMax.y == 0.0f) 
		{
			return (int)ANCHORSTYPE.BOTTOM_LEFT;
		}
		else if (anchorsMin.x == 0.5 && anchorsMin.y == 0.0f && anchorsMax.x == 0.5f && anchorsMax.y == 0.0f) 
		{
			return (int)ANCHORSTYPE.BOTTOM_CENTER;
		}
		else if (anchorsMin.x == 1 && anchorsMin.y == 0.0f && anchorsMax.x == 1 && anchorsMax.y == 0.0f) 
		{
			return (int)ANCHORSTYPE.BOTTOM_RIGHT;
		}
		else if (anchorsMin.x == 0 && anchorsMin.y == 0.0f && anchorsMax.x == 1 && anchorsMax.y == 0.0f) 
		{
			return (int)ANCHORSTYPE.BOTTOM_STRETCH;
		}
		else if (anchorsMin.x == 0 && anchorsMin.y == 0.0f && anchorsMax.x == 0 && anchorsMax.y == 1f) 
		{
			return (int)ANCHORSTYPE.STRETCH_LEFT;
		}
		else if (anchorsMin.x == 0.5f && anchorsMin.y == 0.0f && anchorsMax.x == 0.5f && anchorsMax.y == 1f) 
		{
			return (int)ANCHORSTYPE.STRETCH_CENTER;
		}
		else if (anchorsMin.x == 1 && anchorsMin.y == 0.0f && anchorsMax.x == 1 && anchorsMax.y == 1f) 
		{
			return (int)ANCHORSTYPE.STRETCH_RIGHT;
		}
		else if (anchorsMin.x == 0 && anchorsMin.y == 0.0f && anchorsMax.x == 1 && anchorsMax.y == 1f) 
		{
			return (int)ANCHORSTYPE.STRETCH_STRETCH;
		}

		return (int)ANCHORSTYPE.NONE;
	}

}






































