#if TOOLBOX_NETCODE_GO
using System;
using System.Diagnostics.CodeAnalysis;

namespace Hertzole.UnityToolbox
{
    partial class NetworkGuard
    {
        private static class ThrowHelper
        {
            [DoesNotReturn]
            public static void ThrowAuthorityException(string methodName)
            {
                throw new InvalidOperationException($"{methodName} called on object without authority.");
            }

            [DoesNotReturn]
            public static void ThrowServerException(string methodName)
            {
                throw new InvalidOperationException($"{methodName} called on non-server object.");
            }

            [DoesNotReturn]
            public static void ThrowClientException(string methodName)
            {
                throw new InvalidOperationException($"{methodName} called on non-client object.");
            }

            [DoesNotReturn]
            public static void ThrowOwnerException(string methodName)
            {
                throw new InvalidOperationException($"{methodName} called on non-owner object.");
            }

            [DoesNotReturn]
            public static void ThrowSessionOwnerException(string methodName)
            {
                throw new InvalidOperationException($"{methodName} called on non-session owner object.");
            }
        }
    }
}
#endif