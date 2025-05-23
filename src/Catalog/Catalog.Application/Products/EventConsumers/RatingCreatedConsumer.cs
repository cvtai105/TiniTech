
using Catalog.Application.Common.Abstraction;
using Contracts.RatingIntegrationEvents;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Products.EventConsumers;

public class RatingCreatedConsumer : IConsumer<RatingCreated>
{
    private readonly DbContextAbstract _context;
    public RatingCreatedConsumer(DbContextAbstract context)
    {
        _context = context;
    }
    public async Task Consume(ConsumeContext<RatingCreated> context)
    {
        var productMetric = await _context.ProductMetrics.FirstOrDefaultAsync(x => x.ProductId == context.Message.ProductId, context.CancellationToken);
        if (productMetric == null)
        {
            _context.ProductMetrics.Add(new Domain.Entities.ProductMetric
            {
                Id = context.Message.ProductId,
                RatingAvg = context.Message.Rating,
                RatingCount = 1,
                FeaturedPoint = 1
            });
        }
        else
        {
            productMetric.RatingAvg = (productMetric.RatingAvg * productMetric.RatingCount + context.Message.Rating) / (productMetric.RatingCount + 1);
            productMetric.RatingCount += 1;
            productMetric.FeaturedPoint += 1;
        }

        await _context.SaveChangesAsync(context.CancellationToken);
    }

}
