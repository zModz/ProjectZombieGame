using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace Scripts_.Gameplay.Player
{
    public class PlayerScript : NetworkBehaviour
    {
        public PlayerMovement playerMovement;
        public CameraMovement cameraMovement;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if (!IsOwner) {
                playerMovement.enabled = false;
                cameraMovement.enabled = false;
            };


        }
    }
}

