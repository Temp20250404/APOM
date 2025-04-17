using Game;
using Google.Protobuf;
using ServerCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PacketHandler
{
    // SC_CHAT 패킷을 처리하는 함수
    public static void SC_Chat(PacketSession session, IMessage packet)
    {
        SC_CHAT chatPacket = packet as SC_CHAT;

        // TODO: SC_Chat 패킷 처리 로직을 여기에 구현

        
        //UI_CHAT을 찾아서 ServerByChat 함수를 호출
        //Managers.UI._sceneUI.UI_Chat.ServerByChat(chatPacket.Message, chatPacket.Channel);
        Managers.UI._sceneUI.UI_Chat.ServerByChat(chatPacket.Message, chatPacket.Channel);
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

    // SC_FIND_ID_RESPONSE 패킷을 처리하는 함수
    public static void SC_FindIdResponse(PacketSession session, IMessage packet)
    {
        SC_FIND_ID_RESPONSE findIdResponsePacket = packet as SC_FIND_ID_RESPONSE;

        // TODO: SC_FindIdResponse 패킷 처리 로직을 여기에 구현
    }

    // SC_FIND_PW_RESPONSE 패킷을 처리하는 함수
    public static void SC_FindPwResponse(PacketSession session, IMessage packet)
    {
        SC_FIND_PW_RESPONSE findPwResponsePacket = packet as SC_FIND_PW_RESPONSE;

        // TODO: SC_FindPwResponse 패킷 처리 로직을 여기에 구현
    }

    // SC_LOGIN_RESPONSE 패킷을 처리하는 함수
    public static void SC_LoginResponse(PacketSession session, IMessage packet)
    {
        SC_LOGIN_RESPONSE loginResponsePacket = packet as SC_LOGIN_RESPONSE;

        // TODO: SC_LoginResponse 패킷 처리 로직을 여기에 구현
    }

    // SC_SIGNUP_RESPONSE 패킷을 처리하는 함수
    public static void SC_SignupResponse(PacketSession session, IMessage packet)
    {
        SC_SIGNUP_RESPONSE signupResponsePacket = packet as SC_SIGNUP_RESPONSE;

        // TODO: SC_SignupResponse 패킷 처리 로직을 여기에 구현
    }

    // SC_TRANSFER_CHARACTER_INFO 패킷을 처리하는 함수
    public static void SC_TransferCharacterInfo(PacketSession session, IMessage packet)
    {
        SC_TRANSFER_CHARACTER_INFO transferCharacterInfoPacket = packet as SC_TRANSFER_CHARACTER_INFO;

        // TODO: SC_TransferCharacterInfo 패킷 처리 로직을 여기에 구현
    }

    // SC_CREATE_MONSTER 패킷을 처리하는 함수
    public static void SC_CreateMonster(PacketSession session, IMessage packet)
    {
        SC_CREATE_MONSTER createMonsterPacket = packet as SC_CREATE_MONSTER;

        // TODO: SC_CreateMonster 패킷 처리 로직을 여기에 구현

        Managers.BossManager.SpawnBoss(createMonsterPacket);
    }

    // SC_PLAYER_ATTACK 패킷을 처리하는 함수
    public static void SC_PlayerAttack(PacketSession session, IMessage packet)
    {
        SC_PLAYER_ATTACK playerAttackPacket = packet as SC_PLAYER_ATTACK;

        // TODO: SC_PlayerAttack 패킷 처리 로직을 여기에 구현
    }

    // SC_PLAYER_DAMAGED 패킷을 처리하는 함수
    public static void SC_PlayerDamaged(PacketSession session, IMessage packet)
    {
        SC_PLAYER_DAMAGED playerDamagedPacket = packet as SC_PLAYER_DAMAGED;

        // TODO: SC_PlayerDamaged 패킷 처리 로직을 여기에 구현
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

    // SC_BOSS_PHASE 패킷을 처리하는 함수
    public static void SC_BossPhase(PacketSession session, IMessage packet)
    {
        SC_BOSS_PHASE bossPhasePacket = packet as SC_BOSS_PHASE;

        // TODO: SC_BossPhase 패킷 처리 로직을 여기에 구현

        BossState state = (BossState)bossPhasePacket.BossState;
        Boss boss = Managers.BossManager.GetBoss(bossPhasePacket.BossID);
        Vector3 target = new Vector3(bossPhasePacket.BossPos.PosX, bossPhasePacket.BossPos.PosY, bossPhasePacket.BossPos.PosZ);

        //switch(state)
        //{
        //    case BossState.Idle:
        //        break;
        //    case BossState.Walk:
        //        break;
        //    case BossState.Chase:
        //        break;
        //    case BossState.Attack:
        //        break;
        //    case BossState.Skill1:
        //        break;
        //    case BossState.Skill2:
        //        break;
        //    case BossState.Skill3:
        //        break;
        //    case BossState.Die:
        //        break;
        //    default:
        //        break;
        //}
        if (boss != null)
        {
            boss.StateMachine.ChangeState(state);
            boss.bossAI.MoveSpeed(bossPhasePacket.CurSpeed);
            boss.bossAI.ChaseTarget(target);
        }
    }
}
