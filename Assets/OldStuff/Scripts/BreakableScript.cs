using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableScript : MonoBehaviour
{
    #region Attributes
    private Vector3 surfaceDimensions;
    private Vector3 surfacePosition;
    private Material surfaceMaterial;
    private GameObject[] brokenPieces;
    private Vector3[] brokenPiecesPositions;
    private int numberOfBrokenPieces = 4;
    public float collapseTime = 3f;
    private bool collapseBrokenPieces = false;
    #endregion
    private void Awake()
    {
        surfacePosition = transform.position;
        surfaceDimensions = transform.localScale;
        surfaceMaterial = transform.GetChild(0).gameObject.GetComponent<Renderer>().material;
        brokenPieces = new GameObject[numberOfBrokenPieces];
        brokenPiecesPositions = new Vector3[numberOfBrokenPieces];
    }

    private void Update()
    {
        if (collapseBrokenPieces)
        {
            foreach (GameObject g in brokenPieces)
            {
                Color color = g.GetComponent<Renderer>().material.GetColor("_BaseColor");
                color.a -= Time.deltaTime / collapseTime;
                g.GetComponent<Renderer>().material.SetColor("_BaseColor", color);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 9)
        {
            Destroy(gameObject.transform.GetChild(0).gameObject);
            BreakObject();
            StartCoroutine(DestroyPieces());
        }
    }

    IEnumerator DestroyPieces()
    {
        collapseBrokenPieces = true;
        yield return new WaitForSeconds(collapseTime);
        foreach (GameObject g in brokenPieces)
            Destroy(g);
        Destroy(gameObject);
    }

    private void BreakObject()
    {
        CalculatePositions();
        for(int i = 0; i < numberOfBrokenPieces; i++)
        {
            brokenPieces[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            brokenPieces[i].transform.localScale = surfaceDimensions * 0.5f;
            brokenPieces[i].transform.position = brokenPiecesPositions[i];
            brokenPieces[i].transform.GetComponent<Renderer>().material = surfaceMaterial;
            brokenPieces[i].AddComponent<Rigidbody>().useGravity = true;
        }
        gameObject.GetComponent<BoxCollider>().enabled = false;
    }

    private void CalculatePositions()
    {
        brokenPiecesPositions[0] = surfacePosition + new Vector3(-surfaceDimensions.x * 0.25f, 0f, -surfaceDimensions.z * 0.25f);
        brokenPiecesPositions[1] = surfacePosition + new Vector3(-surfaceDimensions.x * 0.25f, 0f, +surfaceDimensions.z * 0.25f);
        brokenPiecesPositions[2] = surfacePosition + new Vector3(+surfaceDimensions.x * 0.25f, 0f, -surfaceDimensions.z * 0.25f);
        brokenPiecesPositions[3] = surfacePosition + new Vector3(+surfaceDimensions.x * 0.25f, 0f, +surfaceDimensions.z * 0.25f);
    }
}
