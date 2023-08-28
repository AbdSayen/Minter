using UnityEngine;
using UnityEngine.Tilemaps;

public class Builder : MonoBehaviour
{
    [SerializeField] private TileBase block;

    //private void Update()
    //{
    //    if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
    //    {
    //        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

    //        if (hit.collider != null)
    //        {
    //            Tilemap tilemap = hit.collider.GetComponent<Tilemap>();

    //            if (tilemap != null)
    //            {
    //                if (Input.GetMouseButton(0))
    //                {
    //                    Break(tilemap, worldPoint);
    //                }
    //                if (Input.GetMouseButton(1))
    //                {
    //                    SetBlock(tilemap, worldPoint);
    //                }
    //            }
    //        }
    //    }
    //}

    private void Break(Tilemap tilemap, Vector3 worldPoint)
    {
        tilemap.SetTile(tilemap.WorldToCell(worldPoint), null);
    }

    private void SetBlock(Tilemap tilemap, Vector3 worldPoint)
    {
        tilemap.SetTile(tilemap.WorldToCell(worldPoint), block);
    }
}