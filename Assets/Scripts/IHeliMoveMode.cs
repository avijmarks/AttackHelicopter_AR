using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHeliMoveMode
{
    void StartHeliMoveMode();

    void ExecuteHeliMoveModeUpdate();

    public void EndHeliMoveMode();
}
