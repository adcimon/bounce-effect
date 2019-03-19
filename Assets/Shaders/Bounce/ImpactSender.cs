using UnityEngine;

public class ImpactSender : MonoBehaviour
{
    private ImpactReceiver receiver;

    private void Update()
    {
        if( Input.GetMouseButtonDown(0) )
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if( Physics.Raycast(ray, out hit) )
            {
                Debug.DrawLine(ray.origin, hit.point, Color.red);
                receiver = hit.transform.GetComponent<ImpactReceiver>();
            }
        }

        if( receiver && Input.GetMouseButtonUp(0) )
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if( Physics.Raycast(ray, out hit) )
            {
                if( hit.transform.GetComponent<ImpactReceiver>() == receiver )
                {
                    receiver.Impact(hit.point, ray.direction);
                    receiver = null;
                }
            }
        }
    }
}