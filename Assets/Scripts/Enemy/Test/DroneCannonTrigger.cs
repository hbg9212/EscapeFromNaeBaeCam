using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneCannonTrigger : MonoBehaviour
{
    [SerializeField] private DroneCannon DroneCannon;
   private void OnEnable()
    {
        if (DroneCannon != null)
        {
            DroneCannon.CreateProjectile();
        }
    }
}
