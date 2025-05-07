using Game;
using Google.Protobuf;
using ServerCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

class PacketHandler
{
    // SC_CHAT 패킷을 처리하는 함수
    public static void SC_Chat(PacketSession session, IMessage packet)
    {
        SC_CHAT chatPacket = packet as SC_CHAT;

        // TODO: SC_Chat 패킷 처리 로직을 여기에 구현


        //UI_CHAT을 찾아서 ServerByChat 함수를 호출
        //Managers.UI._sceneUI.UI_Chat.ServerByChat(chatPacket.Message, chatPacket.Channel);
        (Managers.UI._sceneUI as IChat)?.ReceiveChat(chatPacket.Message, chatPacket.Channel);
    }

    // SC_KEYINFO 패킷을 처리하는 함수
    public static void SC_Keyinfo(PacketSession session, IMessage packet)
    {
        SC_KEYINFO keyinfoPacket = packet as SC_KEYINFO;

        // TODO: SC_Keyinfo 패킷 처리 로직을 여기에 구현


        Player player = Managers.Player.GetPlayer(keyinfoPacket.PlayerID);
        player.playerID = keyinfoPacket.PlayerID;
        player.inputController.ReciveKeyInfoPacket(keyinfoPacket.KeyInfo, keyinfoPacket.CameraYaw);
    }

    // SC_MONSTER_DAMAGED 패킷을 처리하는 함수
    public static void SC_MonsterDamaged(PacketSession session, IMessage packet)
    {
        SC_MONSTER_DAMAGED monsterDamagedPacket = packet as SC_MONSTER_DAMAGED;

        // TODO: SC_MonsterDamaged 패킷 처리 로직을 여기에 구현
    }

    // SC_MONSTER_DIE 패킷을 처리하는 함수
    public static void SC_MonsterDie(PacketSession session, IMessage packet)
    {
        SC_MONSTER_DIE monsterDiePacket = packet as SC_MONSTER_DIE;

        // TODO: SC_MonsterDie 패킷 처리 로직을 여기에 구현
    }

    // SC_FIND_ID_RESPONSE 패킷을 처리하는 함수
    public static void SC_FindIdResponse(PacketSession session, IMessage packet)
    {
        SC_FIND_ID_RESPONSE findIdResponsePacket = packet as SC_FIND_ID_RESPONSE;

        // TODO: SC_FindIdResponse 패킷 처리 로직을 여기에 구현

        UI_IDFind uI_IDFind = Managers.UI._popupStack.OfType<UI_IDFind>().FirstOrDefault();

        if (findIdResponsePacket.Success)
        {
            if (uI_IDFind != null)
            {
                uI_IDFind.findResultText.text = $"ID: {findIdResponsePacket.Id}";
            }
        }
        else
        {
            if (uI_IDFind != null)
            {
                uI_IDFind.findResultText.text = "등록된 ID가 없습니다.";
            }
        }
    }

    // SC_FIND_PW_RESPONSE 패킷을 처리하는 함수
    public static void SC_FindPwResponse(PacketSession session, IMessage packet)
    {
        SC_FIND_PW_RESPONSE findPwResponsePacket = packet as SC_FIND_PW_RESPONSE;

        // TODO: SC_FindPwResponse 패킷 처리 로직을 여기에 구현

        UI_PWFind uI_PWFind = Managers.UI._popupStack.OfType<UI_PWFind>().FirstOrDefault();
        if (findPwResponsePacket.Success)
        {
            if (uI_PWFind != null)
            {
                uI_PWFind.findResultText.text = $"PW: {findPwResponsePacket.Pw}";
            }
        }
        else
        {
            if (uI_PWFind != null)
            {
                uI_PWFind.findResultText.text = "등록된 ID가 없습니다. \nID와 Email을 확인해주세요";
            }
        }
    }

