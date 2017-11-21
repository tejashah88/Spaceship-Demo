using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller_enemy : MonoBehaviour {
public bool warping = false;
  public WarpAnimator warpFx;
  public float speedMod = 5f;
  public GameObject shipObject;
  public GameObject phaserPrefab;
  public GameObject beamPrefab;

  public Animator shipAnimator;

  public AudioClip phaserSound;
  public AudioClip warpSound;
  public AudioClip warpReverseSound;

  public AudioSource phaserSoundSource;
  public AudioSource warpSoundSource;
  public AudioSource warpReverseSoundSource;

  public GameObject beamBoltObj = null;
  public float maxBank = 15.0f;
  private bool isShootingFromRight;
  private bool clearToBeam;

  public Transform beamTransformParent;

  public Transform leftGun;
  public Transform rightGun;

  private bool isFiringBeam = false;

  private bool isPhaserOnCooldown;
  private float currentPhaserCooldown;
  public float maxPhaserCooldown = 0.25f;

  public float normalSpeed;
  public float warpSpeed;

  private bool definitelyNotWarping = true;
  public bool inTransitionOfWarping = false;

  public void playWarpSoundEnter() {
    warpSoundSource.PlayOneShot(warpSound);
  }

  public void playWarpSoundExit() {
    warpReverseSoundSource.PlayOneShot(warpReverseSound);
  }

  IEnumerator ToggleWarp() {
    warping = !warping;

    if (!inTransitionOfWarping) {
      inTransitionOfWarping = true;

      if (warping) {
        playWarpSoundEnter();
        definitelyNotWarping = false;
        isFiringBeam = false;

        yield return new WaitForSeconds(0.5f);

        //yield return warpFx._engage();

        for (float percentage = 0; percentage <= 1.05f; percentage += 0.05f) {
          speedMod = Mathf.Lerp(normalSpeed, warpSpeed, percentage);
        }

        speedMod = warpSpeed;
      } else {
        playWarpSoundExit();
        yield return new WaitForSeconds(1.8f);

        //warpFx.Disengage();

        for (float percentage = 0; percentage <= 1.02f; percentage += 0.02f) {
          speedMod = Mathf.Lerp(warpSpeed, normalSpeed, percentage);
          yield return null;
        }

        speedMod = normalSpeed;
        definitelyNotWarping = true;
      }

      inTransitionOfWarping = false;
    }
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

  float minWarpDist = 300;

  public GameObject player;

  private int leftBankState = 0;
  private int rightBankState = 0;

  public Animator anim;

  void Update () {
    float movementMod = 50;

    if (beamBoltObj != null) {
      beamBoltObj.transform.localEulerAngles = shipObject.transform.localEulerAngles / 2;
    }

    Vector3 targetDir = player.transform.position - this.transform.position;
    float step = 1 * Time.deltaTime;
    Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
    Debug.DrawRay(transform.position, newDir * minWarpDist, Color.red);
    transform.rotation = Quaternion.LookRotation(newDir);

    float moveVertical = Mathf.Clamp(Time.time, 1.0F, 3.0F);

    /*if (AnimatorOnStates("Idle")) {
      Vector3 localEulers = shipObject.transform.localEulerAngles;
      localEulers.z = definitelyNotWarping ? -Input.GetAxis("Horizontal") * maxBank : 0;
      shipObject.transform.localEulerAngles = localEulers;
    }*/
    if (!inTransitionOfWarping) {
      if (definitelyNotWarping && (player.transform.position - this.transform.position).sqrMagnitude > minWarpDist * minWarpDist) {
        StartCoroutine("ToggleWarp");
      }

      if (!definitelyNotWarping && (player.transform.position - this.transform.position).sqrMagnitude < minWarpDist * minWarpDist) {
        StartCoroutine("ToggleWarp");
      }
    }

    if (Physics.Raycast(this.transform.position, this.transform.forward, 100f) && !isFiringBeam && definitelyNotWarping) {
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

    this.transform.Translate(Vector3.forward * speedMod * Time.deltaTime);
  }
}
