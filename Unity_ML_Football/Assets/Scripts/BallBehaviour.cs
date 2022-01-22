using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    //足球是否進入球門
    public static bool complete;

    /// <summary>
    /// 觸發開始事件:碰到勾選isTrigger物件會執行一次
    /// </summary>
    /// <param name="other">碰到的物件碰撞資訊</param>
    void OnTriggerEnter(Collider other)
    {
        //如果碰到的名稱為進球感應區
        if(other.name == "GoalTrigger")
        {
            //進入球門
            complete = true;
        }
    }
}
