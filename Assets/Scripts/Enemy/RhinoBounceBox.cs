using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhinoBounceBox : MonoBehaviour

{
    [HideInInspector] public float slideTime;
    [HideInInspector] public float slideSpeed;
    [HideInInspector] public float slideDistance;
    Animal animal;
    Vector3 slidePosition;
    bool isSliding = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isSliding)
        {
            return;
        }
        if (other.gameObject.tag == "Animal")
        {
            animal = other.gameObject.GetComponent<Animal>();
            Debug.Log(animal.gameObject.name);
            StartCoroutine(Slide());
        }
    }

    IEnumerator Slide()
    {
        StartSlide();
        yield return new WaitForSeconds(slideTime);
        EndSlide();
    }

    void StartSlide()
    {
        isSliding = true;
        Vector3 direction = (animal.transform.position - transform.position).normalized;
        slidePosition = animal.transform.position + direction * slideDistance;
        slidePosition = new Vector3(slidePosition.x, animal.transform.position.y, slidePosition.z);
        animal.GetComponent<NavMeshObstacleAgent>().enabled = false;
        animal.enabled = false;
    }
    public void EndSlide()
    {
        animal.GetComponent<NavMeshObstacleAgent>().enabled = true;
        animal.enabled = true;
        isSliding = false;
    }
    
    public void Update()
    {
        if (isSliding)
        {
            animal.transform.position = Vector3.MoveTowards(
                animal.transform.position, slidePosition, slideSpeed * Time.deltaTime);
        }
    }
}
