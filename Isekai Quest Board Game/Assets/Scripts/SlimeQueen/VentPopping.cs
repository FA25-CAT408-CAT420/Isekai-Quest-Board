using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentPopping : State
{
    public BossSpawn bossSpawn;
    public Transform currentVent;
    public SpriteRenderer sprite;
    public Collider2D hitbox;

    public float appearDuration = 3f;
    public float disappearDelay = 1f;

    private bool isTransitioning;

    public override void Enter()
    {
        if (bossSpawn == null)
        {
            Debug.LogError("[BossVentBehavior] Missing VentManager reference!");
            isComplete = true;
            return;
        }

        // Start at the center vent if not already assigned
        if (currentVent == null)
        {
            currentVent = bossSpawn.vents[0];
        }

        // Move Boss to the vent position
        core.transform.position = currentVent.position;

        // Makes sure Slime Queen is visible and solid 
        SetVisible(true);
        isTransitioning = false;

        // Start the venting cycle
        StartCoroutine(VentCycle());
    }

    private IEnumerator VentCycle()
    {
        // Stay Visible for a bit
        yield return new WaitForSeconds(appearDuration);

        // Begin disappearing
        isTransitioning = true;
        yield return StartCoroutine(Disappear());

        // Pick a new vent
        Transform newVent = bossSpawn.GetRandomVent(currentVent);
        currentVent = newVent;

        // Move boss to the new vent
        core.transform.position = currentVent.position;

        // Appear again
        yield return StartCoroutine(Appear());

        // Repeat indefinitely
        isTransitioning = false;
        StartCoroutine(VentCycle());
    }

    private IEnumerator Disappear()
    {
        // Play visual fade-out or shrinking effect
        float fadeTime = 0.5f;
        float t = 0f;
        Color c = sprite.color;

        while (t < fadeTime)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, t / fadeTime);
            yield return null;
        }

        // Disable collisions while "underground"
        SetVisible(false);
        yield return new WaitForSeconds(disappearDelay);
    }

    private IEnumerator Appear()
    {
        // Reappear from the new vent
        SetVisible(true);
        float fadeTime = 0.5f;
        float t = 0f;
        Color c = sprite.color;

        while (t < fadeTime)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, t / fadeTime);
            sprite.color = c;
            yield return null;
        }
    }

    private void SetVisible(bool state)
    {
        if (sprite != null)
            sprite.enabled = state;
            
        if (hitbox != null)
            hitbox.enabled = state;
    }

    public override void Exit()
    {
        StopAllCoroutines();
        SetVisible(true);
    }
}
