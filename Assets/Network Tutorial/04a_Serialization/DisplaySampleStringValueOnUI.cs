using ECSish;

public class DisplaySampleStringValueOnUI : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var personEntity in GetEntities<SampleSerializedPersonObject, SampleDeserializeTag>())
            foreach (var uiEntity in GetEntities<SampleStringValueUI, UIText>())
                uiEntity.Item2.textMesh.text = personEntity.Item1.serializedValue;
    }
}