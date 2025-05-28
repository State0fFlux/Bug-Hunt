using UnityEngine;

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

    // public Vector3 spawnAreaCenter = Vector3.zero;
    // public float spawnAreaRadius = 15f;

    void Start()
    {
        foreach (var bugType in bugTypes)
        {
            for (int i = 0; i < bugType.spawnCount; i++)
            {
                Vector3 spawnPos = bugType.settings.origin + Random.insideUnitSphere * bugType.settings.boundaryRadius;
                spawnPos.y = 0f; // keep on ground plane

                GameObject bugInstance = Instantiate(bugType.bugPrefab, spawnPos, Quaternion.identity);

                Travel wander = bugInstance.GetComponent<Travel>();
                if (wander != null)
                {
                    wander.settings = bugType.settings;

                    // Optionally set origin here for each bug individually
                    // wander.settings.origin = spawnAreaCenter;
                }
            }
        }
    }
}
