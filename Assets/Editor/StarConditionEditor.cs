using BattleInterface.Structures;
using UnityEditor;

[CustomEditor(typeof(StarCondition))]
[CanEditMultipleObjects]
public class StarConditionEditor : Editor
{
    SerializedProperty type;
    SerializedProperty value;

    void OnEnable()
    {
        type = serializedObject.FindProperty("type");
        value = serializedObject.FindProperty("value");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(type);
        if (type.enumValueIndex > 0)
        {
            EditorGUILayout.PropertyField(value);
        }
        serializedObject.ApplyModifiedProperties();
    }
}
