using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class RobotBehaviour : Agent
{
    [Header("速度"), Range(1, 50)]
    public float movementSpeed = 10f;

    //機器人鋼體
    Rigidbody rigRobot;
    //球的鋼體
    [SerializeField]
    Rigidbody rigBall;

    void Start()
    {
        rigRobot = GetComponent<Rigidbody>();
        rigBall = rigBall.GetComponent<Rigidbody>();
    }

    /// <summary>
    /// 事件開始時:重新設定機器人與足球位置
    /// </summary>
    public override void OnEpisodeBegin()
    {
        //將鋼體的速度及角速度歸零
        rigRobot.velocity = Vector3.zero;
        rigRobot.angularVelocity = Vector3.zero;
        rigBall.velocity = Vector3.zero;
        rigBall.angularVelocity = Vector3.zero;

        //隨機機器人位置
        Vector3 posRobot = new Vector3(Random.Range(-2f, 2f), 0.05f, Random.Range(-2f, 0f));
        transform.position = posRobot;
        //隨機足球位置
        Vector3 posBall = new Vector3(Random.Range(-2f, 2f), 0.2f, Random.Range(1f, 2f));
        rigBall.position = posBall;

        //足球尚未進入球門
        BallBehaviour.complete = false;
    }

    /// <summary>
    /// 收集觀測資料
    /// </summary>
    public override void CollectObservations(VectorSensor sensor)
    {
        //加入觀測資料:機器人座標、足球座標、機器人速度x、機器人速度z
        sensor.AddObservation(transform.position);
        sensor.AddObservation(rigBall.position);
        sensor.AddObservation(rigRobot.velocity.x);
        sensor.AddObservation(rigRobot.velocity.z);
    }

    /// <summary>
    /// 動作:控制機器人與回饋
    /// </summary>
    /// <param name="actions"></param>
    public override void OnActionReceived(ActionBuffers actions)
    {
        //使用參數控制機器人
        Vector3 control = Vector3.zero;
        control.x = actions.ContinuousActions[0];
        control.z = actions.ContinuousActions[1];
        rigRobot.AddForce(control * movementSpeed);

        //球進入球門--> 成功:加一分並結束
        if (BallBehaviour.complete)
        {
            SetReward(1);
            EndEpisode();
        }
        //機器人會足球掉到地板下方--> 失敗:扣一分並結束
        if(transform.position.y < 0 || rigBall.position.y < 0)
        {
            SetReward(-1);
            EndEpisode();
        }
    }

    /// <summary>
    /// 探索:讓開發者測試環境
    /// </summary>
    /// <param name="actionsOut"></param>
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        //提供開發者控制的方式
        var action = actionsOut.ContinuousActions;
        action[0] = Input.GetAxis("Horizontal");
        action[1] = Input.GetAxis("Vertical");
    }

}
