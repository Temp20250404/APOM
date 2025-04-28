using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using APOM_Data;


public class ColliderEditorTool : EditorWindow
{
    private GameObject Object; // 콜라이더를 적용할 대상 오브젝트
    private List<CustomColliderData> colliders = new(); // 콜라이더 설정 정보를 담는 리스트

    // Unity 메뉴바에 툴 등록
    [MenuItem("Tools/Collider Tool")]
    public static void ShowWindow()
    {
        GetWindow<ColliderEditorTool>("Collider Tool"); // 윈도우 타이틀 설정
    }

    // 에디터 창이 활성화될 때 SceneView GUI 콜백 등록
    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    // 에디터 창이 비활성화될 때 콜백 제거
    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    // 에디터 윈도우 UI 정의
    private void OnGUI()
    {
        GUILayout.Label("Effect Object", EditorStyles.boldLabel); // 제목 라벨
        Object = (GameObject)EditorGUILayout.ObjectField(Object, typeof(GameObject), true); // 대상 오브젝트 설정

        EditorGUILayout.Space(); // 공간 띄우기

        // 시트 데이터 미리보기
        if (GUILayout.Button("Preview UNIGS Data"))
        {
            var list = Collider_Data.GetList();
            ColliderDataPreviewWindow.ShowWindow(list);
        }

        // 현재 에디터 상태 → 시트에 저장(USG의 해당 오브젝트와 같은 이름)
        if (GUILayout.Button("Save To Google Sheet"))
        {
            SaveCurrentToUGS();
        }

        // 오브젝트 이름과 시트 name 필드 일치 시 해당 데이터 불러오기
        if (GUILayout.Button("Load Collider By Object Name"))
        {
            string objName = Object?.name;

            if (string.IsNullOrEmpty(objName))
            {
                Debug.LogWarning("EffectObject가 선택되지 않았습니다.");
                return;
            }

            var dataList = Managers.Data.colliderData.GetByName(objName);

            if (dataList.Count > 0)
            {
                colliders.Clear();
                foreach (var d in dataList)
                {
                    colliders.Add(ConvertToCustom(d));
                }

                Debug.Log($"[ColliderTool] {Object.name}의 콜라이더 {dataList.Count}개 불러옴");
            }
            else
            {
                Debug.LogWarning($"[ColliderTool] {Object.name}에 해당하는 데이터 없음");
            }
        }

        EditorGUILayout.Space();
        // 오브젝트가 선택되지 않은 경우 경고 출력
        if (Object == null)
        {
            EditorGUILayout.HelpBox("이펙트 오브젝트를 먼저 선택하세요.", MessageType.Warning);
            return;
        }

        // 등록된 콜라이더 리스트를 UI로 표시
        for (int i = 0; i < colliders.Count; i++)
        {
            var col = colliders[i];
            EditorGUILayout.BeginVertical("box"); // 박스 스타일 UI 시작
            EditorGUILayout.LabelField($"Collider {i + 1}", EditorStyles.boldLabel); // 콜라이더 인덱스 라벨

            // 콜라이더 타입 선택
            col.type = (ColliderType)EditorGUILayout.EnumPopup("Collider Type", col.type);
            col.center = EditorGUILayout.Vector3Field("Center", col.center); // 중심 좌표 설정

            // 타입별 속성 입력
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

            // 콜라이더 제거 버튼
            if (GUILayout.Button("Remove"))
            {
                colliders.RemoveAt(i);
                break;
            }

            EditorGUILayout.EndVertical(); // 박스 스타일 UI 종료
        }

        // 새 콜라이더 추가 버튼
        if (GUILayout.Button("Add Collider"))
        {
            colliders.Add(new CustomColliderData());
        }

        EditorGUILayout.Space();

        // 저장 버튼 - 설정한 데이터를 실제 콜라이더로 적용
        if (GUILayout.Button("Save"))
        {
            SaveColliders();
        }
    }