    // SC_LOGIN_RESPONSE 패킷을 처리하는 함수
    public static void SC_LoginResponse(PacketSession session, IMessage packet)
    {
        SC_LOGIN_RESPONSE loginResponsePacket = packet as SC_LOGIN_RESPONSE;

        // TODO: SC_LoginResponse 패킷 처리 로직을 여기에 구현\

        UI_Login uI_Login = Managers.UI._popupStack.OfType<UI_Login>().FirstOrDefault();

        if (loginResponsePacket.Success)
        {
            if (uI_Login != null)
            {
                uI_Login.loginResultText.text = "로그인 성공";
                SceneManager.LoadScene("HMJScene");
            }
        }
        else
        {
            if (uI_Login != null)
            {
                if(loginResponsePacket.ErrorCode == 1)
                {
                    uI_Login.loginResultText.text = "ID가 존재하지 않음";
                }
                else if (loginResponsePacket.ErrorCode == 2)
                {
                    uI_Login.loginResultText.text = "비밀번호 불일치";
                }
            }
        }
    }

    // SC_REQUEST_ITEM_INFO 패킷을 처리하는 함수
    public static void SC_RequestItemInfo(PacketSession session, IMessage packet)
    {
        SC_REQUEST_ITEM_INFO requestItemInfoPacket = packet as SC_REQUEST_ITEM_INFO;

        // TODO: SC_RequestItemInfo 패킷 처리 로직을 여기에 구현
    }

    // SC_RESPONSE_CHARACTER_INFO 패킷을 처리하는 함수
    public static void SC_ResponseCharacterInfo(PacketSession session, IMessage packet)
    {
        SC_RESPONSE_CHARACTER_INFO responseCharacterInfoPacket = packet as SC_RESPONSE_CHARACTER_INFO;

        // TODO: SC_ResponseCharacterInfo 패킷 처리 로직을 여기에 구현
    }

    // SC_SIGNUP_RESPONSE 패킷을 처리하는 함수
    public static void SC_SignupResponse(PacketSession session, IMessage packet)
    {
        SC_SIGNUP_RESPONSE signupResponsePacket = packet as SC_SIGNUP_RESPONSE;

        // TODO: SC_SignupResponse 패킷 처리 로직을 여기에 구현

        UI_SignUp uI_SignUp = Managers.UI._popupStack.OfType<UI_SignUp>().FirstOrDefault();

        if (signupResponsePacket.Success)
        {
            if(uI_SignUp != null)
            {
                uI_SignUp.signUpResultText.text = "회원가입 성공";
            }
        }
        else
        {
            if (uI_SignUp != null)
            {
                uI_SignUp.signUpResultText.text = signupResponsePacket.ErrorCode.ToString();
            }
        }
    }

    // SC_CREATE_MONSTER 패킷을 처리하는 함수
    public static void SC_CreateMonster(PacketSession session, IMessage packet)
    {
        SC_CREATE_MONSTER createMonsterPacket = packet as SC_CREATE_MONSTER;

        // TODO: SC_CreateMonster 패킷 처리 로직을 여기에 구현

        Managers.BossManager.SpawnBoss(createMonsterPacket);
    }

    // SC_PLAYER_DAMAGED 패킷을 처리하는 함수
    public static void SC_PlayerDamaged(PacketSession session, IMessage packet)
    {
        SC_PLAYER_DAMAGED playerDamagedPacket = packet as SC_PLAYER_DAMAGED;

        // TODO: SC_PlayerDamaged 패킷 처리 로직을 여기에 구현
        Managers.Player.GetPlayer(playerDamagedPacket.PlayerID).Stat.TakeDamage(playerDamagedPacket.Damage);
    }

    // SC_PLAYER_DIE 패킷을 처리하는 함수
    public static void SC_PlayerDie(PacketSession session, IMessage packet)
    {
        SC_PLAYER_DIE playerDiePacket = packet as SC_PLAYER_DIE;

        // TODO: SC_PlayerDie 패킷 처리 로직을 여기에 구현

    }

