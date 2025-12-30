using UnityEngine;
using System.Collections;

public class FishMovement : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 1.5f;
    public float rotationSpeed = 5f;

    [Header("Pause")]
    public float minPauseTime = 1f;
    public float maxPauseTime = 3f;
    public float minMoveTime = 2f;
    public float maxMoveTime = 5f;

    [Header("Area (Ice bounds)")]
    public Vector2 areaMin; // нижний левый угол
    public Vector2 areaMax; // верхний правый угол

    private Vector2 direction;
    private bool isMoving = true;

    private void Start()
    {
        PickRandomDirection();
        StartCoroutine(MovePauseRoutine());
    }

    private void Update()
    {
        if (!isMoving)
            return;

        // Движение
        transform.position += (Vector3)direction * speed * Time.deltaTime;

        // Поворот "носом вперёд"
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRot = Quaternion.Euler(0, 0, angle - 90f);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);

        CheckBounds();
    }

    private void PickRandomDirection()
    {
        direction = Random.insideUnitCircle.normalized;
    }

    private void CheckBounds()
    {
        Vector3 pos = transform.position;

        if (pos.x < areaMin.x) pos.x = areaMax.x;
        if (pos.x > areaMax.x) pos.x = areaMin.x;
        if (pos.y < areaMin.y) pos.y = areaMax.y;
        if (pos.y > areaMax.y) pos.y = areaMin.y;

        transform.position = pos;
    }

    private IEnumerator MovePauseRoutine()
    {
        while (true)
        {
            // Плывём
            isMoving = true;
            PickRandomDirection();
            yield return new WaitForSeconds(Random.Range(minMoveTime, maxMoveTime));

            // Стоим
            isMoving = false;
            yield return new WaitForSeconds(Random.Range(minPauseTime, maxPauseTime));
        }
    }
}
