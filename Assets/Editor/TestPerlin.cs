using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PerlinNoise))]
public class TestPerlin : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        PerlinNoise noise = (PerlinNoise)target;
        if(GUILayout.Button("Update"))
            noise.UpdatePerlin();
    }
}