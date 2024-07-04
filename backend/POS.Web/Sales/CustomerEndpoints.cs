using Microsoft.AspNetCore.Mvc;
using POS.Application.Abstractions;
using POS.Core.Sales;

namespace POS.Web.Sales;

public static class CustomerEndpoints
{
    public static void AddCustomerEndpoints(this IEndpointRouteBuilder routes)
    {
        var customers = routes.MapGroup("customers");
        customers.MapPost("/", Create);
        customers.MapPut("/", Update);
        customers.MapDelete("/", Remove);
        customers.MapGet("/", GetByID);
        customers.MapGet("all", GetAll);
        customers.MapGet("count", GetCount);
    }

    private static Task Remove(
        [FromServices] ICustomerService customerService,
        [FromQuery] int id
    )
    {
        return customerService.Remove(id);
    }

    private static async Task<Customer> Create(
        [FromServices] ICustomerService customerService,
        [FromBody] Customer customer
    )
    {
        await customerService.Create(customer);
        return customer;
    }

    private static async Task<Customer> Update(
        [FromServices] ICustomerService customerService,
        [FromBody] Customer customer
    )
    {
        await customerService.Update(customer);
        return customer;
    }

    private static Task<Customer?> GetByID(
        [FromServices] ICustomerService customerService,
        [FromQuery] int id
    )
    {
        return customerService.GetByID(id);
    }

    private static Task<List<Customer>> GetAll(
        [FromServices] ICustomerService customerService,
        [FromQuery] int? page,
        [FromQuery] int? pageSize
    )
    {
        return customerService.GetAll(page, pageSize);
    }

    private static Task<int> GetCount(
        [FromServices] ICustomerService customerService
    )
    {
        return customerService.GetCount();
    }
}
