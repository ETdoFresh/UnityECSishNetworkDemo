using ECSish;
using System;
using System.Collections.Generic;

public class InputBuffer : MonoBehaviourComponentData
{
    public List<InputState> inputs = new List<InputState>();

    public void AddNewInput(int tick, float horizontal, float vertical, bool jumpPress)
    {
        inputs.RemoveAll(i => i.tick == tick);
        inputs.Add(new InputState { tick = tick, horizontal = horizontal, vertical = vertical, jumpPressed = jumpPress });
    }

    public void RemoveTimesOnOrBeforeTick(int tick)
    {
        inputs.RemoveAll(i => i.tick <= tick);
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
