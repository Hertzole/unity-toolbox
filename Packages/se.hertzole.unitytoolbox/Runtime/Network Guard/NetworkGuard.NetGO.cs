#if TOOLBOX_NETCODE_GO
using System.Runtime.CompilerServices;
using Unity.Netcode;

namespace Hertzole.UnityToolbox
{
    public static partial class NetworkGuard
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MustHaveAuthority(bool hasAuthority, [CallerMemberName] string caller = "")
        {
            if (hasAuthority)
            {
                return;
            }

            ThrowHelper.ThrowAuthorityException(caller);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MustHaveAuthority(NetworkBehaviour behaviour, [CallerMemberName] string caller = "")
        {
            MustHaveAuthority(behaviour.HasAuthority, caller);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MustHaveAuthority(NetworkObject networkObject, [CallerMemberName] string caller = "")
        {
            MustHaveAuthority(networkObject.HasAuthority, caller);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MustBeServer(bool isServer, [CallerMemberName] string caller = "")
        {
            if (isServer)
            {
                return;
            }

            ThrowHelper.ThrowServerException(caller);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MustBeServer(NetworkBehaviour behaviour, [CallerMemberName] string caller = "")
        {
            MustBeServer(behaviour.IsServer, caller);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MustBeClient(bool isClient, [CallerMemberName] string caller = "")
        {
            if (isClient)
            {
                return;
            }

            ThrowHelper.ThrowClientException(caller);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MustBeClient(NetworkBehaviour behaviour, [CallerMemberName] string caller = "")
        {
            MustBeClient(behaviour.IsClient, caller);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MustBeOwner(bool isOwner, [CallerMemberName] string caller = "")
        {
            if (isOwner)
            {
                return;
            }

            ThrowHelper.ThrowOwnerException(caller);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MustBeOwner(NetworkBehaviour behaviour, [CallerMemberName] string caller = "")
        {
            MustBeOwner(behaviour.IsOwner, caller);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MustBeOwner(NetworkObject networkObject, [CallerMemberName] string caller = "")
        {
            MustBeOwner(networkObject.IsOwner, caller);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MustBeSessionOwner(bool isSessionOwner, [CallerMemberName] string caller = "")
        {
            if (isSessionOwner)
            {
                return;
            }

            ThrowHelper.ThrowSessionOwnerException(caller);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MustBeSessionOwner(NetworkBehaviour behaviour, [CallerMemberName] string caller = "")
        {
            MustBeSessionOwner(behaviour.IsSessionOwner, caller);
        }
    }
}
#endif