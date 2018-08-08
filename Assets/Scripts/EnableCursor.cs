using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableCursor : MonoBehaviour {

	void Start () {
        Cursor.lockState = CursorLockMode.None;
	}
}
