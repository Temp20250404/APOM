using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어의 모든 서버 동기화 데이터를 담는 순수 데이터 클래스
public class PlayerData
{
    // 플레이어 고유 ID
    public string PlayerId { get; set; }

    // 플레이어 닉네임
    public string Nickname { get; set; }

    // 최대 체력/마나
    public uint MaxHp { get; set; }
    public uint MaxMp { get; set; }

    // 직업 아이콘
    public uint JonIcon {  get; set; }

    // 기타 서버에서 내려주는 데이터...
}

// 전역에서 PlayerData를 한 곳에서 관리·갱신하는 싱글톤 매니저
public class PlayerDataManager : IManager
{
    public static PlayerDataManager Instance { get; private set; }

    // 실제 게임 로직이 참조할 플레이어 데이터
    private PlayerData _playerData;

    // 서버에서 받아온 JSON, Protobuf 등을 파싱해서 세팅
    public void Initialize(PlayerData data)
    {
        _playerData = data;
    }

    // 현재 데이터를 반환
    public PlayerData GetData()
    {
        return _playerData;
    }

    // 데이터를 로컬 저장소(PlayerPrefs, 파일 등)에 저장하고 싶다면 여기에 구현
    public void SaveToLocal()
    {
        // TODO: JSON 직렬화 후 파일 쓰기 또는 PlayerPrefs 활용
    }

    // 필요 시 서버에 다시 보낼 데이터가 있다면 이 메서드로 처리
    public void SyncToServer()
    {
        // TODO: 서버 API 호출

        // 지금은 사용하지 않음. DB 연동 때 사용 예정
    }

    public void Init()
    {

    }

    public void Clear()
    {

    }
}
