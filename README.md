# Vertex Shader: Bounce Effect

Vertex shader that creates a bounce effect in geometry.

<p align="center">
  <img align="center" src="example.gif" title="My goods are the highest quality"><br>
  Art from the <a href="https://assetstore.unity.com/packages/essentials/tutorial-projects/adventure-sample-game-76216">Adventure Sample Game</a>.
</p>

At GDC 2013, Jonathan Lindquist from Epic Games did a <a href="https://www.youtube.com/watch?v=7Fl3so0Z5Tc">talk</a> about Fornite's procedural animations. These animations were based on vertex displacements using vertex shaders. The main goal of these animations was to make hitting and destroying things fun. The technique used to create the bounce effect is simple, elegant and the final result is very engaging.

The first thing needed is the impact position on the object. In this example a ray is casted from the camera to the scene and checks if the gameobject hit has an `ImpactReceiver` component.

```
Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
RaycastHit hit;
if( Physics.Raycast(ray, out hit) )
{
  ImpactReceiver receiver = hit.transform.GetComponent<ImpactReceiver>();
  if( receiver )
  {
    receiver.Impact(hit.point, ray.direction);
  }
}
```

Then, the `ImpactReceiver` sets the material properties that the shader is going to use.
<ul>
  <li><strong>_ImpactPosition</strong>. Position of the impact in world space.</li>
  <li><strong>_ImpactDirection</strong>. Direction of the impact.</li>
  <li><strong>_DamageRadius</strong>. Radius of the impact.</li>
  <li><strong>_BounceAmplitude</strong>. Amplitude of the bounce effect.</li>
  <li><strong>_AnimationValue</strong>. Value from 0 to 1 used to animate, evaluated using an <a href="https://docs.unity3d.com/ScriptReference/AnimationCurve.html">AnimationCurve</a>.</li>
  <li><strong>_Bounce</strong>. Boolean flag used to play and stop the animation.</li>
</ul>

```
material.SetVector("_ImpactPosition", position);
material.SetVector("_ImpactDirection", direction);
material.SetFloat("_DamageRadius", damageRadius);
material.SetFloat("_BounceAmplitude", bounceAmplitude);
material.SetFloat("_AnimationValue", curve.Evaluate(currentTime / totalTime));
material.SetInt("_Bounce", 1);
currentTime = 0;
isAnimating = true;
```

When the animation is playing the `_AnimationValue` is updated every frame in the `Update` method.

```
currentTime += Time.deltaTime;
material.SetFloat("_AnimationValue", curve.Evaluate(currentTime / totalTime));
```

In the shadergraph a distance value is calculated from the vertex position in object space to the `ImpactPosition` (transformed from world space to object space). This distance is divided by the `ImpactRadius`, clamped (between 0 and 1) and the result is substracted from 1. Then this result is multiplied by the `ImpactDirection` * `BounceAmplitude` * `AnimationValue` (the offset in the normal direction at the given animation time). The next step is to add this value to the vertex position in object space and lastly the boolean flag `Bounce` is checked to output the final result (or the vertex position not displaced) to the position input of the main node.

<p align="center">
  <img align="center" src="shadergraph.png"><br>
</p>

References.
> <a href="https://www.gdcvault.com/play/1018192/The-Inner-Workings-of-Fortnite">The Inner Workings of Fornite's Shader-Based Procedural Animations</a>
