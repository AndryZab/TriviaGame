using UnityEngine;
using UnityEngine.UI;

public class WheelFortune : MonoBehaviour
{
    [Header("Wheel Settings")]
    [SerializeField] private Button backButton;

    public float RotatePowerMin;
    public float RotatePowerMax;

    public float StopPowerMin;
    public float StopPowerMax;

    public int sectorCount = 18;

    private Rigidbody2D rb;

    private bool isRotating;

    private float stopDelay;
    private float stopPower;
    private float rotatePower;

    private WheelFortuneTimer spinTimer;
    private WheelEffectsController wheelEffectsController;

    [Header("Wheel Rewards")]
    [SerializeField] private int[] rewards;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spinTimer = GetComponent<WheelFortuneTimer>();
        wheelEffectsController = GetComponent<WheelEffectsController>();
    }

    private void Update()
    {
        if (rb.angularVelocity > 0)
        {
            rb.angularVelocity -= stopPower * Time.deltaTime;
            rb.angularVelocity = Mathf.Clamp(rb.angularVelocity, 0, 1440);
        }

        if (rb.angularVelocity == 0 && isRotating)
        {
            stopDelay += Time.deltaTime;
            if (stopDelay >= 0.5f)
            {
                GetReward();
                isRotating = false;
                stopDelay = 0;
            }
        }
    }

    public void Rotate()
    {
        if (!isRotating)
        {
            rotatePower = Random.Range(RotatePowerMin, RotatePowerMax);
            stopPower = Random.Range(StopPowerMin, StopPowerMax);

            rb.AddTorque(rotatePower);
            isRotating = true;
            spinTimer.StartTimer();
            backButton.interactable = false;
        }
    }

    private void GetReward()
    {
        float rot = transform.eulerAngles.z;
        float sectorAngle = 360f / sectorCount;
        float offset = sectorAngle / 2f;
        rot = (rot + offset) % 360f;
        int sectorIndex = Mathf.FloorToInt(rot / sectorAngle);
        int reward = CalculateReward(sectorIndex);

        EndSpin(reward);

    }

    private void EndSpin(int reward)
    {
        backButton.interactable = true;
        CoinsManager.Instance.AddCoins(reward);
        wheelEffectsController.Play();

    }

    private int CalculateReward(int index)
    {
        if (index < 0 || index >= rewards.Length)
        {
            index = 0;
        }
        return rewards[index];
    }
}
