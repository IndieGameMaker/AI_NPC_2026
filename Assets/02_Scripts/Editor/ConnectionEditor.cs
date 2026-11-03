using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Connection))]
public class ConnectionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        var connection = (Connection)target;

        if (GUILayout.Button("연결 테스트"))
        {
            connection.StartCoroutine(connection.OllamaConnect());
        }
    }
}