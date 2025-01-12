﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;
public class Player : NetworkBehaviour // NETWORKBEHAVIOUR FOR NETWORKING
{
    public GameObject bombPrefab;

    public Camera attachedCamera;
    public Transform attachedVirtualCamera;
    
    public float speed = 10f, jump = 10f;
    public LayerMask ignoreLayers;
    public float rayDistance = 10f;
    public bool isGrounded = false;
    private Rigidbody rigid;
    #region Unity Events
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * rayDistance);
    }
    private void OnDestroy()
    {
        Destroy(attachedCamera.gameObject);
        Destroy(attachedVirtualCamera.gameObject);
    }
    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        
       
        attachedCamera.transform.SetParent(null);
        attachedVirtualCamera.transform.SetParent(null);
        //FOR SPLIT SCREEN PURPOSE!!!
        //if this is the player the camera will be turn ON 
        if(isLocalPlayer)
        {
            attachedCamera.enabled = true;
            //attachedCamera.rect = new Rect(0f, 0f, 0.5f, 1f); // Making 50:50 ratio screen
            attachedVirtualCamera.gameObject.SetActive(true);
        }
        else
        {
            attachedCamera.enabled = false;
            //attachedCamera.rect = new Rect(0.5f, 0f, 0.5f, 1f);
            attachedVirtualCamera.gameObject.SetActive(false);
        }
    }
    private void FixedUpdate()
    {
        Ray groundRay = new Ray(transform.position, Vector3.down);
        isGrounded = Physics.Raycast(groundRay, rayDistance, ~ignoreLayers);
    }
    private void OnTriggerEnter(Collider col)
    {
        Item item = col.GetComponent<Item>();
        if (item)
        {
            item.Collect();
        }
    }
    private void Update()
    {
        if (isLocalPlayer)
        {
            
            float inputH = Input.GetAxis("Horizontal");
            float inputV = Input.GetAxis("Vertical");
            Move(inputH, inputV);
            if(Input.GetKeyDown(KeyCode.F))
            {
                //Spawn bomb on server
                CmdSpawnBomb(transform.position);
            }
            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
        }

    }
    #endregion
    #region Commands
    [Command]

    // NEED TO USE Cmd Before the name for COMMAND
    public void CmdSpawnBomb(Vector3 position)
    {
        GameObject bomb = Instantiate(bombPrefab, position, Quaternion.identity);
        NetworkServer.Spawn(bomb); 
    }


    #endregion
    #region Custom
    private void Jump()
    {
        if (isGrounded)
        {
            rigid.AddForce(Vector3.up * jump, ForceMode.Impulse);
        }
    }
    private void Move(float inputH, float inputV)
    {
        Vector3 direction = new Vector3(inputH, 0, inputV);

        // [Optional] Move with camera
        Vector3 euler = Camera.main.transform.eulerAngles;
        direction = Quaternion.Euler(0, euler.y, 0) * direction; // Convert direction to relative direction to camera only on Y

        rigid.AddForce(direction * speed);
    }
    #endregion
}

