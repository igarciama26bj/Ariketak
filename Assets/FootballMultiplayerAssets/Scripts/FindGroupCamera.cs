using Unity.Cinemachine;
using UnityEngine;

public class FindGroupCamera : MonoBehaviour
{
    public void Start()
    {
        if (GameObject.FindGameObjectWithTag("GroupFollowCamera").TryGetComponent(out CinemachineTargetGroup ctg))
        {
            ctg.AddMember(transform, 1, 0.5f);
        }
    }
}
