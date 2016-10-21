using UnityEngine;
using System.Collections;

public class PickUpObject : MonoBehaviour {

    // object being held
    GameObject heldObj;
    bool isCarrying = false;

    // get middle of screen
    int x;
    int y;

    // main camera
    Camera mainCamera;

    // get the TextCanvas object
    GameObject textCanvas;

    // float to control distance from camera of heldObj
    public float distance;

    // rigidBody for heldObj
    Rigidbody rbHeldObj;


	// Use this for initialization
	void Start () {

        // get necessary values
        mainCamera = GetComponentInChildren<Camera>();
        x = Screen.width / 2;
        y = Screen.height / 2;

        textCanvas = GameObject.FindWithTag("Text_PickUp");
	}
	
	// Update is called once per frame
	void Update () {

        // do not display this text unless it changes true later in the frame calc.
        textCanvas.SetActive(false);

        // test to drop the heldObj
        if (isCarrying && Input.GetKeyDown(KeyCode.E))
        {
            isCarrying = false;
            rbHeldObj.isKinematic = false;
            rbHeldObj = null;
            heldObj = null;
        }

        // as to not interfere with picking up the dropped object the same frame
        else 
        {
            // raycast
            Ray ray = mainCamera.ScreenPointToRay(new Vector3(x, y));
            RaycastHit hit;

            // if something was found
            if (Physics.Raycast(ray, out hit))
            {
                Pickupable p = hit.collider.GetComponent<Pickupable>();

                // hovering over something pickupable and in range
                if (p != null && Vector3.Distance(p.gameObject.transform.position, mainCamera.transform.position) <= 4.0f)
                {
                    // display message that it can be picked up
                    if (heldObj == null)
                    {
                        textCanvas.SetActive(true);
                    }

                    // no object being held, E is pressed, pick it up
                    if (Input.GetKeyDown(KeyCode.E) && heldObj == null)
                    {
                        isCarrying = true;
                        heldObj = p.gameObject;
                        rbHeldObj = heldObj.GetComponent<Rigidbody>();
                    }
                }

                else
                {
                    
                }
            }
        }

        if (isCarrying) 
        { 
            carry(heldObj);
        }
	}

    void carry(GameObject held) 
    {
        held.transform.position = mainCamera.transform.position + (mainCamera.transform.forward * distance);
        rbHeldObj.isKinematic = true;
    }
}
