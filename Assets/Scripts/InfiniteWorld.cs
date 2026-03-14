using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteWorld : MonoBehaviour
{
    public GameObject floorPrefab;
    public GameObject pillarPrefab;
    public GameObject spherePrefab;
    public Transform player;

    public float chunkSize = 300f;
    public int viewDistance = 2;

    public float chanceToSpawnFinish = 0.95f;
    public LayerMask pillarLayer;

    [HideInInspector] public bool isFinishInWorld = false;

    private Dictionary<Vector2Int, GameObject> spawnedChunks = new Dictionary<Vector2Int, GameObject>();

    void Update()
    {
        if (player == null) return;

        int currentX = Mathf.RoundToInt(player.position.x / chunkSize);
        int currentZ = Mathf.RoundToInt(player.position.z / chunkSize);

        for (int x = -viewDistance; x <= viewDistance; x++)
        {
            for (int z = -viewDistance; z <= viewDistance; z++)
            {
                Vector2Int coord = new Vector2Int(currentX + x, currentZ + z);

                if (!spawnedChunks.ContainsKey(coord))
                {
                    SpawnNewArea(coord);
                }
            }
        }
        List<Vector2Int> keysToRemove = new List<Vector2Int>();

        foreach (var chunkCoord in spawnedChunks.Keys)
        {
            if (Mathf.Abs(chunkCoord.x - currentX) > viewDistance + 1 ||
                Mathf.Abs(chunkCoord.y - currentZ) > viewDistance + 1)
            {
                keysToRemove.Add(chunkCoord);
            }
        }

        foreach (Vector2Int coord in keysToRemove)
        {
            Destroy(spawnedChunks[coord]);
            spawnedChunks.Remove(coord);
        }
    }
    void SpawnNewArea(Vector2Int coord)
    {
        Vector3 pos = new Vector3(coord.x * chunkSize, 0, coord.y * chunkSize);
        GameObject chunkContainer = new GameObject($"ChunkContainer_{coord.x}_{coord.y}");
        chunkContainer.transform.position = pos;

        GameObject newFloor = Instantiate(floorPrefab, pos, Quaternion.identity);
        newFloor.transform.SetParent(chunkContainer.transform);
        if (!isFinishInWorld && Random.value < chanceToSpawnFinish)
        {
            Vector3 spherePos = Vector3.zero;
            bool posFound = false;
            int attempts = 0;

            while (!posFound && attempts < 15)
            {
                attempts++;
                float rx = Random.Range(-100f, 100f);
                float rz = Random.Range(-100f, 100f);
                spherePos = pos + new Vector3(rx, 1.2f, rz);
                if (!Physics.CheckSphere(spherePos, 15f, pillarLayer))
                {
                    posFound = true;
                }
            }
            if (posFound)
            {
                GameObject sphere = Instantiate(spherePrefab, spherePos, Quaternion.identity);
                sphere.transform.SetParent(chunkContainer.transform);
                sphere.GetComponent<FinishObject>().worldManager = this;
                isFinishInWorld = true;
                Debug.Log("—фера по€вилась в чанке: " + coord);
            }
        }
        for (int i = 0; i < 6; i++)
        {
            float rx = Random.Range(-100f, 100f);
            float rz = Random.Range(-100f, 100f);
            Vector3 pillarPos = pos + new Vector3(rx, 25f, rz);

            float angle = (Random.value > 0.5f) ? 30f : -30f;
            int axis = Random.Range(0, 3);

            Quaternion pillarRot = Quaternion.identity;
            if (axis == 0) pillarRot = Quaternion.Euler(angle, 0, 0);
            else if (axis == 1) pillarRot = Quaternion.Euler(0, angle, 0);
            else pillarRot = Quaternion.Euler(0, 0, angle);

            GameObject pillar = Instantiate(pillarPrefab, pillarPos, pillarRot);
            pillar.transform.SetParent(chunkContainer.transform);
        }

        spawnedChunks.Add(coord, chunkContainer);
    }
}