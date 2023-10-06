using UnityEngine;

namespace Code.Scripts.Parallax
{
    /// <summary>
    /// Controls the positioning of the parallax layers
    /// </summary>
    public class ParallaxLayer : MonoBehaviour
    {
        [SerializeField] private GameObject[] images;
        [SerializeField] private Transform character;

        private SpriteRenderer[] sprites;
        private int[] positions;
        private float size;

        private void Awake()
        {
            positions = new int[images.Length];
            sprites = new SpriteRenderer[images.Length];

            for (int i = 0; i < images.Length; i++)
            {
                sprites[i] = images[i].GetComponent<SpriteRenderer>();
                positions[i] = i;
            }

            size = sprites[0].bounds.size.x;
        }

        private void Update()
        {
            UpdateClosestPositions();

            for (int i = 0; i < images.Length; i++)
                PlaceByIndex(i, positions[i]);
        }

        /// <summary>
        /// Place the image by a position on the layer grid
        /// </summary>
        /// <param name="imageIndex">Number of the image</param>
        /// <param name="index">Number of the position</param>
        private void PlaceByIndex(int imageIndex, int index)
        {
            Vector3 pos = images[imageIndex].transform.position;
            pos.x = size * index;
            images[imageIndex].transform.position = pos;
        }

        /// <summary>
        /// Set the positions of the images each frame
        /// </summary>
        private void UpdateClosestPositions()
        {
            positions[images.Length / 2] = GetClosestIndex();

            for (int i = 1; i <= images.Length / 2; i++)
            {
                positions[images.Length / 2 - i] = GetClosestIndex() - i;

                if (images.Length / 2 + i <= images.Length - 1)
                    positions[images.Length / 2 + i] = GetClosestIndex() + i;
            }
        }

        /// <summary>
        /// Gets the index of the grid closest to the character
        /// </summary>
        /// <returns></returns>
        private int GetClosestIndex()
        {
            int pos = Mathf.RoundToInt(character.position.x / size);

            if (Vector2.Distance(new Vector2(pos * size, 0), character.position) >
                Vector2.Distance(new Vector2((pos + 1) * size, 0), character.position))
                pos++;

            return pos;
        }
    }
}