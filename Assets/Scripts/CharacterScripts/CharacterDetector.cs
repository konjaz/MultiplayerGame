using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterDetector : MonoBehaviour {
    public CharacterSystem character;
    public List<CharacterSystem> detectedCharacters;
	// Use this for initialization
	void Awake() {
        if (!character)
        { 
            character = gameObject.GetComponentInParent<CharacterSystem>();
        }
            detectedCharacters = new List<CharacterSystem>();
	}
	
	void Update () 
    {

	}

    public CharacterSystem GetClosestEnemy() 
    { 
        if(detectedCharacters.Count>0)
        {
            CharacterSystem closestEnemy = detectedCharacters[0];
            foreach(CharacterSystem enemy in detectedCharacters)
            {
                if (Vector3.Distance(closestEnemy.transform.position, transform.position) > Vector3.Distance(enemy.transform.position, transform.position)) 
                {
                    closestEnemy = enemy;
                }
            }
            return closestEnemy;
        }
        return null;
    }
    public void OnTriggerEnter(Collider col) 
    {
        CharacterSystem newCharDetected;
        if (newCharDetected = col.GetComponent<CharacterSystem>()) 
        {
            if (newCharDetected.charAliment != character.charAliment && !detectedCharacters.Contains(newCharDetected)) 
            {
                detectedCharacters.Add(newCharDetected);
            }
        }
    }

    public void OnTriggerExit(Collider col) 
    {
        CharacterSystem oldDetectedChar;
        if (oldDetectedChar = col.GetComponent<CharacterSystem>())
        {
            if (detectedCharacters.Contains(oldDetectedChar))
            {
                detectedCharacters.Remove(oldDetectedChar);
            }
        }
    }
}

