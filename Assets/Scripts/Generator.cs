using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Generator : MonoBehaviour
{
    [Header("Base Options")]
    [SerializeField] private int chunkWidth = 64;
    [SerializeField] private int chunksCount;
    [SerializeField] private int minYPos;

    [Header("Detailed Options")]
    [SerializeField] private int dirtFaultPos;
    [SerializeField] private int stoneFaultPos;

    [Header("Additional Options")]
    [SerializeField] private GameObject chunkPrefab;
    [SerializeField] private Grid _grid;
    [SerializeField] private TileBase grass;
    [SerializeField] private TileBase dirt;
    [SerializeField] private TileBase stone;

    private List<int> dirtHeightMap;
    private List<int> stoneHeightMap;
    private GameObject player;
    private List<Tilemap> activeChunks;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        activeChunks = new List<Tilemap>();

        GenerateWorld();
        StartCoroutine(RenderChunksCoroutine());
    }

    private void GenerateWorld()
    {
        dirtHeightMap = GenerateHeightMap(150, 150 - dirtFaultPos, 150 + dirtFaultPos);
        stoneHeightMap = GenerateHeightMap(130, 130 - stoneFaultPos, 130 + stoneFaultPos);
    }

    private void RenderChunks(float playerX)
    {
        int playerChunk = (int)(playerX / chunkWidth);

        for (int i = playerChunk - 1; i <= playerChunk + 1; i++)
        {
            if (!IsChunkActive(i) && i >= 0 && !(i < activeChunks.Count))
            {
                Tilemap chunk = Instantiate(chunkPrefab).GetComponent<Tilemap>();
                chunk.transform.SetParent(_grid.transform);

                StartCoroutine(GenerateChunk(chunk, dirt, dirtHeightMap, i, true));
                StartCoroutine(GenerateChunk(chunk, stone, stoneHeightMap, i, true));
                StartCoroutine(GenerateChunk(chunk, grass, dirtHeightMap, i, false));

                chunk.transform.position = new Vector3(i * chunkWidth, 0, 0);

                activeChunks.Add(chunk);
            }
        }

        RemoveInactiveChunks(playerChunk - 2, playerChunk + 2);
    }
    private IEnumerator GenerateChunk(Tilemap chunk, TileBase tile, List<int> heightMap, int queue, bool isFilled)
    {
        Vector3Int cellPosition = new Vector3Int();

        for (int i = 0; i < chunkWidth; i++)
        {
            cellPosition.x = i;
            cellPosition.y = heightMap[i + chunkWidth * queue];

            chunk.SetTile(cellPosition, tile);

            if (isFilled)
            {
                for (int j = heightMap[i + queue * chunkWidth]; j > minYPos; j--)
                {
                    cellPosition.y = j;
                    chunk.SetTile(cellPosition, tile);
                }
            }
        }

        yield break;
    }

    private List<int> GenerateHeightMap(int startValue, int minValue, int maxValue)
    {
        List<int> heightMap = new List<int>();
        int height = startValue;

        for (int i = 0; i < chunkWidth * chunksCount; i++)
        {
            height += Random.Range(-1, 2);
            if (height < minValue) height++;
            else if (height > maxValue) height--;

            heightMap.Add(height);
        }

        return heightMap;
    }

    private IEnumerator RenderChunksCoroutine()
    {
        for (; ; )
        {
            RenderChunks(player.transform.position.x);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void RemoveInactiveChunks(int startChunk, int endChunk)
    {
        for (int i = activeChunks.Count - 1; i >= 0; i--)
        {
            int chunkPos = (int)(activeChunks[i].transform.position.x / chunkWidth);

            if (chunkPos < startChunk || chunkPos > endChunk)
            {
                if (i >= 0 && i < activeChunks.Count)
                {
                    Destroy(activeChunks[i].gameObject);
                    activeChunks.RemoveAt(i);
                }
            }
        }
    }

    private bool IsChunkActive(int chunkIndex)
    {
        foreach (var chunk in activeChunks)
        {
            int chunkPos = (int)(chunk.transform.position.x / chunkWidth);
            if (chunkPos == chunkIndex)
            {
                return true;
            }
        }
        return false;
    }
}
