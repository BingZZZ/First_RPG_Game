using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethod
{
    private static float fTargetThreshold = 0f;
    private static float fAttackRangeThreshold = 0.5f;
    public static bool IsFacingTarget(this Transform transform, Transform target)
    {
        return Vector3.Dot(transform.forward, (target.position - transform.position).normalized) > fTargetThreshold;
    }
    public static bool IsFacingAttackRange(this Transform transform, Transform target)
    {
        return Vector3.Dot(transform.forward, (target.position - transform.position).normalized) > fAttackRangeThreshold;
    }
    public static bool IsFacingAttackRange(this Transform transform, Vector3 direction)
    {
        return Vector3.Dot(transform.forward, direction) > fAttackRangeThreshold;
    }
}
