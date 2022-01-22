using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class RobotBehaviour : Agent
{
    [Header("�t��"), Range(1, 50)]
    public float movementSpeed = 10f;

    //�����H����
    Rigidbody rigRobot;
    //�y������
    [SerializeField]
    Rigidbody rigBall;

    void Start()
    {
        rigRobot = GetComponent<Rigidbody>();
        rigBall = rigBall.GetComponent<Rigidbody>();
    }

    /// <summary>
    /// �ƥ�}�l��:���s�]�w�����H�P���y��m
    /// </summary>
    public override void OnEpisodeBegin()
    {
        //�N���骺�t�פΨ��t���k�s
        rigRobot.velocity = Vector3.zero;
        rigRobot.angularVelocity = Vector3.zero;
        rigBall.velocity = Vector3.zero;
        rigBall.angularVelocity = Vector3.zero;

        //�H�������H��m
        Vector3 posRobot = new Vector3(Random.Range(-2f, 2f), 0.05f, Random.Range(-2f, 0f));
        transform.position = posRobot;
        //�H�����y��m
        Vector3 posBall = new Vector3(Random.Range(-2f, 2f), 0.2f, Random.Range(1f, 2f));
        rigBall.position = posBall;

        //���y�|���i�J�y��
        BallBehaviour.complete = false;
    }

    /// <summary>
    /// �����[�����
    /// </summary>
    public override void CollectObservations(VectorSensor sensor)
    {
        //�[�J�[�����:�����H�y�СB���y�y�СB�����H�t��x�B�����H�t��z
        sensor.AddObservation(transform.position);
        sensor.AddObservation(rigBall.position);
        sensor.AddObservation(rigRobot.velocity.x);
        sensor.AddObservation(rigRobot.velocity.z);
    }

    /// <summary>
    /// �ʧ@:��������H�P�^�X
    /// </summary>
    /// <param name="actions"></param>
    public override void OnActionReceived(ActionBuffers actions)
    {
        //�ϥΰѼƱ�������H
        Vector3 control = Vector3.zero;
        control.x = actions.ContinuousActions[0];
        control.z = actions.ContinuousActions[1];
        rigRobot.AddForce(control * movementSpeed);

        //�y�i�J�y��--> ���\:�[�@���õ���
        if (BallBehaviour.complete)
        {
            SetReward(1);
            EndEpisode();
        }
        //�����H�|���y����a�O�U��--> ����:���@���õ���
        if(transform.position.y < 0 || rigBall.position.y < 0)
        {
            SetReward(-1);
            EndEpisode();
        }
    }

    /// <summary>
    /// ����:���}�o�̴�������
    /// </summary>
    /// <param name="actionsOut"></param>
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        //���Ѷ}�o�̱���覡
        var action = actionsOut.ContinuousActions;
        action[0] = Input.GetAxis("Horizontal");
        action[1] = Input.GetAxis("Vertical");
    }

}
