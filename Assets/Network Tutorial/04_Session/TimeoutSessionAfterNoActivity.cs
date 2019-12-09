using ECSish;
using System.Linq;
using UnityEngine;

public class TimeoutSessionAfterNoActivity : MonoBehaviourSystem
{
    private void Update()
    {
        var sessions = GetEntities<Session>().Select(e => e.Item1);

        foreach(var session in sessions)
        {
            if (Time.time > session.lastReceived + session.timeout)
            {
                var connection = session.connection;
                Entity.Destroy(session.gameObject);
            }
        }
    }
}