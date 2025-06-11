using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class BugManager : MonoBehaviour
{
    [System.Serializable]
    public class BugTypeEntry
    {
        public GameObject bugPrefab;
        public BugSettings settings;
        public int spawnCount = 5;
    }

    public BugTypeEntry[] bugTypes;

    private Transform bugsParent;
    private List<Transform> bugTypeParents = new List<Transform>();

    public void Awake()
    {
        SetupParentFolder();
        for (int i = 0; i < bugTypes.Length; ++i)
        {
            BugTypeEntry entry = bugTypes[i];
            Transform transform = bugTypeParents[i];
            SpawnCategory(entry.bugPrefab, entry.settings.spawnCount, transform, entry);
            // Debug.Log($"Spawning {entry.settings.bugName}");
        }
        
    }

    void SetupParentFolder()
    {
        bugsParent = GetOrCreateParent("SpawnedBugs");

        if (!bugsParent) Debug.LogError("bugsParent is null!");

        foreach (BugTypeEntry type in bugTypes)
        {
            Transform bugsChild = GetOrCreateBugParent(type.settings.bugName);
            bugTypeParents.Add(bugsChild);
        }
    }

    Transform GetOrCreateParent(string name)
    {
        GameObject parent = GameObject.Find(name);
        if (!parent)
        {
            parent = new GameObject(name);
#if UNITY_EDITOR
            Undo.RegisterCreatedObjectUndo(parent, $"Create {name}");
#endif
        }
        if (parent == null)
        {
            Debug.LogError($"Failed ot find or create parent {name}!");
            return null;
        }
        return parent.transform;
    }

    Transform GetOrCreateBugParent(string name)
    {
        Transform child = bugsParent.Find(name);
        if (child != null)
        {
            return child;
        }
        GameObject childObject = new GameObject(name);
        childObject.transform.parent = bugsParent;
        return childObject.transform;
    }

    void SpawnCategory(GameObject prefab, int amount, Transform parent, BugTypeEntry bugType)
    {
        if (parent == null)
        {
            Debug.LogError("SpawnCategory called with null parent! Aborting spawn.");
            return;
        }
        // if (prefabs == null || prefabs.Length == 0)
        // {
        //     PopulateResources();
        // }

    int groundLayerMask = LayerMask.GetMask("Walls & Floor"); // only detects objects on the "Ground" layer

        for (int i = 0; i < amount; i++)
        {
            // GameObject prefab = prefabs[Random.Range(0, prefabs.Length - 1)];
            Vector3 position = GetRandomPosition(bugType);

            Vector3 rayOrigin = new Vector3(position.x, 200f, position.z);
            if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit groundHit, 200f, groundLayerMask) && groundHit.collider.CompareTag("Ground"))
            {
                Vector3 surfacePoint = groundHit.point;
                SpawnAt(prefab, surfacePoint, parent);

                //Debug.Log("Hit!");
            }
        }
    }

    void SpawnAt(GameObject prefab, Vector3 position, Transform parent)
    {
        /*
        GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        obj.transform.position = position;
        obj.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        obj.transform.localScale *= Random.Range(0.5f, 1.5f); // increased scale range for more variety
        obj.transform.SetParent(parent);
        Undo.RegisterCreatedObjectUndo(obj, "Spawn Bug");

        Animator animator = obj.GetComponentInChildren<Animator>();
        animator.Play("Walk");*/
        GameObject obj = Instantiate(prefab, position, Quaternion.Euler(0, Random.Range(0, 360), 0), parent);
        obj.transform.localScale *= Random.Range(0.5f, 1.5f); // random scale

        Animator animator = obj.GetComponentInChildren<Animator>();
        if (animator != null)
        {
            animator.Play("Walk");
        }

    }

    Vector3 GetRandomPosition(BugTypeEntry bugType)
    {

        Vector2 offset = Random.insideUnitCircle * bugType.settings.boundaryRadius;
        Vector3 position = new Vector3(offset.x, 0f, offset.y) + bugType.settings.origin;
        return position;
    }
}
