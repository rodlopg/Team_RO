using UnityEngine;
using UnityEngine.UI;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ActivationStation : MonoBehaviour
{

	public string	scriptNote = "Activation Station - By Paul Dela Cruz";
	public bool		maximizedScript = true;
	
	public int      activationStyleIndex = 0;
	public string[] activationStyles = new string[]
	{
		/*0*/  "The Checklist",
		/*1*/  "Whichever Works...",
		/*2*/  "Follow the Leader!",
		/*3*/  "Always Rebel!",
		/*4*/  "Mouse Activation",
		/*5*/  "Keyboard Activation"
	};

	public int      inputConditionIndex = 0;
	public string[] inputConditions = new string[]
	{
		/*0*/  "Press",
		/*1*/  "Hold",
		/*2*/  "Release"
	};

	public string 	keyButtonCondition = "";
	public int      mouseButtonConditionIndex = 0;
	public string[] mouseButtonConditions = new string[]
	{
		/*0*/  "Left MB",
		/*1*/  "Right MB",
		/*2*/  "Middle MB"
	};

	public int      locationButtonConditionIndex = 0;
	public string[] locationButtonConditions = new string[]
	{
		/*0*/  "On Anywhere",
		/*1*/  "On This",
		/*2*/  "Not on This"
	};

	public string  mouseInputStatus = "";
	public string  mouseButtonStatus = "";
	public string  mouseLocationStatus = "Not on This";

	public string  keyInputStatus = "";
	public string  keyButtonStatus = "";
	public string  keyLocationStatus = "Not on This";

	public int     conditionCount = 0;
	public int     conditionsMet = 0;

	public float   originalCountdown = 0;
	public float   currentCountdown = 0;

	public int     numOfCycles = 0;
	public bool    finalPersistence = true;

	public bool    mouseInputSuccess = false;
	public bool    mouseButtonSuccess = false;
	public bool    mouseLocationSuccess = false;
	
	public bool    keyInputSuccess = false;
	public bool    keyLocationSuccess = false;
	
	public bool    noConsequences = true;

	public int  condition1Status = 0;
	public int  condition2Status = 0;
	public int  condition3Status = 0;
	public int  condition4Status = 0;
	public int  condition5Status = 0;
	public int  condition6Status = 0;
	public int  condition7Status = 0;
	public int  condition8Status = 0;
	public int  condition9Status = 0;

	public int  impact1Status = 0;
	public int  impact2Status = 0;
	public int  impact3Status = 0;
	public int  impact4Status = 0;
	public int  impact5Status = 0;
	public int  impact6Status = 0;
	public int  impact7Status = 0;
	public int  impact8Status = 0;
	public int  impact9Status = 0;

	public int  consequence1Status = 0;
	public int  consequence2Status = 0;
	public int  consequence3Status = 0;
	public int  consequence4Status = 0;
	public int  consequence5Status = 0;
	public int  consequence6Status = 0;
	public int  consequence7Status = 0;
	public int  consequence8Status = 0;
	public int  consequence9Status = 0;
		
	public GameObject ifObject1, ifObject2, ifObject3, ifObject4, ifObject5, ifObject6, ifObject7, ifObject8, ifObject9;
	public bool isActivation1, isActivation2, isActivation3, isActivation4, isActivation5, isActivation6, isActivation7, isActivation8, isActivation9;

	public GameObject setObject1, setObject2, setObject3, setObject4, setObject5, setObject6, setObject7, setObject8, setObject9;
	public bool toActivation1, toActivation2, toActivation3, toActivation4, toActivation5, toActivation6, toActivation7, toActivation8, toActivation9;

	public bool elseToActivation1, elseToActivation2, elseToActivation3, elseToActivation4, elseToActivation5, elseToActivation6, elseToActivation7, elseToActivation8, elseToActivation9;


	// Use this for initialization
	void OnEnable ()
	{
		currentCountdown = originalCountdown;
	}

	void OnMouseOver()
	{
		// Get Location Status
		mouseLocationStatus = "On This";
		keyLocationStatus = "On This";
	}

	void OnMouseExit()
	{ 
		// Get Location Status
		mouseLocationStatus = "Not on This";
		keyLocationStatus = "Not on This";
	}
	
	// Update is called once per frame
	void Update()
	{

		// Get Mouse Button Status
		if      (Input.GetMouseButton(0)) { mouseButtonStatus = "Left MB"; }
		else if (Input.GetMouseButton(1)) { mouseButtonStatus = "Right MB"; }
		else if (Input.GetMouseButton(2)) { mouseButtonStatus = "Middle MB"; }
		else { mouseButtonStatus = ""; }

		// Get Mouse Input Status
		if      (Input.GetMouseButtonDown(mouseButtonConditionIndex)) { mouseInputStatus = "Pressed"; }
		else if (Input.GetMouseButton(mouseButtonConditionIndex))     { mouseInputStatus = "Held"; }
		else if (Input.GetMouseButtonUp(mouseButtonConditionIndex))   { mouseInputStatus = "Released"; }
		else { mouseInputStatus = ""; }

		// Get Keyboard Button Status
		if      (Input.GetKey("a"))          { keyButtonStatus = "a"; }          else if (Input.GetKey("b"))          { keyButtonStatus = "b"; }          else if (Input.GetKey("c"))           { keyButtonStatus = "c"; }
		else if (Input.GetKey("d"))          { keyButtonStatus = "d"; }          else if (Input.GetKey("e"))          { keyButtonStatus = "e"; }          else if (Input.GetKey("f"))           { keyButtonStatus = "f"; }
		else if (Input.GetKey("g"))          { keyButtonStatus = "g"; }          else if (Input.GetKey("h"))          { keyButtonStatus = "h"; }          else if (Input.GetKey("i"))           { keyButtonStatus = "i"; }
		else if (Input.GetKey("j"))          { keyButtonStatus = "j"; }          else if (Input.GetKey("k"))          { keyButtonStatus = "k"; }          else if (Input.GetKey("l"))           { keyButtonStatus = "l"; }
		else if (Input.GetKey("m"))          { keyButtonStatus = "m"; }          else if (Input.GetKey("n"))          { keyButtonStatus = "n"; }          else if (Input.GetKey("o"))           { keyButtonStatus = "o"; }
		else if (Input.GetKey("p"))          { keyButtonStatus = "p"; }          else if (Input.GetKey("q"))          { keyButtonStatus = "q"; }          else if (Input.GetKey("r"))           { keyButtonStatus = "r"; }
		else if (Input.GetKey("s"))          { keyButtonStatus = "s"; }          else if (Input.GetKey("t"))          { keyButtonStatus = "t"; }          else if (Input.GetKey("u"))           { keyButtonStatus = "u"; }
		else if (Input.GetKey("v"))          { keyButtonStatus = "v"; }          else if (Input.GetKey("w"))          { keyButtonStatus = "w"; }          else if (Input.GetKey("x"))           { keyButtonStatus = "x"; }
		else if (Input.GetKey("y"))          { keyButtonStatus = "y"; }          else if (Input.GetKey("z"))          { keyButtonStatus = "z"; }          else if (Input.GetKey("1"))           { keyButtonStatus = "1"; }
		else if (Input.GetKey("2"))          { keyButtonStatus = "2"; }          else if (Input.GetKey("3"))          { keyButtonStatus = "3"; }          else if (Input.GetKey("4"))           { keyButtonStatus = "4"; }
		else if (Input.GetKey("5"))          { keyButtonStatus = "5"; }          else if (Input.GetKey("6"))          { keyButtonStatus = "6"; }          else if (Input.GetKey("7"))           { keyButtonStatus = "7"; }
		else if (Input.GetKey("8"))          { keyButtonStatus = "8"; }          else if (Input.GetKey("9"))          { keyButtonStatus = "9"; }          else if (Input.GetKey("0"))           { keyButtonStatus = "0"; }
		else if (Input.GetKey("="))          { keyButtonStatus = "="; }          else if (Input.GetKey("+"))          { keyButtonStatus = "+"; }          else if (Input.GetKey("-"))           { keyButtonStatus = "-"; }
		else if (Input.GetKey("*"))          { keyButtonStatus = "*"; }          else if (Input.GetKey("/"))          { keyButtonStatus = "/"; }          else if (Input.GetKey("_"))           { keyButtonStatus = "_"; }
        else if (Input.GetKey("["))          { keyButtonStatus = "["; }          else if (Input.GetKey("]"))          { keyButtonStatus = "]"; }          else if (Input.GetKey(":"))           { keyButtonStatus = ":"; }
		else if (Input.GetKey(";"))          { keyButtonStatus = ";"; }          else if (Input.GetKey("'"))          { keyButtonStatus = "'"; }          else if (Input.GetKey(","))           { keyButtonStatus = ","; }
		else if (Input.GetKey("<"))          { keyButtonStatus = "<"; }          else if (Input.GetKey("."))          { keyButtonStatus = "."; }          else if (Input.GetKey(">"))           { keyButtonStatus = ">"; }           
		else if (Input.GetKey("?"))          { keyButtonStatus = "?"; }          else if (Input.GetKey("`"))          { keyButtonStatus = "`"; }          else if (Input.GetKey("!"))           { keyButtonStatus = "!"; }
		else if (Input.GetKey("@"))          { keyButtonStatus = "@"; }          else if (Input.GetKey("#"))          { keyButtonStatus = "#"; }          else if (Input.GetKey("$"))           { keyButtonStatus = "$"; }
		else if (Input.GetKey("^"))          { keyButtonStatus = "^"; }          else if (Input.GetKey("&"))          { keyButtonStatus = "&"; }          else if (Input.GetKey("*"))           { keyButtonStatus = "*"; }
		else if (Input.GetKey("("))          { keyButtonStatus = "("; }          else if (Input.GetKey(")"))          { keyButtonStatus = ")"; }          else if (Input.GetKey("\\"))          { keyButtonStatus = "\\"; }
		else if (Input.GetKey("\""))         { keyButtonStatus = "\""; }         else if (Input.GetKey("f1"))         { keyButtonStatus = "f1"; }         else if (Input.GetKey("f2"))          { keyButtonStatus = "f2"; }
		else if (Input.GetKey("f3"))         { keyButtonStatus = "f3"; }         else if (Input.GetKey("f4"))         { keyButtonStatus = "f4"; }         else if (Input.GetKey("f5"))          { keyButtonStatus = "f5"; }
		else if (Input.GetKey("f6"))         { keyButtonStatus = "f6"; }         else if (Input.GetKey("f7"))         { keyButtonStatus = "f7"; }         else if (Input.GetKey("f8"))          { keyButtonStatus = "f8"; }          
		else if (Input.GetKey("f9"))         { keyButtonStatus = "f9"; }         else if (Input.GetKey("f11"))        { keyButtonStatus = "f11"; }        else if (Input.GetKey("f12"))         { keyButtonStatus = "f12"; }         
		else if (Input.GetKey("tab"))        { keyButtonStatus = "tab"; }        else if (Input.GetKey("end"))        { keyButtonStatus = "end"; }        else if (Input.GetKey("up"))          { keyButtonStatus = "up"; }
		else if (Input.GetKey("down"))       { keyButtonStatus = "down"; }       else if (Input.GetKey("left"))       { keyButtonStatus = "left"; }       else if (Input.GetKey("right"))       { keyButtonStatus = "right"; }
		else if (Input.GetKey("home"))       { keyButtonStatus = "home"; }       else if (Input.GetKey("space"))      { keyButtonStatus = "space"; }      else if (Input.GetKey("enter"))       { keyButtonStatus = "enter"; }       
		else if (Input.GetKey("return"))     { keyButtonStatus = "return"; }     else if (Input.GetKey("escape"))     { keyButtonStatus = "escape"; }     else if (Input.GetKey("delete"))      { keyButtonStatus = "delete"; }
		else if (Input.GetKey("insert"))     { keyButtonStatus = "insert"; }     else if (Input.GetKey("backspace"))  { keyButtonStatus = "backspace"; }  else if (Input.GetKey("page up"))     { keyButtonStatus = "page up"; }
		else if (Input.GetKey("page down"))  { keyButtonStatus = "page down"; }  else if (Input.GetKey("left alt"))   { keyButtonStatus = "left alt"; }   else if (Input.GetKey("left cmd"))    { keyButtonStatus = "left cmd"; }
		else if (Input.GetKey("right alt"))  { keyButtonStatus = "right alt"; }  else if (Input.GetKey("right cmd"))  { keyButtonStatus = "right cmd"; }  else if (Input.GetKey("left ctrl"))   { keyButtonStatus = "left ctrl"; }
		else if (Input.GetKey("right ctrl")) { keyButtonStatus = "right ctrl"; } else if (Input.GetKey("left shift")) { keyButtonStatus = "left shift"; } else if (Input.GetKey("right shift")) { keyButtonStatus = "right shift"; }
		else { keyButtonStatus = ""; }

		// Get Keyboard Input Status
		if (keyButtonCondition != "")
		{
			if (Input.GetKeyDown(keyButtonCondition))
			{ keyInputStatus = "Pressed"; }
			else if (Input.GetKey(keyButtonCondition))
			{ keyInputStatus = "Held"; }
			else if (Input.GetKeyUp(keyButtonCondition))
			{ keyInputStatus = "Released"; }
			else { keyInputStatus = ""; }
		}

		// Count Down if Greater than Zero
		if (numOfCycles > 0 && currentCountdown > 0) { currentCountdown -= Time.deltaTime; }

		if (activationStyleIndex == 0) // The Checklist
		{ 
			if (ifObject1 != null)
			{
				conditionCount += 1;
				if (ifObject1.activeInHierarchy == isActivation1) { conditionsMet += 1; condition1Status = 1; }
				else { condition1Status = -1; }
			}   else { condition1Status =  0; }
			if (ifObject2 != null)
			{
				conditionCount += 1;
				if (ifObject2.activeInHierarchy == isActivation2) { conditionsMet += 1; condition2Status = 1; }
				else { condition2Status = -1; }
			}   else { condition2Status =  0; }
			if (ifObject3 != null)
			{
				conditionCount += 1;
				if (ifObject3.activeInHierarchy == isActivation3) { conditionsMet += 1; condition3Status = 1; }
				else { condition3Status = -1; }
			}   else { condition3Status =  0; }
			if (ifObject4 != null)
			{
				conditionCount += 1;
				if (ifObject4.activeInHierarchy == isActivation4) { conditionsMet += 1; condition4Status = 1; }
				else { condition4Status = -1; }
			}   else { condition4Status =  0; }
			if (ifObject5 != null)
			{
				conditionCount += 1;
				if (ifObject5.activeInHierarchy == isActivation5) { conditionsMet += 1; condition5Status = 1; }
				else { condition5Status = -1; }
			}   else { condition5Status =  0; }
			if (ifObject6 != null)
			{
				conditionCount += 1;
				if (ifObject6.activeInHierarchy == isActivation6) { conditionsMet += 1; condition6Status = 1; }
				else { condition6Status = -1; }
			}   else { condition6Status = 0; }
			if (ifObject7 != null)
			{
				conditionCount += 1;
				if (ifObject7.activeInHierarchy == isActivation7) { conditionsMet += 1; condition7Status = 1; }
				else { condition7Status = -1; }
			}   else { condition7Status =  0; }
			if (ifObject8 != null)
			{
				conditionCount += 1;
				if (ifObject8.activeInHierarchy == isActivation8) { conditionsMet += 1; condition8Status = 1; }
				else { condition8Status = -1; }
			}   else { condition8Status =  0; }
			if (ifObject9 != null)
			{
				conditionCount += 1;
				if (ifObject9.activeInHierarchy == isActivation9) { conditionsMet += 1; condition9Status = 1; }
				else { condition9Status = -1; }
			}   else { condition9Status =  0; }
				
			if (conditionCount == conditionsMet)
			{
				if (setObject1 != null) { setObject1.SetActive(toActivation1); impact1Status = 1; consequence1Status = 0; } else { impact1Status = 0; }
				if (setObject2 != null) { setObject2.SetActive(toActivation2); impact2Status = 1; consequence2Status = 0; } else { impact2Status = 0; }
				if (setObject3 != null) { setObject3.SetActive(toActivation3); impact3Status = 1; consequence3Status = 0; } else { impact3Status = 0; }
				if (setObject4 != null) { setObject4.SetActive(toActivation4); impact4Status = 1; consequence4Status = 0; } else { impact4Status = 0; }
				if (setObject5 != null) { setObject5.SetActive(toActivation5); impact5Status = 1; consequence5Status = 0; } else { impact5Status = 0; }
				if (setObject6 != null) { setObject6.SetActive(toActivation6); impact6Status = 1; consequence6Status = 0; } else { impact6Status = 0; }
				if (setObject7 != null) { setObject7.SetActive(toActivation7); impact7Status = 1; consequence7Status = 0; } else { impact7Status = 0; }
				if (setObject8 != null) { setObject8.SetActive(toActivation8); impact8Status = 1; consequence8Status = 0; } else { impact8Status = 0; }
				if (setObject9 != null) { setObject9.SetActive(toActivation9); impact9Status = 1; consequence9Status = 0; } else { impact9Status = 0; }
			}
			else
			{
				if (noConsequences == false)
				{
					{
						if (setObject1 != null) { setObject1.SetActive(elseToActivation1); consequence1Status = 1; impact1Status = 0; } else { consequence1Status = 0; }
						if (setObject2 != null) { setObject2.SetActive(elseToActivation2); consequence2Status = 1; impact2Status = 0; } else { consequence2Status = 0; }
						if (setObject3 != null) { setObject3.SetActive(elseToActivation3); consequence3Status = 1; impact3Status = 0; } else { consequence3Status = 0; }
						if (setObject4 != null) { setObject4.SetActive(elseToActivation4); consequence4Status = 1; impact4Status = 0; } else { consequence4Status = 0; }
						if (setObject5 != null) { setObject5.SetActive(elseToActivation5); consequence5Status = 1; impact5Status = 0; } else { consequence5Status = 0; }
						if (setObject6 != null) { setObject6.SetActive(elseToActivation6); consequence6Status = 1; impact6Status = 0; } else { consequence6Status = 0; }
						if (setObject7 != null) { setObject7.SetActive(elseToActivation7); consequence7Status = 1; impact7Status = 0; } else { consequence7Status = 0; }
						if (setObject8 != null) { setObject8.SetActive(elseToActivation8); consequence8Status = 1; impact8Status = 0; } else { consequence8Status = 0; }
						if (setObject9 != null) { setObject9.SetActive(elseToActivation9); consequence9Status = 1; impact9Status = 0; } else { consequence9Status = 0; }
					}
				}
			}
		}

		if (activationStyleIndex == 1) // Whichever Works...
		{ 
			if (ifObject1 != null)
			{ 
				if (ifObject1.activeInHierarchy == isActivation1) { conditionsMet += 1; condition1Status = 1; } 
				else { condition1Status = 0; }
			}   else { condition1Status = 0; }
			if (ifObject2 != null)
			{ 
				if (ifObject2.activeInHierarchy == isActivation2) { conditionsMet += 1; condition2Status = 1; } 
				else { condition2Status = 0; }
			}   else { condition2Status = 0; }
			if (ifObject3 != null)
			{  
				if (ifObject3.activeInHierarchy == isActivation3) { conditionsMet += 1; condition3Status = 1; } 
				else { condition3Status = 0; }
			}   else { condition3Status = 0; }
			if (ifObject4 != null)
			{  
				if (ifObject4.activeInHierarchy == isActivation4) { conditionsMet += 1; condition4Status = 1; } 
				else { condition4Status = 0; }
			}   else { condition4Status = 0; }
			if (ifObject5 != null)
			{   
				if (ifObject5.activeInHierarchy == isActivation5) { conditionsMet += 1; condition5Status = 1; } 
				else { condition5Status = 0; }
			}   else { condition5Status = 0; }
			if (ifObject6 != null)
			{    
				if (ifObject6.activeInHierarchy == isActivation6) { conditionsMet += 1; condition6Status = 1; } 
				else { condition6Status = 0; }
			}   else { condition6Status = 0; }
			if (ifObject7 != null)
			{  
				if (ifObject7.activeInHierarchy == isActivation7) { conditionsMet += 1; condition7Status = 1; } 
				else { condition7Status = 0; }
			}   else { condition7Status = 0; }
			if (ifObject8 != null)
			{   
				if (ifObject8.activeInHierarchy == isActivation8) { conditionsMet += 1; condition8Status = 1; } 
				else { condition8Status = 0; }
			}   else { condition8Status = 0; }
			if (ifObject9 != null)
			{   
				if (ifObject9.activeInHierarchy == isActivation9) { conditionsMet += 1; condition9Status = 1; } 
				else { condition9Status = 0; }
			}   else { condition9Status = 0; }

			if (conditionsMet > 0)
			{
				if (setObject1 != null) { setObject1.SetActive(toActivation1); impact1Status = 1; consequence1Status = 0; }
				if (setObject2 != null) { setObject2.SetActive(toActivation2); impact2Status = 1; consequence2Status = 0; }
				if (setObject3 != null) { setObject3.SetActive(toActivation3); impact3Status = 1; consequence3Status = 0; }
				if (setObject4 != null) { setObject4.SetActive(toActivation4); impact4Status = 1; consequence4Status = 0; }
				if (setObject5 != null) { setObject5.SetActive(toActivation5); impact5Status = 1; consequence5Status = 0; }
				if (setObject6 != null) { setObject6.SetActive(toActivation6); impact6Status = 1; consequence6Status = 0; }
				if (setObject7 != null) { setObject7.SetActive(toActivation7); impact7Status = 1; consequence7Status = 0; }
				if (setObject8 != null) { setObject8.SetActive(toActivation8); impact8Status = 1; consequence8Status = 0; }
				if (setObject9 != null) { setObject9.SetActive(toActivation9); impact9Status = 1; consequence9Status = 0; }
			}
			else
			{
				if (noConsequences == false)
				{
					{
						if (setObject1 != null) { setObject1.SetActive(elseToActivation1); consequence1Status = 1; impact1Status = 0; }
						if (setObject2 != null) { setObject2.SetActive(elseToActivation2); consequence2Status = 1; impact2Status = 0; }
						if (setObject3 != null) { setObject3.SetActive(elseToActivation3); consequence3Status = 1; impact3Status = 0; }
						if (setObject4 != null) { setObject4.SetActive(elseToActivation4); consequence4Status = 1; impact4Status = 0; }
						if (setObject5 != null) { setObject5.SetActive(elseToActivation5); consequence5Status = 1; impact5Status = 0; }
						if (setObject6 != null) { setObject6.SetActive(elseToActivation6); consequence6Status = 1; impact6Status = 0; }
						if (setObject7 != null) { setObject7.SetActive(elseToActivation7); consequence7Status = 1; impact7Status = 0; }
						if (setObject8 != null) { setObject8.SetActive(elseToActivation8); consequence8Status = 1; impact8Status = 0; }
						if (setObject9 != null) { setObject9.SetActive(elseToActivation9); consequence9Status = 1; impact9Status = 0; }
					}
				}
			}
		}

		if (activationStyleIndex == 2) // Follow the Leader!
		{ 
			if (ifObject1 != null)
			{
				if (setObject1 != null) { setObject1.SetActive(ifObject1.activeInHierarchy); }
				if (setObject2 != null) { setObject2.SetActive(ifObject1.activeInHierarchy); }
				if (setObject3 != null) { setObject3.SetActive(ifObject1.activeInHierarchy); }
				if (setObject4 != null) { setObject4.SetActive(ifObject1.activeInHierarchy); }
				if (setObject5 != null) { setObject5.SetActive(ifObject1.activeInHierarchy); }
				if (setObject6 != null) { setObject6.SetActive(ifObject1.activeInHierarchy); }
				if (setObject7 != null) { setObject7.SetActive(ifObject1.activeInHierarchy); }
				if (setObject8 != null) { setObject8.SetActive(ifObject1.activeInHierarchy); }
				if (setObject9 != null) { setObject9.SetActive(ifObject1.activeInHierarchy); }
			}
		}

		if (activationStyleIndex == 3) // Always Rebel!
		{ 
			if (ifObject1 != null)
			{
				if (setObject1 != null) { setObject1.SetActive(!ifObject1.activeInHierarchy); }
				if (setObject2 != null) { setObject2.SetActive(!ifObject1.activeInHierarchy); }
				if (setObject3 != null) { setObject3.SetActive(!ifObject1.activeInHierarchy); }
				if (setObject4 != null) { setObject4.SetActive(!ifObject1.activeInHierarchy); }
				if (setObject5 != null) { setObject5.SetActive(!ifObject1.activeInHierarchy); }
				if (setObject6 != null) { setObject6.SetActive(!ifObject1.activeInHierarchy); }
				if (setObject7 != null) { setObject7.SetActive(!ifObject1.activeInHierarchy); }
				if (setObject8 != null) { setObject8.SetActive(!ifObject1.activeInHierarchy); }
				if (setObject9 != null) { setObject9.SetActive(!ifObject1.activeInHierarchy); }
			}
		}

		if (activationStyleIndex == 4) // Mouse Activation
		{
			if (inputConditionIndex == 0) { if (mouseInputStatus == "Pressed") { mouseInputSuccess = true; } else { mouseInputSuccess = false; } }
			if (inputConditionIndex == 1) { if (mouseInputStatus == "Held") { mouseInputSuccess = true; } else { mouseInputSuccess = false; } }
			if (inputConditionIndex == 2) { if (mouseInputStatus == "Released") { mouseInputSuccess = true; } else { mouseInputSuccess = false; } }

			if (locationButtonConditionIndex == 0) { mouseLocationSuccess = true; }
			if (locationButtonConditionIndex == 1) { if (mouseLocationStatus == "On This") { mouseLocationSuccess = true; } else { mouseLocationSuccess = false; } }
			if (locationButtonConditionIndex == 2) { if (mouseLocationStatus == "Not on This") { mouseLocationSuccess = true; } else { mouseLocationSuccess = false; } }

			if (mouseInputSuccess == true && mouseLocationSuccess == true)
			{
				if (setObject1 != null) { setObject1.SetActive(toActivation1); impact1Status = 1; consequence1Status = 0; }
				if (setObject2 != null) { setObject2.SetActive(toActivation2); impact2Status = 1; consequence2Status = 0; }
				if (setObject3 != null) { setObject3.SetActive(toActivation3); impact3Status = 1; consequence3Status = 0; }
				if (setObject4 != null) { setObject4.SetActive(toActivation4); impact4Status = 1; consequence4Status = 0; }
				if (setObject5 != null) { setObject5.SetActive(toActivation5); impact5Status = 1; consequence5Status = 0; }
				if (setObject6 != null) { setObject6.SetActive(toActivation6); impact6Status = 1; consequence6Status = 0; }
				if (setObject7 != null) { setObject7.SetActive(toActivation7); impact7Status = 1; consequence7Status = 0; }
				if (setObject8 != null) { setObject8.SetActive(toActivation8); impact8Status = 1; consequence8Status = 0; }
				if (setObject9 != null) { setObject9.SetActive(toActivation9); impact9Status = 1; consequence9Status = 0; }
			}
			else
			{
				if (noConsequences == false)
				{
					{
						if (setObject1 != null) { setObject1.SetActive(elseToActivation1); consequence1Status = 1; impact1Status = 0; }
						if (setObject2 != null) { setObject2.SetActive(elseToActivation2); consequence2Status = 1; impact2Status = 0; }
						if (setObject3 != null) { setObject3.SetActive(elseToActivation3); consequence3Status = 1; impact3Status = 0; }
						if (setObject4 != null) { setObject4.SetActive(elseToActivation4); consequence4Status = 1; impact4Status = 0; }
						if (setObject5 != null) { setObject5.SetActive(elseToActivation5); consequence5Status = 1; impact5Status = 0; }
						if (setObject6 != null) { setObject6.SetActive(elseToActivation6); consequence6Status = 1; impact6Status = 0; }
						if (setObject7 != null) { setObject7.SetActive(elseToActivation7); consequence7Status = 1; impact7Status = 0; }
						if (setObject8 != null) { setObject8.SetActive(elseToActivation8); consequence8Status = 1; impact8Status = 0; }
						if (setObject9 != null) { setObject9.SetActive(elseToActivation9); consequence9Status = 1; impact9Status = 0; }
					}
				}
			}
		}
		
		if (activationStyleIndex == 5) // Keyboard Activation
		{
			if (inputConditionIndex == 0) { if (keyInputStatus == "Pressed") { keyInputSuccess = true; } else { keyInputSuccess = false; } }
			if (inputConditionIndex == 1) { if (keyInputStatus == "Held") { keyInputSuccess = true; } else { keyInputSuccess = false; } }
			if (inputConditionIndex == 2) { if (keyInputStatus == "Released") { keyInputSuccess = true; } else { keyInputSuccess = false; } }

			if (locationButtonConditionIndex == 0) { keyLocationSuccess = true; }
			if (locationButtonConditionIndex == 1) { if (keyLocationStatus == "On This") { keyLocationSuccess = true; } else { keyLocationSuccess = false; } }
			if (locationButtonConditionIndex == 2) { if (keyLocationStatus == "Not on This") { keyLocationSuccess = true; } else { keyLocationSuccess = false; } }

			if (keyInputSuccess == true && keyLocationSuccess == true)
			{
				if (setObject1 != null) { setObject1.SetActive(toActivation1); impact1Status = 1; consequence1Status = 0; }
				if (setObject2 != null) { setObject2.SetActive(toActivation2); impact2Status = 1; consequence2Status = 0; }
				if (setObject3 != null) { setObject3.SetActive(toActivation3); impact3Status = 1; consequence3Status = 0; }
				if (setObject4 != null) { setObject4.SetActive(toActivation4); impact4Status = 1; consequence4Status = 0; }
				if (setObject5 != null) { setObject5.SetActive(toActivation5); impact5Status = 1; consequence5Status = 0; }
				if (setObject6 != null) { setObject6.SetActive(toActivation6); impact6Status = 1; consequence6Status = 0; }
				if (setObject7 != null) { setObject7.SetActive(toActivation7); impact7Status = 1; consequence7Status = 0; }
				if (setObject8 != null) { setObject8.SetActive(toActivation8); impact8Status = 1; consequence8Status = 0; }
				if (setObject9 != null) { setObject9.SetActive(toActivation9); impact9Status = 1; consequence9Status = 0; }
			}
			else
			{
				if (noConsequences == false)
				{
					{
						if (setObject1 != null) { setObject1.SetActive(elseToActivation1); consequence1Status = 1; impact1Status = 0; }
						if (setObject2 != null) { setObject2.SetActive(elseToActivation2); consequence2Status = 1; impact2Status = 0; }
						if (setObject3 != null) { setObject3.SetActive(elseToActivation3); consequence3Status = 1; impact3Status = 0; }
						if (setObject4 != null) { setObject4.SetActive(elseToActivation4); consequence4Status = 1; impact4Status = 0; }
						if (setObject5 != null) { setObject5.SetActive(elseToActivation5); consequence5Status = 1; impact5Status = 0; }
						if (setObject6 != null) { setObject6.SetActive(elseToActivation6); consequence6Status = 1; impact6Status = 0; }
						if (setObject7 != null) { setObject7.SetActive(elseToActivation7); consequence7Status = 1; impact7Status = 0; }
						if (setObject8 != null) { setObject8.SetActive(elseToActivation8); consequence8Status = 1; impact8Status = 0; }
						if (setObject9 != null) { setObject9.SetActive(elseToActivation9); consequence9Status = 1; impact9Status = 0; }
					}
				}
			}
		}

		conditionCount = 0;
		conditionsMet = 0;

	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(ActivationStation))]
