using System;

namespace Pochete.Models.Wallets
{
    public class SnapshotSecurity
    {
        public int Id { get; set; }

        public int WalletId { get; set; }

        public DateTime Date { get; set; }

        public virtual Wallet Wallet { get; set; }

        public SnapshotSecurity()
        {
        }
    }
}