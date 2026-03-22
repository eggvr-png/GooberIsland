using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Photon.Pun;

public class ChunkManager : MonoBehaviourPunCallbacks
{
    public static ChunkManager instance;
    public Vector2 worldSize;
    public int resolution = 16;
    public float chunkSize = 128f;
    public float islandRadius = 900;
    public float noiseScale = 0.005f;
    public float heightScale = 150f;
    public int octaves = 4;
    [Range(0f, 1f)] public float persistence = 0.5f;
    public float lacunarity = 2f;
    public float seaLevel = 10f;
    public float seaDepth = 30f;
    [Range(0.5f, 5f)] public float shoreSmoothness = 1f;
    public float seaFloorExtent = 3f;
    public Material material;
    public Material seaFloorMaterial;
    public GameObject treePrefab;
    public int treesPerChunk = 15;
    public float minTreeHeight = 5f;
    public float minTreeSpacing = 4f;
    public GameObject raftPrefab;
    public float shoreSearchStep = 8f;
    public float shoreThreshold = 3f;
    public float raftShoreOffset = 10f;
    public int islandLayer = 0;
    public PhysicMaterial islandPhysicsMaterial;
    public List<GameObject> structurePrefabs;
    public float structureMinHeight = 15f;
    public Vector2 worldCenter;
    public int seed;

    private void Awake()
    {
        instance = this;
    }

    [PunRPC]
    public void StartGeneratingRPC(int seedSent)
    {
        seed = seedSent;
        worldCenter = new Vector2((worldSize.x / 2) * chunkSize, (worldSize.y / 2) * chunkSize);
        CreateSeaFloor();
        StartCoroutine(GenerateChunks());
    }

    [PunRPC]
    public void SyncRaftPosition(Vector3 chosen)
    {
        GameObject connectionManager = GameObject.Find("ConnectionManager");
        if (connectionManager != null)
            connectionManager.transform.position = chosen + Vector3.up * 3f;
        else
            Debug.LogWarning("ChunkManager: couldn't find ConnectionManager gameobject");
    }

    void CreateSeaFloor()
    {
        float size = Mathf.Max(worldSize.x, worldSize.y) * chunkSize * seaFloorExtent;
        float y = -seaDepth;

        GameObject sf = new GameObject("SeaFloor", typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));
        sf.transform.parent = transform;

        Vector3 origin = new Vector3(worldCenter.x - size / 2f, y, worldCenter.y - size / 2f);

        Mesh mesh = new Mesh();
        mesh.vertices = new Vector3[] {
            origin,
            origin + new Vector3(size, 0, 0),
            origin + new Vector3(0, 0, size),
            origin + new Vector3(size, 0, size)
        };
        mesh.triangles = new int[] { 0, 2, 1, 1, 2, 3 };
        mesh.uv = new Vector2[] {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };
        mesh.RecalculateNormals();

