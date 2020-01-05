using ECSish;

public class AddRTTToSessions : MonoBehaviourSystem
{
    private void Update()
    {
        foreach(var entity in GetEntities<Session>())
        {
            var session = entity.Item1;
            var rtt = session.GetComponent<RoundTripTime>();
            if (!rtt) session.gameObject.AddComponent<RoundTripTime>();
        }
    }
}
