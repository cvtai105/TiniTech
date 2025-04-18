using Application.Products.Commands.ActivateProductCommand;
using Application.Products.Commands.CreateProductCommand;
using Application.Products.Commands.SoftDeleteProductCommand;
using Application.Products.Queries.GetDetailBySlug;
using Application.Products.Queries.GetNewProduct;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers.Base;

namespace WebAPI.Controllers;

[Route("api/products")]
public class ProductController : ApiController
{
    private readonly ILogger<ProductController> _logger;

    public ProductController(ILogger<ProductController> logger, ISender sender) : base(sender)
    {
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductCommand cmd)
    {
        var result = await Sender.Send(cmd);

        return result.Match(
            r => CreatedAtAction(nameof(GetBySlug), new { id = r }, new Response
            {
                Title = "Product Created",
                Status = "Success",
                Detail = "Product Created Successfully",
                Data = r,
                Errors = null
            }),
            e => HandleFailure<CreateProductCommand>(e)
        );
    }

    // [HttpPut("{id}")]
    // public async Task<IActionResult> Update(int id, [FromBody] UpdateProductCommand cmd)
    // {
    //     if (id != cmd.Id)
    //     {
    //         return BadRequest(new Response
    //         {
    //             Title = "Bad Request",
    //             Status = "Error",
    //             Detail = "ID in URL must match ID in request body",
    //             Data = null,
    //             Errors = new[] { "ID mismatch" }
    //         });
    //     }

    //     var result = await Sender.Send(cmd);

    //     return result.Match(
    //         r => Ok(new Response
    //         {
    //             Title = "Product Updated",
    //             Status = "Success",
    //             Detail = "Product Updated Successfully",
    //             Data = r,
    //             Errors = null
    //         }),
    //         e => HandleFailure<UpdateProductCommand>(e)
    //     );
    // }
    public record PatchStatusData(int Id, ProductStatus Status);

    [HttpPatch("status")]
    public async Task<IActionResult> UpdateStatus([FromBody] PatchStatusData body)
    {
        if (body.Status == ProductStatus.Deleted)
        {
            var result = await Sender.Send(new SoftDeleteProductCommand { Id = body.Id });

            return result.Match(
                r => Ok(new Response
                {
                    Title = "Product Deleted",
                    Status = "Success",
                    Detail = "Product status changed to deleted",
                    Data = r,
                    Errors = null
                }),
                e => HandleFailure<SoftDeleteProductCommand>(e)
            );
        }
        else if (body.Status == ProductStatus.Active)
        {
            var result = await Sender.Send(new ActivateProductCommand { Id = body.Id });

            return result.Match(
                r => Ok(new Response
                {
                    Title = "Product Restored",
                    Status = "Success",
                    Detail = "Product status changed to active",
                    Data = r,
                    Errors = null
                }),
                e => HandleFailure<ActivateProductCommand>(e)
            );
        }
        else return BadRequest(
            new Response
            {
                Title = "Bad Request",
                Status = "Error",
                Detail = "Invalid Product Status",
                Data = null,
                Errors = new[] { "Invalid Product Status" }
            });
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        var result = await Sender.Send(new GetProductDetailBySlug { Slug = slug });

        return result.Match(
            r => Ok(new Response
            {
                Title = "Ok",
                Status = "Success",
                Detail = "Product Retrieved Successfully",
                Data = r,
                Errors = null
            }),
            e => HandleFailure<GetProductDetailBySlug>(e)
        );
    }

    [HttpGet("new")]
    public async Task<IActionResult> GetNewProducts([FromQuery] GetNewProductsQuery query)
    {
        var result = await Sender.Send(query);

        return result.Match(
            r => Ok(new Response
            {
                Title = "Ok",
                Status = "Success",
                Detail = "New Products Retrieved Successfully",
                Data = r,
                Errors = null
            }),
            e => HandleFailure<GetNewProductsQuery>(e)
        );
    }
}
