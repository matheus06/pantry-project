using System.Diagnostics.Metrics;

namespace Microservice.ProductManager.Application.Metrics
{
    public class ProductManagerMetrics
    {
        public ProductManagerMetrics(IMeterFactory meterFactory)
        {
            Meter meter = meterFactory.Create("ProductManager");
            ProductCounter = meter.CreateCounter<int>("product.creation.count");
            
        }

        public Counter<int> ProductCounter {  get; private set; }
    }
}
