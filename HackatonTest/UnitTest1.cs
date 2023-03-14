using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FeedbackBros.SuperRetro_1_0.Functionality.ServiceToSolution.AgentInPrivate.SuperRetro.ManageRetrospectives_bundle.IWantToBeAbleToAddNewRetrospective_group;
using FeedbackBros.SuperRetro.SuperRetro.Retrospective;
using Uniscale.Core;
using Uniscale.Designtime;
using Uniscale.ProductCatalogue.Platform.Fundamentals;
using Xunit;
using Xunit.Sdk;

namespace HackatonTest;

public class UnitTest1
{
    private readonly HttpClient _httpClient = new() { BaseAddress = new Uri("http://localhost:5295/") };
    
    public static StringContent GetJsonStringContent<T>(T model)
        => new(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
    
    [Fact]
    public async Task Test1()
    {
        #region hide
        var session = await Platform.Builder()
            .WithInterceptors(i =>
                i.InterceptPattern("*", async (input, ctx) =>
                {
                    var json = GatewayRequest.From(input, ctx).ToJson();
                    var response = await _httpClient.PostAsync("/Retro", GetJsonStringContent(json));
                    var resultJson = await response.Content.ReadAsStringAsync();
                    var result = Result<object>.FromJson(resultJson);
                    return result;
                })
            )
            .Build();
        var actorSceneSession = session
            .AsSolution(Guid.NewGuid(), ActorInScene.AgentInPrivateScene);

        var caller = actorSceneSession
            .WithTransactionId(Guid.NewGuid())
            .WithLocale("en-GB")
            .WithDataTenant(Guid.NewGuid().ToString());
        #endregion hide
        
        var response = await caller
            .Request(NewRetrospective.With(new NewRetrospectiveInput {Name = "Testa"}));
        Console.WriteLine(response.Value);
        
    }
}