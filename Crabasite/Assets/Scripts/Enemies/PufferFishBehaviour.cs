using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PufferFishBehaviour : Enemy
{
    void Awake()
    {
        Texture2D _sprite = Resources.Load<Texture2D>("Sprites/EnemyPlaceholder_256x256");
        Sprite sprite = Sprite.Create(_sprite, //texture
                                        new Rect(0.0f, 0.0f, _sprite.width, _sprite.height), //subpart of the texture to create the sprite from
                                        new Vector2(0.5f, 0.5f), //new sprite origin \in [0,1]^2
                                        100.0f, //number of pixels in the sprite that correspond to one unit in world space
                                        0, //amount by which the sprite mesh should be expanded outwards
                                        SpriteMeshType.FullRect //mesh type
                                        );
        initialSetup(100,//health 
                        100,//max health 
                        10,//damage
                        0.5f,//speed
                        "Puffer fish enemy",//name 
                        sprite,//sprite 
                        0.5f//sprite scale modifier
                        );

        // gameObject.transform.position = new Vector3(0, 0, 0);
    }

    void Update()
    {
        float[] ctx_map_interest = chase(GameObject.FindWithTag("Player").transform.position);
        float[] ctx_map_danger = new float[8];

        //the maximum distance at which to regard obstacles
        float maxDangerDistance = 3.0f;

        // //get all possible obstacles
        // GameObject[] collectables = GameObject.FindGameObjectsWithTag("Collectable");
        // GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // //concatenate all possible obstacles into one array
        // GameObject[] obstacles = new GameObject[collectables.Length+enemies.Length];
        // collectables.CopyTo(obstacles, 0);
        // enemies.CopyTo(obstacles, collectables.Length);

        //get highest danger over all *near* obstacles 
        foreach(Vector2 _direction in getNormalizedDirectionMap()){
            // Vector2 obstaclePositionRelativeToEnemy = obstacle.transform.position-transform.position;
            // obstaclePositionRelativeToEnemy.Normalize();
            Vector3 direction = new Vector3(_direction.x, _direction.y, 0);
            BoxCollider2D bc = getCollider();
            bc.enabled = false;
            //take only obstacles into account that are a maximum distance away
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.position+direction, maxDangerDistance, LayerMask.GetMask("Enemies"));
            bc.enabled = true;
            if(hit.collider != null){
                float[] this_ctx_map_danger = avoid(hit.point);
                for(int i = 0; i < 8; ++i){
                    ctx_map_danger[i] = Mathf.Max(ctx_map_danger[i], this_ctx_map_danger[i]);
                }
            }
        }
        // foreach(GameObject obstacle in obstacles){
        //     //check, if the obstacle's collider can be hit via raycast
        //     Vector2 obstaclePositionRelativeToEnemy = obstacle.transform.position-transform.position;
        //     obstaclePositionRelativeToEnemy.Normalize();
        //     BoxCollider2D bc = getCollider();
        //     bc.enabled = false;
        //     //take only obstacles into account that are a maximum distance away
        //     RaycastHit2D hit = Physics2D.Raycast(transform.position, obstaclePositionRelativeToEnemy, maxDangerDistance, LayerMask.GetMask("Enemies"));
        //     bc.enabled = true;
        //     if(hit.collider != null){
        //         float[] this_ctx_map_danger = avoid(hit.point);
        //         for(int i = 0; i < 8; ++i){
        //             ctx_map_danger[i] = Mathf.Max(ctx_map_danger[i], this_ctx_map_danger[i]);
        //         }
        //     }
        // }

        // float minimal_danger = Mathf.Infinity;
        // for(int i = 0; i < 8; ++i){
        //     if(minimal_danger > ctx_map_danger[i])
        //         minimal_danger = ctx_map_danger[i];
        // }

        //combine the context maps for danger and interest
        float[] ctx_map_combined = new float[8];
        for(int i = 0; i < 8; ++i){
            ctx_map_combined[i] = ctx_map_interest[i]-ctx_map_danger[i];
            // if(ctx_map_danger[i] > minimal_danger)
            //     ctx_map_combined[i] = 0;
            // else
            //     ctx_map_combined[i] = ctx_map_interest[i];
        }
        Debug.Log("interest:");
        printArray(ctx_map_interest);
        Debug.Log("danger:");
        printArray(ctx_map_danger);
        Debug.Log("combined:");
        printArray(ctx_map_combined);
        getRigidbody().AddForce(getDirectionWithMaximalInterest(getNormalizedDirectionMap(), ctx_map_combined)*getSpeed());
    }    

    float[] chase(Vector2 target){
        Vector2[] direction_map = getNormalizedDirectionMap();
        float[] ctx_map_interest = new float[8];
        Vector2 positionRelativeToTarget = target-new Vector2(transform.position.x, transform.position.y);
        positionRelativeToTarget.Normalize();
        for(int i = 0; i < 8; ++i){
            // ctx_map_interest[i] = Mathf.Clamp(Vector2.Dot(direction_map[i], positionRelativeToTarget),0,1);
            ctx_map_interest[i] = (1+Vector3.Dot(direction_map[i], positionRelativeToTarget))/2;//rescale [-1,1] -> [0,1]
        }
        return ctx_map_interest;
    }

    float[] avoid(Vector2 obstacle){
        Vector2[] direction_map = getNormalizedDirectionMap();
        float[] ctx_map_danger = new float[8];
        Vector2 positionRelativeToObstacle = obstacle-new Vector2(transform.position.x, transform.position.y);
        positionRelativeToObstacle.Normalize();
        for(int i = 0; i < 8; ++i){
            // ctx_map_danger[i] = Mathf.Clamp(Vector3.Dot(direction_map[i], positionRelativeToObstacle),0,1);
            ctx_map_danger[i] = (1+Vector2.Dot(direction_map[i], positionRelativeToObstacle))/2;//rescale [-1,1] -> [0,1]
        }
        return ctx_map_danger;
    }

    private static Vector2[] getNormalizedDirectionMap(){
        Vector2[] direction_map = new Vector2[]{new Vector2(0, 1),
                                                new Vector2(.5f, .5f),
                                                new Vector2(1, 0),
                                                new Vector2(.5f, -.5f),
                                                new Vector2(0, -1),
                                                new Vector2(-.5f, -.5f),
                                                new Vector2(-1, 0),
                                                new Vector2(-.5f, .5f)
                                                };
        foreach(Vector2 vec in direction_map){
            vec.Normalize();
        }
        return direction_map;
    }

    static Vector2 getDirectionWithMaximalInterest(Vector2[] direction_map, float[] ctx_map_interest){
        Vector2 result = Vector2.zero;
        float highest_interest = -Mathf.Infinity;
        for(int i = 0; i < 8; ++i){
            if(ctx_map_interest[i] > highest_interest){
                result = direction_map[i];
                highest_interest = ctx_map_interest[i];
            }
        }
        if(highest_interest > 0)
            return result*(1+highest_interest);//scale movement speed with the desire to move in this direction
        return Vector2.zero;
    }

    static void printArray(float[] array){
        string str = "[ ";
        foreach(float item in array){
            str += item+" | ";
        }
        Debug.Log(str+"]");
    }

    static void printArray(Vector2[] array){
        string str = "[ ";
        foreach(Vector2 item in array){
            str += item+" | ";
        }
        Debug.Log(str+"]");
    }
}
