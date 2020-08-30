using UnityEngine;

public class BounceReceiver : MonoBehaviour
{
    public AnimationCurve curve;
    public float radius = 1;
    public float amplitude = 1;
    public float totalTime = 1;

    private Material material;
    private bool isAnimating = false;
    private float currentTime = 0;

    private void Awake()
    {
        material = this.GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
        if( isAnimating )
        {
            currentTime += Time.deltaTime;
            material.SetFloat("_Value", curve.Evaluate(currentTime / totalTime));
        }

        if( currentTime > totalTime )
        {
            currentTime = 0;
            isAnimating = false;
            material.SetInt("_Bounce", 0);
        }
    }

    public void Bounce( Vector3 position, Vector3 direction )
    {
        if( curve == null || !material )
        {
        	return;
        }

        material.SetVector("_TargetPosition", position);
        material.SetVector("_Direction", direction);
        material.SetFloat("_Radius", radius);
        material.SetFloat("_Amplitude", amplitude);
        material.SetFloat("_Value", curve.Evaluate(currentTime / totalTime));
        material.SetInt("_Bounce", 1);
        currentTime = 0;
        isAnimating = true;
    }
}