using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;

#pragma warning disable 0414

[CustomEditor(typeof(UIImage))]
public class UIImageEditor : Editor {
    public UIImage m_target = null;
    public SerializedProperty m_imagePathProperty;

    private Image m_image = null;

    private GUIContent m_imagePathContent = new GUIContent("ImagePath");

    private void OnEnable()
    {
        m_target = target as UIImage;
        m_imagePathProperty = serializedObject.FindProperty("m_imagePath");
        m_image = m_target.GetComponent<Image>();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUILayout.Space(5);
        EditorGUILayout.BeginHorizontal("box");

        if (string.IsNullOrEmpty( m_imagePathProperty.stringValue))
        {
            GetPath();
        }

        string path = string.Format("{0}:{1}", "Path", m_imagePathProperty.stringValue);
        GUILayout.Label(path);

       
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(5);

        if (GUILayout.Button("Reset"))
        {
            GetPath();
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
        serializedObject.ApplyModifiedProperties();
    }

    private void GetPath()
    {
        if (m_image != null && m_image.sprite != null)
        {
            string path = AssetDatabase.GetAssetPath(m_image.sprite);

            path = CutPath(path);
            m_imagePathProperty.stringValue = path;
        }
    }

    private const string m_spritePackage = "SpritePackage";
    private string CutPath(string path)
    {
        int index = path.IndexOf(m_spritePackage);
        if (index <= 0)
        {
            return string.Empty;
        }

        index += m_spritePackage.Length + 1;

        path = path.Substring(index);
        return Path.GetDirectoryName(path);
    }
}
