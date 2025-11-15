using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawn : MonoBehaviour
{
    public List<Transform> vents = new List<Transform>();

    public Transform GetRandomVent(Transform exclude = null)
    {
        if (vents.Count == 0) return null;

        List<Transform> available = new List<Transform>(vents);

        int index = Random.Range(0, available.Count);
        return available[index];
    }
}