    // SC_POSITION_SYNC 패킷을 처리하는 함수
    public static void SC_PositionSync(PacketSession session, IMessage packet)
    {
        SC_POSITION_SYNC positionSyncPacket = packet as SC_POSITION_SYNC;

        // TODO: SC_PositionSync 패킷 처리 로직을 여기에 구현

        Player player = Managers.Player.GetPlayer(positionSyncPacket.PlayerID);
        player.inputController.ReciveTransformSyncPosition(positionSyncPacket);
        player.inputController.ReciveTransformSyncRotation(positionSyncPacket);
    }

    // SC_REMOVE_CHARACTER 패킷을 처리하는 함수
    public static void SC_RemoveCharacter(PacketSession session, IMessage packet)
    {
        SC_REMOVE_CHARACTER removeCharacterPacket = packet as SC_REMOVE_CHARACTER;

        // TODO: SC_RemoveCharacter 패킷 처리 로직을 여기에 구현
        Managers.Player.RemovePlayer(removeCharacterPacket.PlayerID);
    }

    // SC_SPAWN_CHARACTER 패킷을 처리하는 함수
    public static void SC_SpawnCharacter(PacketSession session, IMessage packet)
    {
        SC_SPAWN_CHARACTER spawnCharacterPacket = packet as SC_SPAWN_CHARACTER;

        // TODO: SC_SpawnCharacter 패킷 처리 로직을 여기에 구현
        Managers.Player.SpawnPlayer(spawnCharacterPacket);
    }
    
    // SC_START_AI_CALCULATE 패킷을 처리하는 함수
    public static void SC_StartAiCalculate(PacketSession session, IMessage packet)
    {
        SC_START_AI_CALCULATE startAiCalculatePacket = packet as SC_START_AI_CALCULATE;

        // TODO: SC_StartAiCalculate 패킷 처리 로직을 여기에 구현
    }

    // SC_STOP_AI_CALCULATE 패킷을 처리하는 함수
    public static void SC_StopAiCalculate(PacketSession session, IMessage packet)
    {
        SC_STOP_AI_CALCULATE stopAiCalculatePacket = packet as SC_STOP_AI_CALCULATE;

        // TODO: SC_StopAiCalculate 패킷 처리 로직을 여기에 구현
    }

    // SC_BOSS_PHASE 패킷을 처리하는 함수
    public static void SC_BossPhase(PacketSession session, IMessage packet)
    {
        SC_BOSS_PHASE bossPhasePacket = packet as SC_BOSS_PHASE;

        // TODO: SC_BossPhase 패킷 처리 로직을 여기에 구현

        BossState state = (BossState)bossPhasePacket.BossState;
        Boss boss = Managers.BossManager.GetBoss(bossPhasePacket.BossID);
        Vector3 bossPos = new Vector3(bossPhasePacket.BossPos.PosX, bossPhasePacket.BossPos.PosY, bossPhasePacket.BossPos.PosZ);
        Vector3 target = new Vector3(bossPhasePacket.TargetMovementPos.PosX, bossPhasePacket.TargetMovementPos.PosY, bossPhasePacket.TargetMovementPos.PosZ);

        Debug.Log($"보스 {bossPhasePacket.BossID} 상태: {state}");
        if (boss != null)
        {
            // 스킬이 실행 중일 경우, 패킷을 처리하지 않음
            if (boss.bossAI.isSkillActive)
            {
                Debug.Log("스킬이 실행 중이므로 다른 패킷을 처리하지 않습니다.");
                return;
            }

            boss.StateMachine.ChangeState(state);
            boss.bossAI.SCChaseTarget(target);
            boss.bossAI.SCMoveSpeed(bossPhasePacket.CurSpeed);

            // 스킬 실행을 위한 플래그 처리 (애니메이션 완료 후 리셋)
            if (state == BossState.Skill1 || state == BossState.Skill2 || state == BossState.Skill3)
            {
                boss.bossAI.isSkillActive = true;
            }
        }
    }

    // SC_TEST_PACKET 패킷을 처리하는 함수
    public static void SC_TestPacket(PacketSession session, IMessage packet)
    {
        SC_TEST_PACKET testPacketPacket = packet as SC_TEST_PACKET;
        
        // TODO: SC_TestPacket 패킷 처리 로직을 여기에 구현
    }
}
