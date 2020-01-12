using ECSish;
using UnityEngine;

public class PredictionPresentationSystem : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<ClientPrediction, Movement>())
        {
            var prediction = entity.Item1.transform;
            var tweenSpeed = entity.Item1.tweenSpeed;
            var closeSnapDistance = entity.Item1.closeSnapDistance;
            var farSnapDistance = entity.Item1.farSnapDistance;

            var presentation = prediction.GetComponentInChildren<ClientPresentation>();
            if (presentation)
            {
                var presentationTransform = presentation.transform;
                var distance = Vector3.Distance(prediction.position, presentation.previousPosition);
                if (distance < closeSnapDistance || distance > farSnapDistance)
                    presentationTransform.position = prediction.position;
                else
                    presentationTransform.position = Vector3.Lerp(presentation.previousPosition, prediction.position, tweenSpeed);

                distance = Vector3.Distance(prediction.rotation.eulerAngles, presentation.previousRotation.eulerAngles);
                if (distance < closeSnapDistance || distance > farSnapDistance)
                    presentationTransform.rotation = prediction.rotation;
                else
                    presentationTransform.rotation = Quaternion.Slerp(presentation.previousRotation, prediction.rotation, tweenSpeed);

                distance = Vector3.Distance(prediction.localScale, presentation.previousLocalScale);
                if (distance < closeSnapDistance || distance > farSnapDistance)
                    presentationTransform.localScale = prediction.localScale;
                else
                    presentationTransform.localScale = Vector3.Lerp(presentation.previousLocalScale, prediction.localScale, tweenSpeed);

                presentation.previousPosition = presentationTransform.position;
                presentation.previousRotation = presentationTransform.rotation;
                presentation.previousLocalScale = presentationTransform.localScale;
            }
        }

        foreach (var entity in GetEntities<ClientPrediction>())
            if (entity.Item1.nextUpdate == 0) entity.Item1.nextUpdate = Time.time;

        foreach (var sessionEntity in GetEntities<Session>())
            foreach (var splitScreenInputEntity in GetEntities<SplitScreenInput>())
                foreach (var entity in GetEntities<ClientPrediction, Movement, RigidbodyComponent>())
                {
                    var sessionId = sessionEntity.Item1.id;
                    var splitScreenInput = splitScreenInputEntity.Item1;
                    var clientPrediction = entity.Item1;
                    var movement = entity.Item2;
                    var rigidbody = entity.Item3.rigidbody;

                    if (clientPrediction.sessionId != sessionId)
                        continue;

                    while (Time.time >= clientPrediction.nextUpdate)
                    {
                        clientPrediction.nextUpdate += Time.fixedDeltaTime;
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
}
