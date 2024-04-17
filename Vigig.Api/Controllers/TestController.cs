using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Vigig.Api.Controllers.Base;
using Vigig.DAL.Interfaces;
using Vigig.Domain.Models;

namespace Vigig.Api.Controllers;
[Microsoft.AspNetCore.Mvc.Route("/api/[controller]")]
public class TestController : BaseApiController
{
    private readonly IGenericRepository<Booking> _repository;
    public TestController(IGenericRepository<Booking> repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public IQueryable<Booking> hello()
    {
        return _repository.GetAll();
    }
}