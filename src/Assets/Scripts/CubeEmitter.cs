using UnityEngine;
using TMPro;
using System;

public class CubeEmitter : MonoBehaviour
{
    [Header("Input fields")]
    [SerializeField] TMP_InputField speedInputField;
    [SerializeField] TMP_InputField timeInputField;
    [SerializeField] TMP_InputField XInputField;
    [SerializeField] TMP_InputField YInputField;
    [SerializeField] TMP_InputField ZInputField;

    private float speed,delay;
    private float x,y,z;
    private Vector3 CurrentTarget;

    [Space(15)]
    [SerializeField] GameObject prefab;
    [SerializeField] GameObject cube;

    [Header("Debug")]
    [SerializeField] TextMeshProUGUI targetPosText;
    [SerializeField] TextMeshProUGUI posText;
    [SerializeField] TextMeshProUGUI delayText;

    private float delayTime, _delay;
    void Start()
    {
        speedInputField.onEndEdit.AddListener(delegate { ChangeSpeed(speedInputField.text); });
        timeInputField.onEndEdit.AddListener(delegate { ChangeDelay(timeInputField.text); });
        XInputField.onEndEdit.AddListener(delegate { ChangeXTarget(XInputField.text); });
        YInputField.onEndEdit.AddListener(delegate { ChangeYTarget(YInputField.text); });
        ZInputField.onEndEdit.AddListener(delegate { ChangeZTarget(ZInputField.text); });

        CreateCube();
    }

    void ChangeSpeed(string input) => speed = Convert.ToInt64(input);
    void ChangeDelay(string input) => delay = Convert.ToInt64(input);
    void ChangeXTarget(string input) => x = Convert.ToInt64(input);
    void ChangeYTarget(string input) => y = Convert.ToInt64(input);
    void ChangeZTarget(string input) => z = Convert.ToInt64(input);

    void CreateCube() => cube = Instantiate(prefab, this.transform.position, Quaternion.identity);
    void MoveToTarget()
    {
        if (cube != null)
        {
            cube.transform.position 
                = Vector3.MoveTowards(cube.transform.position, CurrentTarget, Time.deltaTime * speed);
        }
    }

    void Update()
    {
        #region Debug section
        if (cube != null)
        {
            posText.text = $"pos : " +
                $"x:{(int)cube.transform.position.x} | " +
                $"y:{(int)cube.transform.position.y} | " +
                $"z:{(int)cube.transform.position.z}";
        }
        targetPosText.text = $"target : " +
            $"x:{(int)CurrentTarget.x} | " +
            $"y:{(int)CurrentTarget.y} | " +
            $"z:{(int)CurrentTarget.z}";
        delayText.text = $"delay: {_delay}";
        #endregion

        //Change target position every frame
        CurrentTarget = new Vector3(this.x, this.y, this.z);

        if (cube != null)
        {
            //If we reach target, destroy cube and instantiate new one
            if (Vector3.Distance(cube.transform.position, CurrentTarget) == 0f)
            {
                cube.transform.position = CurrentTarget;
                Destroy(cube);
                delayTime = Time.time + delay;
                _delay = delay;
            }
        }
        else if (cube == null && Time.time >= delayTime)
        {
            CreateCube();
        }

        //Move to target every frame
        MoveToTarget();
    }
}