    // 설정된 데이터를 바탕으로 실제 콜라이더 컴포넌트 생성
    private void SaveColliders()
    {
        if (Object == null) return;

        // Prefab Asset은 직접 수정 불가
        if (PrefabUtility.IsPartOfPrefabAsset(Object))
        {
            Debug.LogError("오브젝트는 Prefab Asset이 아닌, 씬에 오브젝트여야 합니다.");
            return;
        }

        // 기존 콜라이더 모두 제거
        foreach (var col in Object.GetComponents<Collider>())
        {
            DestroyImmediate(col);
        }

        // 콜라이더 타입에 맞는 컴포넌트 생성 및 속성 설정
        foreach (var data in colliders)
        {
            switch (data.type)
            {
                case ColliderType.Box:
                    var box = Object.AddComponent<BoxCollider>();
                    box.center = data.center;
                    box.size = data.size;
                    break;
                case ColliderType.Sphere:
                    var sphere = Object.AddComponent<SphereCollider>();
                    sphere.center = data.center;
                    sphere.radius = data.radius;
                    break;
                case ColliderType.Capsule:
                    var capsule = Object.AddComponent<CapsuleCollider>();
                    capsule.center = data.center;
                    capsule.radius = data.radius;
                    capsule.height = data.height;
                    break;
            }
        }

        EditorUtility.SetDirty(Object); // 오브젝트 변경 표시
    }

    private Queue<Collider_Data> saveQueue = new Queue<Collider_Data>();

    /// 현재 에디터에 설정된 콜라이더 데이터를 UNIGS(Google Sheets)에 저장 요청하는 메서드
    public void SaveCurrentToUGS()
    {
        // 현재 시트에서 가져온 전체 콜라이더 데이터 리스트
        var list = Collider_Data.GetList();

        // 저장 대기 큐 초기화
        saveQueue.Clear();

        // 에디터에 설정된 모든 콜라이더 데이터를 순회
        foreach (var col in colliders)
        {
            // 동일한 인덱스를 가진 시트 항목 검색
            var existing = list.Find(d => d.index == col.index);
            if (existing != null)
            {
                // 필드 매핑 (에디터 → 시트 구조체)
                existing.ECollidertype = (ECollidertype)col.type;
                existing.centerX = col.center.x;
                existing.centerY = col.center.y;
                existing.centerZ = col.center.z;
                existing.sizeX = col.size.x;
                existing.sizeY = col.size.y;
                existing.sizeZ = col.size.z;
                existing.radius = col.radius;
                existing.height = col.height;

                // 큐에 저장 요청 항목 추가
                saveQueue.Enqueue(existing);
            }
        }

        // 저장 작업 시작 (큐에서 첫 번째 항목 처리)
        SaveNextFromQueue();
    }

    /// 큐에 등록된 데이터를 하나씩 꺼내 Google Sheets에 저장 요청하는 재귀 메서드
    private void SaveNextFromQueue()
    {
        // 큐가 비어 있으면 저장 완료
        if (saveQueue.Count == 0)
        {
            Debug.Log("[UGS] 전체 저장 완료");
            return;
        }

        // 저장할 데이터 하나 꺼내기
        var data = saveQueue.Dequeue();

        // 시트에 저장 요청
        Collider_Data.Write(data, result =>
        {
            // 저장 실패 혹은 반환 값 없음
            if (result == null)
            {
                Debug.LogWarning($"[UGS] 저장 결과 없음: index={data.index}");
            }
            else if (result.hasError())
            {
                Debug.LogError($"[UGS] 저장 실패: index={data.index}, error={result.error.message}");
            }
            else
            {
                // 정상 저장 완료 (업데이트인지 신규 생성인지 확인)
                string state = result.isUpdate ? "업데이트됨" : "신규 생성됨";
                Debug.Log($"[UGS] 저장 완료: index={data.index}, 상태: {state}");
            }

            // 다음 항목 재귀 호출로 저장 계속
            SaveNextFromQueue();
        });
    }



    // UNIGS 구조에서 에디터용 데이터로 변환
    private CustomColliderData ConvertToCustom(Collider_Data data)
    {
        return new CustomColliderData
        {
            index = data.index,
            name = data.name,
            type = (ColliderType)data.ECollidertype,
            center = new Vector3(data.centerX, data.centerY, data.centerZ),
            size = new Vector3(data.sizeX, data.sizeY, data.sizeZ),
            radius = data.radius,
            height = data.height
        };
    }

    private void DrawWireCapsuleHemisphere(Vector3 center, float radius, bool isTop)
    {
        Vector3 direction = isTop ? Vector3.down : Vector3.up;

        // 앞뒤 방향 반원 아크 (Z축 기준 회전 → X-Y 평면에 휘어짐)
        Handles.DrawWireArc(center, Vector3.right, isTop ? Vector3.back : Vector3.forward, 180f, radius);

        // 좌우 방향 반원 아크 (X축 기준 회전 → Y-Z 평면에 휘어짐)
        Handles.DrawWireArc(center, Vector3.forward, isTop ? Vector3.right : Vector3.left, 180f, radius);

        // 수평 디스크 (XY 평면)
        Handles.DrawWireDisc(center, Vector3.up, radius);
    }


