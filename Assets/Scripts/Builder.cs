using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Builder : MonoBehaviour
{
    [Header("Tile Progress")]
    [SerializeField] private List<TileBase> breakProgress = new List<TileBase>();
    [SerializeField] private GameObject tilemapPrefab;
    [SerializeField] private Tilemap tileProgress;

    [Header("Block Placement")]
    [SerializeField] private TileBase block;

    private Vector3Int breakingPlace;
    private bool isBreaking = false;
    private float progress;
    private GameObject progressTilePrefab;

    private void Start()
    {
        progressTilePrefab = Instantiate(tilemapPrefab);
        tileProgress = progressTilePrefab.GetComponent<Tilemap>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            HandleMouseInput();
        }
        else
        {
            ClearProgress();
        }
    }

    private void HandleMouseInput()
    {
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, 1), Vector2.zero);

        if (hit.collider != null)
        {
            Tilemap tilemap = hit.collider.GetComponent<Tilemap>();

            if (tilemap != null)
            {
                if (Input.GetMouseButton(0) && Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero))
                {
                    if (!isBreaking) StartCoroutine(Break(tilemap, worldPoint));
                }
                if (Input.GetMouseButton(1))
                {
                    SetBlock(tilemap, worldPoint);
                    progress = 0;
                }
            }
        }
    }

    private IEnumerator Break(Tilemap tilemap, Vector3 worldPoint)
    {
        isBreaking = true;

        if (breakingPlace == tilemap.WorldToCell(worldPoint))
        {
            if (progress >= 1)
            {
                tilemap.SetTile(tilemap.WorldToCell(worldPoint), null);
                isBreaking = false;
                ClearProgress();
            }
            else
            {
                ShowProgress(tilemap, worldPoint);

                progress += 0.2f;
                yield return new WaitForSeconds(0.2f);
                isBreaking = false;
            }
        }
        else
        {
            breakingPlace = tilemap.WorldToCell(worldPoint);
            ClearProgress();
        }
        isBreaking = false;
    }

    private void SetBlock(Tilemap tilemap, Vector3 worldPoint)
    {
        if (Vector2.Distance(worldPoint, transform.position) > 0.8f)
        {
            tilemap.SetTile(tilemap.WorldToCell(worldPoint), block);
        }
    }

    private void ShowProgress(Tilemap tilemap, Vector3 worldPoint)
    {
        progressTilePrefab.SetActive(true);
        progressTilePrefab.transform.position = new Vector3((int)worldPoint.x + 0.5f, (int)worldPoint.y + 0.5f, -5);

        int breakProgressIndex = Mathf.FloorToInt(progress * 5);
        breakProgressIndex = Mathf.Clamp(breakProgressIndex, 0, breakProgress.Count - 1);

        tileProgress.SetTile(tilemap.WorldToCell(worldPoint), breakProgress[breakProgressIndex]);
    }

    private void ClearProgress()
    {
        foreach (var position in tileProgress.cellBounds.allPositionsWithin)
        {
            tileProgress.SetTile(position, null);
        }

        progress = 0;
        progressTilePrefab.SetActive(false);
    }
}
