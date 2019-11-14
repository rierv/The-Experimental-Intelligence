using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_Activable {
	void Activate();
    void Activate(bool twoFunctions); //if twoFunctions == true -> Activate first function, else -> Activate second function
	void Deactivate();
}
