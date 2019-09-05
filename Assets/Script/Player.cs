using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    [Header("General")]
    [Tooltip("In ms^-1")][SerializeField] float ControlSpeed = 4f;
    [Tooltip("In m^")][SerializeField] float xRange = 4f;
    [Tooltip("In m^")][SerializeField] float yRange = 3f;
    [SerializeField] GameObject[] guns;

    [Header("Screen-positon")]
    [SerializeField] float positionPitchFactor = -5f;
    [SerializeField] float positionYawFactor = -20f;

    [Header("Control-throw")]
    [SerializeField] float controlRollFactor = 5f;
    [SerializeField] float controlPitchFactor = -20f;

    float xThrow, yThrow;
    bool isControlEnabled = true;

    // Update is called once per frame
    void Update()
    {
        if (isControlEnabled)
        {
            ProcessTranslation();
            ProcessRotation();
            ProcessFiring();
        }
    }

    void OnPlayerDeath()
    {
        isControlEnabled = false;
    }

    void ProcessRotation()
    {
        float pitch = transform.localPosition.y * positionPitchFactor + yThrow * controlPitchFactor;

        float yaw = transform.localPosition.x * positionYawFactor;

        float roll = xThrow * controlRollFactor;
        transform.localRotation = Quaternion.Euler(pitch,yaw,roll);
    }

    void ProcessTranslation()
    {
        xThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        yThrow = CrossPlatformInputManager.GetAxis("Vertical");

        float xOffset = xThrow * ControlSpeed * Time.deltaTime;
        float yOffset = yThrow * ControlSpeed * Time.deltaTime;

        float rawXpos = transform.localPosition.x + xOffset;
        float clampedXPos = Mathf.Clamp(rawXpos, -xRange, xRange);

        float rawYpos = transform.localPosition.y + yOffset;
        float clampedYPos = Mathf.Clamp(rawYpos, -yRange, yRange);

        transform.localPosition = new Vector3(clampedXPos, clampedYPos, transform.localPosition.z);
    }
    void ProcessFiring()
    {
        if (CrossPlatformInputManager.GetButton("Fire"))
        {
            SetGunAcitve(true);
        }
        else
        {
            SetGunAcitve(false);
        }
    }

    private void SetGunAcitve(bool isActive)
    {
        foreach (GameObject gun in guns)
        {
            var emissionModule = gun.GetComponent<ParticleSystem>().emission;
            emissionModule.enabled = isActive;
        }
    }
}
