using System.Collections;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    [SerializeField] private float timer;
    private void Start()
    {
        StartCoroutine(Delete());
    }

    private IEnumerator Delete()
    {
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }
}