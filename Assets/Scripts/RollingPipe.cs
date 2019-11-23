using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingPipe : MonoBehaviour
{
    public bool jump;
    float lerpValue=0;
    public float velocity;
    public float jumpPeakHeight;
    public Vector3 tileA, tileB;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (jump)
        {
            lerpValue += Time.deltaTime; 
            float jumpLerpValue = -4f * lerpValue * lerpValue + 4f * lerpValue;
            float jumpHeight = Mathf.Lerp(0f, jumpPeakHeight, jumpLerpValue);
            transform.position = Vector3.Lerp(tileA, tileB + new Vector3(0f, jumpHeight, 0f), lerpValue* velocity);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,0,-90), lerpValue / 13);
        }
    }
}
