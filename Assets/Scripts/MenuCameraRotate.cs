using UnityEngine;

public class MenuCameraSmoothRotate : MonoBehaviour
{
    public float baseSpeed = 10f;

    public float speedVariation = 5f;

    public float speedFrequency = 0.5f;

    private float time;

    void Update()
    {
        time += Time.deltaTime;

        // Генерируем синусоиду колебания скорости
        float sinValue = Mathf.Sin(time * speedFrequency);
        float currentSpeed = baseSpeed + sinValue * speedVariation;

        transform.Rotate(Vector3.up, currentSpeed * Time.deltaTime, Space.World);
    }
}
