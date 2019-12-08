using ECSish;
using System.Linq;

public class MonitorUsedIds : MonoBehaviourSystem
{
    private void Update()
    {
        var sessions = GetAllEntities<Session>().Select(e => e.Item1);
        foreach(var session in sessions)
            if (!Session.usedIds.Contains(session.id))
                Session.usedIds.Add(session.id);

        for (int i = Session.usedIds.Count - 1; i >= 0; i--)
            if (sessions.Where(s => s.id == Session.usedIds[i]).Count() == 0)
                Session.usedIds.RemoveAt(i);
    }
}