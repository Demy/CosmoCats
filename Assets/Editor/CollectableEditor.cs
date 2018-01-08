using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Collectable))]
[CanEditMultipleObjects]
public class CollectableEditor : Editor
{
    SerializedProperty type;
    SerializedProperty value;
    SerializedProperty duration;
    SerializedProperty isTemporary;

    void OnEnable()
    {
        type = serializedObject.FindProperty("type");
        value = serializedObject.FindProperty("value");
        isTemporary = serializedObject.FindProperty("isTemporary");
        duration = serializedObject.FindProperty("duration");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(type);
        EditorGUILayout.PropertyField(value);
        if (type.enumValueIndex == 0)
        {
            EditorGUILayout.PropertyField(isTemporary);
            if (isTemporary.boolValue) EditorGUILayout.PropertyField(duration);
        }
        serializedObject.ApplyModifiedProperties();
    }
}
