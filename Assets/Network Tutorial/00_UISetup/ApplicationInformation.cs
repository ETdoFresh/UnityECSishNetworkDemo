using ECSish;

public class ApplicationInformation : MonoBehaviourComponentData
{
    public float fps;
    public string platform;
    public string version;
    public string companyName;
    public string productName;
    public string unityVersion;
    public int targetFrameRate;
    public int vSyncCount = -1;
}
