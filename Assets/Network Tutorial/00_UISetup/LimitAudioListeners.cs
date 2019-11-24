using ECSish;
using System.Linq;

public class LimitAudioListeners : MonoBehaviourSystem
{
    private void Update()
    {
        var entities = GetEntities<AudioListenerComponent>().Where(e => e.Item1.audioListener.enabled);
        for (int i = 1; i < entities.Count(); i++)
            entities.ElementAt(i).Item1.audioListener.enabled = false;
    }
}