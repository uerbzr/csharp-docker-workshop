using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using workshop.wwwapi.Data;
using workshop.wwwapi.DataModels;
using workshop.wwwapi.DataTransferObjects;
using workshop.wwwapi.Repository;

namespace workshop.wwwapi.Endpoints
{
    public static class CarEndpoint
    {
        public static void ConfigureCarEndpoint(this WebApplication app)
        {
           
            var cars = app.MapGroup("cars");
            cars.MapGet("/", GetAll);
            cars.MapPost("/", AddCar).AddEndpointFilter(async (invocationContext, next) =>
            {
                var car = invocationContext.GetArgument<CarPost>(1);

                if (string.IsNullOrEmpty(car.Make) || string.IsNullOrEmpty(car.Model))
                {
                    return Results.BadRequest("You must enter a Make AND Model");
                }
                return await next(invocationContext);
            }); ;
            cars.MapGet("/{id}", GetById);


        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetAll(IRepository<Car> repository)
        {
            return TypedResults.Ok(await repository.Get());
        }
       
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> GetById(IRepository<Car> repository, int id)
        {
            var result = await repository.GetById(id);
            if(result==null)
            {
                return TypedResults.NotFound($"Could not find Car with Id:{id}");
            }
            return TypedResults.Ok(result);
        }
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> AddCar(IRepository<Car> repository, CarPost model)
        {
            var results = await repository.Get();
            
            if (results.Any(x => x.Model.Equals(model.Model, StringComparison.OrdinalIgnoreCase)))
            {
                return Results.BadRequest("Car with provided name already exists");
            }

            var entity = new Car() { Make = model.Make, Model = model.Model };
            await repository.Insert(entity);
            return TypedResults.Created($"/{entity.Id}", entity);

        }
    }
}
