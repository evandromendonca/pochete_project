namespace Pochete.Models.Wallets
{
    public class SnapshotCash : SnapshotSecurity
    {
        public decimal Value { get; set; }

        public int CurrencyId { get; set; }

        public Currency Currency { get; set; }

        public SnapshotCash()
        {
        }
    }
}