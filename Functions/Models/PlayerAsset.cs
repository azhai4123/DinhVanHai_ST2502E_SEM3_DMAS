using System;
namespace Functions.Models
{
    public class PlayerAsset
    {
        public Guid PlayerId { get; set; }
        public Guid AssetId { get; set; }
        public DateTime AcquiredDate { get; set; }
        public int Quantity { get; set; }

        public Player? Player { get; set; }
        public Asset? Asset { get; set; }
    }
}
