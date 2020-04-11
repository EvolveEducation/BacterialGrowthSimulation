using System.Collections;
using UnityEngine;

public class DishRotation : MonoBehaviour
{
    private Quaternion targetAngle;
    private Vector3 mouseLoc;

    void Start()
    {
        targetAngle = transform.rotation;
    }

    public void Down()
    {
        mouseLoc = Input.mousePosition;
    }

    public void Drag()
    {
        Vector3 shift = mouseLoc - Input.mousePosition;
        transform.Rotate(new Vector3(shift.y, shift.x, 0) * Time.deltaTime * 0.75f, Space.World);
        mouseLoc = shift.magnitude > 300 ? Input.mousePosition : mouseLoc;
    }

    public void StartRelease()
    {
        StartCoroutine(Release());
    }

    IEnumerator Release()
    {
        while (transform.rotation != targetAngle)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetAngle, 0.25f);
            yield return null;
        }
        yield return null;
    }
}
