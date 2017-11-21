using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicControls : MonoBehaviour {
	public WarpAnimator wa;

  public bool warping;

	void Update () {
    if (Input.GetKeyDown("space")) {
      warping = !warping;
      if (warping) {
        wa.Engage();
      } else {
        wa.Disengage();
      }
    }
	}
}
