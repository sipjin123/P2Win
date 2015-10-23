using UnityEngine;
using System.Collections;

public interface ISignalListener {

	void Execute(SignalType type, ISignalParameters param);

}
