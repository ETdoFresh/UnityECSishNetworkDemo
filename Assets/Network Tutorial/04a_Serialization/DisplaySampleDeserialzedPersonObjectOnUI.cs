using ECSish;

public class DisplaySampleDeserialzedPersonObjectOnUI : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var personEntity in GetEntities<SamplePersonObject, SampleDeserializeTag>())
            foreach (var uiEntity in GetEntities<SampleDeserialzedPersonObjectUI, UIText>())
                uiEntity.Item2.textMesh.text = $"Name: {personEntity.Item1.name}\n" +
                    $"Gender: {personEntity.Item1.gender}\n" +
                    $"Age: {personEntity.Item1.age}";
    }
}