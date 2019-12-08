using ECSish;
using System;
using System.Collections.Generic;

public class Session : MonoBehaviourComponentData
{
    public int id;
    public string build;
    public string nickname;
    public SocketClientConnection connection;

    private static Random random = new Random();
    public static List<int> usedIds = new List<int>();
    public static int GetNextId()
    {
        return GetSequentialId();
        //return GetRandomId();
    }

    private static int GetSequentialId()
    {
        for (int i = 0; i <= int.MaxValue; i++)
            if (!usedIds.Contains(i))
            {
                usedIds.Add(i);
                return i;
            }

        throw new Exception("No more Ids");
    }

    private static int GetRandomId()
    {
        for (int i = 0; i <= int.MaxValue; i++)
        {
            var id = random.Next(int.MinValue, int.MaxValue);
            if (!usedIds.Contains(id))
            {
                usedIds.Add(id);
                return id;
            }
        }
        throw new Exception("No more Ids");
    }
}
