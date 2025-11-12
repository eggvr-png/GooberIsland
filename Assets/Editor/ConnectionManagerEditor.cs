using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ConnectionManager))]
public class ConnectionManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        ConnectionManager cm = (ConnectionManager)target;

        if (GUILayout.Button("Manual Connect"))
        {
            cm.connectToServers();
        }
    }
}
