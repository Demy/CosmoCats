using UnityEditor;

[CustomEditor(typeof(Enemy))]
[CanEditMultipleObjects]
public class EnemyEditor : Editor
{
    SerializedProperty range;
    SerializedProperty followPlayer;
    SerializedProperty verticalSpeed;
    SerializedProperty horizontalSpeed;
    SerializedProperty keepSameSpeed;

    void OnEnable()
    {
        range = serializedObject.FindProperty("range");
        followPlayer = serializedObject.FindProperty("followPlayer");
        verticalSpeed = serializedObject.FindProperty("verticalSpeed");
        horizontalSpeed = serializedObject.FindProperty("horizontalSpeed");
        keepSameSpeed = serializedObject.FindProperty("keepSameSpeed");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(range);
        EditorGUILayout.PropertyField(followPlayer);
        if (followPlayer.boolValue)
        {
            EditorGUILayout.PropertyField(verticalSpeed);
            if (!keepSameSpeed.boolValue) EditorGUILayout.PropertyField(horizontalSpeed);
            EditorGUILayout.PropertyField(keepSameSpeed);
        }
        serializedObject.ApplyModifiedProperties();
    }
}
