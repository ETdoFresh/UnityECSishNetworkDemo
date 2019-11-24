using ECSish;

public class DisplaySampleSerializedPersonObjectOnUI : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var personEntity in GetEntities<SampleSerializedPersonObject, SampleSerializeTag>())
            foreach (var uiEntity in GetEntities<SampleSerialzedPersonObjectUI, UIText>())
                uiEntity.Item2.textMesh.text = personEntity.Item1.serializedValue;
    }
}