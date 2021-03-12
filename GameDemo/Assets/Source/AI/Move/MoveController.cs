using UnityEngine;

public class MoveController {
    public static readonly MoveController instance = new MoveController();

	public float Move(Transform tr, Vector3 targetPosition, float speed)
    {
        Vector3 moveDir = (targetPosition - tr.position).normalized;
        tr.Translate(moveDir * speed * Time.deltaTime, Space.World);

        return Vector3.Distance(tr.position, targetPosition);
    }
}