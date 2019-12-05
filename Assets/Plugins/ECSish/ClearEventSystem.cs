﻿namespace ECSish
{
    public class ClearEventSystem : MonoBehaviourSystem
    {
        private void Update()
        {
            EventSystem.ClearEvents();

            foreach (var entity in GetAllEntities<EntityDestroyed>())
                if(entity.Item1 && entity.Item1.gameObject)
                    Destroy(entity.Item1.gameObject);
        }
    }
}
