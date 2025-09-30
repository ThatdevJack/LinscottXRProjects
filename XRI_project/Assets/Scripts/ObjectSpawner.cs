using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem.XR;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectPrefab; // object to spawn
    public Transform spawnPoint; // Where it spawns
    public XRNode controllerNode = XRNode.LeftHand; // assigning right hand controller
    public float spawnCooldown = 1.0f; // Need a coroutine
    private bool canSpawn = true; // Time in seconds between spawns

    // Update is called once per frame
    void Update()
    {
        if (canSpawn && IsAButtonPressed())
        {
            StartCoroutine(SpawnObjectWithCooldown());
        }
    }

    bool IsAButtonPressed()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(controllerNode);
        bool buttonPressed = false;

        if (device.TryGetFeatureValue(CommonUsages.primaryButton, out buttonPressed) && buttonPressed)
        {
            return true;
        }

        return false;
    }

    IEnumerator SpawnObjectWithCooldown()
    {
        canSpawn = false;
        SpawnObject();
        yield return new WaitForSeconds(spawnCooldown);
        canSpawn = true;
    }

    void SpawnObject()
    {
        if(objectPrefab != null && spawnPoint != null)
        {
            GameObject spawnedObject = Instantiate(objectPrefab, spawnPoint.position, spawnPoint.rotation);
        }

        else
        {
            Debug.LogError("Assign objectPrefab and spawnPoint");
        }
    }
}
