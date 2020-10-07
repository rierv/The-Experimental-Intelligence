using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainRuntimeModifier : MonoBehaviour
{
    Terrain terr; // terrain to modify
    int hmWidth; // heightmap width
    int hmHeight; // heightmap height

    int posXInTerrain; // position of the game object in terrain width (x axis)
    int posYInTerrain; // position of the game object in terrain height (z axis)

    int size = 14; // the diameter of terrain portion that will raise under the game object
    float desiredHeight = .5f; // the height we want that portion of terrain to be
    float[,] startingHeights;

    float angle = 160;
    float speed = 1;// (2 * Mathf.PI); //2*PI in degress is 360, so you get 5 seconds to complete a circle
    float radius = 1f;
    float x, y;
    bool firstTouch = true, ready = true;
    JumpButtonScript jumpTrigger;
    PlayerMove player;
    void Start()
    {

        terr = Terrain.activeTerrain;
        hmWidth = terr.terrainData.heightmapResolution;
        hmHeight = terr.terrainData.heightmapResolution;
        startingHeights = terr.GetComponent<PerlinTerrain>().GetH();
        jumpTrigger = GameObject.Find("Jump_Button").GetComponent<JumpButtonScript>();
        player = GetComponentInParent<PlayerMove>();
    }

    void FixedUpdate()
    {
        if (!player.jumping)
        {
            int offset = size / 2;
            float[,] heights = null;
            if (jumpTrigger.jumpButtonHold && !firstTouch)
            {
                player.gameObject.transform.position = Vector3.Lerp(player.gameObject.transform.position, player.gameObject.transform.position+ Vector3.up*2f, Time.fixedDeltaTime);

                Vector3 tempCoord = (transform.position - terr.gameObject.transform.position);
                Vector3 coord;
                coord.x = tempCoord.x / terr.terrainData.size.x;
                coord.y = tempCoord.y / terr.terrainData.size.y;
                coord.z = tempCoord.z / terr.terrainData.size.z;

                // get the position of the terrain heightmap where this game object is
                posXInTerrain = (int)(coord.x * hmWidth);
                posYInTerrain = (int)(coord.z * hmHeight);

                if (startingHeights == null) startingHeights = terr.GetComponent<PerlinTerrain>().GetH();

                heights = terr.terrainData.GetHeights(posXInTerrain - offset, posYInTerrain - offset, size, size);


                for (int i = 0; i < size; i++)
                    for (int j = 0; j < size; j++)
                    {
                        //if (j > size - 3 || j < 3 || i < 3 || i > size - 3)
                        heights[i, j] = startingHeights[posYInTerrain - offset + i, posXInTerrain - offset + j] + 0.08f;
                        //else heights[i, j] = startingHeights[posYInTerrain - offset + i, posXInTerrain - offset + j];
                    }





                terr.terrainData.SetHeights(posXInTerrain - offset, posYInTerrain - offset, heights);


            }
            else if (jumpTrigger.jumpButtonHold && firstTouch && ready)
            {
                player.gameObject.transform.position = Vector3.Lerp(player.gameObject.transform.position, player.gameObject.transform.position + Vector3.up * 5f, Time.fixedDeltaTime*10);
                StartCoroutine(firstTouchToFalse());
                /*angle = 0; //if you want to switch direction, use -= instead of +=
                x = Mathf.Cos(angle) * radius;
                y = Mathf.Sin(angle) * radius;
                transform.localPosition = new Vector3(x, transform.localPosition.y, y);
                while (angle < 360)
                {
                    angle += speed * Time.deltaTime; //if you want to switch direction, use -= instead of +=
                    x = Mathf.Cos(angle) * radius;
                    y = Mathf.Sin(angle) * radius;
                    transform.localPosition = new Vector3(x, transform.localPosition.y, y);
                    // get the normalized position of this game object relative to the terrain
                    Vector3 tempCoord = (transform.position - terr.gameObject.transform.position);
                    Vector3 coord;
                    coord.x = tempCoord.x / terr.terrainData.size.x;
                    coord.y = tempCoord.y / terr.terrainData.size.y;
                    coord.z = tempCoord.z / terr.terrainData.size.z;

                    // get the position of the terrain heightmap where this game object is
                    posXInTerrain = (int)(coord.x * hmWidth);
                    posYInTerrain = (int)(coord.z * hmHeight);

                    // we set an offset so that all the raising terrain is under this game object
                    startingHeights = terr.GetComponent<PerlinTerrain>().GetH();

                    transform.position = new Vector3(transform.position.x, startingHeights[posYInTerrain, posXInTerrain] * terr.terrainData.size.y, transform.position.z);

                    // get the heights of the terrain under this game object
                    heights = terr.terrainData.GetHeights(posXInTerrain - offset, posYInTerrain - offset, size, size);

                    // we set each sample of the terrain in the size to the desired height

                    for (int i = 0; i < size; i++)
                        for (int j = 0; j < size; j++)
                            heights[i, j] = transform.position.y / terr.terrainData.size.y + 0.06f;

                    // go raising the terrain slowly
                    Debug.Log(desiredHeight + " " + transform.position.y);
                    if (desiredHeight < transform.position.y / terr.terrainData.size.y + 0.03f)
                        desiredHeight += Time.fixedDeltaTime / 2;
                    else if (desiredHeight > transform.position.y / terr.terrainData.size.y + 0.05f)
                        desiredHeight -= Time.fixedDeltaTime / 2;

                    // set the new height     
                    terr.terrainData.SetHeights(posXInTerrain - offset, posYInTerrain - offset, heights);


                } */

            }
            else if (!jumpTrigger.jumpButtonHold)
            {
                firstTouch = true;
                Vector3 tempCoord = (transform.position - terr.gameObject.transform.position);
                Vector3 coord;
                coord.x = tempCoord.x / terr.terrainData.size.x;
                coord.y = tempCoord.y / terr.terrainData.size.y;
                coord.z = tempCoord.z / terr.terrainData.size.z;

                // get the position of the terrain heightmap where this game object is
                posXInTerrain = (int)(coord.x * hmWidth);
                posYInTerrain = (int)(coord.z * hmHeight);

                if (startingHeights == null) startingHeights = terr.GetComponent<PerlinTerrain>().GetH();

                heights = terr.terrainData.GetHeights(posXInTerrain - offset, posYInTerrain - offset, size, size);


                for (int i = 0; i < size; i++)
                    for (int j = 0; j < size; j++)
                    {
                        heights[i, j] = startingHeights[posYInTerrain - offset + i, posXInTerrain - offset + j];
                    }





                terr.terrainData.SetHeights(posXInTerrain - offset, posYInTerrain - offset, heights);

            }
        }

    }
    IEnumerator firstTouchToFalse()
    {
        ready = false;
        yield return new WaitForSeconds(0.1f);
        ready = true;
        firstTouch = false;
    }
}