using ECSish;
using System;
using System.Collections.Generic;
using System.Linq;

public class InputBuffer : MonoBehaviourComponentData
{
    public List<InputState> inputs = new List<InputState>();

    public void AddNewInput(int tick, float horizontal, float vertical, bool jumpPress)
    {
        if (!inputs.Where(i => i.tick == tick).Any())
            inputs.Add(new InputState { tick = tick, horizontal = horizontal, vertical = vertical, jumpPressed = jumpPress });
    }

    public void RemoveTimesOnOrBeforeTick(int tick)
    {
        for (int i = inputs.Count - 1; i >= 0; i--)
            if (inputs[i].tick <= tick)
                inputs.RemoveAt(i);
    }

    [Serializable]
    public class InputState
    {
        public int tick;
        public float horizontal;
        public float vertical;
        public bool jumpPressed;
    }
}
