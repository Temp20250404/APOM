using ServerCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using Google.Protobuf;

public class NetworkManager : IManager
{
    ServerSession _session = new ServerSession();
    

    public void Send(IMessage packet)
    {
        _session.Send(packet);
    }

    public void Init()
    {
        int port = 12201;
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Loopback, port);

        Connector connector = new Connector();

        connector.Connect(endPoint,
            () => { return _session; },
            1);
    }

    public void Update()
    {
        List<PacketMessage> list = PacketQueue.Instance.PopAll();
        foreach (PacketMessage packet in list)
        {
            Action<PacketSession, IMessage> handler = Managers.Packet.GetPacketHandler(packet.Id);
            if (handler != null)
                handler.Invoke(_session, packet.Message);
        }
    }


    public void Clear()
    {
    }
}