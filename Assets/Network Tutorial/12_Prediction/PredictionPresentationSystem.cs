using ECSish;
using UnityEngine;

public class PredictionPresentationSystem : MonoBehaviourSystem
{
    private void Update()
    {
        foreach(var entity in GetEntities<ClientPrediction, Movement>())
        {
            var prediction = entity.Item1.transform;
            
            var force = entity.Item2.force; // Do mass calculation if rigidbody exists
            //var positionChange = currentVelocity * Time.deltaTime + 0.5f * force / mass * Time.deltaTime * Time.deltaTime;
            var estimatedPositionChange = force * Time.deltaTime * 2;
            var closeSnapDistance = 0.25f;
            var farSnapDistance = 5;

            var presentation = prediction.GetComponentInChildren<ClientPresentation>();
            if (presentation)
            {
                var presentationTransform = presentation.transform;
                var distance = Vector3.Distance(prediction.position, presentation.previousPosition);
                if (distance < closeSnapDistance || distance > farSnapDistance)
                    presentationTransform.position = prediction.position;
                else
                    presentationTransform.position = Vector3.Lerp(presentation.previousPosition, prediction.position, estimatedPositionChange);

                distance = Vector3.Distance(prediction.rotation.eulerAngles, presentation.previousRotation.eulerAngles);
                if (distance < closeSnapDistance || distance > farSnapDistance)
                    presentationTransform.rotation = prediction.rotation;
                else
                    presentationTransform.rotation = Quaternion.Slerp(presentation.previousRotation, prediction.rotation, estimatedPositionChange);

                distance = Vector3.Distance(prediction.localScale, presentation.previousLocalScale);
                if (distance < closeSnapDistance || distance > farSnapDistance)
                    presentationTransform.localScale = prediction.localScale;
                else
                    presentationTransform.localScale = Vector3.Lerp(presentation.previousLocalScale, prediction.localScale, estimatedPositionChange);

                presentation.previousPosition = presentationTransform.position;
                presentation.previousRotation = presentationTransform.rotation;
                presentation.previousLocalScale = presentationTransform.localScale;
            }
        }
    }

    private void FixedUpdate()
    {
        foreach(var splitScreenInputEntity in GetEntities<SplitScreenInput>())
        foreach (var entity in GetEntities<ClientPrediction, Movement, RigidbodyComponent>())
        {
            var splitScreenInput = splitScreenInputEntity.Item1;
            var movement = entity.Item2;
            var rigidbody = entity.Item3.rigidbody;

            var forward = Camera.main.transform.forward;
            var right = Camera.main.transform.right;
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            var input = forward * splitScreenInput.vertical;
            input += right * splitScreenInput.horizontal;
            var jump = splitScreenInput.jumpPressed;

            rigidbody.AddForce(input * movement.force);
            if (jump)
                rigidbody.AddForce(Vector3.up * movement.force, ForceMode.Impulse);

            // TODO: Figure out how to ignore jump again.. for now, just unset it...
            splitScreenInput.jumpPressed = false;
        }
    }
}
