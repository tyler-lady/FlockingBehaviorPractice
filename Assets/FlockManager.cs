using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public GameObject fishPrefab; //will hold our fish prefab for instantiation
    public int numFish = 50; //number of fish to be created
    public GameObject[] allFish; //array that will hold our created fish
    public Vector3 swimLimits = new Vector3(5, 5, 5); //meant to be a box around our flock manager where they can swim, can function as a container for fish
    public Vector3 goalPos;

    [Header("Fish Settings")] //this gives us header in the inspector displaying the string (very cool)
    [Range(0.0f, 5.0f)] //this gives us a slider for the below public float in the inspector
    public float minSpeed; //is assigning a variable connected to the range above thru the inspector
    [Range(0.0f, 5.0f)] //we can set values of 0-5 in the inspector
    public float maxSpeed; //is assigning a variable connected to the range above thru the inspector
    [Range(0.0f, 20.0f)]
    public float neighbourDistance;
    [Range(0.0f, 5.0f)]
    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        allFish = new GameObject[numFish]; //creates our array will all the fish
        for (int i = 0; i < numFish; i++) {
            Vector3 pos = this.transform.position + new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
                                                                Random.Range(-swimLimits.y, swimLimits.y),
                                                                Random.Range(-swimLimits.z, swimLimits.z));
            allFish[i] = (GameObject)Instantiate(fishPrefab, pos, Quaternion.identity);
            allFish[i].GetComponent<Flock>().myManager = this; //links our two files together
        }
        // for each fish, create a position for them inside our swimLimits, then instantiate the fish using our fish prefab
        goalPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.Range(0, 100) < 10) //creates a conditional based on a 10% chance. Very light work needed to check that every fram
        {
            goalPos = this.transform.position + new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
                                                                Random.Range(-swimLimits.y, swimLimits.y),
                                                                Random.Range(-swimLimits.z, swimLimits.z));
        }
    }
}
