using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockManager myManager; //link to our manager
    float speed;
    bool turning = false; //become true when swim limits reached

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        //bounding box of the manager cube //Bounds is an invisible rectangular prism, created around a point, and at a certain size
        Bounds b = new Bounds(myManager.transform.position, myManager.swimLimits * 2);

        //if fish leaves bounding box or is about to collide w/ object, turn it around
        RaycastHit hit = new RaycastHit(); //we create a raycast hit
        Vector3 direction = Vector3.zero;
        if (!b.Contains(transform.position))
        {
            turning = true;
            direction = myManager.transform.position - transform.position;
        }
        else if (Physics.Raycast(transform.position, this.transform.forward * 50, out hit))
        {
            turning = true;
            //Debug.DrawRay(this.transform.position, this.transform.forward * 50, Color.red); //draws the projected ray to show us correct collision prediction
            direction = Vector3.Reflect(this.transform.forward, hit.normal);
        }
        else
        {
            turning = false;
        }
        if (turning)
        {
            //turn towards center of bounding box
            
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), myManager.rotationSpeed * Time.deltaTime);
        }
        else
        {
            //can use conditionals here with random % to make things more spontaneous
            if (Random.Range(0, 100) < 10)
            {
                speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
            }
            if (Random.Range(0, 100) < 20) //could also use an invoke function here
            {
                ApplyRules();
            }
        }
        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    void ApplyRules()
    {
        GameObject[] gos; //array for all of our current flock's fish
        gos = myManager.allFish; //here we fill the array from myManager

        Vector3 vcentre = Vector3.zero; //we initialize our vectors as zeroes
        Vector3 vavoid = Vector3.zero;
        float gSpeed = 0.01f; //global speed of the group
        float nDistance; //neighbour distance
        int groupSize = 0; //initialized group size

        foreach (GameObject go in gos) //loop thru all of the flock's fish
        {
            if (go != this.gameObject) //as long as the fish isn't our fish (the one running the script) do this 
            {
                nDistance = Vector3.Distance(go.transform.position, this.transform.position); //distance from the loop fish to our current fish
                if (nDistance <= myManager.neighbourDistance) //if that distance is inside of our desired neighbour distancce
                {
                    vcentre += go.transform.position; //adds fish position to the vcentre vector
                    groupSize++; //fish is added to group size

                    if (nDistance < 1.0f) //distance between fish (my bubble)
                    {
                        vavoid = vavoid + (this.transform.position - go.transform.position); //adds to the current avoid vector the position of the loop fish
                    }

                    Flock anotherFlock = go.GetComponent<Flock>(); //grabbing the other fishes code to pull speed
                    gSpeed = gSpeed + anotherFlock.speed; //using the fish's speed to add to the global
                }
            }
        }

        if (groupSize > 0)
        {
            vcentre = vcentre / groupSize + (myManager.goalPos - this.transform.position); //here we added a goal position vector to the center of the group vector to create a goal for the overall movement of teh group
            speed = gSpeed / groupSize;

            Vector3 direction = (vcentre + vavoid) - transform.position; //finding the direction our fish needs to go
            if (direction != Vector3.zero) //if desired direction is not 0
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), myManager.rotationSpeed * Time.deltaTime); //perform this rotation to reach desired direction
            }
        }
    }
}
