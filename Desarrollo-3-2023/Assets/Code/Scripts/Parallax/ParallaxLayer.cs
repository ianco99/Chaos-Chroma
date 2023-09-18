using UnityEngine;

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

        for (var i = 0; i < images.Length; i++)
        {
            sprites[i] = images[i].GetComponent<SpriteRenderer>();
            positions[i] = i;
        }

        size = sprites[0].bounds.size.x;
    }

    private void Update()
    {
        UpdateClosestPositions();

        for (var i = 0; i < images.Length; i++)
            PlaceByIndex(i, positions[i]);

        foreach (var position in positions)
        {
            Debug.Log(position);
        }
    }

    private void PlaceByIndex(int imageIndex, int index)
    {
        var pos = images[imageIndex].transform.position;
        pos.x = size * index;
        images[imageIndex].transform.position = pos;
    }

    private void UpdateClosestPositions()
    {
        positions[images.Length / 2] = GetClosestIndex();

        for (var i = 1; i <= images.Length / 2; i++)
        {
            positions[images.Length / 2 - i] = GetClosestIndex() - i;

            if (images.Length / 2 + i <= images.Length - 1)
                positions[images.Length / 2 + i] = GetClosestIndex() + i;
        }
    }


    private int GetClosestIndex()
    {
        var pos = Mathf.RoundToInt(character.position.x / size);

        if (Vector2.Distance(new Vector2(pos * size, 0), character.position) >
            Vector2.Distance(new Vector2((pos + 1) * size, 0), character.position))
            pos++;

        return pos;
    }
}