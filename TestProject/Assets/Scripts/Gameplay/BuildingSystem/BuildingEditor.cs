#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Building))]
public class BuildingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Building building = (Building)target;

        DrawDefaultInspector();

        building.rows = EditorGUILayout.IntField("Rows", building.rows);
        building.columns = EditorGUILayout.IntField("Columns", building.columns);

        if (building.matrixData == null || building.matrixData.Length != building.rows * building.columns)
        {
            building.matrixData = new int[building.rows * building.columns];
            for (int i = 0; i < building.matrixData.Length; i++)
            {
                building.matrixData[i] = 1;
            }
        }

        EditorGUILayout.LabelField("Matrix");
        for (int i = 0; i < building.rows; i++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int j = 0; j < building.columns; j++)
            {
                building.matrixData[i * building.columns + j] = EditorGUILayout.IntField(building.matrixData[i * building.columns + j]);
            }
            EditorGUILayout.EndHorizontal();
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(building);
        }
    }
}
#endif