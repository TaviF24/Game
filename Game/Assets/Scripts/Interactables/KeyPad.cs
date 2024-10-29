using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class KeyPad : Interactable
{
    [SerializeField]
    private GameObject door;
	private bool doorOpen = false;
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	protected override void Interact()
	{
		doorOpen = !doorOpen;
        door.GetComponent<Animator>().SetBool("IsOpen", doorOpen);
	}
}
