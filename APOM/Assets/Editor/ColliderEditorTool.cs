using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class ColliderEditorTool : EditorWindow
{
    private GameObject effectObject;
    private List<CustomColliderData> colliders = new();

    [MenuItem("Tools/Collider Tool")]
    public static void ShowWindow()
    {
        GetWindow<ColliderEditorTool>("Collider Tool");
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnGUI()
    {
        GUILayout.Label("Effect Object", EditorStyles.boldLabel);
        effectObject = (GameObject)EditorGUILayout.ObjectField(effectObject, typeof(GameObject), true);

        EditorGUILayout.Space();

        if (effectObject == null)
        {
            EditorGUILayout.HelpBox("이펙트 오브젝트를 먼저 선택하세요.", MessageType.Warning);
            return;
        }

        for (int i = 0; i < colliders.Count; i++)
        {
            var col = colliders[i];
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField($"Collider {i + 1}", EditorStyles.boldLabel);

            col.type = (ColliderType)EditorGUILayout.EnumPopup("Collider Type", col.type);
            col.center = EditorGUILayout.Vector3Field("Center", col.center);

            switch (col.type)
            {
                case ColliderType.Box:
                    col.size = EditorGUILayout.Vector3Field("Size", col.size);
                    break;
                case ColliderType.Sphere:
                    col.radius = EditorGUILayout.FloatField("Radius", col.radius);
                    break;
                case ColliderType.Capsule:
                    col.radius = EditorGUILayout.FloatField("Radius", col.radius);
                    col.height = EditorGUILayout.FloatField("Height", col.height);
                    break;
            }

            if (GUILayout.Button("Remove"))
            {
                colliders.RemoveAt(i);
                break;
            }

            EditorGUILayout.EndVertical();
        }

        if (GUILayout.Button("Add Collider"))
        {
            colliders.Add(new CustomColliderData());
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Save"))
        {
            SaveColliders();
        }
    }

    private void SaveColliders()
    {
        if (effectObject == null) return;

        if (PrefabUtility.IsPartOfPrefabAsset(effectObject))
        {
            Debug.LogError("이펙트 오브젝트는 Prefab Asset이 아닌, 씬에 존재하는 인스턴스여야 합니다.");
            return;
        }

        foreach (var col in effectObject.GetComponents<Collider>())
        {
            DestroyImmediate(col);
        }

        foreach (var data in colliders)
        {
            switch (data.type)
            {
                case ColliderType.Box:
                    var box = effectObject.AddComponent<BoxCollider>();
                    box.center = data.center;
                    box.size = data.size;
                    break;
                case ColliderType.Sphere:
                    var sphere = effectObject.AddComponent<SphereCollider>();
                    sphere.center = data.center;
                    sphere.radius = data.radius;
                    break;
                case ColliderType.Capsule:
                    var capsule = effectObject.AddComponent<CapsuleCollider>();
                    capsule.center = data.center;
                    capsule.radius = data.radius;
                    capsule.height = data.height;
                    break;
            }
        }

        EditorUtility.SetDirty(effectObject);
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        if (effectObject == null) return;

        Handles.color = new Color(0f, 1f, 0f, 0.4f);
        Transform t = effectObject.transform;

        for (int i = 0; i < colliders.Count; i++)
        {
            var data = colliders[i];
            Vector3 worldPos = t.TransformPoint(data.center);

            switch (data.type)
            {
                case ColliderType.Box:
                    data.center = t.InverseTransformPoint(Handles.PositionHandle(worldPos, Quaternion.identity));
                    data.size = Handles.ScaleHandle(data.size, worldPos, Quaternion.identity, 1f);
                    Handles.DrawWireCube(worldPos, data.size);
                    break;

                case ColliderType.Sphere:
                    data.center = t.InverseTransformPoint(Handles.PositionHandle(worldPos, Quaternion.identity));
                    float newRadius = Handles.RadiusHandle(Quaternion.identity, worldPos, data.radius);
                    data.radius = newRadius;
                    Handles.DrawWireDisc(worldPos, Vector3.up, data.radius);
                    break;

                case ColliderType.Capsule:
                    data.center = t.InverseTransformPoint(Handles.PositionHandle(worldPos, Quaternion.identity));
                    data.radius = Handles.RadiusHandle(Quaternion.identity, worldPos, data.radius);
                    Handles.DrawWireDisc(worldPos, Vector3.up, data.radius);
                    Handles.DrawLine(worldPos + Vector3.up * data.height * 0.5f, worldPos - Vector3.up * data.height * 0.5f);
                    break;
            }
        }
    }
}

public enum ColliderType { Box, Sphere, Capsule }

[System.Serializable]
public class CustomColliderData
{
    public ColliderType type = ColliderType.Box;
    public Vector3 center = Vector3.zero;
    public Vector3 size = Vector3.one;
    public float radius = 0.5f;
    public float height = 2f;
}
