using ECSish;
using System.Linq;

public class KeepOnlyOneDirectionalLight : MonoBehaviourSystem
{
    private void Update()
    {
        var entities = GetAllEntities<OneDirectionalLight>();
        for (int i = 1; i < entities.Count(); i++)
            entities.ElementAt(i).Item1.gameObject.SetActive(false);
    }
}
