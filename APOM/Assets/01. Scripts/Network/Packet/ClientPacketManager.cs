using Game;
using Google.Protobuf;
using ServerCore;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PacketManager : IManager
{
    Dictionary<ushort, Action<PacketSession, ArraySegment<byte>, ushort>> _onRecv = new Dictionary<ushort, Action<PacketSession, ArraySegment<byte>, ushort>>();
    Dictionary<ushort, Action<PacketSession, IMessage>> _handler = new Dictionary<ushort, Action<PacketSession, IMessage>>();

    public Action<PacketSession, IMessage, ushort> CustomHandler { get; set; }

    public void Init()
    {
        Register();
    }

    public void Clear()
    {
    }

    // 패킷 수신 및 핸들러를 등록하는 메서드입니다.
    public void Register()
    {
        _onRecv.Add((ushort)Game.PacketID.ScLoginResponse, MakePacket<SC_LOGIN_RESPONSE>);
        _handler.Add((ushort)Game.PacketID.ScLoginResponse, PacketHandler.SC_LoginResponse);

        _onRecv.Add((ushort)Game.PacketID.ScRegisterResponse, MakePacket<SC_REGISTER_RESPONSE>);
        _handler.Add((ushort)Game.PacketID.ScRegisterResponse, PacketHandler.SC_RegisterResponse);

        _onRecv.Add((ushort)Game.PacketID.ScChat, MakePacket<SC_CHAT>);
        _handler.Add((ushort)Game.PacketID.ScChat, PacketHandler.SC_Chat);

        _onRecv.Add((ushort)Game.PacketID.ScKeyinfo, MakePacket<SC_KEYINFO>);
        _handler.Add((ushort)Game.PacketID.ScKeyinfo, PacketHandler.SC_Keyinfo);

        _onRecv.Add((ushort)Game.PacketID.ScPositionSync, MakePacket<SC_POSITION_SYNC>);
        _handler.Add((ushort)Game.PacketID.ScPositionSync, PacketHandler.SC_PositionSync);

        _onRecv.Add((ushort)Game.PacketID.ScRemoveCharacter, MakePacket<SC_REMOVE_CHARACTER>);
        _handler.Add((ushort)Game.PacketID.ScRemoveCharacter, PacketHandler.SC_RemoveCharacter);

        _onRecv.Add((ushort)Game.PacketID.ScSpawnCharacter, MakePacket<SC_SPAWN_CHARACTER>);
        _handler.Add((ushort)Game.PacketID.ScSpawnCharacter, PacketHandler.SC_SpawnCharacter);

        _onRecv.Add((ushort)Game.PacketID.ScBossPhase, MakePacket<SC_BOSS_PHASE>);
        _handler.Add((ushort)Game.PacketID.ScBossPhase, PacketHandler.SC_BossPhase);
    } 

    public void OnRecvPacket(PacketSession session, ushort id, ArraySegment<byte> buffer)
    {
        Action<PacketSession, ArraySegment<byte>, ushort> action = null;
         if (_onRecv.TryGetValue(id, out action))
                action.Invoke(session, buffer, id);
    }

    void MakePacket<T>(PacketSession session, ArraySegment<byte> buffer, ushort id) where T : IMessage, new()
    {
        T pkt = new T();

        // Protobuf 메시지를 buffer 전체에서 파싱
        pkt.MergeFrom(buffer.Array, buffer.Offset, buffer.Count);
        //Debug.Log($"PacketHandler 호출: {id}");

        if (CustomHandler != null)
        {
            CustomHandler.Invoke(session, pkt, id);
        }
        else
        {
            Action<PacketSession, IMessage> action = null;

            if (_handler.TryGetValue(id, out action))
            {
                JobQueue.Push(() => action.Invoke(session, pkt));

            }
        }
    }

    public Action<PacketSession, IMessage> GetPacketHandler(ushort id)
    {
        Action<PacketSession, IMessage> action = null;
        if (_handler.TryGetValue(id, out action))
            return action;
        return null;
    }
}