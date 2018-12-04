using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public TransformState vicinityState;

    public Transform[] itemPrefabs;

    [Header("Limit")]
    public int maxInstances = 10;

    [Header("Spawn Frequency")]
    public float frequency;
    [Range(0f, 1f)]
    public float frequencyErrorMin;
    [Range(0f, 1f)]
    public float frequencyErrorMax;

    [Header("Spawn Field")]
    public float radius;

    private bool IsEnabled = false;

    void Start()
    {
        Reset();
    }

    public void Reset()
    {
        IsEnabled = true;
        StartCoroutine(SpawnByInterval());
    }

    IEnumerator SpawnByInterval()
    {
        while (IsEnabled)
        {
            if (transform.childCount < maxInstances)
            {
                var randomIndex = UnityHelpers.RandomIndex(0, itemPrefabs.Length);

                var center = transform.position.ToVector2();
                if (vicinityState)
                {
                    center = vicinityState.position.ToVector2();
                }

                var offset = center + (Random.insideUnitCircle.normalized * radius);
                var item = Instantiate(itemPrefabs[randomIndex], offset, Quaternion.identity) as Transform;
                item.SetParent(transform);
            }

            var errorOffset = Random.Range(frequencyErrorMin, frequencyErrorMax) * frequency;
            errorOffset = errorOffset * Mathf.Sign(Random.Range(-1f, 1f));
            var interval = frequency + errorOffset;
            yield return new WaitForSeconds(interval);
        }
    }

    //void OnDrawGizmos()
    //{
    //    if (!IsEnabled)
    //    {
    //        return;
    //    }

    //    var center = transform.position.ToVector2();
    //    if (vicinityState)
    //    {
    //        center = vicinityState.position.ToVector2();
    //    }

    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireSphere(center, radius);
    //}
}
