using UnityEditor;
using UnityEngine;

namespace Agents
{
    [CustomEditor(typeof(GroundConfigurator))]
    public class GroundEditor : Editor
    {
        private GroundConfigurator generator;

        private void OnEnable()
        {
            generator = (GroundConfigurator) target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();
            if (GUILayout.Button("Generate Mesh"))
            {
                Undo.RecordObjects(new Object[]
                {
                    generator,
                    generator.groundMeshFilter,
                }, "Generate Ground");
                generator.Generate(randomizeConfig: false);
            }
            if (GUILayout.Button("Randomize Config and Generate Mesh"))
            {
                Undo.RecordObjects(new Object[]
                {
                    generator,
                    generator.groundMeshFilter,
                }, "Generate Ground");
                generator.Generate(randomizeConfig: true);
            }
        }
    }
}
