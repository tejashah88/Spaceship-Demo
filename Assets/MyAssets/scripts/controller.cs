using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller : MonoBehaviour {
    public bool warping = false;
    public WarpAnimator warpFx;
    public float speedMod = 5f;
    public GameObject shipObject;
    public GameObject phaserPrefab;
    public GameObject beamPrefab;

    public Animator shipAnimator;

    public AudioClip beamSound;
    public AudioClip phaserSound;
    public AudioClip warpSound;
    public AudioClip warpReverseSound;

    public AudioSource beamSoundSource;
    public AudioSource phaserSoundSource;
    public AudioSource warpSoundSource;
    public AudioSource warpReverseSoundSource;

    public GameObject beamBoltObj = null;
    public float maxBank = 15.0f;
    private bool isShootingFromRight;

    public SmoothFollow2 sf2;

    public Transform beamTransformParent;

    public Transform leftGun;
    public Transform rightGun;

    private bool isFiringBeam = false;

    private bool isPhaserOnCooldown;
    private float currentPhaserCooldown;
    public float maxPhaserCooldown = 0.25f;

    public float lowSf2Distance;
    public float highSf2Distance;

    public float normalSpeed;
    public float warpSpeed;

    public Material[] skyboxMaterials;
    private List<Material> finalSkyboxMaterials;
    public int skyboxBlendIndex = 0;

    private bool definitelyNotWarping = true;
    public bool inTransitionOfWarping = false;

    private Shader blendShader;

    public static Material CreateBlendingSkyboxMaterial(Material startMat, Material finishMat) {
        Material result = new Material(Shader.Find("Skybox/Blended"));
        result.SetTexture("_FrontTex", startMat.GetTexture("_FrontTex"));
        result.SetTexture("_BackTex", startMat.GetTexture("_BackTex"));
        result.SetTexture("_LeftTex", startMat.GetTexture("_LeftTex"));
        result.SetTexture("_RightTex", startMat.GetTexture("_RightTex"));
        result.SetTexture("_UpTex", startMat.GetTexture("_UpTex"));
        result.SetTexture("_DownTex", startMat.GetTexture("_DownTex"));

        result.SetTexture("_FrontTex2", finishMat.GetTexture("_FrontTex"));
        result.SetTexture("_BackTex2", finishMat.GetTexture("_BackTex"));
        result.SetTexture("_LeftTex2", finishMat.GetTexture("_LeftTex"));
        result.SetTexture("_RightTex2", finishMat.GetTexture("_RightTex"));
        result.SetTexture("_UpTex2", finishMat.GetTexture("_UpTex"));
        result.SetTexture("_DownTex2", finishMat.GetTexture("_DownTex"));
        return result;
    }

    void Start() {
        finalSkyboxMaterials = new List<Material>();
        blendShader = Shader.Find("Skybox/Blended");

        if (skyboxMaterials != null && skyboxMaterials.Length > 0) {
            for (int index = 0; index < skyboxMaterials.Length - 1; index++) {
                Material startMat = skyboxMaterials[index];
                Material finishMat = skyboxMaterials[index + 1];
                finalSkyboxMaterials.Add(CreateBlendingSkyboxMaterial(startMat, finishMat));
            }

            finalSkyboxMaterials.Add(
                CreateBlendingSkyboxMaterial(
                    skyboxMaterials[skyboxMaterials.Length - 1], skyboxMaterials[0]
                )
            );
        }
    }

    public float timeWaitForBlending;
    private bool shouldBlendSkyboxes = false;

    IEnumerator BlendSkyboxWhileWarping() {
        shouldBlendSkyboxes = true;
        while (shouldBlendSkyboxes) {
            yield return new WaitForSeconds(timeWaitForBlending);

            if (!shouldBlendSkyboxes)
            break;

            for (float progress = 0.0f; progress <= 1f; progress += Time.deltaTime) {
                Material renderedMat = finalSkyboxMaterials[skyboxBlendIndex];
                renderedMat.SetFloat("_Blend", progress);

                RenderSettings.skybox = renderedMat;

                yield return null;
            }

            skyboxBlendIndex++;

            if (skyboxBlendIndex == finalSkyboxMaterials.Count)
            skyboxBlendIndex = 0;

            yield return null;
        }
    }

    IEnumerator ToggleWarp() {
        warping = !warping;

        if (!inTransitionOfWarping) {
            inTransitionOfWarping = true;

            if (warping) {
                playWarpSoundEnter();
                definitelyNotWarping = false;
                isFiringBeam = false;
                ControlBeam(false);

                yield return new WaitForSeconds(0.5f);

                yield return warpFx._engage();

                //yield return new WaitForSeconds(1f);

                for (float percentage = 0; percentage <= 1.05f; percentage += 0.05f) {
                    speedMod = Mathf.Lerp(normalSpeed, warpSpeed, percentage);
                    sf2.distance = Mathf.Lerp(highSf2Distance, lowSf2Distance, percentage);
                    // yield return new WaitForSeconds(0.01f);
                }

                StartCoroutine("BlendSkyboxWhileWarping");

                speedMod = warpSpeed;
                sf2.distance = lowSf2Distance;
            } else {
                playWarpSoundExit();

                shouldBlendSkyboxes = false;
                yield return new WaitForSeconds(1.8f);

                warpFx.Disengage();

                for (float percentage = 0; percentage <= 1.02f; percentage += 0.02f) {
                    sf2.distance = Mathf.Lerp(lowSf2Distance, highSf2Distance, percentage);
                    speedMod = Mathf.Lerp(warpSpeed, normalSpeed, percentage);
                    yield return null;
                }

                sf2.distance = highSf2Distance;
                speedMod = normalSpeed;
                definitelyNotWarping = true;
            }

            inTransitionOfWarping = false;
        }
    }

    void ControlBeam(bool isFiring) {
        if (isFiring) {
            beamSoundSource.Play();
            beamBoltObj = Instantiate(beamPrefab, beamTransformParent.position, beamTransformParent.rotation);
            beamBoltObj.GetComponent<BeamBoltsController>().shouldCutOff = false;
            beamBoltObj.transform.parent = beamTransformParent;
        } else {
            beamSoundSource.Stop();
            if (beamBoltObj != null) {
                beamBoltObj.GetComponent<BeamBoltsController>().shouldCutOff = true;
                Destroy(beamBoltObj, .25f);
            }
        }
    }

    public void playWarpSoundEnter() {
        warpSoundSource.PlayOneShot(warpSound);
    }

    public void playWarpSoundExit() {
        warpReverseSoundSource.PlayOneShot(warpReverseSound);
    }

    public bool AnimatorIsPlaying() {
        AnimatorStateInfo asi = anim.GetCurrentAnimatorStateInfo(0);
        return asi.length * 0.9f > asi.normalizedTime;
    }

    public bool AnimatorOnStates(params string[] states) {
        foreach (string state in states) {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName(state)) {
                return true;
            }
        }

        return false;
    }

    private int leftBankState = 0;
    private int rightBankState = 0;

    public Animator anim;

    // Update is called once per frame
    void Update () {
        if (Input.GetKey("escape"))
        Application.Quit();

        float movementMod = 50;

        if (beamBoltObj != null) {
            beamBoltObj.transform.localEulerAngles = shipObject.transform.localEulerAngles / 2;
        }

        this.transform.Rotate(
            Vector3.right,
            definitelyNotWarping ? -Input.GetAxis("Vertical") * movementMod * Time.deltaTime : 0
        );

        this.transform.Rotate(
            Vector3.up,
            definitelyNotWarping ? Input.GetAxis("Horizontal") * movementMod * Time.deltaTime : 0
        );

        if (AnimatorOnStates("Idle")) {
            Vector3 localEulers = shipObject.transform.localEulerAngles;
            localEulers.z = definitelyNotWarping ? -Input.GetAxis("Horizontal") * maxBank : 0;
            shipObject.transform.localEulerAngles = localEulers;
        }

        if (Input.GetKeyDown("z")) {
            StartCoroutine("ToggleWarp");
        }

        /*if (Input.GetKeyDown("a") && definitelyNotWarping && AnimatorOnStates("Idle")) {
            anim.Play("Tilt Left");
            }

            if (Input.GetKeyDown("d") && definitelyNotWarping && AnimatorOnStates("Idle")) {
            anim.Play("Tilt Right");
            }

            if (Input.GetKeyUp("a") && definitelyNotWarping && AnimatorOnStates("Hold Left")) {
            anim.Play("Stabilize Left");
            }

            if (Input.GetKeyUp("d") && definitelyNotWarping && AnimatorOnStates("Hold Right")) {
            anim.Play("Stabilize Right");
            }*/

        if (Input.GetKey(KeyCode.Mouse2) && definitelyNotWarping && !AnimatorIsPlaying() && AnimatorOnStates("Idle")) {
            if (Input.GetKey(KeyCode.A)) anim.Play("Roll Left");
            else if (Input.GetKey(KeyCode.D)) anim.Play("Roll Right");
            else if (Input.GetKey(KeyCode.W)) anim.Play("Back Flip");
        }

        if (Input.GetAxis("Fire1") > 0.9f && !isFiringBeam && definitelyNotWarping) {
            // shoot phasers
            if (!isPhaserOnCooldown) {
                Transform shootableFromTransform = isShootingFromRight ? rightGun : leftGun;
                GameObject phaserBolt = Instantiate(
                    phaserPrefab,
                    shootableFromTransform.position,
                    shootableFromTransform.rotation
                );

                phaserSoundSource.PlayOneShot(phaserSound);

                isPhaserOnCooldown = true;
                currentPhaserCooldown = 0.0f;
                isShootingFromRight = !isShootingFromRight;
            }

            currentPhaserCooldown += Time.deltaTime;
            if (currentPhaserCooldown >= maxPhaserCooldown) {
                isPhaserOnCooldown = false;
            }
        }

        if (Input.GetAxis("Fire2") > 0.9f && definitelyNotWarping && !isFiringBeam) {
            // shoot beam
            isFiringBeam = true;
            ControlBeam(isFiringBeam);
        }

        if (Input.GetAxis("Fire2") < 0.05f) {
            isFiringBeam = true;
        }

        this.transform.Translate(Vector3.forward * speedMod * Time.deltaTime);
    }
}
