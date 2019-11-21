using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneResizer : MonoBehaviour
{
    bool active;
    public List<Transform> bones;
    Vector3 direction;
    GameObject Root;
    // Start is called before the first frame update
    void Start()
    {
        Root = GameObject.Find("Root");
        for (int i=0; i<Root.transform.childCount; i++) bones.Add(Root.transform.GetChild(i));
    }

    // Update is called once per frame
    void Update()
    {
        direction = Vector3.zero;
        if (active)
        {
            foreach (Transform bone in bones) {
                if(Mathf.Abs(bone.position.x)> Mathf.Abs(transform.position.x + transform.localScale.x / 2))
                {
                    direction -= Vector3.right * (Mathf.Abs(bone.position.x) + Mathf.Abs(transform.position.x + transform.localScale.x / 2));
                }
                if(Mathf.Abs(bone.position.y) > Mathf.Abs(transform.position.y + transform.localScale.y / 2))
                {
                    direction += Vector3.up * (Mathf.Abs(bone.position.y) - Mathf.Abs(transform.position.y + transform.localScale.y / 2));
                }
                if (Mathf.Abs(bone.position.z) > Mathf.Abs(transform.position.z + transform.localScale.z / 2))
                {
                    direction += Vector3.forward * (Mathf.Abs(bone.position.z) + Mathf.Abs(transform.position.z + transform.localScale.z / 2));
                }
                bone.position = Vector3.Lerp(bone.position, bone.position + direction, Time.deltaTime / 5f);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") active = true;
        other.GetComponent<Rigidbody>().isKinematic = true;
        foreach (Transform bone in bones)
        {
            bone.GetComponent<JellyBone>().enabled = false;
            bone.GetComponent<SphereCollider>().enabled = false;
            bone.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player") active = false;
        other.GetComponent<Rigidbody>().isKinematic = false;

        foreach (Transform bone in bones)
        {
            bone.GetComponent<JellyBone>().enabled = true;
            bone.GetComponent<SphereCollider>().enabled = true;

            bone.GetComponent<Rigidbody>().isKinematic = false;
        }

    }
}
