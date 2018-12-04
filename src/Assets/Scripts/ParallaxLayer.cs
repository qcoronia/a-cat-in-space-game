using System.Collections;
using UnityEngine;

[AddComponentMenu ("Background2D/ParallaxLayer")]
public class ParallaxLayer : Background
{

    public Transform itemTemplate;
    public float gridSize;
    public float gridSpan;
    public bool randomizeRotations = true;
    private Transform target;

    void Start ()
    {
        var targetObj = GameObject.FindWithTag ("MainCamera");
        target = targetObj.transform;

        var bgSize = new Vector3 (gridSize * 0.1f, gridSize * 0.1f, 1f);
        var halfSpan = (gridSpan - 1) / 2;
        for (var y = 0; y < gridSpan; y++)
        {
            var yPos = (y - halfSpan) * gridSize;
            for (var x = 0; x < gridSpan; x++)
            {
                var xPos = (x - halfSpan) * gridSize;
                var pos = transform.position.ToVector2 () + new Vector2 (xPos, yPos);
                var item = Instantiate (itemTemplate, pos.ToVector3 (transform.position.z), GetRandomOrientation ()) as Transform;
                var itemSpriteRenderer = item.GetComponent<SpriteRenderer> ();
                itemSpriteRenderer.sprite = background;
                item.localScale = bgSize;
                item.SetParent (transform);
            }
        }

        // for (var i = 0; i < transform.childCount; i++) {
        //     var item = transform.GetChild(i);
        //     var rr = item.GetComponent<SpriteRenderer>();
        //     rr.sprite = background;
        // }
    }

    void Update ()
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            var item = transform.GetChild (i);

            var offsetCheck = gridSize * 2;
            var offsetValue = gridSize * gridSpan;

            var hasChangedPosition = false;
            var offset = Vector3.zero;
            if (target.position.x < (item.position.x - offsetCheck))
            {
                offset -= Vector3.right * offsetValue;
                hasChangedPosition = true;
            }

            if (target.position.x > (item.position.x + offsetCheck))
            {
                offset += Vector3.right * offsetValue;
                hasChangedPosition = true;
            }

            if (target.position.y < (item.position.y - offsetCheck))
            {
                offset -= Vector3.up * offsetValue;
                hasChangedPosition = true;
            }

            if (target.position.y > (item.position.y + offsetCheck))
            {
                offset += Vector3.up * offsetValue;
                hasChangedPosition = true;
            }

            if (hasChangedPosition)
            {
                item.rotation = GetRandomOrientation ();
                item.position += offset;
            }
        }
    }

    public Quaternion GetRandomOrientation ()
    {
        var randomAngle = 90 * Random.Range (0, 3);
        return Quaternion.AngleAxis (randomAngle, Vector3.forward);
    }
}
