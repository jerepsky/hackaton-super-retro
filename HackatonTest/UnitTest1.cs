using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FeedbackBros.SuperRetro_1_0.Functionality.ServiceToSolution.AgentInPrivate.SuperRetro.ManageRetrospectives_bundle.IWantToBeAbleToAddNewRetrospective_group;
using FeedbackBros.SuperRetro_1_0.Functionality.ServiceToSolution.AgentInPrivate.SuperRetro.Retrospective.ViewRetrospective_bundle.IWantToGetRetrospectiveInfo_group;
using FeedbackBros.SuperRetro.SuperRetro.Retrospective;
using Uniscale.Core;
using Uniscale.Designtime;
using Xunit;

namespace HackatonTest;

public class UnitTest1
{
    private readonly HttpClient _httpClient = new() { BaseAddress = new Uri("http://localhost:5295/") };

    private async Task<PlatformSession> PlatformInit()
    {
        return await Platform.Builder()
            .WithInterceptors(i =>
                i.InterceptPattern("*", async (input, ctx) =>
                {
                    var json = GatewayRequest.From(input, ctx).ToJson();
                    var response = await _httpClient.PostAsync("/Retro", new StringContent(json, Encoding.UTF8, "application/json"));
                    var content = await response.Content.ReadAsStringAsync();
                    return Result<object>.FromJson(content);
                })
            )
            .Build();
    }

    [Fact]
    public async Task Test1()
    {
        var actorSceneSession = (await PlatformInit())
            .AsSolution(Guid.NewGuid(), ActorInScene.AgentInPrivateScene);

        var dispatcher = actorSceneSession
            .WithTransactionId(Guid.NewGuid())
            .WithLocale("en-GB")
            .WithDataTenant(Guid.NewGuid().ToString());
        
        var retroToCreate = new NewRetrospectiveInput {Name = "Testa"};
        var newRetroIdResult = await dispatcher
            .Request(NewRetrospective.With(retroToCreate));

        var retroResult = await dispatcher.Request(GetRetrospective.With(newRetroIdResult.Value));

        Assert.NotNull(retroResult.Value);
        Assert.Equal(retroToCreate.Name, retroResult.Value.Name);
        Assert.Equal(newRetroIdResult.Value, retroResult.Value.RetrospectiveIdentifier);

    }
}