[CanEditMultipleObjects]

public class ActivationStationEditor : Editor
{
	SerializedProperty

		scriptNoteProp, maximizedScriptProp, activationStyleIndexProp,

		inputConditionIndexProp, keyButtonConditionProp, mouseButtonConditionIndexProp, locationButtonConditionIndexProp,

		inputKeyProp,
		mouseInputStatusProp,
		mouseButtonStatusProp,
		mouseLocationStatusProp,

		originalCountdownProp,
		currentCountdownProp,

		numOfCyclesProp,
		finalPersistenceProp,

		mouseInputSuccessProp,
		mouseLocationSuccessProp,
		keyInputSuccessProp,
		keyLocationSuccessProp,
		noConsequencesProp,

		condition1StatusProp,
		condition2StatusProp,
		condition3StatusProp,
		condition4StatusProp,
		condition5StatusProp,
		condition6StatusProp,
		condition7StatusProp,
		condition8StatusProp,
		condition9StatusProp,

		impact1StatusProp,
		impact2StatusProp,
		impact3StatusProp,
		impact4StatusProp,
		impact5StatusProp,
		impact6StatusProp,
		impact7StatusProp,
		impact8StatusProp,
		impact9StatusProp,

		consequence1StatusProp,
		consequence2StatusProp,
		consequence3StatusProp,
		consequence4StatusProp,
		consequence5StatusProp,
		consequence6StatusProp,
		consequence7StatusProp,
		consequence8StatusProp,
		consequence9StatusProp,

