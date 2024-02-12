﻿using Microsoft.AspNetCore.Mvc;
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
            cars.MapPut("/{id}", Update);
            cars.MapDelete("/{id}", Delete);

        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> Delete(IRepository<Car> repository, int id)
        {
            var entity = await repository.GetById(id);
            if (entity == null)
            {
                return TypedResults.NotFound($"Could not find Car with Id:{id}");
            }
            var result = await repository.Delete(entity);
            return result != null ? TypedResults.Ok(new { Make = result.Make,Model = result.Model }) : TypedResults.BadRequest($"Car wasn't deleted");
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> Update(IRepository<Car> repository, int id, CarPatch model)
        {
            var entity = await repository.GetById(id);
            if(entity==null)
            {
                return TypedResults.NotFound($"Could not find Car with Id:{id}");
            }            
            entity.Model = !string.IsNullOrEmpty(model.Model) ? model.Model : entity.Model;
            entity.Make = !string.IsNullOrEmpty(model.Make) ? model.Make : entity.Make;

            var result = await repository.Update(entity);

            return result!=null ? TypedResults.Ok(new { Make = result.Make, Model = result.Model }) : TypedResults.BadRequest("Couldn't save to the database?!");
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetAll(IRepository<Car> repository)
        {
            var entities = await repository.Get();
            List<Object> results = new List<Object>();
            foreach(var car in entities)
            {
                results.Add(new { TemporaryFieldId=car.Id, Make = car.Make, Model = car.Model });
            }
            return TypedResults.Ok(results);
        }
       
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> GetById(IRepository<Car> repository, int id)
        {
            var entity = await repository.GetById(id);
            if(entity==null)
            {
                return TypedResults.NotFound($"Could not find Car with Id:{id}");
            }
            return TypedResults.Ok(new { Make = entity.Make, Model = entity.Model });
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
            return TypedResults.Created($"/{entity.Id}", new { Make = entity.Make, Model = entity.Model });

        }
    }
}
