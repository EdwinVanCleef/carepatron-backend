using api.Data;
using api.Models;
using api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Net.Sockets;

namespace api
{
    public static class ClientEndpointsV1
    {
        public static IEndpointRouteBuilder MapClientApiEndpoints(this IEndpointRouteBuilder routes)
        {
            var baseUrl = "/api/v1";

            routes.MapGet(baseUrl + "/clients", GetClients).WithName("GetClients"); 

            routes.MapGet(baseUrl + "/clients/{name}", SearchClients).WithName("SearchClients");

            routes.MapPost(baseUrl + "/clients", CreateClient).WithName("CreateClient");

            routes.MapPut(baseUrl + "/clients/{id}", UpdateClient).WithName("UpdateClient");

            return routes;
        } 

        public static async Task<IResult> GetClients(IClientRepository clientRepository)
        {
            var clients =  await clientRepository.Get();
            return Results.Ok(clients);
        }

        public static async Task<IResult> SearchClients(string name, IClientRepository clientRepository)
        {
            var clientList = await clientRepository.SearchClient(name);
            return clientList.Length > 0 ? Results.Ok(clientList) : Results.NotFound();
        }

        public static async Task<IResult> CreateClient(Client client, IClientRepository clientRepository)
        {
            if (client == null) return Results.NotFound();

            try
            {
                await clientRepository.Create(client);

                return Results.Created($"/clients/{client.Id}", client);
            }
            catch (ArgumentException ae)
            {
                return Results.BadRequest(ae.Message);
            }
        }

        public static async Task<IResult> UpdateClient(string id, Client client, IClientRepository clientRepository)
        {
            try
            {
                await clientRepository.Update(id, client);

                return Results.Ok($"Client {id} successfully updated");
            }
            catch (ArgumentException ae)
            {
                return Results.BadRequest(ae.Message);
            }
        }
    }
}
