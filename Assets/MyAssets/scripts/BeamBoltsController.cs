﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamBoltsController : MonoBehaviour {
    public bool shouldCutOff = false;
    public BeamParam left;
    public BeamParam right;

    void Update () {
        left.bEnd = shouldCutOff;
        right.bEnd = shouldCutOff;
    }
}