		ifObject1Prop, ifObject2Prop, ifObject3Prop, ifObject4Prop, ifObject5Prop, ifObject6Prop, ifObject7Prop, ifObject8Prop, ifObject9Prop,
		isActivation1Prop, isActivation2Prop, isActivation3Prop, isActivation4Prop, isActivation5Prop, isActivation6Prop, isActivation7Prop,
		isActivation8Prop, isActivation9Prop,

		setObject1Prop, setObject2Prop, setObject3Prop, setObject4Prop, setObject5Prop, setObject6Prop, setObject7Prop, setObject8Prop, setObject9Prop,
		toActivation1Prop, toActivation2Prop, toActivation3Prop, toActivation4Prop, toActivation5Prop, toActivation6Prop, toActivation7Prop,
		toActivation8Prop, toActivation9Prop,

		elseToActivation1Prop, elseToActivation2Prop, elseToActivation3Prop, elseToActivation4Prop, elseToActivation5Prop, elseToActivation6Prop,
		elseToActivation7Prop, elseToActivation8Prop, elseToActivation9Prop;
			

	void OnEnable()
	{
		
		// Setup the SerializedProperties.
		scriptNoteProp = serializedObject.FindProperty("scriptNote");
		maximizedScriptProp = serializedObject.FindProperty("maximizedScript");
		activationStyleIndexProp = serializedObject.FindProperty("activationStyleIndex");

		inputConditionIndexProp = serializedObject.FindProperty("inputConditionIndex");
		keyButtonConditionProp = serializedObject.FindProperty("keyButtonCondition");
		mouseButtonConditionIndexProp = serializedObject.FindProperty("mouseButtonConditionIndex");
		locationButtonConditionIndexProp = serializedObject.FindProperty("locationButtonConditionIndex");

		inputKeyProp = serializedObject.FindProperty("inputKey");
		mouseInputStatusProp = serializedObject.FindProperty("mouseInputStatus");
		mouseButtonStatusProp = serializedObject.FindProperty("mouseButtonStatus");
		mouseLocationStatusProp = serializedObject.FindProperty("mouseLocationStatus");

		originalCountdownProp = serializedObject.FindProperty("originalCountdown");
		currentCountdownProp = serializedObject.FindProperty("currentCountdown");

		numOfCyclesProp = serializedObject.FindProperty("numOfCycles");
		finalPersistenceProp = serializedObject.FindProperty("finalPersistence");

		mouseInputSuccessProp = serializedObject.FindProperty("mouseInputSuccess");
		mouseLocationSuccessProp = serializedObject.FindProperty("mouseLocationSuccess");
		keyInputSuccessProp = serializedObject.FindProperty("keyInputSuccess");
		keyLocationSuccessProp = serializedObject.FindProperty("keyLocationSuccess");
		noConsequencesProp = serializedObject.FindProperty("noConsequences");

		condition1StatusProp = serializedObject.FindProperty("condition1Status");
		condition2StatusProp = serializedObject.FindProperty("condition2Status");
		condition3StatusProp = serializedObject.FindProperty("condition3Status");
		condition4StatusProp = serializedObject.FindProperty("condition4Status");
		condition5StatusProp = serializedObject.FindProperty("condition5Status");
		condition6StatusProp = serializedObject.FindProperty("condition6Status");
		condition7StatusProp = serializedObject.FindProperty("condition7Status");
		condition8StatusProp = serializedObject.FindProperty("condition8Status");
		condition9StatusProp = serializedObject.FindProperty("condition9Status");

		impact1StatusProp = serializedObject.FindProperty("impact1Status");
		impact2StatusProp = serializedObject.FindProperty("impact2Status");
		impact3StatusProp = serializedObject.FindProperty("impact3Status");
		impact4StatusProp = serializedObject.FindProperty("impact4Status");
		impact5StatusProp = serializedObject.FindProperty("impact5Status");
		impact6StatusProp = serializedObject.FindProperty("impact6Status");
		impact7StatusProp = serializedObject.FindProperty("impact7Status");
		impact8StatusProp = serializedObject.FindProperty("impact8Status");
		impact9StatusProp = serializedObject.FindProperty("impact9Status");

		consequence1StatusProp = serializedObject.FindProperty("consequence1Status");
		consequence2StatusProp = serializedObject.FindProperty("consequence2Status");
		consequence3StatusProp = serializedObject.FindProperty("consequence3Status");
		consequence4StatusProp = serializedObject.FindProperty("consequence4Status");
		consequence5StatusProp = serializedObject.FindProperty("consequence5Status");
		consequence6StatusProp = serializedObject.FindProperty("consequence6Status");
		consequence7StatusProp = serializedObject.FindProperty("consequence7Status");
		consequence8StatusProp = serializedObject.FindProperty("consequence8Status");
		consequence9StatusProp = serializedObject.FindProperty("consequence9Status");

		ifObject1Prop = serializedObject.FindProperty("ifObject1");
		isActivation1Prop = serializedObject.FindProperty("isActivation1");

		ifObject2Prop = serializedObject.FindProperty("ifObject2");
		isActivation2Prop = serializedObject.FindProperty("isActivation2");

		ifObject3Prop = serializedObject.FindProperty("ifObject3");
		isActivation3Prop = serializedObject.FindProperty("isActivation3");

		ifObject4Prop = serializedObject.FindProperty("ifObject4");
		isActivation4Prop = serializedObject.FindProperty("isActivation4");

		ifObject5Prop = serializedObject.FindProperty("ifObject5");
		isActivation5Prop = serializedObject.FindProperty("isActivation5");

		ifObject6Prop = serializedObject.FindProperty("ifObject6");
		isActivation6Prop = serializedObject.FindProperty("isActivation6");

		ifObject7Prop = serializedObject.FindProperty("ifObject7");
		isActivation7Prop = serializedObject.FindProperty("isActivation7");

		ifObject8Prop = serializedObject.FindProperty("ifObject8");
		isActivation8Prop = serializedObject.FindProperty("isActivation8");

		ifObject9Prop = serializedObject.FindProperty("ifObject9");
		isActivation9Prop = serializedObject.FindProperty("isActivation9");


		setObject1Prop = serializedObject.FindProperty("setObject1");
		toActivation1Prop = serializedObject.FindProperty("toActivation1");

		setObject2Prop = serializedObject.FindProperty("setObject2");
		toActivation2Prop = serializedObject.FindProperty("toActivation2");

		setObject3Prop = serializedObject.FindProperty("setObject3");
		toActivation3Prop = serializedObject.FindProperty("toActivation3");

		setObject4Prop = serializedObject.FindProperty("setObject4");
		toActivation4Prop = serializedObject.FindProperty("toActivation4");

		setObject5Prop = serializedObject.FindProperty("setObject5");
		toActivation5Prop = serializedObject.FindProperty("toActivation5");

		setObject6Prop = serializedObject.FindProperty("setObject6");
		toActivation6Prop = serializedObject.FindProperty("toActivation6");

		setObject7Prop = serializedObject.FindProperty("setObject7");
		toActivation7Prop = serializedObject.FindProperty("toActivation7");

		setObject8Prop = serializedObject.FindProperty("setObject8");
		toActivation8Prop = serializedObject.FindProperty("toActivation8");

		setObject9Prop = serializedObject.FindProperty("setObject9");
		toActivation9Prop = serializedObject.FindProperty("toActivation9");

		elseToActivation1Prop = serializedObject.FindProperty("elseToActivation1");
		elseToActivation2Prop = serializedObject.FindProperty("elseToActivation2");
		elseToActivation3Prop = serializedObject.FindProperty("elseToActivation3");
		elseToActivation4Prop = serializedObject.FindProperty("elseToActivation4");
		elseToActivation5Prop = serializedObject.FindProperty("elseToActivation5");
		elseToActivation6Prop = serializedObject.FindProperty("elseToActivation6");
		elseToActivation7Prop = serializedObject.FindProperty("elseToActivation7");
		elseToActivation8Prop = serializedObject.FindProperty("elseToActivation8");
		elseToActivation9Prop = serializedObject.FindProperty("elseToActivation9");

	}

