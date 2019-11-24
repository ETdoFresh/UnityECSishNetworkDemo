using ECSish;

public class DisplaySamplePersonObjectOnUI : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var personEntity in GetEntities<SamplePersonObject, SampleSerializeTag>())
            foreach (var uiEntity in GetEntities<SamplePersonObjectUI, UIText>())
                uiEntity.Item2.textMesh.text = $"Name: {personEntity.Item1.name}\n" +
                    $"Gender: {personEntity.Item1.gender}\n" +
                    $"Age: {personEntity.Item1.age}";
    }
}
