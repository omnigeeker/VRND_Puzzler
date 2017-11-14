using UnityEngine;
using System.Collections;

public class LightUp : MonoBehaviour
{
	// The initial material of the orb.
	private Material defaultMaterial;

	// The material used to light up the orb.
	public Material lightUpMaterial;

	// The gameobject that has the GameLogic.cs script attached.
	public GameLogic gameLogic;


	void Start()
	{
		// Assign the initial material of the orb as the default material.
		defaultMaterial = this.GetComponent<MeshRenderer>().material;
	}
		
	// Called when the orb is clicked.
	// This function can be hooked up in Unity by adding a Pointer Click event trigger to the orb.
	public void PlayerSelection()
	{
		// Call the GameLogic.PlayerSelection(GameObject sphere) method (see GameLogic.cs script) passing in the orb 
		// this script is attached to.
		gameLogic.PlayerSelection(this.gameObject);

		// Get the GVR audio source component on this orb and play the audio.
		/* Uncomment the line below during 'A Little More Feedback!' lesson.*/
		this.GetComponent<GvrAudioSource>().Play();
	}

	// Called when the reticle moves over the orb.
	// This function can be hooked up in Unity by adding a Pointer Enter event trigger to the orb.
	public void GazeLightUp()
	{
		// Assign the lightup material to the orb.
		this.GetComponent<MeshRenderer>().material = lightUpMaterial;
	}

	// Called when the reticle is moved away from orb.
	// This function can be hooked up in Unity by adding a Pointer Exit event trigger to the orb.
	public void AestheticReset()
	{
		// Revert to the orb's default material.
		this.GetComponent<MeshRenderer>().material = defaultMaterial; 
	}

	// Lightup behavior for displaying the orb lightup pattern.
	// Called when the GameLogic.DisplayPattern() function is invoked (see GameLogic.cs script).
	public void PatternLightUp(float duration)
	{ 
		StartCoroutine(LightFor(duration));
	}

	// Called from PatternLightUp(float duration) to light up the orb for a given duration.
	IEnumerator LightFor(float duration)
	{ 
		// Assign the lightup material to the orb and play the audio...
		PatternLightUp();

		// ...wait...
		yield return new WaitForSeconds(duration - 0.1f);

		// ...revert the material back to the orb's default material.
		AestheticReset();
	}

	// Called from LightFor(float duration) to light up the orb and play the audio.
	void PatternLightUp()
	{ 
		// Assign the lightup material to the orb.
		this.GetComponent<MeshRenderer>().material = lightUpMaterial;

		// Get the GVR audio source component on this orb and play the audio.
		this.GetComponent<GvrAudioSource>().Play(); 
	}
}
