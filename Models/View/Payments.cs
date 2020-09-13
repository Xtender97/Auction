using System.Collections.Generic;
using Projekat.Models.Database;

namespace Projekat.Models.View{ 

    public class PaymentsModel {

        public List<Order> Orders;
        public int pageNumber { get; set; }
    }
}