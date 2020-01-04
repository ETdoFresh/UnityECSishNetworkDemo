using ECSish;

public class EnqueueJumpQueue : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<SplitScreenInput>())
        {
            var input = entity.Item1;
            if (input.jumpPress)
                foreach (var jumpQueueEntity in GetEntities<JumpQueue>())
                    jumpQueueEntity.Item1.jumpQueue.Enqueue(true);
        }
    }
}
