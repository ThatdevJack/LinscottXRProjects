using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class DoorFeatures : CoreFeatures
{

    [Header("Door Configuration")]
    [SerializeField]
    private Transform doorPivot;

    [SerializeField]
    private float maxAngle = 90.0f; //Probably will need to be <90 degrees, but okay starting point

    [SerializeField]
    private bool reverseAngleDirection = false; //Flips direction

    [SerializeField]
    private float doorSpeed = 2.0f;

    [SerializeField]
    private bool open = false;

    [SerializeField]
    private bool MakeKinematicOnOpen = false;

    [Header("Interactions Configuration")]
    [SerializeField]
    private XRSocketInteractor socketInteractor;

    [SerializeField]
    private XRSimpleInteractable simpleInteractable;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        //When key gets close to the socket, add a listener
        //s = Shorthand, SelectEnterEvents
        socketInteractor?.selectEntered.AddListener((s) =>
        {
            //OpenDoor();
            PlayOnStart();
        });
        socketInteractor?.selectExited.AddListener((s) =>
        {
            PlayOnEnd();
            socketInteractor.socketActive = featureUsage == FeatureUsage.Once ? false : true;
        });

        simpleInteractable?.selectEntered.AddListener((s) =>
        {
            //OpenDoor();
        });

    }

    public void OpenDoor()
    {
        //if the door is not open, play the OnStart sound
        if (!open)
        {
            PlayOnStart();
            open = true;
            StartCoroutine(ProcessMotion());
        }
    }

    private IEnumerator ProcessMotion()
    {
        while(open)
        {
            var angle = doorPivot.localEulerAngles.y <180 ? doorPivot.localEulerAngles.y :
                doorPivot.localEulerAngles.y - 360;

            angle = reverseAngleDirection ? Mathf.Abs(angle) : angle;

            if (angle <= maxAngle)
            {
                doorPivot?.Rotate(Vector3.up, doorSpeed * Time.deltaTime * (reverseAngleDirection ? -1 : 1));
            }

            else
            {
                open = false;
                var featureRigiBody = GetComponent<Rigidbody>();
                if (featureRigiBody != null && MakeKinematicOnOpen) featureRigiBody.isKinematic = true;
            }

                yield return null;
        }
    }
}
