using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelEditor))]
[CanEditMultipleObjects]
public class LevelEditorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Save"))
        {
            var levelEditor = (LevelEditor)serializedObject.targetObject;
            levelEditor.Save();
        }
    }
}
