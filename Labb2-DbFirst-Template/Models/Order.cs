using System;
using System.Collections.Generic;

namespace Labb2_DbFirst_Template.Models;

public partial class Order
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public DateOnly OrderDate { get; set; }

    public DateOnly ReadyToCollectDate { get; set; }

    public int CollectionShopId { get; set; }

    public virtual Shop CollectionShop { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
