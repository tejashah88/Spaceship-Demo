using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpAnimator : MonoBehaviour {
    public float rotMod;

    public float lowStartSpeed;
    public float highStartSpeed;

    public float lowRateOverTime;
    public float highRateOverTime;

    public float lowLengthScale;
    public float highLengthScale;

    public float lowCameraFov;
    public float highCameraFov;

    public ParticleSystem particles;
    public ParticleSystem altParticles;

    private ParticleSystem.MainModule main;
    private ParticleSystem.EmissionModule emission;
    private ParticleSystemRenderer rrenderer;

    void Start() {
        main = particles.main;
        main.startSpeedMultiplier = lowStartSpeed;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        emission = particles.emission;
        emission.rateOverTimeMultiplier = lowRateOverTime;

        rrenderer = particles.GetComponent<ParticleSystemRenderer>();
        rrenderer.lengthScale = lowLengthScale;

        Camera.main.fieldOfView = lowCameraFov;

        particles.Play(true);
    }

    void Update() {
        altParticles.transform.Rotate(0, 0, Time.deltaTime * rotMod);
    }

    public IEnumerator _engage() {
        float initStartSpeedMult = main.startSpeedMultiplier;
        float initRateOverTime = emission.rateOverTimeMultiplier;
        float initLengthScale = rrenderer.lengthScale;
        float initCameraFov = Camera.main.fieldOfView;

        float percentage;

        main.prewarm = false;

        for (percentage = 0f; percentage <= 1.02f; percentage += 0.02f) {
            rrenderer.lengthScale = Mathf.Lerp(initLengthScale, highLengthScale, percentage);
            main.startSpeedMultiplier = Mathf.Lerp(initStartSpeedMult, highStartSpeed, percentage);
            emission.rateOverTimeMultiplier = Mathf.Lerp(initRateOverTime, highRateOverTime, percentage);
            yield return new WaitForSeconds(0.045f);
        }

        altParticles.Play(false);
        yield return new WaitForSeconds(0.2f);

        for (percentage = 0f; percentage <= 1.2f; percentage += 0.2f) {
            Camera.main.fieldOfView = Mathf.Lerp(initCameraFov, highCameraFov, percentage);
            yield return null;
        }

        Debug.Log("on");
    }

    public IEnumerator _disengage() {
        float initStartSpeedMult = main.startSpeedMultiplier;
        float initRateOverTime = emission.rateOverTimeMultiplier;
        float initLengthScale = rrenderer.lengthScale;
        float initCameraFov = Camera.main.fieldOfView;

        float percentage;

//particles.Play(false);
        altParticles.Stop(false, ParticleSystemStopBehavior.StopEmitting);

        for (percentage = 0f; percentage <= 1.1f; percentage += 0.1f) {
            rrenderer.lengthScale = Mathf.Lerp(initLengthScale, lowLengthScale, percentage);
            main.startSpeedMultiplier = Mathf.Lerp(initStartSpeedMult, lowStartSpeed, percentage);
            yield return null;//new WaitForSeconds(0.045f);
        }

        yield return new WaitForSeconds(0.1f);

        for (percentage = 0f; percentage <= 1.15f; percentage += 0.15f) {
            Camera.main.fieldOfView = Mathf.Lerp(initCameraFov, lowCameraFov, percentage);
            yield return null;
        }

        for (percentage = 0f; percentage <= 1.01f; percentage += 0.01f) {
            emission.rateOverTimeMultiplier = Mathf.Lerp(initRateOverTime, lowRateOverTime, percentage);
            yield return null;
        }

        yield return new WaitForSeconds(0.8f);
        main.prewarm = true;
        //main.simulationSpace = ParticleSystemSimulationSpace.World;

        //particles.Stop(false, ParticleSystemStopBehavior.StopEmitting);

        Debug.Log("off");
    }

    public void Engage() {
        StartCoroutine("_engage");
    }

    public void Disengage() {
        StartCoroutine("_disengage");
    }
}
