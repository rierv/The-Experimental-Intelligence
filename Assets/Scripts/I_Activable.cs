using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_Activable {
	void Activate(bool type = true);
	void Deactivate();
}
