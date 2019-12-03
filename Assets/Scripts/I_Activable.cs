using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_Activable {
    void canActivate(bool enabled);
	void Activate(bool type = true);
	void Deactivate();
}
