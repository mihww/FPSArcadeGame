using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClickToPoint : MonoBehaviour
{
    private NavMeshAgent navAgent;


    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // creates a ray from the camera to the mouse position
            RaycastHit hit;


            if(Physics.Raycast(ray, out hit, Mathf.Infinity, NavMesh.AllAreas)) // if the ray hits something
            {
                navAgent.SetDestination(hit.point); // moves the navAgent to the hit point
            }
        }
    }
}
