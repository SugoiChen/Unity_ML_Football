using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    //���y�O�_�i�J�y��
    public static bool complete;

    /// <summary>
    /// Ĳ�o�}�l�ƥ�:�I��Ŀ�isTrigger����|����@��
    /// </summary>
    /// <param name="other">�I�쪺����I����T</param>
    void OnTriggerEnter(Collider other)
    {
        //�p�G�I�쪺�W�٬��i�y�P����
        if(other.name == "GoalTrigger")
        {
            //�i�J�y��
            complete = true;
        }
    }
}
