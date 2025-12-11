using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentPopping : MonoBehaviour
{
    public BossAi bossAi;
    public Transform[] ventPoints;         // All vent locations
    public SpriteRenderer queenSprite;     // The Slime Queen's sprite
    public GameObject slime;

    public int slimesPerVent = 1;
    public float fadeDuration = 1.5f;      // Time to fade in/out
    public float visibleDuration = 3f;     // Time she stays visible
    public bool inPhase2 = false;
    public float phase2Duration = 5f;

    private int currentVentIndex = 0;

    void Start()
    {
        // Start at the first vent (center vent)
        transform.position = ventPoints[currentVentIndex].position;

        // Start invisible
        Color c = queenSprite.color;
        c.a = 0;
        queenSprite.color = c;

        // Begin the cycle
        StartCoroutine(VentCycle());
    }

    public void ActivatePhase2()
    {
        inPhase2 = true;
    }

    IEnumerator VentCycle()
    {
        while (true)
        {
            // Normal behavior
            if (!inPhase2)
            {
                yield return FirstPhase();
            }
            else
            {
                yield return Phase2Behavior();
            }
        }
    }

    IEnumerator FirstPhase()
    {
            yield return FadeIn();
            bossAi.Shoot();
            yield return new WaitForSeconds(visibleDuration);
            yield return FadeOut();

            // Move to next vent
            currentVentIndex = (currentVentIndex + 1) % ventPoints.Length;
            transform.position = ventPoints[currentVentIndex].position;
    }

    IEnumerator FadeIn()
    {
        float t = 0;
        while (t < fadeDuration)
        {
            float alpha = t / fadeDuration;
            SetAlpha(alpha);
            t += Time.deltaTime;
            yield return null;
        }
        SetAlpha(1);
    }

    IEnumerator FadeOut()
    {
        float t = 0;
        while (t < fadeDuration)
        {
            float alpha = 1 - (t / fadeDuration);
            SetAlpha(alpha);
            t += Time.deltaTime;
            yield return null;
        }
        SetAlpha(0);
    }

    IEnumerator Phase2Behavior()
    {
        // Step 1 — go to center vent
        transform.position = ventPoints[currentVentIndex].position;

        // Step 2 — fade out
        yield return FadeOut();

        List<GameObject> spawnedSlimes = new List<GameObject>();

        // Step 3 — Spawn slimes
        foreach (Transform vent in ventPoints)
        {
            for (int i = 0; i < slimesPerVent; i++)
            {
                 GameObject s = Instantiate(slime, vent.position, Quaternion.identity);
                spawnedSlimes.Add(s);
                yield return new WaitForSeconds(0.2f);
            }
        }

        yield return new WaitForSeconds(phase2Duration);

        foreach (GameObject s in spawnedSlimes)
        {
            if (s != null)
            Destroy(s);
        }

        currentVentIndex = (currentVentIndex + 1) % ventPoints.Length;
        transform.position = ventPoints[currentVentIndex].position;

        // fade back in
        yield return FadeIn();

        // Phase 2 ends — return to normal popping cycle
        inPhase2 = false; 

    }


    void SetAlpha(float a)
    {
        Color c = queenSprite.color;
        c.a = a;
        queenSprite.color = c;
    }
}
