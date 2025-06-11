using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

public class FloraSpawner : MonoBehaviour
{
    [Header("General Settings")]
    public float villageRadius = 40f;
    public float worldRadius = 200f;

    [Header("Trees")]
    private GameObject[] treePrefabs;
    public int numberOfTrees = 100;

    [Header("Flowers")]
    private GameObject[] flowerPrefabs;
    public int numberofFlowers = 50;

        [Header("Grass")]
    private GameObject[] grassPrefabs;
    public int numberofGrasses = 100;

    [Header("Rocks")]
    private GameObject[] rockPrefabs;
    public int numberOfRocks = 100;

    private Transform treesParent;
    private Transform flowerParent;
    private Transform grassParent;
    private Transform rocksParent;

    public void SpawnAll()
    {
        SetupParentFolders();
        SpawnCategory(treePrefabs, numberOfTrees, treesParent);
        SpawnCategory(grassPrefabs, numberofGrasses, grassParent);
        SpawnCategory(flowerPrefabs, numberofFlowers, flowerParent);
        SpawnCategory(rockPrefabs, numberOfRocks, rocksParent);
    }

    public void ClearAll()
    {
        ClearChildren(treesParent);
        ClearChildren(flowerParent);
        ClearChildren(grassParent);
        ClearChildren(rocksParent);
    }

void PopulateResources()
{
        treePrefabs = Resources.LoadAll<GameObject>("Trees");
        flowerPrefabs = Resources.LoadAll<GameObject>("Flowers");
        grassPrefabs = Resources.LoadAll<GameObject>("Grasses");
        rockPrefabs = Resources.LoadAll<GameObject>("Rocks");
}

void SetupParentFolders()
{
    treesParent = GetOrCreateParent("SpawnedTrees");
    flowerParent = GetOrCreateParent("SpawnedFlowers");
    grassParent = GetOrCreateParent("SpawnedGrasses");
    rocksParent = GetOrCreateParent("SpawnedRocks");

    if (!treesParent) Debug.LogError("treesParent is null!");
    if (!flowerParent) Debug.LogError("flowerParent is null!");
    if (!grassParent) Debug.LogError("grassParent is null!");
    if (!rocksParent) Debug.LogError("rocksParent is null!");
}

Transform GetOrCreateParent(string name)
{
    GameObject parent = GameObject.Find(name);
    if (!parent)
    {
        parent = new GameObject(name);
        Undo.RegisterCreatedObjectUndo(parent, $"Create {name}");
    }
    if (parent == null)
    {
        Debug.LogError($"Failed to find or create parent {name}!");
        return null;
    }
    return parent.transform;
}

    void ClearChildren(Transform parent)
    {
        if (!parent) return;
        while (parent.childCount > 0)
        {
            Transform child = parent.GetChild(0);
            Undo.DestroyObjectImmediate(child.gameObject);
        }
    }

    void SpawnCategory(GameObject[] prefabs, int amount, Transform parent)
    {
        if (parent == null)
        {
            Debug.LogError("SpawnCategory called with null parent! Aborting spawn.");
            return;
        }
        if (prefabs == null || prefabs.Length == 0)
        {
            PopulateResources();
        }

        for (int i = 0; i < amount; i++)
        {
            GameObject prefab = prefabs[Random.Range(0, prefabs.Length-1)];
            Vector3 position = GetRandomDonutPosition();

            Vector3 rayOrigin = new Vector3(position.x, 200f, position.z);
            if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit groundHit, 200f) && groundHit.collider.CompareTag("Ground"))
            {
                Vector3 surfacePoint = groundHit.point;
                SpawnAt(prefab, surfacePoint, parent);
            }
        }
    }

    void SpawnAt(GameObject prefab, Vector3 position, Transform parent)
    {
        GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        obj.transform.position = position;
        obj.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        obj.transform.localScale *= Random.Range(0.8f, 1.2f);
        obj.transform.SetParent(parent);
        Undo.RegisterCreatedObjectUndo(obj, "Spawn Flora");
    }

    Vector3 GetRandomDonutPosition()
    {
        Vector2 circle = Random.insideUnitCircle.normalized * Random.Range(villageRadius, worldRadius);
        return new Vector3(circle.x, 0f, circle.y) + transform.position;
    }
}
#endif // UNITY_EDITOR