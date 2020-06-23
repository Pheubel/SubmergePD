using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class HandInteraction : MonoBehaviour
{

    // public SteamVR_Action_Boolean m_GrabAction = null;
    public SteamVR_Action_Single m_GrabAction = null;

    private SteamVR_Behaviour_Pose m_Pose = null;
    private FixedJoint m_Joint = null;

    public GameObject networkManager;

    private Interactable m_CurrentInteractable = null;
    public List<Interactable> m_ContactInteractables = new List<Interactable>();


    private void Awake()
    {
        m_Pose = GetComponent<SteamVR_Behaviour_Pose>();
        m_Joint = GetComponent<FixedJoint>();

    }

    private void FixedUpdate()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Interactable"))
            return;

        m_ContactInteractables.Add(other.gameObject.GetComponent<Interactable>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Interactable"))
            return;

        m_ContactInteractables.Remove(other.gameObject.GetComponent<Interactable>());

    }

    public void Pickup()
    {
        //get nearest
        m_CurrentInteractable = GetNearestInteractable();

        //Not null
        if (!m_CurrentInteractable)
            return;

        //Position
        m_CurrentInteractable.transform.position = transform.position;
    
        //Attach
        Rigidbody targetBody = m_CurrentInteractable.GetComponent<Rigidbody>();
        m_Joint.connectedBody = targetBody;


        //Set active hand
        m_CurrentInteractable.m_ActiveHand = this;

    }

    public void Drop()
    {
        //null check
        if (!m_CurrentInteractable)
            return;

        if (m_CurrentInteractable.GetComponent<CustomTag>().HasTag("cue_image")){
            if (ActiveUser.userID != null)
            {
                Transform transform = m_CurrentInteractable.GetComponent<Transform>();


                    Image_cue image_cue = new Image_cue(ActiveUser.userID, m_CurrentInteractable.GetComponent<Image_cue>().id, 0, (float)System.Math.Round(transform.position.x, 6), (float)System.Math.Round(transform.position.y, 6), (float)System.Math.Round(transform.position.z, 6), m_CurrentInteractable.GetComponent<Image_cue>().photo);

                    m_CurrentInteractable.GetComponent<Image_cue>().setImage_Cue(image_cue);

                    networkManager.GetComponent<NetworkManager>().saveImage(image_cue);
               

                    



            }
        }



        //detach
        m_Joint.connectedBody = null;

      
   
        //clear
        m_CurrentInteractable.m_ActiveHand = null;
        m_CurrentInteractable = null;

    
       
        
    }

    private Interactable GetNearestInteractable()
    {
        Interactable nearest = null;
        float minDistance = float.MaxValue;
        float distance = 0.0f;

        foreach(Interactable interactable in m_ContactInteractables)
        {
            distance = (interactable.transform.position - transform.position).sqrMagnitude;
            if (distance <minDistance)
            {
                minDistance = distance;
                nearest = interactable;
            }
        }
        return nearest;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (m_GrabAction.GetAxis(m_Pose.inputSource) >0)
        {
            Pickup();
        }

        if (m_GrabAction.GetAxis(m_Pose.inputSource) == 0)
        {
            Drop();
        }
    }
}
