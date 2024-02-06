 using UnityEngine;

//author jani kauhanen 23sep22
public class Follow : MonoBehaviour
{
    public GameObject Second;

    void Update()
    {
        if (Input.touchCount == 0)
        {
            transform.position = new Vector3(100, 100, 0);
            Second.transform.position = new Vector3(100, 100, 0);
        }
        if (Input.touchCount == 1)
        {
            Second.transform.position = new Vector3(100, 100, 0);
        }
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Began)
            {
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(touch.position);

                Vector3 newPos = transform.position;

                newPos = worldPos;

                newPos.x = worldPos.x;
                newPos.y = worldPos.y;

                transform.position = newPos;



               


            }
            if (Input.touchCount > 1)
            {
                Touch touch1 = Input.GetTouch(1);
                if (touch1.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Began)
                {

                    

                    Vector3 worldPos1 = Camera.main.ScreenToWorldPoint(Input.GetTouch(1).position);
                    Vector3 newPos1 = transform.position;

                    newPos1.x = worldPos1.x;
                    newPos1.y = worldPos1.y;
                    Second.transform.position = newPos1;
                }


            }

        }
    }
}