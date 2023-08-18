using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKnockBackable
{

    void OnKnockBack(Vector2 direction, float amount
        , AnimationCurve xAxisCurve, AnimationCurve yAxisCurve, float time);


}
