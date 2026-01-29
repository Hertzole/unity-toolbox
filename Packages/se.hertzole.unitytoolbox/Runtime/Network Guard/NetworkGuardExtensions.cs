#if TOOLBOX_NETCODE_GO
using System.Runtime.CompilerServices;
using Unity.Netcode;

namespace Hertzole.UnityToolbox
{
    public static class NetworkGuardExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MustHaveAuthority(this NetworkBehaviour nb, [CallerMemberName] string caller = "")
        {
            NetworkGuard.MustHaveAuthority(nb.HasAuthority, caller);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MustHaveAuthority(this NetworkObject no, [CallerMemberName] string caller = "")
        {
            NetworkGuard.MustHaveAuthority(no.HasAuthority, caller);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MustBeServer(this NetworkBehaviour nb, [CallerMemberName] string caller = "")
        {
            NetworkGuard.MustBeServer(nb.IsServer, caller);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MustBeClient(this NetworkBehaviour nb, [CallerMemberName] string caller = "")
        {
            NetworkGuard.MustBeClient(nb.IsClient, caller);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MustBeOwner(this NetworkBehaviour nb, [CallerMemberName] string caller = "")
        {
            NetworkGuard.MustBeOwner(nb.IsOwner, caller);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MustBeOwner(this NetworkObject no, [CallerMemberName] string caller = "")
        {
            NetworkGuard.MustBeOwner(no.IsOwner, caller);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MustBeSessionOwner(this NetworkBehaviour nb, [CallerMemberName] string caller = "")
        {
            NetworkGuard.MustBeSessionOwner(nb.IsSessionOwner, caller);
        }
    }
}
#endif