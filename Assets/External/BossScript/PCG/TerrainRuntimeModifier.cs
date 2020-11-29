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

    int size = 5; // the diameter of terrain portion that will raise under the game object
    float desiredHeight = .4f; // the height we want that portion of terrain to be
    float[,] startingHeights;

    bool firstTouch = true, ready = true;
    JumpButtonScript jumpTrigger;
    PlayerMove player;

    int circleRadius = 12;
    int innerCircleRadius = 7;
    public float paintWeight = 0.001f;
    public float rayTimeInterval = 0.1f;
    public float scaleFactor = 1f;
    public float maxHight = 1f;
    Vector2 userInput = Vector2.up;
    private float rayTimer = 0;

    private TerrainData terrainData;
    private Vector3 terrainSize;

    void Start()
    {

        terr = Terrain.activeTerrain;
        hmWidth = terr.terrainData.heightmapResolution;
        hmHeight = terr.terrainData.heightmapResolution;
        terrainData = terr.terrainData;
        terrainSize = terr.terrainData.size;
        startingHeights = terr.GetComponent<PerlinTerrain>().GetH();
        jumpTrigger = GameObject.Find("Jump_Button").GetComponent<JumpButtonScript>();
        player = GetComponentInParent<PlayerMove>();

    }

    void FixedUpdate()
    {
        rayTimer += Time.fixedDeltaTime;

        if (rayTimer < rayTimeInterval)
            return;

        rayTimer = 0;
        

        if (!player.jumping)
            EditCircle();
    }

    void EditCircle() {
        
            int offset = size / 2;
            circleRadius = offset;
            innerCircleRadius = (int)(circleRadius / 1.5f);
            float[,] heights = null;
            if (jumpTrigger.jumpButtonHold && !firstTouch)
            {
                player.gameObject.transform.position = Vector3.Lerp(player.gameObject.transform.position, player.gameObject.transform.position + Vector3.up * .1f, Time.fixedDeltaTime);

                Vector3 tempCoord = (transform.position - terr.gameObject.transform.position);
                Vector3 coord;
                coord.x = tempCoord.x / terr.terrainData.size.x;
                coord.y = tempCoord.y / terr.terrainData.size.y;
                coord.z = tempCoord.z / terr.terrainData.size.z;

                // get the position of the terrain heightmap where this game object is
                posXInTerrain = Mathf.RoundToInt(coord.x * hmWidth);
                posYInTerrain = Mathf.RoundToInt(coord.z * hmHeight);

                if (startingHeights == null) startingHeights = terr.GetComponent<PerlinTerrain>().GetH();

                heights = terr.terrainData.GetHeights(posXInTerrain - offset, posYInTerrain - offset, size, size);

                userInput = player.jsMovement.InputDirection;

                for (int i = -circleRadius; i < circleRadius; i++)
                    for (int j = -circleRadius; j < circleRadius; j++)
                    {
                        Vector2 calc = new Vector2(i, j);
                        if ((Vector2.Distance(calc, userInput * circleRadius) > scaleFactor && Vector2.Distance(calc, -userInput * circleRadius) > scaleFactor && userInput.x!=0&&userInput.y!=0)||
                        (Vector2.Distance(calc, new Vector2(userInput.y, userInput.x) * circleRadius) > scaleFactor && Vector2.Distance(calc, -new Vector2(userInput.y, userInput.x) * circleRadius) > scaleFactor && (userInput.x == 0 || userInput.y == 0))
                        ||( userInput.x == 0 && userInput.y == 0))
                        {
                            // for a circle, calcualate a relative Vector2
                            // check if the magnitude is within the circle radius
                            if (calc.magnitude <= circleRadius && calc.magnitude > innerCircleRadius)
                            {

                                if (heights[i + circleRadius, j + circleRadius] < startingHeights[posYInTerrain + i, posXInTerrain + j] + maxHight)
                                    heights[i + circleRadius, j + circleRadius] += paintWeight;
                                        //heights[i + circleRadius, j + circleRadius] = startingHeights[posYInTerrain - offset + i, posXInTerrain - offset + j] + paintWeight;

                                    //heights[i, j] = startingHeights[posYInTerrain - offset + i, posXInTerrain - offset + j] + 1;
                            }
                            else if (heights[i + circleRadius, j + circleRadius] > startingHeights[posYInTerrain + i, posXInTerrain + j])
                                heights[i + circleRadius, j + circleRadius] -= paintWeight;
                        }
                        //if (j > size - 3 || j < 3 || i < 3 || i > size - 3)
                        //else heights[i, j] = startingHeights[posYInTerrain - offset + i, posXInTerrain - offset + j];
                    }


                terrainData.SetHeights(posXInTerrain - offset, posYInTerrain - offset, heights);


            }
            else if (jumpTrigger.jumpButtonHold && firstTouch && ready)
            {
                player.gameObject.transform.position = Vector3.Lerp(player.gameObject.transform.position, player.gameObject.transform.position + Vector3.up * .5f, Time.fixedDeltaTime * 5);
                StartCoroutine(firstTouchToFalse());


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

            userInput = player.jsMovement.InputDirection;

            for (int i = -circleRadius; i < circleRadius; i++)
                for (int j = -circleRadius; j < circleRadius; j++)
                {
                    Vector2 calc = new Vector2(i, j);
                    
                    
                        // for a circle, calcualate a relative Vector2
                        // check if the magnitude is within the circle radius
                        if (calc.magnitude < circleRadius && heights[i + circleRadius, j + circleRadius] > startingHeights[posYInTerrain + i, posXInTerrain + j])
                                heights[i + circleRadius, j + circleRadius] -= paintWeight;
                    //if (j > size - 3 || j < 3 || i < 3 || i > size - 3)
                    //else heights[i, j] = startingHeights[posYInTerrain - offset + i, posXInTerrain - offset + j];
                }

            terr.terrainData.SetHeights(posXInTerrain - offset, posYInTerrain - offset, heights);

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