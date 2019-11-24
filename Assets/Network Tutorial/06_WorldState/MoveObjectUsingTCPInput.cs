using ECSish;
using UnityEngine;

public class MoveObjectUsingTCPInput : MonoBehaviourSystem
{
    private void FixedUpdate()
    {
        foreach(var entity in GetEntities<TCPReceivedInput, Movement, RigidbodyComponent>())
        {
            var tcpInput = entity.Item1;
            var movement = entity.Item2;
            var rigidbody = entity.Item3.rigidbody;

            var forward = Camera.main.transform.forward;
            var right = Camera.main.transform.right;
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            var input = forward * tcpInput.vertical;
            input += right * tcpInput.horizontal;
            var jump = tcpInput.jumpPressed;

            rigidbody.AddForce(input * movement.force);
            if (jump)
                rigidbody.AddForce(Vector3.up * movement.force, ForceMode.Impulse);

            // TODO: Figure out how to ignore jump again.. for now, just unset it...
            tcpInput.jumpPressed = false;
        }
    }
}
