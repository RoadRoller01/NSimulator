using System.Collections.Generic;
using UnityEngine;

public class NormalHotCube : MonoBehaviour
{
    float speed = 10;
    // Start is called before the first frame update
    // void Start()
    // {
    //     Debug.Log("meow");
    // }

    // Update is called once per frame
    private void start()
    {
        Application.targetFrameRate = 60;
    }
    void Update()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 direction = input.normalized;
        Vector3 velocity = direction * speed;
        Vector3 moveAmount = velocity * Time.deltaTime;

        transform.position += moveAmount;

    }
}
