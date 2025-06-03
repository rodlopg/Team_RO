using UnityEngine;
using System.Collections;

public class ActivationCondition : MonoBehaviour
{
	private ActivationStation activationStation;
	public string  scenario;
	public bool    somethingHappens;

	// Use this for initialization
	void Update()
	{
		if (scenario == "Scenario 1")
		{
			if (somethingHappens && this.gameObject.GetComponent<ActivationStation>().enabled == false)
			{ this.gameObject.GetComponent<ActivationStation>().enabled = true; }
		}
		else if (scenario == "Scenario 2")
		{
			if (somethingHappens && this.gameObject.GetComponent<ActivationStation>().enabled == false)
			{ this.gameObject.GetComponent<ActivationStation>().enabled = true; }
		}
		else if (scenario == "Scenario 3")
		{
			if (somethingHappens && this.gameObject.GetComponent<ActivationStation>().enabled == false)
			{ this.gameObject.GetComponent<ActivationStation>().enabled = true; }
		}
		else if (scenario == "Scenario X")
		{
			if (somethingHappens && this.gameObject.GetComponent<ActivationStation>().enabled == false)
			{ this.gameObject.GetComponent<ActivationStation>().enabled = true; }
		}
	}
}