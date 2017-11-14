using UnityEngine;
using System.Collections;

public class MovementCamera : MonoBehaviour {

    public float speed;
	public GameObject target;
	public float speedMove = 1.0f;

	Vector3 point;

	void Start () {
		point = target.transform.position;
//		transform.LookAt(point);
	}


        void Update()
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.Translate(new Vector3(0, -speed * Time.deltaTime, 0));
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
            }
        }
}