	public override void OnInspectorGUI()
	{
		// Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
		serializedObject.Update();
		ActivationStation activationStation = (ActivationStation)target;
		
		Color UIwhite = new Color ((255F/255F), (255F/255F), (255F/255F));
		Color UIgray  = new Color ((204F/255F), (204F/255F), (204F/255F));
		Color UIgreen = new Color ((138F/255F), (255F/255F), (167F/255F));
		Color UIred   = new Color ((255F/255F), (178F/255F), (178F/255F));
		
		if (Application.isPlaying)
		{
			UIwhite = new Color (((255F*0.80F)/255F), ((255F*0.80F)/255F), ((255F*0.80F)/255F));
			UIgray  = new Color (((204F*0.80F)/255F), ((204F*0.80F)/255F), ((204F*0.80F)/255F));
			UIgreen = new Color (((138F*0.92F)/255F), ((255F*0.92F)/255F), ((167F*0.92F)/255F));
			UIred   = new Color (((255F*0.92F)/255F), ((178F*0.92F)/255F), ((178F*0.92F)/255F));
		}
		
		GUI.color = UIwhite;

		activationStation.activationStyles = new string[]
		{
			/*0*/  "The Checklist",
			/*1*/  "Whichever Works...",
			/*2*/  "Follow the Leader!",
			/*3*/  "Always Rebel!",
			/*4*/  "Mouse Activation",
			/*5*/  "Keyboard Activation"
		};

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(scriptNoteProp.stringValue, EditorStyles.boldLabel);
		if (maximizedScriptProp.boolValue == true) { GUI.color = UIgray; if (GUILayout.Button("Maximize", EditorStyles.miniButton)) { maximizedScriptProp.boolValue = !maximizedScriptProp.boolValue; } }
		else { if (GUILayout.Button("Maximize", EditorStyles.miniButton)) { maximizedScriptProp.boolValue = !maximizedScriptProp.boolValue; } }
		GUI.color = UIwhite;
		EditorGUILayout.EndHorizontal();

		if (maximizedScriptProp.boolValue == true)
		{
			EditorGUILayout.PropertyField(scriptNoteProp);
			EditorGUILayout.Space();

			activationStyleIndexProp.intValue = EditorGUILayout.Popup("Activation Style", activationStyleIndexProp.intValue, activationStation.activationStyles);
			EditorGUILayout.Space();

			// Show Condition Options for The Checklist and Whichever Works
			if (activationStyleIndexProp.intValue == 0 || activationStyleIndexProp.intValue == 1) 
			{
				EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("Conditions", EditorStyles.boldLabel);
					if (GUILayout.Button ("Enable All", EditorStyles.miniButton))
					{ 
						isActivation1Prop.boolValue = true; isActivation2Prop.boolValue = true; isActivation3Prop.boolValue = true;
						isActivation4Prop.boolValue = true; isActivation5Prop.boolValue = true; isActivation6Prop.boolValue = true;
						isActivation7Prop.boolValue = true; isActivation8Prop.boolValue = true; isActivation9Prop.boolValue = true;
					}
					if (GUILayout.Button ("Disable All", EditorStyles.miniButton))
					{  
						isActivation1Prop.boolValue = false; isActivation2Prop.boolValue = false; isActivation3Prop.boolValue = false;
						isActivation4Prop.boolValue = false; isActivation5Prop.boolValue = false; isActivation6Prop.boolValue = false;
						isActivation7Prop.boolValue = false; isActivation8Prop.boolValue = false; isActivation9Prop.boolValue = false;
					}
					if (GUILayout.Button ("Clear All", EditorStyles.miniButton))
					{ 
						ifObject1Prop.objectReferenceValue = null; ifObject2Prop.objectReferenceValue = null; ifObject3Prop.objectReferenceValue = null;
						ifObject4Prop.objectReferenceValue = null; ifObject5Prop.objectReferenceValue = null; ifObject6Prop.objectReferenceValue = null;
						ifObject7Prop.objectReferenceValue = null; ifObject8Prop.objectReferenceValue = null; ifObject9Prop.objectReferenceValue = null;
					}
				EditorGUILayout.EndHorizontal();

				GUI.color = UIwhite; EditorGUILayout.BeginHorizontal();
				if (condition1StatusProp.intValue == -1) { GUI.color = UIred; } if (condition1StatusProp.intValue == 1) { GUI.color = UIgreen; }
				EditorGUILayout.PropertyField(ifObject1Prop, new GUIContent(""));
				EditorGUILayout.PropertyField(isActivation1Prop, new GUIContent(""), GUILayout.MaxWidth(20));
				GUI.color = UIwhite; EditorGUILayout.Space();
				if (condition2StatusProp.intValue == -1) { GUI.color = UIred; } if (condition2StatusProp.intValue == 1) { GUI.color = UIgreen; }
				EditorGUILayout.PropertyField(ifObject2Prop, new GUIContent(""));
				EditorGUILayout.PropertyField(isActivation2Prop, new GUIContent(""), GUILayout.MaxWidth(20));
				GUI.color = UIwhite; EditorGUILayout.Space();
				if (condition3StatusProp.intValue == -1) { GUI.color = UIred; } if (condition3StatusProp.intValue == 1) { GUI.color = UIgreen; }
				EditorGUILayout.PropertyField(ifObject3Prop, new GUIContent(""));
				EditorGUILayout.PropertyField(isActivation3Prop, new GUIContent(""), GUILayout.MaxWidth(20));
				GUI.color = UIwhite; EditorGUILayout.EndHorizontal();

				GUI.color = UIwhite; EditorGUILayout.BeginHorizontal();
				if (condition4StatusProp.intValue == -1) { GUI.color = UIred; } if (condition4StatusProp.intValue == 1) { GUI.color = UIgreen; }
				EditorGUILayout.PropertyField(ifObject4Prop, new GUIContent(""));
				EditorGUILayout.PropertyField(isActivation4Prop, new GUIContent(""), GUILayout.MaxWidth(20));
				GUI.color = UIwhite; EditorGUILayout.Space();
				if (condition5StatusProp.intValue == -1) { GUI.color = UIred; } if (condition5StatusProp.intValue == 1) { GUI.color = UIgreen; }
				EditorGUILayout.PropertyField(ifObject5Prop, new GUIContent(""));
				EditorGUILayout.PropertyField(isActivation5Prop, new GUIContent(""), GUILayout.MaxWidth(20));
				GUI.color = UIwhite; EditorGUILayout.Space();
				if (condition6StatusProp.intValue == -1) { GUI.color = UIred; } if (condition6StatusProp.intValue == 1) { GUI.color = UIgreen; }
				EditorGUILayout.PropertyField(ifObject6Prop, new GUIContent(""));
				EditorGUILayout.PropertyField(isActivation6Prop, new GUIContent(""), GUILayout.MaxWidth(20));
				GUI.color = UIwhite; EditorGUILayout.EndHorizontal();

				GUI.color = UIwhite; EditorGUILayout.BeginHorizontal();
				if (condition7StatusProp.intValue == -1) { GUI.color = UIred; } if (condition7StatusProp.intValue == 1) { GUI.color = UIgreen; }
				EditorGUILayout.PropertyField(ifObject7Prop, new GUIContent(""));
				EditorGUILayout.PropertyField(isActivation7Prop, new GUIContent(""), GUILayout.MaxWidth(20));
				GUI.color = UIwhite; EditorGUILayout.Space();
				if (condition8StatusProp.intValue == -1) { GUI.color = UIred; } if (condition8StatusProp.intValue == 1) { GUI.color = UIgreen; }
				EditorGUILayout.PropertyField(ifObject8Prop, new GUIContent(""));
				EditorGUILayout.PropertyField(isActivation8Prop, new GUIContent(""), GUILayout.MaxWidth(20));
				GUI.color = UIwhite; EditorGUILayout.Space();
				if (condition9StatusProp.intValue == -1) { GUI.color = UIred; } if (condition9StatusProp.intValue == 1) { GUI.color = UIgreen; }
				EditorGUILayout.PropertyField(ifObject9Prop, new GUIContent(""));
				EditorGUILayout.PropertyField(isActivation9Prop, new GUIContent(""), GUILayout.MaxWidth(20));
				GUI.color = UIwhite; EditorGUILayout.EndHorizontal();
			}

			if (activationStyleIndexProp.intValue == 2 || activationStyleIndexProp.intValue == 3) 
			{
				if (activationStyleIndexProp.intValue == 2) { EditorGUILayout.LabelField("Leader", EditorStyles.boldLabel); }
				if (activationStyleIndexProp.intValue == 3) { EditorGUILayout.LabelField("Tyrant", EditorStyles.boldLabel); }

				GUI.color = UIwhite;
				if (ifObject1Prop.objectReferenceValue != null) { if (activationStation.ifObject1.activeInHierarchy == true) { GUI.color = UIgreen; } else { GUI.color = UIred; } }
				EditorGUILayout.PropertyField(ifObject1Prop, new GUIContent(""));
				GUI.color = UIwhite;
			}

			EditorGUILayout.BeginHorizontal();
			if (activationStyleIndexProp.intValue == 4)
			{
				EditorGUILayout.LabelField("Mouse Condition", EditorStyles.boldLabel, GUILayout.Width(156));
				mouseButtonConditionIndexProp.intValue = EditorGUILayout.Popup("", mouseButtonConditionIndexProp.intValue, activationStation.mouseButtonConditions, GUILayout.MaxWidth(70));
				inputConditionIndexProp.intValue = EditorGUILayout.Popup("", inputConditionIndexProp.intValue, activationStation.inputConditions, GUILayout.MaxWidth(60));
				locationButtonConditionIndexProp.intValue = EditorGUILayout.Popup("", locationButtonConditionIndexProp.intValue, activationStation.locationButtonConditions, GUILayout.MaxWidth(87));
			}
			if (activationStyleIndexProp.intValue == 5)
			{
				EditorGUILayout.LabelField("Keyboard Condition", EditorStyles.boldLabel, GUILayout.Width(156));
				EditorGUILayout.PropertyField(keyButtonConditionProp, new GUIContent(""), GUILayout.MaxWidth(70));
				inputConditionIndexProp.intValue = EditorGUILayout.Popup("", inputConditionIndexProp.intValue, activationStation.inputConditions, GUILayout.MaxWidth(60));
				locationButtonConditionIndexProp.intValue = EditorGUILayout.Popup("", locationButtonConditionIndexProp.intValue, activationStation.locationButtonConditions, GUILayout.MaxWidth(87));
				GUI.color = UIgreen; if (GUILayout.Button ("List of Keys", EditorStyles.miniButton, GUILayout.MaxWidth(70))) { Application.OpenURL("https://docs.unity3d.com/Manual/ConventionalGameInput.html"); } GUI.color = UIwhite;
			}
			EditorGUILayout.EndHorizontal();

			if (activationStyleIndexProp.intValue == 4) 
			{
				GUI.color = UIwhite; EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Mouse Status", EditorStyles.boldLabel, GUILayout.Width(156));
				if (mouseInputSuccessProp.boolValue == true) { GUI.color = UIgreen; } else { GUI.color = UIwhite; }
				EditorGUILayout.LabelField("", activationStation.mouseButtonStatus, EditorStyles.numberField, GUILayout.MaxWidth(70));
				EditorGUILayout.LabelField("", activationStation.mouseInputStatus, EditorStyles.numberField, GUILayout.MaxWidth(60));
				if (mouseLocationSuccessProp.boolValue == true) { GUI.color = UIgreen; } else { GUI.color = UIwhite; }
				EditorGUILayout.LabelField("", activationStation.mouseLocationStatus, EditorStyles.numberField, GUILayout.MaxWidth(87));
				GUI.color = UIwhite; EditorGUILayout.EndHorizontal();
			}
			if (activationStyleIndexProp.intValue == 5) 
			{
				GUI.color = UIwhite; EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Keyboard Status", EditorStyles.boldLabel, GUILayout.Width(156));
				if (keyInputSuccessProp.boolValue == true) { GUI.color = UIgreen; } else { GUI.color = UIwhite; }
				EditorGUILayout.LabelField("", activationStation.keyButtonStatus, EditorStyles.numberField, GUILayout.MaxWidth(70));
				EditorGUILayout.LabelField("", activationStation.keyInputStatus, EditorStyles.numberField, GUILayout.MaxWidth(60));
				if (keyLocationSuccessProp.boolValue == true) { GUI.color = UIgreen; } else { GUI.color = UIwhite; }
				EditorGUILayout.LabelField("", activationStation.keyLocationStatus, EditorStyles.numberField, GUILayout.MaxWidth(87));
				GUI.color = UIwhite; EditorGUILayout.EndHorizontal();
			}

			EditorGUILayout.Space();

			EditorGUILayout.BeginHorizontal();
				if (activationStyleIndexProp.intValue == 0 || activationStyleIndexProp.intValue == 4 || activationStyleIndexProp.intValue == 5)
					{ EditorGUILayout.LabelField ("Impact of Met Conditions", EditorStyles.boldLabel); }

				if (activationStyleIndexProp.intValue == 1)
					{ EditorGUILayout.LabelField ("Impact of a Met Condition", EditorStyles.boldLabel); }
				
				if (activationStyleIndexProp.intValue != 2 && activationStyleIndexProp.intValue != 3)
				{
					if (GUILayout.Button ("Enable All", EditorStyles.miniButton))
					{  
						toActivation1Prop.boolValue = true; toActivation2Prop.boolValue = true; toActivation3Prop.boolValue = true;
						toActivation4Prop.boolValue = true; toActivation5Prop.boolValue = true; toActivation6Prop.boolValue = true;
						toActivation7Prop.boolValue = true; toActivation8Prop.boolValue = true; toActivation9Prop.boolValue = true;
					}
					if (GUILayout.Button ("Disable All", EditorStyles.miniButton))
					{   
						toActivation1Prop.boolValue = false; toActivation2Prop.boolValue = false; toActivation3Prop.boolValue = false;
						toActivation4Prop.boolValue = false; toActivation5Prop.boolValue = false; toActivation6Prop.boolValue = false;
						toActivation7Prop.boolValue = false; toActivation8Prop.boolValue = false; toActivation9Prop.boolValue = false;
					}
					if (GUILayout.Button ("Clear All", EditorStyles.miniButton))
					{ 
						setObject1Prop.objectReferenceValue = null; setObject2Prop.objectReferenceValue = null; setObject3Prop.objectReferenceValue = null;
						setObject4Prop.objectReferenceValue = null; setObject5Prop.objectReferenceValue = null; setObject6Prop.objectReferenceValue = null;
						setObject7Prop.objectReferenceValue = null; setObject8Prop.objectReferenceValue = null; setObject9Prop.objectReferenceValue = null;
					}
				}
			EditorGUILayout.EndHorizontal();

			if (activationStyleIndexProp.intValue == 2 || activationStyleIndexProp.intValue == 3) 
			{
				EditorGUILayout.BeginHorizontal();
					if (activationStyleIndexProp.intValue == 2) { EditorGUILayout.LabelField ("Followers", EditorStyles.boldLabel); }
					if (activationStyleIndexProp.intValue == 3) { EditorGUILayout.LabelField ("Rebels", EditorStyles.boldLabel); }
					if (GUILayout.Button ("Clear All", EditorStyles.miniButton, GUILayout.MaxWidth(60)))
					{ 
						setObject1Prop.objectReferenceValue = null; setObject2Prop.objectReferenceValue = null; setObject3Prop.objectReferenceValue = null;
						setObject4Prop.objectReferenceValue = null; setObject5Prop.objectReferenceValue = null; setObject6Prop.objectReferenceValue = null;
						setObject7Prop.objectReferenceValue = null; setObject8Prop.objectReferenceValue = null; setObject9Prop.objectReferenceValue = null;
					}
				EditorGUILayout.EndHorizontal();

				GUI.color = UIwhite; EditorGUILayout.BeginHorizontal();
				if (setObject1Prop.objectReferenceValue != null) { if (activationStation.setObject1.activeInHierarchy == true) { GUI.color = UIgreen; } else { GUI.color = UIred; } } else { GUI.color = UIwhite; }
				EditorGUILayout.PropertyField(setObject1Prop, new GUIContent(""));
				if (setObject2Prop.objectReferenceValue != null) { if (activationStation.setObject2.activeInHierarchy == true) { GUI.color = UIgreen; } else { GUI.color = UIred; } } else { GUI.color = UIwhite; }
				EditorGUILayout.PropertyField(setObject2Prop, new GUIContent(""));
				if (setObject3Prop.objectReferenceValue != null) { if (activationStation.setObject3.activeInHierarchy == true) { GUI.color = UIgreen; } else { GUI.color = UIred; } } else { GUI.color = UIwhite; }
				EditorGUILayout.PropertyField(setObject3Prop, new GUIContent(""));
				GUI.color = UIwhite; EditorGUILayout.EndHorizontal();

				GUI.color = UIwhite; EditorGUILayout.BeginHorizontal();
				if (setObject4Prop.objectReferenceValue != null) { if (activationStation.setObject4.activeInHierarchy == true) { GUI.color = UIgreen; } else { GUI.color = UIred; } } else { GUI.color = UIwhite; }
				EditorGUILayout.PropertyField(setObject4Prop, new GUIContent(""));
				if (setObject5Prop.objectReferenceValue != null) { if (activationStation.setObject5.activeInHierarchy == true) { GUI.color = UIgreen; } else { GUI.color = UIred; } } else { GUI.color = UIwhite; }
				EditorGUILayout.PropertyField(setObject5Prop, new GUIContent(""));
				if (setObject6Prop.objectReferenceValue != null) { if (activationStation.setObject6.activeInHierarchy == true) { GUI.color = UIgreen; } else { GUI.color = UIred; } } else { GUI.color = UIwhite; }
				EditorGUILayout.PropertyField(setObject6Prop, new GUIContent(""));
				GUI.color = UIwhite; EditorGUILayout.EndHorizontal();

				GUI.color = UIwhite; EditorGUILayout.BeginHorizontal();
				if (setObject7Prop.objectReferenceValue != null) { if (activationStation.setObject7.activeInHierarchy == true) { GUI.color = UIgreen; } else { GUI.color = UIred; } } else { GUI.color = UIwhite; }
				EditorGUILayout.PropertyField(setObject7Prop, new GUIContent(""));
				if (setObject8Prop.objectReferenceValue != null) { if (activationStation.setObject8.activeInHierarchy == true) { GUI.color = UIgreen; } else { GUI.color = UIred; } } else { GUI.color = UIwhite; }
				EditorGUILayout.PropertyField(setObject8Prop, new GUIContent(""));
				if (setObject9Prop.objectReferenceValue != null) { if (activationStation.setObject9.activeInHierarchy == true) { GUI.color = UIgreen; } else { GUI.color = UIred; } } else { GUI.color = UIwhite; }
				EditorGUILayout.PropertyField(setObject9Prop, new GUIContent(""));
				GUI.color = UIwhite; EditorGUILayout.EndHorizontal();
			}
			else
			{
				GUI.color = UIwhite; EditorGUILayout.BeginHorizontal();
				if (impact1StatusProp.intValue == 1) { GUI.color = UIgreen; }
				EditorGUILayout.PropertyField(setObject1Prop, new GUIContent(""));
				if (activationStyleIndexProp.intValue != 2 && activationStyleIndexProp.intValue != 3) { EditorGUILayout.PropertyField(toActivation1Prop, new GUIContent(""), GUILayout.MaxWidth(20)); }
				GUI.color = UIwhite; EditorGUILayout.Space();
				if (impact2StatusProp.intValue == 1) { GUI.color = UIgreen; }
				EditorGUILayout.PropertyField(setObject2Prop, new GUIContent(""));
				if (activationStyleIndexProp.intValue != 2 && activationStyleIndexProp.intValue != 3) { EditorGUILayout.PropertyField(toActivation2Prop, new GUIContent(""), GUILayout.MaxWidth(20)); }
				GUI.color = UIwhite; EditorGUILayout.Space();
				if (impact3StatusProp.intValue == 1) { GUI.color = UIgreen; }
				EditorGUILayout.PropertyField(setObject3Prop, new GUIContent(""));
				if (activationStyleIndexProp.intValue != 2 && activationStyleIndexProp.intValue != 3) { EditorGUILayout.PropertyField(toActivation3Prop, new GUIContent(""), GUILayout.MaxWidth(20)); }
				GUI.color = UIwhite; EditorGUILayout.EndHorizontal();

				GUI.color = UIwhite; EditorGUILayout.BeginHorizontal();
				if (impact4StatusProp.intValue == 1) { GUI.color = UIgreen; }
				EditorGUILayout.PropertyField(setObject4Prop, new GUIContent(""));
				if (activationStyleIndexProp.intValue != 2 && activationStyleIndexProp.intValue != 3) { EditorGUILayout.PropertyField(toActivation4Prop, new GUIContent(""), GUILayout.MaxWidth(20)); }
				GUI.color = UIwhite; EditorGUILayout.Space();
				if (impact5StatusProp.intValue == 1) { GUI.color = UIgreen; }
				EditorGUILayout.PropertyField(setObject5Prop, new GUIContent(""));
				if (activationStyleIndexProp.intValue != 2 && activationStyleIndexProp.intValue != 3) { EditorGUILayout.PropertyField(toActivation5Prop, new GUIContent(""), GUILayout.MaxWidth(20)); }
				GUI.color = UIwhite; EditorGUILayout.Space();
				if (impact6StatusProp.intValue == 1) { GUI.color = UIgreen; }
				EditorGUILayout.PropertyField(setObject6Prop, new GUIContent(""));
				if (activationStyleIndexProp.intValue != 2 && activationStyleIndexProp.intValue != 3) { EditorGUILayout.PropertyField(toActivation6Prop, new GUIContent(""), GUILayout.MaxWidth(20)); }
				GUI.color = UIwhite; EditorGUILayout.EndHorizontal();

				GUI.color = UIwhite; EditorGUILayout.BeginHorizontal();
				if (impact7StatusProp.intValue == 1) { GUI.color = UIgreen; }
				EditorGUILayout.PropertyField(setObject7Prop, new GUIContent(""));
				if (activationStyleIndexProp.intValue != 2 && activationStyleIndexProp.intValue != 3) { EditorGUILayout.PropertyField(toActivation7Prop, new GUIContent(""), GUILayout.MaxWidth(20)); }
				GUI.color = UIwhite; EditorGUILayout.Space();
				if (impact8StatusProp.intValue == 1) { GUI.color = UIgreen; }
				EditorGUILayout.PropertyField(setObject8Prop, new GUIContent(""));
				if (activationStyleIndexProp.intValue != 2 && activationStyleIndexProp.intValue != 3) { EditorGUILayout.PropertyField(toActivation8Prop, new GUIContent(""), GUILayout.MaxWidth(20)); }
				GUI.color = UIwhite; EditorGUILayout.Space();
				if (impact9StatusProp.intValue == 1) { GUI.color = UIgreen; }
				EditorGUILayout.PropertyField(setObject9Prop, new GUIContent(""));
				if (activationStyleIndexProp.intValue != 2 && activationStyleIndexProp.intValue != 3) { EditorGUILayout.PropertyField(toActivation9Prop, new GUIContent(""), GUILayout.MaxWidth(20)); }
				GUI.color = UIwhite; EditorGUILayout.EndHorizontal();
			}

			EditorGUILayout.Space();

			// Show Consequence Options for The Checklist and Whichever Works
			if (activationStyleIndexProp.intValue == 0 || activationStyleIndexProp.intValue == 1 || activationStyleIndexProp.intValue == 4 || activationStyleIndexProp.intValue == 5) 
			{
				EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField ("Consequences of Unmet Conditions", EditorStyles.boldLabel);
					if (GUILayout.Button ("Enable All", EditorStyles.miniButton))
					{   
						elseToActivation1Prop.boolValue = true; elseToActivation2Prop.boolValue = true; elseToActivation3Prop.boolValue = true;
						elseToActivation4Prop.boolValue = true; elseToActivation5Prop.boolValue = true; elseToActivation6Prop.boolValue = true;
						elseToActivation7Prop.boolValue = true; elseToActivation8Prop.boolValue = true; elseToActivation9Prop.boolValue = true;
					}
					if (GUILayout.Button ("Disable All", EditorStyles.miniButton))
					{    
						elseToActivation1Prop.boolValue = false; elseToActivation2Prop.boolValue = false; elseToActivation3Prop.boolValue = false;
						elseToActivation4Prop.boolValue = false; elseToActivation5Prop.boolValue = false; elseToActivation6Prop.boolValue = false;
						elseToActivation7Prop.boolValue = false; elseToActivation8Prop.boolValue = false; elseToActivation9Prop.boolValue = false;
					}
					if (noConsequencesProp.boolValue == false) 
					{ GUI.color = UIgray; if (GUILayout.Button("Maximize", EditorStyles.miniButton)) { noConsequencesProp.boolValue = !noConsequencesProp.boolValue; } }
					else { if (GUILayout.Button("Maximize", EditorStyles.miniButton)) { noConsequencesProp.boolValue = !noConsequencesProp.boolValue; } }
					GUI.color = UIwhite;
				EditorGUILayout.EndHorizontal();

				if (noConsequencesProp.boolValue == false)
				{
					GUI.color = UIwhite; EditorGUILayout.BeginHorizontal();
					if (consequence1StatusProp.intValue == 1) { GUI.color = UIgreen; }
					EditorGUILayout.PropertyField(setObject1Prop, new GUIContent(""));
					EditorGUILayout.PropertyField(elseToActivation1Prop, new GUIContent(""), GUILayout.MaxWidth(20));
					GUI.color = UIwhite; EditorGUILayout.Space();
					if (consequence2StatusProp.intValue == 1) { GUI.color = UIgreen; }
					EditorGUILayout.PropertyField(setObject2Prop, new GUIContent(""));
					EditorGUILayout.PropertyField(elseToActivation2Prop, new GUIContent(""), GUILayout.MaxWidth(20));
					GUI.color = UIwhite; EditorGUILayout.Space();
					if (consequence3StatusProp.intValue == 1) { GUI.color = UIgreen; }
					EditorGUILayout.PropertyField(setObject3Prop, new GUIContent(""));
					EditorGUILayout.PropertyField(elseToActivation3Prop, new GUIContent(""), GUILayout.MaxWidth(20));
					GUI.color = UIwhite; EditorGUILayout.EndHorizontal();

					GUI.color = UIwhite; EditorGUILayout.BeginHorizontal();
					if (consequence4StatusProp.intValue == 1) { GUI.color = UIgreen; }
					EditorGUILayout.PropertyField(setObject4Prop, new GUIContent(""));
					EditorGUILayout.PropertyField(elseToActivation4Prop, new GUIContent(""), GUILayout.MaxWidth(20));
					GUI.color = UIwhite; EditorGUILayout.Space();
					if (consequence5StatusProp.intValue == 1) { GUI.color = UIgreen; }
					EditorGUILayout.PropertyField(setObject5Prop, new GUIContent(""));
					EditorGUILayout.PropertyField(elseToActivation5Prop, new GUIContent(""), GUILayout.MaxWidth(20));
					GUI.color = UIwhite; EditorGUILayout.Space();
					if (consequence6StatusProp.intValue == 1) { GUI.color = UIgreen; }
					EditorGUILayout.PropertyField(setObject6Prop, new GUIContent(""));
					EditorGUILayout.PropertyField(elseToActivation6Prop, new GUIContent(""), GUILayout.MaxWidth(20));
					GUI.color = UIwhite; EditorGUILayout.EndHorizontal();

					GUI.color = UIwhite; EditorGUILayout.BeginHorizontal();
					if (consequence7StatusProp.intValue == 1) { GUI.color = UIgreen; }
					EditorGUILayout.PropertyField(setObject7Prop, new GUIContent(""));
					EditorGUILayout.PropertyField(elseToActivation7Prop, new GUIContent(""), GUILayout.MaxWidth(20));
					GUI.color = UIwhite; EditorGUILayout.Space();
					if (consequence8StatusProp.intValue == 1) { GUI.color = UIgreen; }
					EditorGUILayout.PropertyField(setObject8Prop, new GUIContent(""));
					EditorGUILayout.PropertyField(elseToActivation8Prop, new GUIContent(""), GUILayout.MaxWidth(20));
					GUI.color = UIwhite; EditorGUILayout.Space();
					if (consequence9StatusProp.intValue == 1) { GUI.color = UIgreen; }
					EditorGUILayout.PropertyField(setObject9Prop, new GUIContent(""));
					EditorGUILayout.PropertyField(elseToActivation9Prop, new GUIContent(""), GUILayout.MaxWidth(20));
					GUI.color = UIwhite; EditorGUILayout.EndHorizontal();

					EditorGUILayout.Space();
				}
			}

			if (ifObject1Prop.objectReferenceValue == null) { isActivation1Prop.boolValue = false; }
			if (ifObject2Prop.objectReferenceValue == null) { isActivation2Prop.boolValue = false; }
			if (ifObject3Prop.objectReferenceValue == null) { isActivation3Prop.boolValue = false; }
			if (ifObject4Prop.objectReferenceValue == null) { isActivation4Prop.boolValue = false; }
			if (ifObject5Prop.objectReferenceValue == null) { isActivation5Prop.boolValue = false; }
			if (ifObject6Prop.objectReferenceValue == null) { isActivation6Prop.boolValue = false; }
			if (ifObject7Prop.objectReferenceValue == null) { isActivation7Prop.boolValue = false; }
			if (ifObject8Prop.objectReferenceValue == null) { isActivation8Prop.boolValue = false; }
			if (ifObject9Prop.objectReferenceValue == null) { isActivation9Prop.boolValue = false; }

			if (setObject1Prop.objectReferenceValue == null) { toActivation1Prop.boolValue = false; elseToActivation1Prop.boolValue = false; }
			if (setObject2Prop.objectReferenceValue == null) { toActivation2Prop.boolValue = false; elseToActivation2Prop.boolValue = false; }
			if (setObject3Prop.objectReferenceValue == null) { toActivation3Prop.boolValue = false; elseToActivation3Prop.boolValue = false; }
			if (setObject4Prop.objectReferenceValue == null) { toActivation4Prop.boolValue = false; elseToActivation4Prop.boolValue = false; }
			if (setObject5Prop.objectReferenceValue == null) { toActivation5Prop.boolValue = false; elseToActivation5Prop.boolValue = false; }
			if (setObject6Prop.objectReferenceValue == null) { toActivation6Prop.boolValue = false; elseToActivation6Prop.boolValue = false; }
			if (setObject7Prop.objectReferenceValue == null) { toActivation7Prop.boolValue = false; elseToActivation7Prop.boolValue = false; }
			if (setObject8Prop.objectReferenceValue == null) { toActivation8Prop.boolValue = false; elseToActivation8Prop.boolValue = false; }
			if (setObject9Prop.objectReferenceValue == null) { toActivation9Prop.boolValue = false; elseToActivation9Prop.boolValue = false; }

			EditorGUILayout.Space();
		}
		// Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
		serializedObject.ApplyModifiedProperties();
	}
}
#endif