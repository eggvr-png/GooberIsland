using System;
using System.Collections;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    public static ChunkManager instance;
    public Vector2 worldSize;
    public int resolution = 16;
    public float islandRadius = 900;
    public Material material;
    public Vector2 worldCenter;
    public int seed;

    private void Awake()
    {
        instance = this;
        seed = UnityEngine.Random.Range(1, 10000000);
    }

    void Start()
    {
        worldCenter = new Vector2((worldSize.x / 2) * 128, (worldSize.y / 2) * 128);
        StartCoroutine(GenerateChunks());
    }

    IEnumerator GenerateChunks(){
        for (int x = 0; x < worldSize.x; x++){
            for (int y = 0; y < worldSize.y; y++) {
                TerrainGenerator tg = new TerrainGenerator();
                GameObject current = new GameObject("Terrain" + (x * y), typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));
                current.transform.parent = transform;
                current.transform.localPosition = new Vector3(x * 128, 0, y * 128);
                tg.Init(current);
                tg.Generate(material);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}

class TerrainGenerator {
    MeshFilter filter;
    MeshRenderer renderer;
    MeshCollider collider;
    Vector3[] verts;
    int[] triangles;
    Vector2[] uvs;
    Mesh mesh;

    public void Init(GameObject current) {
        filter = current.GetComponent<MeshFilter>();
        renderer = current.GetComponent<MeshRenderer>();
        collider = current.GetComponent<MeshCollider>();
        mesh = new Mesh();
    }

    public void Generate(Material mat){
        Vector2 worldPos = new Vector2(filter.gameObject.transform.localPosition.x, filter.gameObject.transform.localPosition.z);
        int resolution = ChunkManager.instance.resolution;
        verts = new Vector3[(resolution + 1) * (resolution + 1)];
        uvs = new Vector2[verts.Length];
        Vector2 worldCenter = ChunkManager.instance.worldCenter;
        for (int i = 0, x = 0; x <= resolution; x++) {
            for (int z = 0; z <= resolution; z++) {
                Vector2 vertexWorldPos = new Vector2(worldPos.x + (x * (128f / resolution)), worldPos.y + (z * (128f / resolution)));
                float distance = Vector2.Distance(worldCenter, vertexWorldPos);
                float noise = Mathf.PerlinNoise((vertexWorldPos.x + ChunkManager.instance.seed) * 0.005f, (vertexWorldPos.y + ChunkManager.instance.seed) * 0.005f);
                float dropoff = Mathf.Clamp01(1f - Mathf.Pow(distance / ChunkManager.instance.islandRadius, 2));
                float y = noise * dropoff * 150f;
                verts[i] = new Vector3(x * (128f / resolution), y, z * (128f / resolution));
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
}
