using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GroundExample))]
public class GroundEditor : Editor
{
    private GroundExample generator;

    private void OnEnable()
    {
        generator = (GroundExample) target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();
        if (GUILayout.Button("Generate mesh"))
        {
            Undo.RecordObjects(new Object[]
            {
                generator,
                generator.groundMeshFilter,
            }, "Generate ground");
            generator.Generate(randomizeConfig: false);
        }
        if (GUILayout.Button("Randomize config and generate mesh"))
        {
            Undo.RecordObjects(new Object[]
            {
                generator,
                generator.groundMeshFilter,
            }, "Generate ground");
            generator.Generate(randomizeConfig: true);
        }
    }
}