        sf.GetComponent<MeshFilter>().mesh = mesh;
        sf.GetComponent<MeshCollider>().sharedMesh = mesh;
        sf.GetComponent<MeshRenderer>().material = seaFloorMaterial != null ? seaFloorMaterial : material;
    }

    IEnumerator GenerateChunks()
    {
        List<GameObject> chunks = new List<GameObject>();

        for (int x = 0; x < worldSize.x; x++)
        {
            for (int y = 0; y < worldSize.y; y++)
            {
                TerrainGenerator tg = new TerrainGenerator();
                GameObject current = new GameObject("Terrain" + (x * y), typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));
                current.transform.parent = transform;
                current.transform.localPosition = new Vector3(x * chunkSize, 0, y * chunkSize);
                tg.Init(current);
                tg.Generate(material);
                tg.SpawnTrees(current.transform, treePrefab, treesPerChunk, minTreeHeight);
                chunks.Add(current);
                yield return new WaitForSeconds(0);
            }
        }

        MergeChunks(chunks);

        if (PhotonNetwork.IsMasterClient)
        {
            SpawnRaft();
            TrySpawnStructure();
        }

        ConnectionManager cm = FindObjectOfType<ConnectionManager>();
        if (cm != null)
            cm.SpawnPlayer();
    }

    void MergeChunks(List<GameObject> chunks)
    {
        CombineInstance[] combine = new CombineInstance[chunks.Count];

        for (int i = 0; i < chunks.Count; i++)
        {
            MeshFilter mf = chunks[i].GetComponent<MeshFilter>();
            combine[i].mesh = mf.sharedMesh;
            combine[i].transform = mf.transform.localToWorldMatrix;
        }

        Mesh merged = new Mesh();
        merged.indexFormat = IndexFormat.UInt32;
        merged.CombineMeshes(combine, true, true);
        merged.RecalculateBounds();
        merged.RecalculateNormals();

        GameObject mergedObj = new GameObject("TerrainMerged", typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));
        mergedObj.transform.parent = transform;
        mergedObj.GetComponent<MeshFilter>().mesh = merged;
        mergedObj.GetComponent<MeshCollider>().sharedMesh = merged;
        mergedObj.GetComponent<MeshCollider>().sharedMaterial = islandPhysicsMaterial;
        mergedObj.GetComponent<MeshRenderer>().material = material;
        mergedObj.layer = islandLayer;

        foreach (var chunk in chunks)
        {
            for (int i = chunk.transform.childCount - 1; i >= 0; i--)
                chunk.transform.GetChild(i).SetParent(mergedObj.transform);
            UnityEngine.Object.Destroy(chunk);
        }
    }

    void TrySpawnStructure()
    {
        if (structurePrefabs == null || structurePrefabs.Count == 0) return;

        System.Random rng = new System.Random(seed + 77777);

        if (rng.Next(5) != 0) return;

        List<Vector3> inlandPoints = new List<Vector3>();

        for (int x = 0; x < worldSize.x; x++)
        {
            for (int z = 0; z < worldSize.y; z++)
            {
                Vector2 center = new Vector2(
                    x * chunkSize + chunkSize * 0.5f,
                    z * chunkSize + chunkSize * 0.5f
                );

                float distance = Vector2.Distance(worldCenter, center);
                float noise = TerrainGenerator.SampleNoise(center);
                float dropoff = Mathf.Clamp01(1f - Mathf.Pow(distance / islandRadius, 2));
                float y = TerrainGenerator.ApplySeaLevel(noise * dropoff * heightScale, dropoff);

                if (y >= structureMinHeight)
                    inlandPoints.Add(new Vector3(center.x, y, center.y));
            }
        }

        if (inlandPoints.Count == 0)
        {
            Debug.LogWarning("ChunkManager: no inland points found for structure, try lowering structureMinHeight");
            return;
        }

        Vector3 chosen = inlandPoints[rng.Next(inlandPoints.Count)];
        GameObject prefab = structurePrefabs[rng.Next(structurePrefabs.Count)];
        PhotonNetwork.Instantiate(prefab.name, chosen, Quaternion.Euler(0, (float)(rng.NextDouble() * 360f), 0));
    }

    void SpawnRaft()
    {
        if (raftPrefab == null) return;

        float totalWidth = worldSize.x * chunkSize;
        float totalHeight = worldSize.y * chunkSize;

        List<Vector3> shorePoints = new List<Vector3>();

        for (float wx = 0; wx < totalWidth; wx += shoreSearchStep)
        {
            for (float wz = 0; wz < totalHeight; wz += shoreSearchStep)
            {
                Vector2 worldPos2D = new Vector2(wx, wz);
                float distance = Vector2.Distance(worldCenter, worldPos2D);
                float noise = TerrainGenerator.SampleNoise(worldPos2D);
                float dropoff = Mathf.Clamp01(1f - Mathf.Pow(distance / islandRadius, 2));
                float y = TerrainGenerator.ApplySeaLevel(noise * dropoff * heightScale, dropoff);

                if (Mathf.Abs(y) <= shoreThreshold)
                    shorePoints.Add(new Vector3(wx, 0f, wz));
            }
        }

        if (shorePoints.Count == 0)
        {
            Debug.LogWarning("ChunkManager: no shore points found, try raising shoreThreshold or lowering shoreSearchStep");
            return;
        }

        System.Random rng = new System.Random(seed + 99999);
        Vector3 chosen = shorePoints[rng.Next(shorePoints.Count)];

        Vector3 toOcean = (chosen - new Vector3(worldCenter.x, 0f, worldCenter.y)).normalized;
        chosen += toOcean * raftShoreOffset;

        PhotonNetwork.Instantiate(raftPrefab.name, chosen, Quaternion.Euler(0, (float)(rng.NextDouble() * 360f), 0));

        GetComponent<PhotonView>().RPC("SyncRaftPosition", RpcTarget.AllBuffered, chosen);
    }
}

class TerrainGenerator
{
    MeshFilter filter;
    MeshRenderer renderer;
    MeshCollider collider;
    Vector3[] verts;
    int[] triangles;
    Vector2[] uvs;
    Mesh mesh;

    public void Init(GameObject current)
    {
        filter = current.GetComponent<MeshFilter>();
        renderer = current.GetComponent<MeshRenderer>();
        collider = current.GetComponent<MeshCollider>();
        mesh = new Mesh();
    }

    public static float SampleNoise(Vector2 worldPos)
    {
        float value = 0f;
        float amplitude = 1f;
        float frequency = ChunkManager.instance.noiseScale;
        float maxValue = 0f;
        int seed = ChunkManager.instance.seed;

        for (int o = 0; o < ChunkManager.instance.octaves; o++)
        {
            float sampleX = (worldPos.x + seed) * frequency;
            float sampleY = (worldPos.y + seed) * frequency;
            value += Mathf.PerlinNoise(sampleX, sampleY) * amplitude;
            maxValue += amplitude;
            amplitude *= ChunkManager.instance.persistence;
            frequency *= ChunkManager.instance.lacunarity;
        }

        return value / maxValue;
    }

