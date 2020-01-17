using ECSish;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PredictionPresentationSystem : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var sessionEntity in GetEntities<Session>())
            foreach (var entity in GetEntities<ClientPrediction, Movement>())
            {
                var sessionId = sessionEntity.Item1.id;
                var clientPrediction = entity.Item1;
                
                if (clientPrediction.sessionId != sessionId)
                    continue;

                if (!clientPrediction.presentation)
                {
                    var newEntity = Instantiate(clientPrediction.prefab);
                    SceneManager.MoveGameObjectToScene(newEntity, clientPrediction.gameObject.scene);
                    clientPrediction.prefab.SetActive(false);
                    clientPrediction.presentation = newEntity.AddComponent<ClientPresentation>();
                    clientPrediction.presentation.target = clientPrediction.transform;
                }

                if (clientPrediction.presentation)
                {
                    var prediction = clientPrediction.transform;
                    var presentation = clientPrediction.presentation.transform;
                    var closeSnapDistance = clientPrediction.closeSnapDistance;
                    var farSnapDistance = clientPrediction.farSnapDistance;
                    var tweenSpeed = clientPrediction.tweenSpeed;

                    var distance = Vector3.Distance(prediction.position, presentation.position);
                    if (distance < closeSnapDistance || distance > farSnapDistance)
                        presentation.position = prediction.position;
                    else
                        presentation.position = Vector3.Lerp(presentation.position, prediction.position, tweenSpeed);

                    //distance = Vector3.Distance(prediction.rotation.eulerAngles, presentation.rotation.eulerAngles) * 10;
                    //if (distance < closeSnapDistance || distance > farSnapDistance)
                    //    presentation.rotation = prediction.rotation;
                    //else
                    presentation.rotation = Quaternion.Slerp(presentation.rotation, prediction.rotation, tweenSpeed);

                    distance = Vector3.Distance(prediction.localScale, presentation.localScale);
                    if (distance < closeSnapDistance || distance > farSnapDistance)
                        presentation.localScale = prediction.localScale;
                    else
                        presentation.localScale = Vector3.Lerp(presentation.localScale, prediction.localScale, tweenSpeed);
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
