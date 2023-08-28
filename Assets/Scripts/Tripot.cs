using UnityEngine;

public class Tripot : MonoBehaviour
{
    [SerializeField] private Transform target;

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x, target.position.y, -10), 0.1f);
    }
}