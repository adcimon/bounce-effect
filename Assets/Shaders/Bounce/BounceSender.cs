using UnityEngine;

public class BounceSender : MonoBehaviour
{
    private BounceReceiver receiver;

    private void Update()
    {
        if( Input.GetMouseButtonDown(0) )
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if( Physics.Raycast(ray, out hit) )
            {
                Debug.DrawLine(ray.origin, hit.point, Color.red);
                receiver = hit.transform.GetComponent<BounceReceiver>();
            }
        }

        if( receiver && Input.GetMouseButtonUp(0) )
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if( Physics.Raycast(ray, out hit) )
            {
                if( hit.transform.GetComponent<BounceReceiver>() == receiver )
                {
                    receiver.Impact(hit.point, ray.direction);
                    receiver = null;
                }
            }
        }
    }
}