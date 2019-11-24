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
                    input.jumpPress = Input.GetButtonDown("Jump");
                    input.jumpHold = Input.GetButton("Jump");
                    input.jumpRelease = Input.GetButtonUp("Jump");
                }
                else
                {
                    input.horizontal = 0;
                    input.vertical = 0;
                    input.jumpPress = false;
                    input.jumpHold = false;
                    input.jumpRelease = false;
                }
            }
        }
    }
}
