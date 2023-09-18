using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [SerializeField] private Transform image;
    [SerializeField] private Transform character;
    [SerializeField] private float width = 3;

    private void Update()
    {
            Vector3 imagePos = image.position;

            if ((imagePos - character.position).x < -width)
                imagePos.x += width * 2;
            else if ((imagePos - character.position).x > width)
                imagePos.x -= width * 2;

            image.position = imagePos;
    }
}