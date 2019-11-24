using ECSish;
using System;

public class DeserializePersonObject : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<SamplePersonObject, SampleSerializedPersonObject, SampleDeserializeTag>())
        {
            var person = entity.Item1;
            var serialized = entity.Item2;

            var delimiter = "|";
            var values = serialized.serializedValue.Split(new[] { delimiter }, StringSplitOptions.None);
            var fields = person.GetType().GetFields();

            try
            {
                for (int i = 0; i < fields.Length; i++)
                    if (fields[i].FieldType.Equals(typeof(int)))
                        fields[i].SetValue(person, Convert.ToInt32(values[i]));
                    else
                        fields[i].SetValue(person, values[i]);
            }
            catch
            {
                foreach(var field in fields)
                    field.SetValue(person, field.FieldType.GetDefaultValue());
            }
        }
    }
}