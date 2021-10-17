using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    // Start is called before the first frame update
    public bool shouldMove;
    private int interpolationFramesCount; // Number of frames to completely interpolate between the 2 positions
    public float unitLength;
    public Vector3 startPosition;
    public Vector3 endPosition;
    int elapsedFrames = 0;
    void Awake()
    {
        shouldMove = false;
        interpolationFramesCount = 420;
        unitLength = 1.07444f;
    }

    // Update is called once per frame
    void Update()
    {
        if(shouldMove){
            float interpolationRatio = (float)elapsedFrames / interpolationFramesCount;
            transform.localPosition = Vector2.Lerp(startPosition, endPosition, interpolationRatio);
 
            elapsedFrames = (elapsedFrames + 1);
            if(elapsedFrames == interpolationFramesCount){
                elapsedFrames = 0;
                shouldMove = false;
            }
            
        }
    }
}
