using ECSish;

public class GlobalSystem : MonoBehaviourSystem
{
    public static GlobalSystem singleton;

    private void Awake()
    {
        if (singleton)
        {
            DestroyImmediate(gameObject);
            return;
        }
        else
        {
            singleton = this;
        }
    }
}
