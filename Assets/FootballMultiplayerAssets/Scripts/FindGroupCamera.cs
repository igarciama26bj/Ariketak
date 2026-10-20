using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;

public class FindGroupCamera : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (GameObject.FindGameObjectWithTag("GroupFollowCamera").TryGetComponent(out CinemachineTargetGroup ctg))
        {
            ctg.AddMember(transform, 1, 0.5f);
        }
    }
}
