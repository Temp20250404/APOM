using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using APOM_Data;

public class ColliderDataPreviewWindow : EditorWindow
{
    // 미리보기용 데이터 리스트 (Collider_Data 구조체 리스트)
    private List<Collider_Data> previewData;

    /// 외부에서 데이터를 넘겨받아 윈도우를 생성 및 출력
    public static void ShowWindow(List<Collider_Data> data)
    {
        var window = GetWindow<ColliderDataPreviewWindow>("Collider Data Preview"); // 윈도우 생성 및 타이틀 설정
        window.previewData = data; // 데이터 저장
    }

    private Vector2 scroll; // 스크롤 위치 저장용 변수

    private void OnGUI()
    {
        // 데이터가 없는 경우 안내 문구 출력
        if (previewData == null || previewData.Count == 0)
        {
            EditorGUILayout.LabelField("불러온 데이터가 없습니다.");
            return;
        }

        // 스크롤 가능한 영역 시작
        scroll = EditorGUILayout.BeginScrollView(scroll);

        // 각 Collider_Data 항목에 대해 UI 구성
        foreach (var d in previewData)
        {
            EditorGUILayout.BeginVertical("box"); // 각 항목을 박스 스타일로 묶음

            EditorGUILayout.LabelField($"Index: {d.index}"); // 고유 인덱스
            EditorGUILayout.LabelField($"Name: {d.name}");   // 오브젝트 이름
            EditorGUILayout.LabelField($"Type: {d.ECollidertype}"); // 콜라이더 타입 (Box, Sphere, Capsule)

            // 중심 좌표 출력
            EditorGUILayout.Vector3Field("Center", new Vector3(d.centerX, d.centerY, d.centerZ));

            // 타입에 따라 추가 속성 출력
            if (d.ECollidertype == ECollidertype.Box)
            {
                EditorGUILayout.Vector3Field("Size", new Vector3(d.sizeX, d.sizeY, d.sizeZ));
            }
            else if (d.ECollidertype == ECollidertype.Sphere)
            {
                EditorGUILayout.FloatField("Radius", d.radius);
            }
            else if (d.ECollidertype == ECollidertype.Capsule)
            {
                EditorGUILayout.FloatField("Radius", d.radius);
                EditorGUILayout.FloatField("Height", d.height);
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();       
        }

        EditorGUILayout.EndScrollView();
    }
}