    // SceneView에 핸들 표시 및 콜라이더 시각 편집
    private void OnSceneGUI(SceneView sceneView)
    {
        if (Object == null) return;

        Handles.color = new Color(0f, 1f, 0f, 0.4f); // 핸들 색상 설정
        Transform t = Object.transform; // 대상 오브젝트의 Transform

        for (int i = 0; i < colliders.Count; i++)
        {
            var data = colliders[i];
            Vector3 worldPos = t.TransformPoint(data.center); // 로컬 -> 월드 좌표 변환

            // 핸들로 위치/크기 조정
            switch (data.type)
            {
                case ColliderType.Box:
                    data.center = t.InverseTransformPoint(Handles.PositionHandle(worldPos, Quaternion.identity)); // 위치 핸들
                    data.size = Handles.ScaleHandle(data.size, worldPos, Quaternion.identity, 1f); // 크기 핸들
                    Handles.DrawWireCube(worldPos, data.size); // 시각화
                    break;

                case ColliderType.Sphere:
                    data.center = t.InverseTransformPoint(Handles.PositionHandle(worldPos, Quaternion.identity));
                    data.radius = Handles.RadiusHandle(Quaternion.identity, worldPos, data.radius);
                    Handles.DrawWireDisc(worldPos, Vector3.up, data.radius);
                    break;

                case ColliderType.Capsule:
                    data.center = t.InverseTransformPoint(Handles.PositionHandle(worldPos, Quaternion.identity));

                    // 반지름 핸들: ScaleSlider로 대체 (중앙 구체 안보이게)
                    float newRadius = Handles.ScaleSlider(data.radius, worldPos, Vector3.right, Quaternion.identity, 1f, 0.1f);
                    newRadius = Handles.ScaleSlider(newRadius, worldPos, Vector3.forward, Quaternion.identity, 1f, 0.1f);
                    newRadius = Handles.ScaleSlider(newRadius, worldPos, Vector3.up, Quaternion.identity, 1f, 0.1f);
                    data.radius = Mathf.Max(0.01f, newRadius); // 최소값 제한

                    data.height = Mathf.Max(data.height, data.radius * 2f);
                    float halfBodyHeight = (data.height - data.radius * 2f) * 0.5f;
                    Vector3 up = Vector3.up;

                    Vector3 top = worldPos + up * halfBodyHeight;
                    Vector3 bottom = worldPos - up * halfBodyHeight;

                    float topOffset = Handles.ScaleSlider(halfBodyHeight, worldPos, up, Quaternion.identity, 1f, 0.1f);
                    float bottomOffset = Handles.ScaleSlider(halfBodyHeight, worldPos, -up, Quaternion.identity, 1f, 0.1f);
                    data.height = Mathf.Max(0.01f, topOffset + bottomOffset + data.radius * 2f);

                    // 실린더
                    Handles.DrawLine(top + Vector3.forward * data.radius, bottom + Vector3.forward * data.radius);
                    Handles.DrawLine(top - Vector3.forward * data.radius, bottom - Vector3.forward * data.radius);
                    Handles.DrawLine(top + Vector3.right * data.radius, bottom + Vector3.right * data.radius);
                    Handles.DrawLine(top - Vector3.right * data.radius, bottom - Vector3.right * data.radius);

                    DrawWireCapsuleHemisphere(top, data.radius, true);    // 위 반구: 아래로 휘어짐
                    DrawWireCapsuleHemisphere(bottom, data.radius, false); // 아래 반구: 위로 휘어짐
                    break;
            }
        }
    }
}

// 콜라이더 타입 정의용 열거형
public enum ColliderType { Box, Sphere, Capsule }

// 콜라이더 데이터 저장용 클래스
[System.Serializable]
public class CustomColliderData
{
    public int index;
    public string name;
    public ColliderType type = ColliderType.Box; // 기본 타입
    public Vector3 center = Vector3.zero; // 중심 위치
    public Vector3 size = Vector3.one; // 박스 크기
    public float radius = 0.5f; // 구/캡슐 반지름
    public float height = 2f; // 캡슐 높이
}
