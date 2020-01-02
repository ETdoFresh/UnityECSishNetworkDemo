using ECSish;
using System.Linq;

public class SetSplitScreenClientNumber : MonoBehaviourSystem
{
    private void Update()
    {
        var entities = GetAllEntities<SplitScreenClientNumber>();
        foreach (var entity in entities)
        {
            var e = entity.Item1;
            if (entities.Count() == 1)
                e.splitScreenClientNumber = 0;

            else if (entities.Count() == 2)
                if (entities.ElementAt(0).Item1 == entity.Item1)
                    e.splitScreenClientNumber = 0;
                else
                    e.splitScreenClientNumber = 1;

            else if (entities.Count() == 3)
                if (entities.ElementAt(0).Item1 == entity.Item1)
                    e.splitScreenClientNumber = 0;
                else if (entities.ElementAt(1).Item1 == entity.Item1)
                    e.splitScreenClientNumber = 1;
                else
                    e.splitScreenClientNumber = 2;
        }
    }
}
