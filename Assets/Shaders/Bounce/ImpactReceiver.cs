using UnityEngine;

public class ImpactReceiver : MonoBehaviour
{
    public AnimationCurve curve;
    public float damageRadius = 1;
    public float bounceAmplitude = 1;
    public float totalTime = 1;

    private Material material;
    private bool isAnimating = false;
    private float currentTime = 0;

    private void Awake()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
        if( isAnimating )
        {
            currentTime += Time.deltaTime;
            material.SetFloat("_AnimationValue", curve.Evaluate(currentTime / totalTime));
        }

        if( currentTime > totalTime )
        {
            currentTime = 0;
            isAnimating = false;
            material.SetInt("_Bounce", 0);
        }
    }

    public void Impact( Vector3 position, Vector3 direction )
    {
        if( curve == null || !material )
        {
        	return;
        }

        material.SetVector("_ImpactPosition", position);
        material.SetVector("_ImpactDirection", direction);
        material.SetFloat("_DamageRadius", damageRadius);
        material.SetFloat("_BounceAmplitude", bounceAmplitude);
        material.SetFloat("_AnimationValue", curve.Evaluate(currentTime / totalTime));
        material.SetInt("_Bounce", 1);
        currentTime = 0;
        isAnimating = true;
    }
}