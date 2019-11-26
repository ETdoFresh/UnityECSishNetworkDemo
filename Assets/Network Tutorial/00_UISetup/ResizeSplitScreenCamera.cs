using ECSish;
using System.Linq;

public class ResizeSplitScreenCamera : MonoBehaviourSystem
{
    private void Update()
    {
        var entities = GetAllEntities<SplitScreenCamera>();

        if (entities.Count() == 1)
        {
            var camera = entities.ElementAt(0).Item1;
            camera.rect.x = 0; camera.rect.y = 0;
            camera.rect.width = 1; camera.rect.height = 1;
            SetRectTransform(camera, 0, 0, 1, 1);
        }

        else if (entities.Count() == 2)
        {
            var camera = entities.ElementAt(0).Item1;
            SetRectTransform(camera, 0, 0, 0.5f, 1);

            camera = entities.ElementAt(1).Item1;
            SetRectTransform(camera, 0.5f, 0, 0.5f, 1);
        }

        else if (entities.Count() == 3)
        {
            var camera = entities.ElementAt(0).Item1;
            SetRectTransform(camera, 0, 0, 0.5f, 1);

            camera = entities.ElementAt(1).Item1;
            SetRectTransform(camera, 0.5f, 0.5f, 0.5f, 0.5f);

            camera = entities.ElementAt(2).Item1;
            SetRectTransform(camera, 0.5f, 0, 0.5f, 0.5f);
        }

        else if (entities.Count() == 4)
        {
            var camera = entities.ElementAt(0).Item1;
            SetRectTransform(camera, 0, 0.5f, 0.5f, 0.5f);

            camera = entities.ElementAt(1).Item1;
            SetRectTransform(camera, 0.5f, 0.5f, 0.5f, 0.5f);

            camera = entities.ElementAt(2).Item1;
            SetRectTransform(camera, 0, 0, 0.5f, 0.5f);

            camera = entities.ElementAt(3).Item1;
            SetRectTransform(camera, 0.5f, 0, 0.5f, 0.5f);
        }
    }

    private void SetRectTransform(SplitScreenCamera camera, float positionX, float positionY, float sizeX, float sizeY)
    {
        camera.rect.x = positionX; camera.rect.y = positionY;
        camera.rect.width = sizeX; camera.rect.height = sizeY;
        camera.camera.rect = camera.rect;
    }
}
