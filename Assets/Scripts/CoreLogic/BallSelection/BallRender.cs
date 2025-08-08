using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallRender : MonoBehaviour
{
    // Material change
    private MeshRenderer meshRenderer;
    

    //
    private void OnSelectBall(SelectBallEventArgs selectBallEventArgs)
    {
        meshRenderer.material = selectBallEventArgs.meshRenderer.material;
    }

    //
    private void Start()
    {
        // Initialize data
        meshRenderer = GetComponentInChildren<MeshRenderer>();

        BallSelection.instance.onSelectBall += OnSelectBall;
    }
}
