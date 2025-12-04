using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerInteractions : MonoBehaviour
{   
    public GameManager gameManager;
    public PlayerInputActions playerControls;
    private InputAction interact;

    private Collider2D nearbySpell;
    public CinemachineVirtualCamera camera;

    private bool cameraInitialized = false;

    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.05f);

        foreach (var h in hits)
        {
            if (h.CompareTag("CameraBounds"))
            {
                if (camera == null)
                    camera = GameObject.FindGameObjectWithTag("Camera").GetComponent<CinemachineVirtualCamera>();

                // Instantly put camera into correct zone
                camera.transform.position = new Vector3(
                    h.transform.position.x,
                    h.transform.position.y,
                    camera.transform.position.z
                );

                cameraInitialized = true;
                break;
            }
        }
    }

    private void OnEnable()
    {
        interact = playerControls.Player.Interact;
        interact.Enable();
        interact.performed += OnInteractPerformed;
    }

    private void OnDisable()
    {
        interact.performed -= OnInteractPerformed;
        interact.Disable();
    }

    void Update()
    {
        //So no fake error messages just in case
        if (camera == null)
        {
            return;
        }
    }

    private void OnTriggerEnter2D (Collider2D other){
        if (other.gameObject.CompareTag("Soul"))
        {
            gameManager.soulPoints++;
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Ghost"))
        {
            int deathSouls = other.gameObject.GetComponent<SoulsRetrieved>().souls;
            gameManager.soulPoints += deathSouls;
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Spell")){
            nearbySpell = other;
        }

        if (other.gameObject.CompareTag("LockedRoom")){
            Debug.Log("HIT THE ROOM");
            other.gameObject.GetComponent<SpawnLockedRoom>().Spawn();
        }
        
        if (other.gameObject.CompareTag("CameraBounds")) {
            //Camera Functions
            if (camera != null)
            {
                StartCoroutine(CamTransition(camera.transform, other.gameObject.transform, 0.2f));
                // new Vector3(
                //     other.gameObject.transform.position.x,
                //     other.gameObject.transform.position.y,
                //     camera.transform.position.z);
            } 
            else if (camera == null) {
                camera = GameObject.FindGameObjectWithTag("Camera").GetComponent<CinemachineVirtualCamera>();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Spell") && other == nearbySpell)
        {
            nearbySpell = null;
        }
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        if (nearbySpell != null)
        {
            nearbySpell.GetComponent<SpellAcquisition>().Interacted();
        }
    }

    //Camera Coroutine
    public IEnumerator CamTransition(Transform target, Transform destination, float duration)
    {
        Vector3 start = target.position;
        Vector3 end = new Vector3(
            destination.position.x,
            destination.position.y,
            start.z);

        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;

            float blend = Mathf.SmoothStep(0, 1, t / duration);

            target.position = Vector3.Lerp(start, end, blend);

            yield return null;
        }
    }
}



