using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(AudioSource))]
public class Weapon : MonoBehaviour
{
    [SerializeField]
    private float gunDmg = 5f,
                  weaponRange = 50f,
                  hitForce = 100f;

    private bool lastSecondaryInput = true;

    public Transform muzzle;

    private Camera mainCamera;

    private AudioSource gunAudioSrc;

    private LineRenderer laserLine;
    private Vector3 midScreenVector;

    private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);

    public float fireRate = 0.5f;

    [SerializeField]
    private ParticleSystem MuzzlePS, CaseEjectPS;

    [SerializeField]
    private GameObject flameThrower;
    private float currentTime;
    private float nextFireTime;

    // Start is called before the first frame update
    void Start()
    {
        midScreenVector = new Vector3(0.5f, 0.5f, 0f);
        gunAudioSrc = GetComponent<AudioSource>();
        mainCamera = GetComponentInParent<Camera>(); //Camera.main;
        laserLine = GetComponent<LineRenderer>();
        SecondaryFire();
        nextFireTime = Time.time;
    }

    public RaycastHit Fire(Transform target = null)
    {

        nextFireTime = currentTime + fireRate;

        StartCoroutine(ShotFired());

        Vector3 rayOrigin, rayDirection;

        if (target is null)
        {
            // Player Atirou
            rayOrigin = mainCamera.ViewportToWorldPoint(midScreenVector);
            rayDirection = mainCamera.transform.forward;
        }
        else
        {
            // Inimigo atirou
            rayOrigin = muzzle.position;
            rayDirection = (target.position - rayOrigin).normalized;
        }



        // Variavel de saida
        RaycastHit hit;

        // Definindo o começo do raio na ponta da nossa arma
        laserLine.SetPosition(0, muzzle.position);

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, weaponRange)) // Houve hit
        {

            // Seta o fim do raio na posicao de hit
            laserLine.SetPosition(1, hit.point);

            HealthSystem health = hit.collider.GetComponentInParent<HealthSystem>();
            health?.Damage(gunDmg);

            // Adiciona forca, havia esquecido do hit force
            hit.rigidbody?.AddForce(-hit.normal * hitForce);

        }
        else
        {
            laserLine.SetPosition(1, rayOrigin + (rayDirection * weaponRange));
        }

        return hit;

    }

    public void SecondaryFire(bool buttonIsPressed = false)
    {

        if (buttonIsPressed == lastSecondaryInput)
            return;

        lastSecondaryInput = buttonIsPressed;
        foreach (ParticleSystem ps in flameThrower.GetComponentsInChildren<ParticleSystem>())
        {
            var emission = ps.emission;
            emission.enabled = buttonIsPressed;
            // ps.emission = emission;
        }
    }

    public bool CanFire()
    {
        currentTime = Time.time;
        return currentTime > nextFireTime;
    }

    private IEnumerator ShotFired()
    {
        MuzzlePS?.Play(); // Dispara efeito de tiro
        gunAudioSrc.Play();
        laserLine.enabled = true;

        yield return shotDuration;

        CaseEjectPS?.Play();
        laserLine.enabled = false;
    }
}