    public static float ApplySeaLevel(float y, float dropoff)
    {
        float seaLevel = ChunkManager.instance.seaLevel;
        float seaDepth = ChunkManager.instance.seaDepth;
        float smoothness = ChunkManager.instance.shoreSmoothness;

        if (y >= seaLevel) return y;

        float t = Mathf.Clamp01(y / seaLevel);
        float smooth = Mathf.Pow(Mathf.SmoothStep(0f, 1f, t), smoothness);
        float deepened = Mathf.Lerp(-seaDepth, seaLevel, smooth);

        float oceanness = 1f - dropoff;
        return Mathf.Lerp(y, deepened, oceanness);
    }

    public void Generate(Material mat)
    {
        float chunkSize = ChunkManager.instance.chunkSize;
        int resolution = ChunkManager.instance.resolution;
        Vector2 worldPos = new Vector2(filter.gameObject.transform.localPosition.x, filter.gameObject.transform.localPosition.z);
        Vector2 worldCenter = ChunkManager.instance.worldCenter;

        verts = new Vector3[(resolution + 1) * (resolution + 1)];
        uvs = new Vector2[verts.Length];

        float step = chunkSize / resolution;

        for (int i = 0, x = 0; x <= resolution; x++)
        {
            for (int z = 0; z <= resolution; z++)
            {
                Vector2 vertexWorldPos = new Vector2(worldPos.x + (x * step), worldPos.y + (z * step));
                float distance = Vector2.Distance(worldCenter, vertexWorldPos);
                float noise = SampleNoise(vertexWorldPos);
                float dropoff = Mathf.Clamp01(1f - Mathf.Pow(distance / ChunkManager.instance.islandRadius, 2));
                float y = ApplySeaLevel(noise * dropoff * ChunkManager.instance.heightScale, dropoff);
                verts[i] = new Vector3(x * step, y, z * step);
                i++;
            }
        }

        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(verts[i].x + worldPos.x, verts[i].z + worldPos.y);
        }

        triangles = new int[resolution * resolution * 6];
        int tris = 0;
        int vert = 0;
        for (int x = 0; x < resolution; x++)
        {
            for (int y = 0; y < resolution; y++)
            {
                triangles[tris] = vert;
                triangles[tris + 1] = vert + 1;
                triangles[tris + 2] = vert + resolution + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + resolution + 2;
                triangles[tris + 5] = vert + resolution + 1;
                vert++;
                tris += 6;
            }
            vert++;
        }

        mesh.Clear();
        mesh.vertices = verts;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        collider.sharedMesh = mesh;
        filter.mesh = mesh;
        renderer.material = mat;
    }

    public void SpawnTrees(Transform chunkTransform, GameObject treePrefab, int count, float minHeight)
    {
        if (treePrefab == null) return;

        float chunkSize = ChunkManager.instance.chunkSize;
        float minSpacing = ChunkManager.instance.minTreeSpacing;
        Vector2 worldPos = new Vector2(chunkTransform.localPosition.x, chunkTransform.localPosition.z);
        Vector2 worldCenter = ChunkManager.instance.worldCenter;

        int chunkSeed = ChunkManager.instance.seed
            + (int)worldPos.x * 73856093
            ^ (int)worldPos.y * 19349663;
        System.Random rng = new System.Random(chunkSeed);

        List<Vector2> placed = new List<Vector2>();

        int attempts = count * 10;
        int spawned = 0;

        for (int i = 0; i < attempts && spawned < count; i++)
        {
            float localX = (float)(rng.NextDouble() * chunkSize);
            float localZ = (float)(rng.NextDouble() * chunkSize);

            Vector2 candidate = new Vector2(worldPos.x + localX, worldPos.y + localZ);

            bool tooClose = false;
            for (int j = 0; j < placed.Count; j++)
            {
                if (Vector2.Distance(candidate, placed[j]) < minSpacing)
                {
                    tooClose = true;
                    break;
                }
            }
            if (tooClose) continue;

            float distance = Vector2.Distance(worldCenter, candidate);
            float noise = SampleNoise(candidate);
            float dropoff = Mathf.Clamp01(1f - Mathf.Pow(distance / ChunkManager.instance.islandRadius, 2));
            float y = ApplySeaLevel(noise * dropoff * ChunkManager.instance.heightScale, dropoff);

            if (y < minHeight) continue;

            Vector3 spawnPos = chunkTransform.position + new Vector3(localX, y, localZ);
            GameObject tree = UnityEngine.Object.Instantiate(treePrefab, spawnPos, Quaternion.identity);
            tree.transform.parent = chunkTransform;
            tree.transform.rotation = Quaternion.Euler(0, (float)(rng.NextDouble() * 360f), 0);
            placed.Add(candidate);
            spawned++;
        }
    }
}