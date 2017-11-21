using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamTurretController : MonoBehaviour {
  public GameObject[] beamTurrets;
  public GameObject target;

	// Update is called once per frame
	void Update () {
		for (int i = 0; i < beamTurrets.Length; i++) {
      GameObject turret = beamTurrets[i];
      turret.SendMessage("Target", target.transform.position);
    }
	}
}
