using System.Collections.Generic;

namespace Pochete.Models.Wallets
{
    public class Wallet
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual List<SnapshotSecurity> SnapshotsSecuritys { get; set; }

        public Wallet ()
        { 
        }
    }
}