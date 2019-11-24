using ECSish;

public class SerializePersonObject : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<SamplePersonObject, SampleSerializedPersonObject, SampleSerializeTag>())
        {
            var person = entity.Item1;
            var serialized = entity.Item2;

            var delimiter = "|";
            serialized.serializedValue = "";
            foreach (var field in person.GetType().GetFields())
                if (serialized.serializedValue == "")
                    serialized.serializedValue = field.GetValue(person).ToString();
                else
                    serialized.serializedValue += delimiter + field.GetValue(person).ToString();
        }
    }
}
