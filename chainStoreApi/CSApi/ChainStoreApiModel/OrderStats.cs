


namespace ChainStoreApiModel
{
    public class OrderStat
    {

        public int TotalOrders { get; set; }
        public string MostOrdered { get; set; }
        public decimal AverageOrderPrice { get; set; }
        public decimal MaximumOrderPrice { get; set; }
        public decimal MinimumOrderPrice { get; set; }


        public OrderStat(int totalOrders, string mostOrdered, decimal averageOrderPrice, decimal maximumOrderPrice, decimal minimumOrderPrice)
        {
            this.TotalOrders = totalOrders;
            this.MostOrdered = mostOrdered;
            this.AverageOrderPrice = averageOrderPrice;
            this.MaximumOrderPrice = maximumOrderPrice;
            this.MinimumOrderPrice = minimumOrderPrice;

        }

    }
}