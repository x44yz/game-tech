using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Book;

public class TestAvoidance : MonoBehaviour
{
    public GameObject aiAgentPrefab;
    public float spawnInterval;
    public Transform spawnPoint;
    public float spawnRange;

    [Header("DEBUG")]
    public Color spawnColor;

    [Header("RUNTIME")]
    public float spawnTick;
    public List<AIAgent> aIAgents = new List<AIAgent>();

    private void Awake()
    {
        aiAgentPrefab.SetActive(false);
    }

    private void Update()
    {
        float dt = Time.deltaTime;
        spawnTick += dt;
        if (spawnTick >= spawnInterval)
        {
            spawnTick -= spawnInterval;
            SpawnAIAgent();
        }
    }

    void SpawnAIAgent()
    {
        var obj = Instantiate(aiAgentPrefab);
        obj.SetActive(true);
        var agent = obj.GetComponent<AIAgent>();
        aIAgents.Add(agent);

        agent.pos = RandPoint(spawnPoint.position, spawnRange);
        agent.forward = RandRot() * agent.forward;
    }

    Quaternion RandRot(float min = 0f, float max = 360f)
    {
        float angle = Random.Range(min, max);
        return Quaternion.AngleAxis(angle, Vector3.up);
    }

    Vector3 RandPoint(Vector3 pt, float radius)
    {
        float angle = Random.Range(0f, 360f);
        float len = Random.Range(0f, radius);
        var rot = Quaternion.AngleAxis(angle, Vector3.up);
        return pt + rot * Vector3.forward * len;
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Handles.color = spawnColor;
        Handles.DrawWireDisc(spawnPoint.position, Vector3.up, spawnRange);
    }
#endif
}
