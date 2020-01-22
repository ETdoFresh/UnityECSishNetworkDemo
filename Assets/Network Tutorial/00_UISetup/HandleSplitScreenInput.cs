using ECSish;
using UnityEngine;

public class HandleSplitScreenInput : MonoBehaviourSystem
{
    private void Update()
    {
        foreach(var entity in GetEntities<SplitScreenPanel>())
        {
            var isFocused = entity.Item1.isFocused;

            foreach (var splitScreenInputEntity in GetEntities<SplitScreenInput>())
            {
                var input = splitScreenInputEntity.Item1;
                if (isFocused)
                {
                    input.horizontal = Input.GetAxisRaw("Horizontal");
                    input.vertical = Input.GetAxisRaw("Vertical");
                    input.jumpPressed = Input.GetButtonDown("Jump");
                    input.jumpHold = Input.GetButton("Jump");
                    input.jumpRelease = Input.GetButtonUp("Jump");

                    if (input.jumpPressed)
                    {
                        foreach (var jumpQueue in GetEntities<ClientJumpQueue>())
                            jumpQueue.Item1.jumpPresses.Enqueue(true);
                        foreach (var jumpQueue in GetEntities<ClientPrediction>())
                            jumpQueue.Item1.jumpPresses.Enqueue(true);
                    }
                }
                else
                {
                    input.horizontal = 0;
                    input.vertical = 0;
                    input.jumpPressed = false;
                    input.jumpHold = false;
                    input.jumpRelease = false;
                }
            }
        }
    }
}
