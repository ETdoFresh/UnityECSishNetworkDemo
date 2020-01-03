﻿using ECSish;

public class AddClientTickAndRTTToSessions : MonoBehaviourSystem
{
    private void Update()
    {
        foreach(var entity in GetEntities<Session>())
        {
            var session = entity.Item1;
            
            var clientTick = session.GetComponent<ClientTick>();
            if (!clientTick) Entity.Add(session.gameObject.AddComponent<ClientTick>());

            var rtt = session.GetComponent<RoundTripTime>();
            if (!rtt) Entity.Add(session.gameObject.AddComponent<RoundTripTime>());
        }
    }
}
