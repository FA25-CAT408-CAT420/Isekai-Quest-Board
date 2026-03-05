using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBehavior : MonoBehaviour
{
    /* Note for self. Make a list of all the spikes then make a switch statement where I assign each spike's numercial value to a Letter value. Then have each one play their animation.
    */

    public SpikeDamage spkDMG;
    public List<SpikeTraps> spikeList = new List<SpikeTraps>();

    public float delayBetweenSpikes = 0.4f;
    public float delayBetweenWaves = 2f;

    private bool isRunning;

    public void StartWave()
    {
        if (!isRunning)
        {
            StartCoroutine(WaveRoutine());
        }
    }

    IEnumerator WaveRoutine()
    {
        isRunning = true;

        for (int i = 0; i < spikeList.Count; i++)
        {
            spikeList[i].ActivateSpike();
            yield return new WaitForSeconds(delayBetweenSpikes);
        }

        yield return new WaitForSeconds(delayBetweenWaves);

        isRunning = false;
    }

    void EnableSpike()
    {
        spkDMG.enabled = true;
    }
    void DisableSpike()
    {
        spkDMG.enabled = false;
    }
}
