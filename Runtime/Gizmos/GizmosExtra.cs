using System.Diagnostics;
using UnityEngine;

namespace Hertzole.UnityToolbox
{
    public static class GizmosExtra
    {
        private const float DEFAULT_ARROW_HEAD_LENGTH = 0.25f;
        private const float DEFAULT_ARROW_HEAD_ANGLE = 20.0f;

        [Conditional("UNITY_EDITOR")]
        public static void DrawArrowToPoint(Vector3 position,
            Vector3 targetPosition,
            float arrowHeadLength = DEFAULT_ARROW_HEAD_LENGTH,
            float arrowHeadAngle = DEFAULT_ARROW_HEAD_ANGLE)
        {
#if UNITY_EDITOR
            Vector3 direction = targetPosition - position;
            DrawArrow(position, direction, 1, arrowHeadLength, arrowHeadAngle);
#endif
        }

        [Conditional("UNITY_EDITOR")]
        public static void DrawArrow(Vector3 pos,
            Vector3 direction,
            float arrowLength,
            float arrowHeadLength = DEFAULT_ARROW_HEAD_LENGTH,
            float arrowHeadAngle = DEFAULT_ARROW_HEAD_ANGLE)
        {
#if UNITY_EDITOR
            Vector3 arrowTip = pos + direction * arrowLength;
            Gizmos.DrawLine(pos, arrowTip);

            Camera c = Camera.current;
            if (c == null)
            {
                return;
            }

            Vector3 right = Quaternion.LookRotation(direction, c.transform.forward) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(direction, c.transform.forward) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Gizmos.DrawLine(arrowTip, arrowTip + right * arrowHeadLength);
            Gizmos.DrawLine(arrowTip, arrowTip + left * arrowHeadLength);
#endif
        }
    }
}